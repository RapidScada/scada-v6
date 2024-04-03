The custom HTML pages demonstrate use of the Rapid SCADA JavaScript API.
The examples are compatible with the HelloWorld project.

How to deploy:
1. Copy the *.html files into the SCADA\ScadaWeb\wwwroot\custom\ directory.
2. Open the project in Administrator and add new records in the Views table:
+----------------------------+---------------+-----------------------------+
| Path                       | View Type     | Arguments                   |
+----------------------------+---------------+-----------------------------+
| Custom\Main API Example    | Web page view | /custom/MainApiExample.html |
+----------------------------+---------------+-----------------------------+
| Custom\Command API Example | Web page view | /custom/CmdApiExample.html  |
+----------------------------+---------------+-----------------------------+
3. Upload the configuration.
