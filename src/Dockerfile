FROM mcr.microsoft.com/dotnet/sdk:5.0 AS sdk

COPY . ./Build
WORKDIR /Build
RUN [ "dotnet", "publish", "-f", "net5.0", "-c", "Release", "Web/DevHive.Web/DevHive.Web.csproj", "-o", "/Out"]

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
COPY --from=sdk /Out /App

WORKDIR /App
ENTRYPOINT [ "dotnet", "DevHive.Web.dll" ]
