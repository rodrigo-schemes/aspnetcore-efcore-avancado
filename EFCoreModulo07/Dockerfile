FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CursoEFCoreAvancadoModulo6/CursoEFCoreAvancadoModulo6.csproj", "CursoEFCoreAvancadoModulo6/"]
RUN dotnet restore "CursoEFCoreAvancadoModulo6/CursoEFCoreAvancadoModulo6.csproj"
COPY . .
WORKDIR "/src/CursoEFCoreAvancadoModulo6"
RUN dotnet build "CursoEFCoreAvancadoModulo6.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CursoEFCoreAvancadoModulo6.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CursoEFCoreAvancadoModulo6.dll"]
