cd Ramone\bin\debug
copy Ramone.dll Ramone.Core.dll 
"C:\Program Files\Microsoft\ILMerge\ilmerge.exe" /target:library /out:Ramone.dll Ramone.Core.dll JsonFx.dll HtmlAgilityPack.dll CuttingEdge.Conditions.dll /targetplatform:v4
xcopy Ramone*.dll ..\..\..\Binaries\ /S /I /R /Y
xcopy Ramone*.pdb ..\..\..\Binaries\ /S /I /R /Y
rem xcopy Ramone*.xml ..\..\..\Binaries\ /S /I /R /Y

cd ..\..\..

copy Documentation\Source\Ramone.pdf Documentation\Ramone.pdf 