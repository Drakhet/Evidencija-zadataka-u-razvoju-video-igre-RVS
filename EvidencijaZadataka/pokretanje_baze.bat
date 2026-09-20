@echo off
echo Pokretanje SQL skripti...

cd /d "%~dp0"

sqlcmd -S localhost -E -i "SlojPodataka\BazaPodataka\BazaPodataka.sql"
if %errorlevel% neq 0 (
    echo GRESKA: BazaPodataka.sql nije uspesno pokrenuta.
    pause
    exit /b 1
)

sqlcmd -S localhost -E -i "SlojPodataka\BazaPodataka\StoredProcedure.sql"
if %errorlevel% neq 0 (
    echo GRESKA: StoredProcedure.sql nije uspesno pokrenuta.
    pause
    exit /b 1
)

echo Baza podataka je uspesno kreirana!
pause