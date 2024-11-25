FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MdOutput/MdOutput.csproj", "MdOutput/"]
RUN dotnet restore "MdOutput/MdOutput.csproj"
COPY . .
WORKDIR "/src/MdOutput"
RUN dotnet build "MdOutput.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MdOutput.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MdOutput.dll"]
