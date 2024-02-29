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
