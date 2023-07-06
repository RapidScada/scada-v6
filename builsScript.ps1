Write-Host "Starting build process..."

$solutions = @(
    "ScadaCommon/ScadaCommon.sln",
    "ScadaAgent/ScadaAgent/ScadaAgent.sln",
    "ScadaComm/ScadaComm/ScadaComm.sln",
    "ScadaServer/ScadaServer/ScadaServer.sln",
    "ScadaWeb/ScadaWeb/ScadaWeb.sln",
    "ScadaAdmin/ScadaAdmin/ScadaAdmin.sln",
    "ScadaReport/ScadaReport.sln",
    "ScadaComm/OpenDrivers/OpenDrivers.sln",
    "ScadaComm/OpenDrivers2/OpenDrivers2.sln",
    "ScadaServer/OpenModules/OpenModules.sln",
    "ScadaWeb/OpenPlugins/OpenPlugins.sln",
    "ScadaAdmin/OpenExtensions/OpenExtensions.sln"
)

foreach ($solution in $solutions) {
    Write-Host "Building $solution..."
    dotnet build $solution -c Release
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Build failed for $solution" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}

Write-Host "Build process completed successfully!" -ForegroundColor Green
