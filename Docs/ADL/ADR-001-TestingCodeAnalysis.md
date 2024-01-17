# Testing code analysis

## Status

1. Proposed: 2024-01-17

## Context

We need very good safety net for code analysis because it's complex process and it's easy to introduce regression.

The input for code analysis is semantic model from Roslyn. It's impossible to prepare it by developer for testing purposes. The only way to test code analysis is to run it on real code.

The output of code analysis is a set of elements and relations. It can have many representations. Internally it's an object representation (`Model`). Externally the main representation is a json file with exactly the same structure for parsers for all languages. This representation is very important because it's public contract between parsers and visualization tools. There can by many other representations. For now we have Mermaid pages which serve also as model visualization at this early stage of the project.

We need to check both:
- if output contains expected elements and relations
- if output doesn't contain any other elements and relations

## Options

### Testing object representation

Assertions are made on object representation be creating `Element` and `Relation` objects and checking if `Model` object contains them.

Pros:
- It's easy to create objects for assertions.
- It's easy to test if model doesn't contain unexpected elements and relations.

Cons:
- Json representation have to be tested separately.

### Testing json representation

Assertions are made on json representation by checking if json file contains expected json elements. These elements are defined as text.

Pros:
- Json serialization is automatically checked and thus public contract is checked.

Cons:
- It's more tedious to create json than objects.
- Output json and expected json elements have to be normalized before assertions. 

## Decision

We decided to test code analysis by object representation because:
1. It's easier in development.
2. Testing json representation can be done quite easily by preparing sample objects, serializing them and comparing with expected json defined as text.

## Impact on quality attributes

Testability is achieved at lowes possible cost. 

