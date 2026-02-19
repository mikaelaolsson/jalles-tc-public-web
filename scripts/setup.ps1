param (
    [Parameter(Mandatory)] [String] $AzureSqlServerAdminPassword,
    [parameter()] [switch] $Yes
)

. ./settings.ps1

Write-Host "This script will download a backup of `"$DatabaseName`" from `"$AzureSqlServerName`" in Azure, create a Docker container called `"$DockerContainer`" and restore data to a database image in that container. If the container exists it will be removed first." -ForegroundColor Green

if ($Yes) {
    $Confirmation = 'y'
} else {
    $Confirmation = Read-Host "Do you want to continue? (y/N)"
}

if ($Confirmation -eq 'y') {
    . ./teardown-docker-container.ps1
    . ./start-docker-container.ps1
    . ./download-database-dump.ps1 -AzureSqlServerAdminPassword $AzureSqlServerAdminPassword
    . ./restore-docker-database.ps1 -BacpacName $BacpacName
}
