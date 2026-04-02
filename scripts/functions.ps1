function WaitForDatabaseConnection {
    param (
        [Parameter(Mandatory)]
        [string] $ConnectionString
    )

    $Attempts = 0;
    $Instance = $null
    do {
        try {
            $Attempts++
            $Instance = Connect-DbaInstance -ConnectionString $ConnectionString
        } catch {
            Start-Sleep -Seconds 2
        }
    } while (-not $Instance -and $Attempts -lt 20)

    if (-not $Instance) {
        Write-Error "Failed to connect to database after $Attempts attempts."
        exit 1
    }
}

function Get-ConnectionString {
    param (
        [Parameter(Mandatory)]
        [string] $DockerContainerPort,

        [Parameter(Mandatory)]
        [string] $DatabasePasswordSa
    )

    return "Data Source=TCP:localhost,$($DockerContainerPort);User ID=sa;Password=$($DatabasePasswordSa);Connect Timeout=30;"
}

function Ensure-CorrectAzureModuleInstalled {
    if(-not (Get-Module -Name Az -ListAvailable)) {
        if(Get-Module -Name AzureRM -ListAvailable) {
            Write-Error "AzureRM module is installed. You must uninstall all packages related to AzureRM to proceed."
            exit 1
        }
        else {
            Write-Output "Installing required Az module..."
            Install-Module -Name Az -Repository PSGallery -Force
            if(Get-Module -Name Az -ListAvailable) {
                Write-Output "Az module is installed. Ready to go!"
            }
        }
    }
}


function Ensure-DbaToolsModuleInstalled {
    if (-not (Get-Module -Name dbatools -ListAvailable)) {
        Write-Host "Downloading dbatools" -ForeGround Yellow
        Install-Module dbatools -Scope CurrentUser -RequiredVersion 2.1.24 -Confirm:$False -Force
    }
}
