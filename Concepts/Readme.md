# Dapper

Simple, lightweight, and fast ORM for .NET

Avantages
- Performant
- Contrôle sur les requêtes SQL
- Relativement simple à utiliser
- Pas de génération de code
- Pas de configuration
- Pas de tracking d'entités
- Pas de gestion de relations

Désavantages
- Pas de génération de code
- Pas de tracking d'entités
- Pas de gestion de relations
- Pas de gestion de migrations


## Quick overview




## Add Extensions methods on IDbConnection

- Execute
  - Execute a command and return the number of affected rows

- Query | QueryFirst | QueryFirstOrDefault
  - Execute a query and map the result to a strongly typed list

- QuerySingle | QuerySingleOrDefault
  - Execute a query and map the result to a strongly typed list
  - Throws an exception if the result contains more than one element

- QueryMultiple
  - Execute a query and map the result to multiple lists


- Plus all __Async__ Versions
