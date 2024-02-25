from enum import Enum
import os
import subprocess
from FileTemplates.dockerfile_templates import *
from FileTemplates.docker_compose_templates import *


class DockerPlatforms(Enum):
    LINUX_ARM64 = "linux/arm64/v8"
    LINUX_AMD64 = "linux/amd64"


class DotnetPlatforms(Enum):
    LINUX_ARM64 = "linux-musl-arm64"
    LINUX_AMD64 = "linux-musl-x64"


DOTNET_PLATFORM = DotnetPlatforms.LINUX_ARM64.value
DOCKER_PLATFORM = DockerPlatforms.LINUX_ARM64.value

PGADMIN_CONTAINER_NAME = "weather_archive_app_pgadmin"
POSTGRES_CONTAINER_NAME = "weather_archive_app_postgres"
WEATHER_ARCHIVE_APP_NAME = "weather_archive_app"
WEATHER_ARCHIVE_APP_DOCKER_PLATFORM = DOCKER_PLATFORM

# Setup WEATHER_ARCHIVE_APP_DOTNET_PLATFORM envrionment variable:
os.environ.setdefault("WEATHER_ARCHIVE_APP_DOTNET_PLATFORM", DOTNET_PLATFORM)

# Generating dockerfile for ASP NET MVC Application:
mvc_app_dockerfile_string = get_mvc_app_dockerfile_string(
    DOCKER_PLATFORM, DOTNET_PLATFORM
)
mvc_app_dockerfile = open(
    f"../WeatherArchiveApplication/WeatherArchiveApplication.dockerfile", "w"
)
mvc_app_dockerfile.write(mvc_app_dockerfile_string)
mvc_app_dockerfile.close()

docker_compose_string_localhost = get_docker_compose_localhost(
    PGADMIN_CONTAINER_NAME,
    POSTGRES_CONTAINER_NAME,
    WEATHER_ARCHIVE_APP_NAME,
    WEATHER_ARCHIVE_APP_DOCKER_PLATFORM,
)

# Building shell command ASP NET MVC Application:
try:
    subprocess.run(
        "dotnet restore ../WeatherArchiveApplication/src", check=True, shell=True
    )
except subprocess.CalledProcessError as ex:
    print(f"ERROR DOTNET RESTORE MVC APPLICATION: {ex.returncode}")

try:
    subprocess.run(
        "dotnet clean ../WeatherArchiveApplication/src", check=True, shell=True
    )
except subprocess.CalledProcessError as ex:
    print(f"ERROR DOTNET CLEAN MVC APPLICATION: {ex.returncode}")

try:
    subprocess.run(
        "dotnet publish ../WeatherArchiveApplication/src", check=True, shell=True
    )
except subprocess.CalledProcessError as ex:
    print(f"ERROR DOTNET PUBLISH MVC APPLICATION: {ex.returncode}")

# Building docker image for ASP NET MVC Application:
try:
    subprocess.run(
        f"docker build -t weather_archive_app -f ../WeatherArchiveApplication/WeatherArchiveApplication.dockerfile ../WeatherArchiveApplication/",
        check=True,
        shell=True,
    )
except subprocess.CalledProcessError as ex:
    print(f"ERROR DOCKER BUILD MVC: {ex.returncode}")

docker_compose = open("../docker-compose.yml", "w")
docker_compose.write(docker_compose_string_localhost)
docker_compose.close()
