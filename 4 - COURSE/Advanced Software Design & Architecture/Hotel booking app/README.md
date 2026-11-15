# P09 · Hotel booking: the booking rule enforced by PostgreSQL (C#)

Extract the ZIP and open a terminal in the extracted folder. The project is the finished P09
app: the P08 app with the double booking repaired. A room can no longer have two Confirmed
reservations covering the same night, and the rule is enforced by the `reservations_no_overlap`
exclusion constraint in the database rather than by any one copy of the application, so it holds
however many instances are running.

docs/interleaving.md has the trace of the race and what each of the four repairs cost;
docs/adr-003-booking-concurrency.md records the decision and the three rejected alternatives.

Start Docker Desktop first. Then, once (internet needed the first time):

```text
# nothing to type: dotnet downloads the packages on the first build (internet needed once)
```

Start PostgreSQL and Redis, run the checks, the demo and the race, then start the API and send
the race tool at it from a second terminal:

```text
docker compose up -d
dotnet test HotelBooking.Tests
dotnet run --project HotelBooking -- --demo
dotnet run --project HotelBooking -- --race
$env:HOTEL_SLOW_STEP = "1"; dotnet run --project HotelBooking            # the API on http://localhost:5080
dotnet run tools/race-http.cs            # in a second terminal
```

The checks print:

```text
Passed!  - Failed: 0, Passed: 19, Skipped: 0, Total: 19
```

`--race` now ends with "Only one guest has R7." rather than "Double booking!": the list store
keeps the same no-overlap promise the constraint keeps.

Started with `HOTEL_SLOW_STEP=1`, the race tool sends two bookings for one room at the same
moment and gets:

```text
Two bookings for R7, 2026-11-12 to 2026-11-14, sent at the same moment.

Maya -> 201 Created   {"id":"R101","status":"Confirmed","notificationFailed":false}
Omar -> 409 Conflict  {"title":"Unavailable","status":409,"detail":"The room is booked for at least one night of this stay."}

Confirmed reservations for R7 on 12-14 Nov: 1
CORRECT: one booking was accepted and one was refused.
```

The rule cannot be bypassed by anything, including a statement typed straight into psql:

```text
ERROR:  conflicting key value violates exclusion constraint "reservations_no_overlap"
DETAIL:  Key (room, daterange(check_in, check_out, '[)'::text))=(R7, [2026-11-13,2026-11-15)) conflicts with existing key (room, daterange(check_in, check_out, '[)'::text))=(R7, [2026-11-12,2026-11-14)).
```

`db/init.sql` creates the constraint when the database is created. To add it to a database that
already exists, and that has no overlapping Confirmed rows left in it:

```sql
CREATE EXTENSION IF NOT EXISTS btree_gist;

ALTER TABLE reservations
    ADD CONSTRAINT reservations_no_overlap
    EXCLUDE USING gist (
        room WITH =,
        daterange(check_in, check_out, '[)') WITH &&
    ) WHERE (status = 'Confirmed');
```

`HOTEL_PORT` starts a second instance on another port, against the same database, which is how
the practice shows that a lock inside one process cannot protect the rule and the constraint can.

If port 5432 or 6379 is already taken on your laptop, change "5432:5432" or "6379:6379" in
docker-compose.yml to "15432:5432" or "16379:6379" and set HOTEL_DB_PORT=15432 or
HOTEL_REDIS_PORT=16379 before starting the app.
