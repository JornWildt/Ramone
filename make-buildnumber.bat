c:\bin\sed --text "s/AssemblyFileVersion(\"\([0-9]*\)\.\([0-9]*\)\.\([0-9]*\)\.\(.*\)\")\]/AssemblyFileVersion(\"\1.\2.\3.%1\")]/g" Ramone\Properties\AssemblyInfo.cs > Ramone\Properties\AssemblyInfo-2.cs

del Ramone\Properties\AssemblyInfo.cs
ren Ramone\Properties\AssemblyInfo-2.cs AssemblyInfo.cs