FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /EmailEngine/src
COPY ["src/Sadin.EmailEngine.Api/Sadin.EmailEngine.Api.csproj", "Sadin.EmailEngine.Api/"]
COPY ["src/Sadin.EmailEngine.Application/Sadin.EmailEngine.Application.csproj", "Sadin.EmailEngine.Application/"]
COPY ["src/Sadin.EmailEngine.Domain/Sadin.EmailEngine.Domain.csproj", "Sadin.EmailEngine.Domain/"]
COPY ["src/Sadin.EmailEngine.Shared/Sadin.EmailEngine.Shared.csproj", "Sadin.EmailEngine.Shared/"]
COPY ["src/Sadin.EmailEngine.Infrastructure/Sadin.EmailEngine.Infrastructure.csproj", "Sadin.EmailEngine.Infrastructure/"]
COPY ["src/Sadin.EmailEngine.Persistence/Sadin.EmailEngine.Persistence.csproj", "Sadin.EmailEngine.Persistence/"]
COPY ["src/Sadin.EmailEngine.Presentation/Sadin.EmailEngine.Presentation.csproj", "Sadin.EmailEngine.Presentation/"]
RUN dotnet restore "/EmailEngine/src/Sadin.EmailEngine.Api/Sadin.EmailEngine.Api.csproj"
COPY ./src/ .
WORKDIR "/EmailEngine/src/Sadin.EmailEngine.Api"
RUN dotnet build "Sadin.EmailEngine.Api.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Sadin.EmailEngine.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sadin.EmailEngine.Api.dll"]
