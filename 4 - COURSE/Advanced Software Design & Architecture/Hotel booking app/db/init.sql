-- The hotel booking app's tables. PostgreSQL runs this file once, when the database is first created.
-- Rooms owns the rooms table and Reservations owns the reservations table, so there is no foreign key
-- between them: each module keeps its own data (L03).

-- P09 Step 7: the overlap rule, enforced by PostgreSQL itself.
-- btree_gist lets a GiST index hold plain equality (room WITH =) next to a range test
-- (daterange WITH &&). Without it PostgreSQL refuses the constraint: gist has no
-- equality operator class for text on its own.
CREATE EXTENSION IF NOT EXISTS btree_gist;

CREATE TABLE rooms (
    number text PRIMARY KEY,
    type   text NOT NULL
);

CREATE TABLE reservations (
    id          text PRIMARY KEY,   -- R101, R102 ...: a second R101 is refused
    guest_name  text NOT NULL,
    guest_email text NOT NULL,
    room        text NOT NULL,
    check_in    date NOT NULL,
    check_out   date NOT NULL,
    status      text NOT NULL CHECK (status IN ('Confirmed', 'Cancelled')),
    CHECK (check_out > check_in),

    CONSTRAINT reservations_no_overlap EXCLUDE USING gist (
        room WITH =,
        daterange(check_in, check_out, '[)') WITH &&
    ) WHERE (status = 'Confirmed')
);

-- P09 Step 1: reservation ids come from here, not from counting the rows.
-- nextval never returns the same number twice, even to two transactions running at the same moment.
CREATE SEQUENCE reservation_ids START 101;

INSERT INTO rooms (number, type) VALUES ('R7', 'Standard'), ('R8', 'Deluxe');
