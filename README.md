# Designing a Domain Model to enforce No Duplicate Names

Some design approaches to enforcing a business rule requiring no duplicates.

## The Problem

You have some entity in your domain model. This entity has a `name` property. You need to ensure that this name is unique within your application. How do you solve this problem? This repository shows 11 different ways to do it in a DDD application where the goal is to keep this business logic/rule in the domain model.

## Approaches

Below are different approaches to solving this problem. In each case the necessary classes are grouped in a folder. Imagine that in a real application these would be split into several projects, with the domain model in one, repository implementations in another, and tests (or actual UI clients) in others still.

## The Database

This simplest approach is to *ignore the rule in your domain model* (which is basically cheating in terms of DDD), and just rely on some infrastructure to take care of it for you. Typically this will take the form of a unique constraint on a SQL database which will throw an exception if an attempt is made to insert or update a duplicate value in the entity's table. This works, but doesn't allow for changes in persistence (to a system that doesn't support unique constraints or doesn't do so in a cost or performance acceptable fashion) nor does it keep business rules in the domain model itself. However, it's also easy and performs well so it's worth considering.

This repo doesn't use an actual database so the behavior is faked in the `ProductRepository`.

[Approach 1 - Database](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/01_Database)

## Domain Service

One option is to detect if the name is duplicated in a domain service, and force updates to the name to flow through the service. This keeps the logic in the domain model, but leads to anemic entities. Note in this approach that the entity has no logic in it and any work with the entity needs to happen through the domain service. A problem with this design is that over time it leads to putting all logic in services and having the services directly manipulate the entities, eliminating all encapsulation of logic in the entities. Why does it lead to this? Because clients of the domain model want a consistent API with which to work, and they don't want to have to work with methods on the entity some of the time, and methods through a service some of the time, with no rhyme or reason (from their perspective) why they need to use one or the other. And any method that starts out on the entity may need to move to the service if it ever has a dependency.

[Approach 2 - Domain Service](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/02_DomainService)

## Pass Necessary Data Into Entity Method

One option is to give the entity method all of the data it needs to perform the check. In this case you would need to pass in a list of every name that it's supposed to be different from. And of course don't include the entity's current name, since naturally it's allowed to be what it already is. This probably doesn't scale well when you could have millions or more entities.

[Approach 3 - Pass Necessary Data into Entity Method](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/03_PassDataToMethod)

## Pass a Service for Checking Uniqueness Into Entity Method

With this option, you perform dependency injection via method injection, and pass the necessary dependency into the entity. Unfortunately, this means the caller will need to figure out how to get this dependency in order to call the method. The caller also could pass in the wrong service, or perhaps no service at all, in which case the domain rule would be bypassed.

[Approach 4 - Pass Service to Method](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/04_MethodInjectionService)

## Pass a Function for Checking Uniqueness Into Entity Method

This is just like the previous option, but instead of passing a type we just pass a function. Unfortunately, the function needs to have all the necessary dependencies and/or data to perform the work, which otherwise would have been encapsulated in the entity method. There's also nothing in the design that requires the calling code to pass in the appropriate function or even any useful function at all (a no-op function could easily be supplied). The lack of encapsulation means the validation of the business rule is not enforced at all by our domain model, but only by the attentiveness and discipline of the client application developer (which even if it's you, is easily missed).

[Approach 5 - Pass Function to Method](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/05_MethodInjectionFunction)

## Pass Filtered Data Into Entity Method

This is a variation of Approach 3 in which the calling code now passes just the existing names that match the proposed name, so that the method can determine if the new name already exists. It seems like it's doing *almost* all of the useful work required for the business rule, without actually doing the check for uniqueness. This to me requires far to much work and knowledge for the calling code.

[Approach 6 - Pass Filtered Data to Method](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/06_PassFilteredDataToMethod)

## Use an Aggregate with Anemic Children

The problem doesn't mention whether the entity in question is standalone or part of an aggregate. If we introduce an aggregate, we can make it responsible for the business invariant (unique name) by making it responsible for all name changes or additions. Having an aggregate responsible for its invariants, especially when they related *between* child entities, often makes sense. However, you want to be careful not to have your aggregate root become a god class and turn all of your child entities into anemic DTOs (as this approach basically does).

[Approach 7 - Aggregate with Anemic Children](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/07_AggregateWithAnemicChildren)

## Use an Aggregate with Double Dispatch

