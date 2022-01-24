SET filepath="%cd%\ScadaServerWkr.exe"
sc create ScadaServer6 binPath= %filepath% start= auto
sc description ScadaServer6 "Rapid SCADA Server"
