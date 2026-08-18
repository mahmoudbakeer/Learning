# Build Stage 
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source
COPY OrderServiceApi/*.csproj OrderServiceApi/
RUN dotnet restore OrderServiceApi/OrderServiceApi.csproj
COPY OrderServiceApi/. OrderServiceApi/
RUN dotnet publish OrderServiceApi/OrderServiceApi.csproj -c release -o /app --no-restore
# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "OrderServiceApi.dll"]