# FileSort
*Sortiert alle Dateien*

# Windows:

1. Platziere den All ordner auf die Festplatte (D:)
2. Win + R 
3. Gebe C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Startup ein
4. Gehe zu "FileSort\FileSort\bin\Debug\FileSort.exe"
5. Ziehe die .exe in den Autostart Ordner

# Linux:

*Note: It is recommended to run the configurator as root*
1. Download the release/Linux directory
2. Go to the downloaded folder (cd /home/(your user)/Downloads/Linux/Sortingsystem)
3. Run "sudo ./Configurator" inside your console
4. If you get an error delete the "SaveLoc" - file and try step 3 again
5. Now move the directory to the place where you want to have the system, I recommend /opt/ so you dont have to edit the .service file. Make sure the name is SortingSystem and it contains all files
6. Setting up the daemon
    - Copy the sortingsystem.service file in /scripts/ to /etc/systemd/system/
    - Next: edit the sortingsystem.service if you need, you have to do it on your own
    - Type "sudo systemctl start sortingsystem"
8. Optional: Add "alias filesort='cd /opt/SortingSystem; sudo /opt/SortingSystem/Configurator'" to your .bash_aliases file