Double dispatch is a pattern in which you pass an instance of a class into a method so that the method can call back to the instance. Often it's "the current instance" or `this` that is passed. It provides a way for an aggregate to allow children to stay responsible for their own behavior, while still calling back to the aggregate to enforce invariants. I prefer to keep relationships one-way in my domain models, so while there is a navigation property from the aggregate to its children, there isn't one going the other way. Thus, to get a reference to the aggregate, one must be passed into the `UpdateName` method. And of course, there's nothing enforcing that the expected thing is actually passed here. Calling code could pass null or a new instance of the aggregate, etc.

[Approach 8 - Aggregate with Double Dispatch](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/08_AggregateWithDblDispatch)

## Use an Aggregate with C# Events

This is the first option that introduces using events. Events make sense when you have logic that needs to respond to an action. For example, "when someone tries to rename a product, it should throw an exception if that name is already in use". This approach uses C# language events, which unfortunately require a lot of code to implement properly.

[Approach 9 - Aggregate with C# Events](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/09_AggregateWithEvents)

## Use an Aggregate with MediatR Events

This approach uses an aggregate combined with MediatR-managed events. To avoid the need to pass MediatR into the entity or its method, I'm using a static helper class. This is a common approach and one that has been used successfully for over a decade (see [Domain Event Salvation from 2009](http://udidahan.com/2009/06/14/domain-events-salvation/)). This approach provides one of the cleanest client experiences in terms of API. Look at the tests and notice that all each test does is fetch the aggregate from the repository and call a method. The test code, which mimics real client code in this case, doesn't need to fiddle with any plumbing code or pass in functions or special services or any of that. The methods are clean, the API is clean, and the business logic is enforced in specific classes with that single responsibility.

[Approach 10 - Aggregates with MediatR Events](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/10_AggregateWithMediatR)


## Use Domain Events (no aggregate)

Sometimes it doesn't make sense to involve or model an aggregate. In this case you can still leverage domain events to provide a way to keep logic encapsulated in the entity while still leveraging infrastructure dependencies. The client experience in this approach is very similar to the previous one, with the exception of [a bit more client logic being required when adding new products](https://github.com/ardalis/DDD-NoDuplicates/blob/master/NoDuplicatesDesigns/11_DomainEventsMediatR/AddProductTests.cs#L44-L45). Otherwise, the client experience is very clean and consistent and not muddied with implementation details leaking from the domain layer.

[Approach 11 - MediatR Domain Events](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/11_DomainEventsMediatR)

## Summary

In most situations where there are business rules that involve multiple entities and their peers, I've found introducing an aggregate makes sense. Assuming you go the aggregate route, be careful to avoid putting all logic in the root and having anemic children. I've had good success using events to communicate up to the aggregate root from child entities (in addition to this sample see [AggregateEvents](https://github.com/ardalis/AggregateEvents)).

If you don't have an aggregate or there would only ever be one and it would have millions of other entities in it (which might be untenable), then you can still use domain events to enforce constraints on the collection as demonstrated in the last example. Using domain events as shown in the last couple of approaches does violate the [Explicit Dependencies Principle](https://deviq.com/explicit-dependencies-principle/), but that principle mainly applies to services, not entities, and in this case I feel that the tradeoff of leveraging domain events to provide a clean interface to my domain and keep logic encapsulated in my entities is worth it.

## Reference

This project was inspired by [this exchange on twitter between Kamil and me](https://twitter.com/kamgrzybek/status/1280868055627763713). Kamil's ideas for approaching this problem included:

1. Pass all the current names to the update method. [Done](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/03_PassDataToMethod)
2. Pass all the names matching the proposed name to the update method. [Done](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/06_PassFilteredDataToMethod)
3. Pass an `IUniquenessChecker` into the update method which returns a count of entities with that name. [Done](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/04_MethodInjectionService)
4. Pass a function that performs the same logic as #3. [Done](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/05_MethodInjectionFunction)
5. Check the invariant in an Aggregate. [Done - Anemic Children](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/07_AggregateWithAnemicChildren)
6. Create a unique constraint in the database. [Done](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/01_Database)

My own two approaches include:

7. Use a domain service (which will lead to an anemic entity) [Done](https://github.com/ardalis/DDD-NoDuplicates/tree/master/NoDuplicatesDesigns/02_DomainService)
8. Use domain events and a handler to perform the logic

### Learn More

[Learn Domain-Driven Design Fundamentals](https://www.pluralsight.com/courses/domain-driven-design-fundamentals)
