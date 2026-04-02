. ./settings.ps1
. ./functions.ps1

$isAlreadyRunning = docker ps -a | Select-String -Pattern $DockerContainer
if ($isAlreadyRunning) {
    Write-Host "Container $DockerContainer already exists" -ForeGround Yellow
    Write-Host "Removing container $DockerContainer" -ForeGround Yellow
    docker rm $DockerContainer --force
}

Write-Host "Starting docker container $DockerContainer" -ForeGround Yellow
docker run --cap-add SYS_PTRACE -e "ACCEPT_EULA=1" -e "MSSQL_SA_PASSWORD=$($DatabasePasswordSa)" -p "$($DockerContainerPort):1433" --name $DockerContainer -d $DockerImage 1> $null

if($LastExitCode -ne 0) {
    Write-Error "Failed to start docker container $DockerContainer"
    exit 1
}

$ConnectionString = Get-ConnectionString -DockerContainerPort $DockerContainerPort -DatabasePasswordSa $DatabasePasswordSa
WaitForDatabaseConnection($ConnectionString)
