FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RapidTime.Api/RapidTime.Api.csproj", "RapidTime.Api/"]
RUN dotnet restore "RapidTime.Api/RapidTime.Api.csproj"
COPY . .
WORKDIR "/src/RapidTime.Api"
RUN dotnet build "RapidTime.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RapidTime.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RapidTime.Api.dll"]
