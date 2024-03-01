## Requirements

* .NET 7 SDK
* Docker
* Docker Compose
* Python 3


## Installation
For installation of application:
1. Download application:
```bash
git clone https://github.com/DevilSmith/WeatherArchiveApplication
cd WeatherArchiveApplication
```
2. Change the architecture in the file for a compatible machine in `./DeployScripts/deploy_project.py`:

```python
DOTNET_PLATFORM = DotnetPlatforms.LINUX_ARM64.value # OR DotnetPlatforms.LINUX_AMD64.value
DOCKER_PLATFORM = DockerPlatforms.LINUX_ARM64.value # OR DotnetPlatforms.LINUX_AMD64.value
```
3. Run deployment process in `DeployScripts/`:
```bash
python3 deploy_project.py 
```
or
```bash
python deploy_project.py 
```
4. Start docker-compose (docker-compose.yml should be generated after the 3rd paragraph of the instruction by the directory above):
```bash
docker-compose up -d --force-recreate
```
5. To access the service, enter the URL in the browser: `http://localhost:5001`
