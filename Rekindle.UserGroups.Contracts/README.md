# Rekindle User Groups Contracts

This package contains the event contracts for the Rekindle User Groups microservice. 
It defines the events that are published by the User Groups service for consumption by other microservices.

## Events

### User Events
- `UserCreatedEvent` - Published when a new user is registered
- `UserUpdatedEvent` - Published when user information is updated

### Group Events
- `GroupCreatedEvent` - Published when a new group is created
- `GroupUpdatedEvent` - Published when group information is updated
- `UserJoinedGroupEvent` - Published when a user joins a group (by any means)
- `UserLeftGroupEvent` - Published when a user leaves a group or is removed

## Usage with Rebus

This package includes a reference to Rebus for message handling compatibility 
across microservices. To use it with RabbitMQ, add the `Rebus.RabbitMQ` package 
to your consuming service.