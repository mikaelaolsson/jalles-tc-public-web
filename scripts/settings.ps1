
# Azure settings
$SubscriptionId = 'd3f794fc-adf5-4e94-a31c-6cd7bb767fbe'
$TenantId = '0924893b-b3b6-43f7-9f3e-55846973daab'
$ResourceGroupName = 'rg-jalles-tc'
$AzureSqlServerName = 'jalles-tc-public-web-server'
$AzureSqlServerAdminUser = 'jalles-tc-public-web-server-admin'
$AzureDatabaseName = 'jalles-tc-public-web-database'
$StorageAccountName = 'jallestcstorage'
$StorageKeytype = 'StorageAccessKey'
$StorageAccountContainerName = 'backups'

# Docker settings
$DockerContainer = 'jalles-web-local'
$DockerContainerPort = 1435
$DockerImage = 'mcr.microsoft.com/mssql/server:2022-latest'
$DatabasePasswordSa = 'YourStrong!Passw0rd'
$DatabaseUserForLogin = 'sa'
$DatabasePasswordForLogin = $DatabasePasswordSa
$DatabaseInstance = 'localhost'
$DatabaseName = $AzureDatabaseName
$ContainedDatabaseAuthentication = $true

# General settings
$BacpacFolderName = 'db-snapshots'

# Calculated settings
$StorageAccount = "https://$($StorageAccountName).blob.core.windows.net"
