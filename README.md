[![.NET](https://github.com/Tiffanatic/Bachelor/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Tiffanatic/Bachelor/actions/workflows/dotnet.yml)
# Bachelor

Bachelor project for:

Jesper Henriksen & Mads Gregers Rynord

## dotnet ef commands
```
PS C:\Users\<username>\<folder containining project>\bachelor\RapidTime.Data> dotnet ef migrations add "<descriptive-text>" --startup-project "..\RapidTime.Api\RapidTime.Api.csproj" 
PS C:\Users\<username>\<folder containining project>\bachelor\RapidTime.Data> dotnet ef database update --startup-project "..\RapidTime.Api\RapidTime.Api.csproj"
```

## Docker informationer

A docker package is released on each succesful build and is available from the attached github docker image repository
The ELK stack can be started using the docker-compose file in "docker elk", beware that it requires at least 10GiB memory.


## Technologies and methods used in this project
- aspnet core
- gRPC
- .net ef core
- ElasticSearch, LogStash and Kibana
- Blazor server
- Serilog
- Clean architecture
- repository pattern
- Docker(-compose)
- PostgreSQL
- xUnit
