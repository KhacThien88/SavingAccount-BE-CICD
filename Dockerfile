FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /build

COPY SavingAccount_BE.csproj ./

RUN dotnet restore

COPY . .

RUN dotnet tool install --global dotnet-ef

ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /out .
COPY wait-for-it.sh /app

RUN chmod +x /app/wait-for-it.sh

ENTRYPOINT ["/app/wait-for-it.sh", "sqlserver:1433", "--", "dotnet", "SavingAccount_BE.dll"]
