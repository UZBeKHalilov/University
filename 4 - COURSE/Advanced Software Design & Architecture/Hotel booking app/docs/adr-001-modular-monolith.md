# ADR-001: Start the hotel booking app as a modular monolith

Status: Accepted, 25 October 2026

## Context
One small team builds and releases the whole app. The hotel has four business
areas: Rooms, Reservations, Payments and Notifications. The most important
scenarios are S2 (no double booking) and S3 (a failed message keeps the booking).
Nothing needs to be released or scaled on its own yet.

## Decision
One app with four modules. Each module has a public contract and internal code.
Reservations owns the booking rule. Architecture tests stop modules from using
each other's internal code.

## Alternatives considered
Separate services for Payments and Notifications: independent releases and
scaling, but network failures, retries, more deployments and more monitoring.

## Consequences
+ One build, one deployment, local method calls; S2 is checked in one place.
- All modules are released together.
- The borders hold only while the architecture tests run (P06, and in CI from P25).

## Revisit when
A module needs its own release schedule or its own scaling, measured, not guessed
(L07/P13 makes that decision with real reasons).
