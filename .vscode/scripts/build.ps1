param([string]$t = 'build', [string]$c = 'Release',
      [string]$p = '/property:GenerateFullPaths=true', [string]$o = '/consoleloggerparameters:Summary')

dotnet $t .\ScadaCommon\ScadaCommon.sln -c $c $p $o
dotnet $t .\ScadaAgent\ScadaAgent\ScadaAgent.sln -c $c $p $o
dotnet $t .\ScadaComm\ScadaComm\ScadaComm.sln -c $c $p $o
dotnet $t .\ScadaWeb\ScadaWeb\ScadaWeb.sln -c $c $p $o
dotnet $t .\ScadaAdmin\ScadaAdmin\ScadaAdmin.sln -c $c $p $o
dotnet $t .\ScadaReport\ScadaReport.sln -c $c $p $o
dotnet $t .\ScadaComm\OpenDrivers\OpenDrivers.sln -c $c $p $o
dotnet $t .\ScadaComm\OpenDrivers2\OpenDrivers2.sln -c $c $p $o
dotnet $t .\ScadaServer\OpenModules\OpenModules.sln -c $c $p $o
dotnet $t .\ScadaWeb\OpenPlugins\OpenPlugins.sln -c $c $o
dotnet $t .\ScadaAdmin\OpenExtensions\OpenExtensions.sln -c $c $p $o
