. ./settings.ps1

# Check if the container exists, supress any output
docker container inspect $DockerContainer 2>&1>$null

# If the container exists, stop and remove it
if ($LastExitCode -eq 0) {
    Write-Host "Stopping and removing container $DockerContainer" -ForeGround Yellow
    docker rm $DockerContainer --force
}
