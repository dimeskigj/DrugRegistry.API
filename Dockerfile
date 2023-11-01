FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app
COPY ./DrugRegistry.API . 
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
COPY . .
COPY --from=build /app/out /app/out
EXPOSE 80
ENTRYPOINT ["dotnet", "/app/out/DrugRegistry.API.dll"]
