# ADR-003: Protect the booking rule in the database

- Status: accepted
- Date: 2026-11-09
- Supersedes: nothing. Extends ADR-001 (the modular monolith) and ADR-002 (the cache).

## Context

The booking rule is: a room never has two Confirmed reservations covering the
same night. Until now that rule lived only in `BookingService.Reserve`, which
checks for an overlap and then saves. With one request at a time it holds. With
two requests at the same moment it does not: both read "no clash", both insert,
both commit, and the room is double-booked. `tools/race-http` reproduces it
every run, and `docs/interleaving.md` has the statement-by-statement trace.

The anomaly is write skew, so Repeatable Read does not stop it, and
`SELECT ... FOR UPDATE` on the reservations themselves does not stop it either:
at check time there is no overlapping row to lock.

## Decision

Enforce the rule in PostgreSQL with an exclusion constraint:

```sql
CREATE EXTENSION IF NOT EXISTS btree_gist;

ALTER TABLE reservations
    ADD CONSTRAINT reservations_no_overlap
    EXCLUDE USING gist (
        room WITH =,
        daterange(check_in, check_out, '[)') WITH &&
    ) WHERE (status = 'Confirmed');
```

Keep the existing pre-check in `BookingService.Reserve`. The pre-check answers
the ordinary case with a readable `409 Unavailable`; the constraint is what
makes the rule true. When the constraint fires, the Reservations module turns
the violation into the same `409`, so a guest never sees a database error.

## Alternatives considered

**An in-process lock.** A mutex around check-then-save. Correct with one
instance and worthless with two: a second process has its own mutex and neither
waits for the other. We ran exactly that experiment in Step 4 and watched the
double booking return. Rejected: capacity planning in `docs/capacity.md`
already assumes we can add instances.

**`SELECT ... FOR UPDATE` on a guard row.** One row per room in a
`room_booking_guard` table, locked at the start of every booking. Correct, and
it is the repair to reach for when the rule is too complicated for a constraint.
Rejected here because it serialises every booking for a room, holds the lock
until commit, and needs a guard row to exist for every room forever - including
rooms added later.

**Optimistic version checking.** The same guard row with a `version` column,
updated with the old version in the `WHERE`; no match means someone else booked
first, so retry. Correct, and better than a lock when clashes are rare.
Rejected here because it costs more code in every write path and still leaves
the rule enforceable only by code that remembers to do it.

**Serializable isolation.** Correct, and it needs no schema change at all.
Rejected for now because every transaction in the app would have to handle
serialization failures and retry, which is a larger change than this one rule
needs. Worth revisiting if we find a second write-skew rule.

## Consequences

- The rule is now true no matter which code path writes a reservation, including
  a `psql` session and any future endpoint or background job.
- A booking can now fail with a constraint violation that the app must catch and
  map to `409`. That mapping is covered by a test.
- The constraint only covers a rule that lives inside the `reservations` table.
  A rule that spans tables needs one of the rejected alternatives.
- Writing a reservation now also maintains a GiST index, so writes are slightly
  more expensive. At the capacity in `docs/capacity.md` (3.33 searches a second,
  a booking every few seconds) this does not matter.
- `db/init.sql` and the running database must both be changed, because
  `init.sql` runs only when the database is created.
- The constraint cannot be added while any room already has two overlapping
  Confirmed reservations. Turning it on means finding and fixing the rows that
  already break the rule - we had to cancel R102 before the `ALTER TABLE`
  would run. Any future constraint on live data will need the same step.

## When to revisit

- When a second rule needs protecting that does not fit in one table.
- When bookings become frequent enough for the GiST index writes to show up in a
  measurement.
- When the app starts writing reservations from more than one service, at which
  point the constraint stops being an optimisation and becomes the only thing
  holding the rule.
