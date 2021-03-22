FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OctopusToSlackDotNet.csproj", "./"]
RUN dotnet restore "OctopusToSlackDotNet.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "OctopusToSlackDotNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OctopusToSlackDotNet.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OctopusToSlackDotNet.dll"]
