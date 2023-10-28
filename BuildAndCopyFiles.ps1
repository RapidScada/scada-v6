#dotnet build ScadaCommon/ScadaCommon.sln -c Release
#dotnet build ScadaAgent/ScadaAgent/ScadaAgent.sln -c Release
#dotnet build ScadaComm/ScadaComm/ScadaComm.sln -c Release
#dotnet build ScadaServer/ScadaServer/ScadaServer.sln -c Release
#dotnet build ScadaWeb/ScadaWeb/ScadaWeb.sln -c Release
#dotnet build ScadaAdmin/ScadaAdmin/ScadaAdmin.sln -c Release
#dotnet build ScadaReport/ScadaReport.sln -c Release
#dotnet build ScadaComm/OpenDrivers/OpenDrivers.sln -c Release
#dotnet build ScadaComm/OpenDrivers2/OpenDrivers2.sln -c Release
#dotnet build ScadaServer/OpenModules/OpenModules.sln -c Release
#dotnet build ScadaWeb/OpenPlugins/OpenPlugins.sln -c Release
#dotnet build ScadaAdmin/OpenExtensions/OpenExtensions.sln -c Release

$sourceRootPath = "./"
$destRootPath = "./SCADA/"

#BaseUserExtDAT
$baseUserExtDatDestPath = $destRootPath + "BaseUserExtDAT/"
if (!(Test-Path -Path  $baseUserExtDatDestPath)) {
    New-Item $baseUserExtDatDestPath -ItemType Directory
}

#ScadaAdmin
$adminPath = $sourceRootPath +"ScadaAdmin/OpenExtensions/ExtWebConfig/bin/Release/net6.0-windows/"
$adminDestPath = $destRootPath + "ScadaAdmin/"
$adminLibDestPath = $destRootPath + "ScadaAdmin/Lib/"
$adminLangDestPath = $destRootPath + "ScadaAdmin/Lang/"
if (!(Test-Path -Path  $adminLibDestPath)) {
    New-Item $adminLibDestPath -ItemType Directory
}
if (!(Test-Path -Path  $adminLangDestPath)) {
    New-Item $adminLangDestPath -ItemType Directory
}
Copy-item -Force -Recurse -Verbose ($adminPath + "ExtWebConfig.dll") -Destination $adminLibDestPath
Copy-item -Force -Recurse -Verbose ($adminPath + "Lang/ExtWebConfig.en-GB.xml") -Destination $adminLangDestPath


#ScadaAgent
$agentPath = $sourceRootPath +"ScadaAgent/ScadaAgent/ScadaAgentEngine/bin/Release/netstandard2.0/"
$agentDestPath = $destRootPath + "ScadaAgent/"
if (!(Test-Path -Path  $agentDestPath)) {
    New-Item $agentDestPath -ItemType Directory
}
Copy-item -Force -Recurse -Verbose ($agentPath + "ScadaAgentEngine.dll") -Destination $agentDestPath

#ScadaServer
$serverPath = $sourceRootPath +"ScadaServer/ScadaServer/ScadaServerWkr/bin/Release/net6.0/"
$serverDestPath = $destRootPath + "ScadaServer/"
if (!(Test-Path -Path  $serverDestPath)) {
    New-Item $serverDestPath -ItemType Directory
}
Copy-item -Force -Recurse -Verbose ($serverPath + "ScadaServerCommon.dll") -Destination $serverDestPath
Copy-item -Force -Recurse -Verbose ($serverPath + "ScadaServerEngine.dll") -Destination $serverDestPath
Copy-item -Force -Recurse -Path ($serverPath + "Lang") -Destination $serverDestPath

#ScadaWeb
$webPath = $sourceRootPath +"ScadaWeb/ScadaWeb/ScadaWeb/bin/Release/net6.0/"
$webCommonPath = $sourceRootPath +"ScadaWeb/ScadaWeb/ScadaWebCommon/"
$webCommonSubsetPath = $sourceRootPath +"ScadaWeb/ScadaWeb/ScadaWebCommon.Subset/bin/Release/net6.0/"
$webPlgSchemePath = $sourceRootPath + "ScadaWeb/OpenPlugins/PlgScheme/"
$webDestPath = $destRootPath + "ScadaWeb/"
$webDestLangPath = $destRootPath + "ScadaWeb/lang"
$webStaticPath =$sourceRootPath + "ScadaWeb/ScadaWeb/ScadaWeb/wwwroot/"
$webStaticDestPath = $destRootPath + "ScadaWeb/wwwroot"
$webStaticDestLibPath = $destRootPath + "ScadaWeb/wwwroot/lib"
$webStaticPagesDestPath = $destRootPath + "ScadaWeb/wwwroot/js/pages"
$webStaticPagesCssDestPath = $destRootPath + "ScadaWeb/wwwroot/css/pages"
$webStaticPagesImgDestPath = $destRootPath + "ScadaWeb/wwwroot/images"
$webPlgSchemeJsDestPath = $destRootPath + "ScadaWeb/wwwroot/plugins/Scheme/js"
$webPlgSchemeCssDestPath = $destRootPath + "ScadaWeb/wwwroot/plugins/Scheme/css"
if (!(Test-Path -Path  $webStaticDestLibPath)) {
    New-Item $webStaticDestLibPath -ItemType Directory
}
if (!(Test-Path -Path  $webStaticPagesDestPath)) {
    New-Item $webStaticPagesDestPath -ItemType Directory
}
if (!(Test-Path -Path  $webStaticPagesCssDestPath)) {
    New-Item $webStaticPagesCssDestPath -ItemType Directory
}
if (!(Test-Path -Path  $webStaticPagesImgDestPath)) {
    New-Item $webStaticPagesImgDestPath -ItemType Directory
}
if (!(Test-Path -Path  $webDestLangPath)) {
    New-Item $webDestLangPath -ItemType Directory
}
if (!(Test-Path -Path  $webPlgSchemeJsDestPath)) {
    New-Item $webPlgSchemeJsDestPath -ItemType Directory
}
if (!(Test-Path -Path  $webPlgSchemeCssDestPath)) {
    New-Item $webPlgSchemeCssDestPath -ItemType Directory
}
Copy-item -Force -Recurse -Verbose ($webPath + "ScadaWeb.dll") -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($webPath + "ScadaWeb.exe") -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($webPath + "ScadaWebCommon.dll") -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($webPath + "ScadaWeb.deps.json") -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($webPlgSchemePath + "bin/Release/net6.0/PlgScheme.dll") -Destination $webDestPath

