cd /d "%~dp0"
type nul > cmd\webreload
curl http://localhost:8080/ConfigReload
del cmd\webreload
