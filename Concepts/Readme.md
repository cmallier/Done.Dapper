# Dapper

Simple, lightweight, and fast ORM for .NET

Lightweight ORM that provides a minimal amount of abstraction over your database.

Avantages
- Performant
- Contrôle sur les requêtes SQL
- Relativement simple à utiliser
- Pas de génération de code
- Pas de configuration
- Pas de tracking d'entités
- Pas de gestion de relations

Désavantages
- Doit comprendre le SQL
- Pas de génération de code (Scaffolding)
- Pas de tracking d'entités (Unit of Work)
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


## Vs. EF Core

La comparaison avec EF Core est difficile car les deux ne jouent pas le même rôle.

- Performance similaire. Généralement un peu plus performant pour Dapper.
  - Note : Le type de requête et les accès à la base de données ont un plus gros impact
- Pas d'intégration avec Linq
- Moins d'abstraction pour Dapper


Les 2 outils ont leur place
Les 2 peuvent être utilisés en même temps dans un même projet.

Par exemple, dans le Clean Architecture, il n'est pas rare d'avoir les 2 outils
EF Core pour l'écriture
Dapper pour la lecture de modèle simple (Summary) ou requête complexe ou de meilleures performances

Note : Dans EF Core 8, il est maintenant possible de l'utiliser un peu comme Dapper
