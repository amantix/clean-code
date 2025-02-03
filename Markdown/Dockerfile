FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 41301
EXPOSE 44352

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Markdown/Markdown.csproj", "Markdown/"]
COPY ["Web/Web.csproj", "Web/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Data/Data.csproj", "Data/"]

RUN dotnet restore "Markdown/Markdown.csproj"
RUN dotnet restore "Web/Web.csproj"

COPY . .

WORKDIR "/src/Markdown"
RUN dotnet build "Markdown.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]