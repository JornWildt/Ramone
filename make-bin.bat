SET ProgFiles86Root=%ProgramFiles(x86)%
IF NOT "%ProgFiles86Root%"=="" GOTO win64
SET ProgFiles86Root=%ProgramFiles%
:win64

cd Ramone\bin\debug

"%ProgFiles86Root%\Microsoft\ILMerge\ilmerge.exe" /target:library /out:Ramone.dll Ramone.Core.dll JsonFx.dll HtmlAgilityPack.dll CuttingEdge.Conditions.dll /targetplatform:v4

xcopy Ramone*.dll ..\..\..\Binaries\ /S /I /R /Y
xcopy Ramone*.pdb ..\..\..\Binaries\ /S /I /R /Y

cd ..\..\..
