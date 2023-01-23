# EntityFrameworkMigration

### Context

When upgrading from Entity Framework core 6 to EF core 7, I had errors when trying to update related entities

The problem is detailed in a stackoverflow thread : https://stackoverflow.com/questions/74896336/replace-multi-level-relationship-in-entity-framework-core-6-vs-7

This is a minimal reproducible example to better illustrate the problem. 
An image of the ERD model of the tables is inside the project (`DB_ERD_Model.png`)

### Steps to reproduce
- Create a DB
- Execute the sql script `entityFrameworkMigration.sql` inside the project to create the tables
- Run the tests inside `ParentServiceRepositoryTest`. They will succeed 
	- The project is configured to use EF core 6
	- Connection string is inside the `ParentServiceRepositoryTest` constructor
- Upgrade to EF core 7 :
	- Upgrade the following packages from 6.0.13 to 7.0.2 :
		- Microsoft.EntityFrameworkCore
		- Microsoft.EntityFrameworkCore.Tools
	- Upgrade the following packages from 6.0.8 to 7.0.1 :
		 - Npgsql.EntityFrameworkCore.PostgreSQL
- Run the tests inside `ParentServiceRepositoryTest` another time. They will fail 

### Code structure
The project contains the following structure :
- Inside the EntityFrameworkMigration project
	- Inside the `Models` folder, there is the DB models and the DB context `test_localContext`
	- `ParentServiceRepository` that contains the method `UpdateParentChild` that makes the update. This method is tested in `ParentServiceRepositoryTest`
- Inside the `EntityFrameworkMigrationTest` project
	- `ParentServiceRepositoryTest` contains the tests for `UpdateParentChild` method