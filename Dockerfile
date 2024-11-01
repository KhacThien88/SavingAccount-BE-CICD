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

EXPOSE 81

ENTRYPOINT ["dotnet", "SavingAccount_BE.dll"]
