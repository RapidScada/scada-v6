cd /d "%~dp0"
type nul > cmd\webreload
curl http://localhost:10008/ConfigReload
del cmd\webreload

:: Alternative
:: "%windir%\system32\inetsrv\appcmd" recycle APPPOOL "ScadaAppPool"
