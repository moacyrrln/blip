# Use the official ASP.NET Core runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official ASP.NET Core SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copie todos os arquivos do diretório de trabalho atual para dentro do container
COPY . ./src

# Execute o comando dotnet restore para restaurar as dependências
RUN dotnet restore "src/GitHubRepoApi.csproj"

# Continuar com as etapas de construção
WORKDIR /src
RUN dotnet build "src/GitHubRepoApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/GitHubRepoApi.csproj" -c Release -o /app/publish

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GitHubRepoApi.dll"]
