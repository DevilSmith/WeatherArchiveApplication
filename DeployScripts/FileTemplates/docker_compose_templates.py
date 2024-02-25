# Generating docker-compose.yml file for ASP NET MVC Application:
def get_docker_compose_localhost(
    PGADMIN_CONTAINER_NAME,
    POSTGRES_CONTAINER_NAME,
    WEATHER_ARCHIVE_APP_NAME,
    WEATHER_ARCHIVE_APP_DOCKER_PLATFORM,
):
    return f"""
version: "3.5"

services:
  postgres:
    container_name: {POSTGRES_CONTAINER_NAME}
    image: postgres:15
    environment:
      POSTGRES_USER: ${{POSTGRES_USER:-postgres}}
      POSTGRES_PASSWORD: ${{POSTGRES_PASSWORD:-123456789}}
      PGDATA: /data/postgres
    volumes:
      - postgres:/data/postgres
    ports:
      - "5432:5432"
    networks:
      - weather_archive_app
    hostname: {POSTGRES_CONTAINER_NAME}
    restart: unless-stopped
    platform: {WEATHER_ARCHIVE_APP_DOCKER_PLATFORM}

  pgadmin:
    container_name: {PGADMIN_CONTAINER_NAME}
    image: dpage/pgadmin4:6.15
    environment:
      PGADMIN_DEFAULT_EMAIL: ${{PGADMIN_DEFAULT_EMAIL:-pgadmin4@pgadmin.org}}
      PGADMIN_DEFAULT_PASSWORD: ${{PGADMIN_DEFAULT_PASSWORD:-admin}}
      PGADMIN_CONFIG_SERVER_MODE: "False"
    volumes:
      - ./app_data/pgadmin:/var/lib/pgadmin/
    ports:
      - "${{PGADMIN_PORT:-5050}}:80"
    networks:
      - weather_archive_app
    hostname: {PGADMIN_CONTAINER_NAME}
    restart: unless-stopped
    platform: {WEATHER_ARCHIVE_APP_DOCKER_PLATFORM}

  weather_archive_app:
    container_name: {WEATHER_ARCHIVE_APP_NAME}
    image: {WEATHER_ARCHIVE_APP_NAME}
    ports:
      - "5001:5001"
    networks:
      - weather_archive_app
    hostname: weather_archive_app
    restart: unless-stopped

networks:
  weather_archive_app:
    driver: bridge

volumes:
  postgres:
  pgadmin:
"""
