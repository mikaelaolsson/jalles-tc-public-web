param (
    [Parameter(Mandatory)] [String] $AzureSqlServerAdminPassword
)

. ./settings.ps1

$ErrorActionPreference = "Stop"

Ensure-CorrectAzureModuleInstalled

Update-AzConfig -DefaultSubscriptionForLogin $SubscriptionId -Scope Process

$azureContext = Get-AzContext

if(!$azureContext -or $azureContext.Tenant.Id -ne $TenantId) {
    Connect-AzAccount -Tenant $TenantId
}

$StorageKey = (Get-AzStorageAccountKey -Name $StorageAccountName -ResourceGroupName $ResourceGroupName 3>$null).Value[0]
$StorageContext = New-AzStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $StorageKey

$BacpacName = "$($AzureDatabaseName)_$(Get-Date -Format "yyyyMMdd_HHmmss").bacpac"
$BacpacUri = "$StorageAccount/$StorageAccountContainerName/$BacpacName"

$SecureSqlServerAdminPassword = ConvertTo-SecureString -String $AzureSqlServerAdminPassword -AsPlainText -Force

$symbols = @("⣾⣿", "⣽⣿", "⣻⣿", "⢿⣿", "⡿⣿", "⣟⣿", "⣯⣿", "⣷⣿",
                "⣿⣾", "⣿⣽", "⣿⣻", "⣿⢿", "⣿⡿", "⣿⣟", "⣿⣯", "⣿⣷")
$symbolNum = 0;

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$exportStatus
$lastStatusCheck = (Get-Date).AddMinutes(-5)

$exportRequest = New-AzSqlDatabaseExport -ResourceGroupName $ResourceGroupName -ServerName $AzureSqlServerName -DatabaseName $AzureDatabaseName -StorageKeytype $StorageKeytype -StorageKey $StorageKey -StorageUri $BacpacUri -AdministratorLogin $AzureSqlServerAdminUser -AdministratorLoginPassword $SecureSqlServerAdminPassword

do
{
    if((Get-Date) - $lastStatusCheck -gt (New-TimeSpan -Seconds 30)) {
        $exportStatus = Get-AzSqlDatabaseImportExportStatus -OperationStatusLink $exportRequest.OperationStatusLink
    }

    $symbol =  $symbols[$symbolNum]
    Write-Host -NoNewline "`r$symbol Waiting for Azure to create backup for database $($AzureDatabaseName) (this might take a while)..." -ForegroundColor Yellow

    Start-Sleep -Milliseconds 100
    $symbolNum++
    if ($symbolNum -eq $symbols.Count){
        $symbolNum = 0;
    }
} while($exportStatus.status -eq "InProgress")

$stopwatch.Stop()

if($exportStatus.status -eq "Failed") {
    Write-Error [Environment]::NewLine "Database backup failed after $($stopwatch.Elapsed). Error message: $($exportStatus.errorMessage)"
    exit 1
}

Write-Output [Environment]::NewLine "Backup is now available at $BacpacUri. It took $($stopwatch.Elapsed)."

Write-Output "Downloading backup..."

$CurrentLocation  = Get-Location
$ParentLocation = (Get-Item $CurrentLocation ).parent
$BacpacPath = "$ParentLocation/$BacpacFolderName/"

New-Item -ItemType Directory -Force -Path $BacpacPath

$LocalBacpacPath = "$BacpacPath/$BacpacName"

try
{
    $BacpacUriWithSas = New-AzStorageBlobSASToken -Container $StorageAccountContainerName -Blob $BacpacName -Permission r -Context $StorageContext -FullUri -ExpiryTime (Get-Date).AddHours(1)

    Get-AzStorageBlobContent -Uri $BacpacUriWithSas -Destination $LocalBacpacPath -Force

    Write-Host "Downloaded backup to $LocalBacpacPath" -ForegroundColor Green
} catch {
    Write-Error "Failed to download backup. Exception: $($_.Exception.Message)"
}

Write-Output "Cleaning up..."

try {
    Remove-AzStorageBlob -Container $StorageAccountContainerName -Blob $BacpacName -Context $StorageContext
    Write-Host "Removed bacpac from storage account." -ForegroundColor Green
} catch {
    Write-Error "Failed to remove bacpac from storage account. Remove $BacpacName from container $StorageAccountContainerName manually."
}

Write-Host "Database download complete." -ForegroundColor Green
