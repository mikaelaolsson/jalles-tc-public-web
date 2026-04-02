param (
    [Parameter(Mandatory = $true)] [String] $BacpacName
)

. ./settings.ps1
. ./functions.ps1

Ensure-DbaToolsModuleInstalled

Write-Host "Downloading sqlpackage" -ForeGround Yellow

$sqlPackageUrl = 'https://aka.ms/sqlpackage-windows'
$sqlPackageExec = 'sqlpackage.exe'

if($IsMacOS) {
    $sqlPackageUrl = 'https://aka.ms/sqlpackage-macos'
    $sqlPackageExec = 'sqlpackage'
} elseif($IsLinux) {
    $sqlPackageUrl = 'https://aka.ms/sqlpackage-linux'
    $sqlPackageExec = 'sqlpackage'
}

New-Item -ItemType Directory -Force -Path ./tools
Invoke-WebRequest $sqlPackageUrl -OutFile ./tools/sqlpackage.zip
Expand-Archive ./tools/sqlpackage.zip -DestinationPath ./tools/sqlpackage -Force

if($IsMacOS || $IsLinux) {
    & chmod +x ./tools/sqlpackage/sqlpackage
}

$ConnectionString = Get-ConnectionString -DockerContainerPort $DockerContainerPort -DatabasePasswordSa $DatabasePasswordSa
$Instance = Connect-DbaInstance -ConnectionString $ConnectionString

$DatabaseExists = Get-DbaDatabase -SqlInstance $Instance -Database $DatabaseName

if ($DatabaseExists) {
    Write-Host 'Removing current database' -ForeGround Yellow
    Remove-DbaDatabase -SqlInstance $Instance -Database $DatabaseName -Confirm:$false
}

$CurrentLocation  = Get-Location
$ParentLocation = (Get-Item $CurrentLocation ).parent
$BacpacPath = "$ParentLocation/$BacpacFolderName/$BacpacName"

Write-Host "Importing backup from $($BacpacPath)" -ForeGround Yellow

if($ContainedDatabaseAuthentication) {
    Set-DbaSpConfigure -SqlInstance $Instance -Name 'contained database authentication' -Value 1
}

& ./tools/sqlpackage/$sqlPackageExec /a:Import /tsn:"$($DatabaseInstance),$($DockerContainerPort)" /tdn:$DatabaseName /tu:sa /tp:$DatabasePasswordSa /TargetTrustServerCertificate:True /sf:$BacpacPath

$ConnectionString = Get-ConnectionString -DockerContainerPort $DockerContainerPort -DatabasePasswordSa $DatabasePasswordSa

# A fresh database instance is necessary after import
$Instance = Connect-DbaInstance -ConnectionString $ConnectionString

if(-not(Get-DbaLogin -SqlInstance $Instance -Login $DatabaseUserForLogin)) {
    Write-Host "`nCreating login $DatabaseUserForLogin" -ForeGround Yellow
    $SecurePasswordForLogin = ConvertTo-SecureString -String $DatabasePasswordForLogin -AsPlainText -Force
    New-DbaLogin -SqlInstance $Instance -Login $DatabaseUserForLogin -SecurePassword $SecurePasswordForLogin
}

Write-Host "`nMaking $DatabaseUserForLogin db_owner of $DatabaseName" -ForeGround Yellow

if($DatabaseUserForLogin -ne 'sa') {
    New-DbaDbUser -SqlInstance $Instance -Login $DatabaseUserForLogin -DefaultSchema 'dbo'
    Add-DbaDbRoleMember -SqlInstance $Instance -Database $DatabaseName -Role db_owner -User $DatabaseUserForLogin -confirm:$false
}


Write-Host "`nAll done. Get your connection string below and fire away!" -ForeGround Green
Write-Host "`n`"ConnectionString`":`"Server=$DatabaseInstance,$DockerContainerPort;Database=$DatabaseName;User Id=$DatabaseUserForLogin;Password=$DatabasePasswordForLogin;TrustServerCertificate=True;`""
