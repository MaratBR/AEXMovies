## Installation

### Migrate database

Migration can be done with the command:

```
dotnet ef database update --connection "Server=DB_DOMAIN_OR_IP;Database=AEXMovies.Production;User=DB_USER;Password=DB_PASSWORD"
```

You will need to have dotnet installed.

### Installing with docker

```
git clone https://github.com/MaratBR/AEXMovies
cd AEXMovies
sudo docker build -f ./AEXMovies/Dockerfile AEXMovies -t aex/movies-test:latest
sudo docker run \
    --net host \
    -e ASPNETCORE_URLS=http://0.0.0.0:8000 \
    -e ConnectionStrings__Default="Server=DB_DOMAIN_OR_IP;Database=AEXMovies.Production;User=DB_USER;Password=DB_PASSWORD" \
    -e JwtConfig__SecretKey="secret key for jwt, keep it, well, secret" \
    aex/movies-test:latest
```


Notes:
* Replace `localhost:8000` with desirable host and port for the backend to listen on.
* This project does not support HTTPS out of the box (use proxy or add it yourself).
* You can generate secret with linux command (`< /dev/urandom tr -dc _A-Z-a-z-0-9 | head -c${1:-64};echo;
  `).
* Replace al `DB_*` strings with your respective values.
* This example uses `--net host` to let out API access database on the host's network.

### Database case-sensitivity

MS SQL (that we use for this project) is case-insensitive meaning that
"Hello World" = "HeLLo WORLd". If you use database that is not case-sensitive
(like PostgreSQL) it will break search a little.