SET filepath="%cd%\ScadaServerWkr.exe"
sc create ScadaServer6 binPath= %filepath%
sc description ScadaServer6 "Rapid SCADA Server"
