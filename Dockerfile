FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["APIGEO.csproj", "./"]
RUN dotnet restore "APIGEO.csproj"
COPY . .
RUN dotnet build "APIGEO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "APIGEO.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIGEO.dll"]
