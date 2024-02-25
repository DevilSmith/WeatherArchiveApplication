# Generating dockerfile for ASP NET MVC Application:
def get_mvc_app_dockerfile_string(DOCKER_PLATFORM, DOTNET_PLATFORM):
    return f"""
FROM --platform={DOCKER_PLATFORM} mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.19
WORKDIR /App
COPY /src/bin/Debug/net7.0/{DOTNET_PLATFORM}/publish /App/
ENTRYPOINT ["./WeatherArchiveApp"]
"""
