# Cache experiment: the free-room search

PostgreSQL is the source of truth. Redis keeps copies of search answers for 30 seconds.

## Measurements

1,000 searches for 12–14 Nov, one after another, with the measurement tool.
Times from one laptop; yours will differ.

| Run | Searches | Database queries | Cache hits | Average | p95 |
|---|---|---|---|---|---|
| No cache | 1,000 | 2,000 | 0 | 2.0 ms | 2.5 ms |
| Cache-aside, TTL 30 s | 1,000 | 2 | 999 | 1.0 ms | 1.3 ms |

S1 asks for p95 ≤ 1 second: both runs meet it here, with two rooms and one user.
The cache removes 1,998 of 2,000 database queries. With 40 rooms it would remove
39,960 of 40,000.

## Stale result (Step 5, before invalidation)

1. Empty cache. Search 12–14 Nov: R7 and R8 (a miss: the answer is cached for 30 s).
2. Maya books R7 for 12–14 Nov: 201 R101.
3. Search again within 30 s: R7 and R8. STALE: R7 is booked.
4. Omar asks for R7 for 13–15 Nov: 409 Unavailable. Booking reads PostgreSQL,
   not the cache, so the booking rule still holds.

## After invalidation (Step 6)

The same four requests: the third search shows only R8, because Maya's booking
raised availability:version and the old copy is no longer read.

## Hot key and the next bottleneck (Step 7)

- New Year: everyone searches 31 Dec–2 Jan, so one key answers almost every search:
  availability:v{version}:2026-12-31:2027-01-02.
- Every booking raises the version, so in a busy sale each booking throws that copy
  away and the next search asks PostgreSQL again.
- Searches now cost Redis, not PostgreSQL. What is left for PostgreSQL is
  POST /reservations: 3 questions per booking, and they must be exact.
  That is the next bottleneck: P09.
