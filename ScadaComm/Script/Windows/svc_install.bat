cd /d "%~dp0"
SET filepath="%cd%\ScadaCommWkr.exe"
sc create ScadaComm6 binPath= %filepath% start= auto
sc description ScadaComm6 "Rapid SCADA Communicator"
