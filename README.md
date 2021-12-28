# datalayer
An implementation of Dapper that provides a re-usable library for accessing Postgres, SQL Server or SQL Lite databases.

If you take a look at the unit tests in the DataLayer.Tests project that should give you some idea of how to implement this library.
  
If you download this code make sure you also download the Models repo as the DataLayer.sln file references this project. I compile the Models and DataLayer projects as Nuget packages and upload them to a package source. If you do that you can remove the project reference to Models from DataLayer and add a package reference using the Package Manager in Visual Studio. I've included a project reference here just to make the code and unit tests easily runnable from a simple download.

If you look through the code you will see a reference to DapperPlus. I use DapperPlus because it has really fast bulk insert capabilities but it is a paid licence. I've commented out the code that references DapperPlus which you can uncomment if you have your own licence.
