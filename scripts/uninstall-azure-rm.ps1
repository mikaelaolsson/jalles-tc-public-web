# It can be a bit tricky to get rid if all the AzureRM modules but these commands should do the trick :)

Uninstall-Module Azure
Uninstall-Module AzureRM

if (Get-Module -ListAvailable | Where {$_.Name -like 'AzureRM.*'}){
    $Modules = Get-Module -ListAvailable | Where {$_.Name -like 'AzureRM.*'}
    Foreach ($Module in $Modules) {Uninstall-Module $Module}
}