Copy-item -Force -Recurse -Verbose ($webPath + "Microsoft.AspNetCore.Authentication.Google.dll") -Destination $webDestPath
#Copy-item -Force -Recurse -Verbose ($webPath + "Microsoft.Bcl.AsyncInterfaces.dll") -Destination $webDestPath
#Copy-item -Force -Recurse -Verbose ($webPath + "Microsoft.Bcl.HashCode.dll") -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($webPath + "Microsoft.IdentityModel.Abstractions.dll") -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($webPath + "Microsoft.IdentityModel.JsonWebTokens.dll") -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($webPath + "Microsoft.IdentityModel.Logging.dll") -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($webPath + "Microsoft.IdentityModel.Tokens.dll") -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($webPath + "System.IdentityModel.Tokens.Jwt.dll") -Destination $webDestPath

Copy-item -Force -Recurse -Path ($webStaticPath + "favicon.ico") -Destination $webStaticDestPath

Copy-item -Force -Recurse -Path ($webStaticPath + "js/pages/view.js") -Destination $webStaticPagesDestPath
Copy-item -Force -Recurse -Path ($webStaticPath + "js/pages/cnl-select.js") -Destination $webStaticPagesDestPath

Copy-item -Force -Recurse -Path ($webPlgSchemePath + "wwwroot/plugins/Scheme/js/scheme-view.js") -Destination $webPlgSchemeJsDestPath
Copy-item -Force -Recurse -Path ($webPlgSchemePath + "wwwroot/plugins/Scheme/css/scheme-view.min.css") -Destination $webPlgSchemeCssDestPath


Copy-item -Force -Recurse -Path ($webStaticPath + "css/pages/login.min.css") -Destination $webStaticPagesCssDestPath
Copy-item -Force -Recurse -Path ($webStaticPath + "images/gear.png") -Destination $webStaticPagesImgDestPath
Copy-item -Force -Recurse -Path ($webStaticPath + "images/bg.png") -Destination $webStaticPagesImgDestPath

Copy-item -Force -Recurse -Verbose ($webStaticPath + "lib/vue.min.js") -Destination $webStaticDestLibPath
Copy-item -Force -Recurse -Verbose ($webStaticPath + "lib/jquery.md5.min.js") -Destination $webStaticDestLibPath
Copy-item -Force -Recurse -Path ($webStaticPath + "lib/layer") -Destination $webStaticDestLibPath
Copy-item -Force -Recurse -Path ($webStaticPath + "lib/layui") -Destination $webStaticDestLibPath
Copy-item -Force -Recurse -Path ($webStaticPath + "lib/jquery-qrcode") -Destination $webStaticDestLibPath
Copy-item -Force -Recurse -Path ($webStaticPath + "lib/fingerprint") -Destination $webStaticDestLibPath

Copy-item -Force -Recurse -Verbose ($webCommonPath + "Lang/ScadaWeb.en-GB.xml") -Destination $webDestLangPath
#
Copy-item -Force -Recurse -Verbose ($webCommonSubsetPath + "ScadaWebCommon.Subset.dll") -Destination $adminDestPath


#ScadaCommon
$commonDllFile = $sourceRootPath +"ScadaCommon/ScadaCommon/bin/Release/netstandard2.0/ScadaCommon.dll"
$commonLangFile = $sourceRootPath +"ScadaCommon/ScadaCommon/Lang/ScadaCommon.en-GB.xml"
$fileStorageDllFile = $sourceRootPath +"ScadaCommon/FileStorage/bin/Release/netstandard2.0/FileStorage.dll"
$postgresStorageDllFile = $sourceRootPath +"ScadaCommon/PostgreSqlStorage/bin/Release/netstandard2.0/PostgreSqlStorage.dll"


Copy-item -Force -Recurse -Verbose ($commonDllFile) -Destination $adminDestPath
Copy-item -Force -Recurse -Verbose ($commonDllFile) -Destination $agentDestPath
Copy-item -Force -Recurse -Verbose ($commonDllFile) -Destination $serverDestPath
Copy-item -Force -Recurse -Verbose ($commonDllFile) -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($fileStorageDllFile) -Destination $serverDestPath
Copy-item -Force -Recurse -Verbose ($fileStorageDllFile) -Destination $webDestPath
Copy-item -Force -Recurse -Verbose ($postgresStorageDllFile) -Destination $serverDestPath
Copy-item -Force -Recurse -Verbose ($postgresStorageDllFile) -Destination $webDestPath

Copy-item -Force -Recurse -Verbose $commonLangFile -Destination $webDestLangPath

