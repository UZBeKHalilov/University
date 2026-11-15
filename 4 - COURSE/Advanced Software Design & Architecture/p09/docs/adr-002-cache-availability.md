# ADR-002: Cache the free-room search in Redis

Status: Accepted, 7 November 2026

## Context
Every search asks PostgreSQL once per room: 2,000 queries for 1,000 searches with
two rooms, and 133 a second at the estimated peak with 40 rooms (docs/capacity.md).
A search answer changes only when someone books or cancels. The booking rule (S2)
must never depend on an old copy.

## Decision
Cache-aside in Redis for GET /rooms/availability only. The key is
availability:v{version}:{checkIn}:{checkOut}, the value is the free room numbers,
the TTL is 30 seconds. A successful booking or cancel raises availability:version.
POST and DELETE never read the cache: PostgreSQL is the source of truth.

## Alternatives considered
- No cache: always fresh and simplest, but every search costs one query per room.
- TTL only: simple, but a search can show a booked room for up to 30 seconds.
- Invalidation only: fresh after every change, but one missed invalidation keeps
  a wrong copy until Redis restarts.

## Consequences
+ 1,000 searches for the same stay cost 2 database queries instead of 2,000
  (docs/cache-experiment.md).
+ If an invalidation is ever missed, the copy is wrong for at most 30 seconds.
- Every booking empties every cached search: in a busy sale the hit rate falls.
- Redis is one more program to run. While it is down, searches get no answer,
  and a booking or cancel is saved but answers with an error, because the version
  cannot be raised. There is no fallback to PostgreSQL yet.
- Two misses at the same moment both ask PostgreSQL.

## Revisit when
Bookings, not searches, limit the app (P09), or the hit rate measured under
load (P23) is much lower than in docs/cache-experiment.md.
