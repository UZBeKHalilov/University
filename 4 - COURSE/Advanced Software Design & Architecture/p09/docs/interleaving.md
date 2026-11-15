# The interleaving that double-books R7

## What I ran

`tools/race-http` sends two `POST /reservations` for room R7, 2026-11-12 to
2026-11-14, at the same moment: one for Maya, one for Omar.

```
Two bookings for R7, 2026-11-12 to 2026-11-14, sent at the same moment.

Maya -> 201 Created   {"id":"R101","status":"Confirmed","notificationFailed":false}
Omar -> 201 Created   {"id":"R102","status":"Confirmed","notificationFailed":false}

Confirmed reservations for R7 on 12-14 Nov: 2
DOUBLE BOOKED: the room has two confirmed reservations for the same nights.
```

## The interleaving, statement by statement

| # | Maya's request | Omar's request | What the database holds |
|---|---|---|---|
| 1 | `BEGIN` | | no confirmed reservation for R7 |
| 2 | `SELECT count(*)` -> 0 | | unchanged |
| 3 | | `BEGIN` | unchanged |
| 4 | | `SELECT count(*)` -> 0 | unchanged: Maya has inserted nothing yet |
| 5 | `INSERT R101` | | R101 exists, uncommitted and invisible to Omar |
| 6 | | `INSERT R102` | both rows exist, each invisible to the other |
| 7 | `COMMIT` | | R101 is now visible to everyone |
| 8 | | `COMMIT` | R101 and R102 are both Confirmed for the same nights |

Both requests read "no clash", and both were telling the truth when they read
it. The gap between step 2 and step 5 is the whole bug: the check and the save
are two separate statements, and another transaction fits between them.

## Which anomaly this is

Write skew. Each transaction reads a set of rows (the confirmed reservations
that overlap the stay), finds it empty, and then **inserts a different row**.
There is no lost update, because neither transaction overwrites the other's
row. Raising the isolation level to Repeatable Read does not stop it: Repeatable
Read guarantees that a row you read does not change under you, and no row that
either transaction read changed. Serializable does stop it, by detecting the
dangerous read-write pattern and aborting one transaction with a
serialization failure.

## The invariant this breaks

> A room never has two Confirmed reservations covering the same night.

## Why locking the reservation rows cannot fix it

The obvious repair is `SELECT ... FOR UPDATE` on the overlapping reservations.
It does nothing here, because at step 2 **there are no overlapping reservations
to lock**. `FOR UPDATE` locks the rows a query returns, and that query returns
none. You cannot lock a row that does not exist yet. That is exactly why write
skew is its own anomaly and not a lost update, and it is why Step 5 locks a
guard row instead.

## The four repairs, and what each one cost

| Repair | Did it stop the double booking? | What it cost, measured |
|---|---|---|
| In-process lock | With one instance, yes. With two, no. | Nothing, and it is worth nothing: the moment there are two processes the rule is broken again. |
| `SELECT ... FOR UPDATE` on a guard row | Yes | Omar waited **0.91 seconds** at his `FOR UPDATE` for a room he was never going to get. Every booking for a room queues behind the one in front, for as long as that transaction lasts. |
| Optimistic version checking | Yes | **Nobody waited.** Both sessions read `version = 0` without taking a lock; the loser's `UPDATE ... WHERE version = 0` matched 0 rows, so it rolled back and did its work a second time. Cheap when clashes are rare, wasteful when they are common. |
| Exclusion constraint | Yes | Only covers a rule that fits inside one table; the error arrives as a constraint violation the app must turn into a readable 409; and it could not be switched on until the rows that already broke it were cleaned up. |

The two middle repairs are the pessimistic and the optimistic answer to the same
question, and the timings say exactly how they differ: one makes the loser
**wait**, the other makes the loser **redo**. Neither is wrong. Which one is
better depends on how often two guests really do reach for the same room at the
same moment.

## What it took to turn the constraint on

The constraint could not simply be added. The table already held the two
confirmed reservations Step 1 created, and PostgreSQL refuses to create a
constraint that the existing rows already break:

```
ERROR:  could not create exclusion constraint "reservations_no_overlap"
DETAIL:  Key (room, daterange(check_in, check_out, '[)'::text))=(R7, [2026-11-12,2026-11-14)) conflicts with key (room, daterange(check_in, check_out, '[)'::text))=(R7, [2026-11-12,2026-11-14)).
```

So one of the two bookings had to be cancelled first. That is not an artefact of
the exercise - it is what adding a constraint to a real database is always like.
The rule you are about to enforce has been broken for as long as it went
unenforced, and the broken rows are still there. Finding them, and deciding what
to do about them, is part of the work; in a real hotel somebody would have to
telephone one of the two guests.

## What I kept

The exclusion constraint, with the pre-check left in front of it. The constraint
is the only one of the four that no code path can forget: a new endpoint, a
background job, a migration script or somebody typing `INSERT` into `psql` all
hit it. The pre-check stays so that the ordinary case still answers a clean
`409 Unavailable` with a readable message, instead of the app having to explain
a database error to a guest.
