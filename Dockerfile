FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

COPY . .

WORKDIR /app/DbContextTest
RUN dotnet restore
RUN dotnet build

ENTRYPOINT ["dotnet", "test"]