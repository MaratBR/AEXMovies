## Installation

### Migrate database

Migration can be done with the command:
```
dotnet ef database update
```


### Installing with docker

```
git clone https://github.com/MaratBR/AEXMovies
cd AEXMovies
docker build . -t aex/movies-test:latest
docker run -p  aex/movies-test:latest
```

## Gotchas

### Database case-sensitivity
MS SQL (that we use for this project) is case-insensitive meaning that 
"Hello World" = "HeLLo WORLd". If you use database that is not case-sensitive
(like PostgreSQL) it will break search a little.