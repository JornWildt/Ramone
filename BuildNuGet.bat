REM 1) Update versions in Ramone AssemblyInfo.cs.
REM 2) Select "Release" mode for compilation.
REM 3) Update release note in Ramone.nuspec.
REM 4) Compile
REM 5) Run this script

\bin\nuget.exe pack Ramone -Prop Configuration=Release
