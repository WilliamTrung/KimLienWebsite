FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY *.sln .
COPY KimLienCustomerView/*.csproj KimLienCustomerView/
COPY AppCore/*.csproj AppCore/
COPY AppService/*.csproj AppService/
COPY AppRepository/*.csproj AppRepository/

RUN dotnet restore

COPY . .
WORKDIR /src/KimLienCustomerView
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KimLienCustomerView.dll"]