## Installation

### Migrate database

Migration can be done with the command:
```
dotnet ef database update
```

### Create distributed cache table

AEXMovies uses `IDistributedCache` to store session info which in turn uses 
a table in your SQL Server. To create this table run the command:
```
dotnet sql-cache create "Server=localhost;Database=AEXMovies.Development;User=sa;Password=Password$" dbo Cache 
```
Please note that the command above uses *local development* database. Read more in [the microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-6.0#distributed-sql-server-cache).