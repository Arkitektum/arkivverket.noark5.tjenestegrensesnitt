@ECHO off

SET generator="C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\xsd.exe"
SET baseNamespace=arkivverket.noark5tj
SET xsdFilePath=.
set version=103

ECHO. && ECHO    Generating C# classes from XSD schema files
ECHO ------------------------------------------------- && ECHO.

:: Parameters: Output filename, namespace (after base namespace), schemafile [, second schemafile]
CALL :GenerateClass Arkivstruktur.cs, arkivstruktur, arkivstruktur%version%.xsd
CALL :GenerateClass Admin.cs, admin, admin%version%.xsd
CALL :GenerateClass LoggOgSporing.cs, loggogsporing, loggogsporing%version%.xsd
CALL :GenerateClass Metadata.cs, metadata, metadata%version%.xsd
CALL :GenerateClass MoeteOgUtvalgsbehandling.cs, moeteogutvalgsbehandling, moeteogutvalgsbehandling%version%.xsd

ECHO. && PAUSE

EXIT /B 0

:GenerateClass
	ECHO    Generating: %~1
	SET command=%generator% %xsdFilePath%\%~3
    IF NOT [%~4]==[] SET command=%command% %xsdFilePath%\%~4
	SET command=%command% /c /n:%baseNamespace%.%~2
	%command% >nul
    SET lastInputXsd=%~3
    IF NOT [%~4]==[] SET lastInputXsd=%~4
    SET outputFileName=%lastInputXsd:.xsd=%
    MOVE %outputFileName%.cs %~1 >nul
