# Designing a Domain Model to enforce No Duplicate Names

Some design approaches to enforcing a business rule requiring no duplicates.

## The Problem

You have some entity in your domain model. This entity has a `name` property. You need to ensure that this name is unique within your application. How do you solve this problem?

## The Database

This simplest approach is to ignore the rule in your domain model, and just rely on some infrastructure to take care of it for you. Typically this will take the form of a unique constraint on a SQL database which will throw an exception if an attempt is made to insert or update a duplicate value in the entity's table. This works, but doesn't allow for changes in persistence nor does it keep business rules in the domain model itself.

## Domain Service

One option is to detect if the name is duplicated in a domain service, and force updates to the name to flow through the service. This keeps the logic in the domain model, but leads to anemic entities.


