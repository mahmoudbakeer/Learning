# Build Stage 
FROM mcr.Microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source
COPY PaymentServiceApi/*.csproj PaymentServiceApi/
RUN dotnet restore PaymentServiceApi/PaymentServiceApi.csproj
COPY PaymentServiceApi/. PaymentServiceApi/
RUN dotnet publish PaymentServiceApi/PaymentServiceApi.csproj -c release -o /app --no-restore

# Final Stage
FROM mcr.Microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT [ "dotnet" , "PaymentServiceApi.dll" ]
