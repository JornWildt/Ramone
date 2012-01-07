cd Ramone\bin\debug
copy Ramone.dll Ramone.Core.dll 
"C:\Program Files\Microsoft\ILMerge\ilmerge.exe" /target:library /out:Ramone.dll Ramone.Core.dll JsonFx.dll /targetplatform:v4
xcopy Ramone*.dll ..\..\..\Binaries\ /S /I /R /Y
