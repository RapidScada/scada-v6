cd /d "%~dp0"
SET filepath="%cd%\ScadaAgentWkr.exe"
sc create ScadaAgent6 binPath= %filepath% start= auto
sc description ScadaAgent6 "Rapid SCADA Agent"
