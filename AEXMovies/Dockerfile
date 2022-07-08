# Note: this dockerfile assumes that the context passed to docker 
# only includes main project folder

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AEXMovies.csproj", "./"]
RUN dotnet restore "AEXMovies.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "AEXMovies.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AEXMovies.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AEXMovies.dll"]
