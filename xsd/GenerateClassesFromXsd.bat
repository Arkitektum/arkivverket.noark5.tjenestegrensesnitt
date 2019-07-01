@echo off
echo "============================ GENERATE C# classes ============================"

REM NOTE! last filename must have .\ to reset output filename. The filename is getting to long....
set xsdfiles=vsm.xsd vsm\pptsystem.xsd vsm\nikita-felter.xsd rest.xsd admin.xsd loggingogsporing.xsd metadata.xsd sakarkiv.xsd  MatrikkelFelles.xsd PlanFelles.xsd Geometri.xsd Kodeliste.xsd .\arkivstruktur.xsd

"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe" /nologo %xsdfiles% /c /n:arkivverket.noark5tj.models

move /y arkivstruktur.cs ..\arkivverket.noark5.tjenestegrensesnitt.eksempel\Models\Noark5tj.cs

pause
