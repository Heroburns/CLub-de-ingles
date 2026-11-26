#!/bin/bash
# Starts SQL Server for ClubIngles in Docker (local development only)
if docker ps -a --format '{{.Names}}' | grep -q '^sqlclub$'; then
  docker start sqlclub
else
  docker run --platform linux/amd64 -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=ClubIDbjpAWRO1ca9!" \
    -p 1433:1433 --name sqlclub -d mcr.microsoft.com/mssql/server:2022-latest
fi
