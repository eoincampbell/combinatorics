@echo off
devenv ..\Combinatorics.sln /Clean
devenv ..\Combinatorics.sln /Build Debug

if exist lib rmdir /S /Q lib

mkdir lib
mkdir lib\net20
mkdir lib\net40
mkdir lib\sl40

copy "..\Combinatorics.Net20\bin\*.dll" "lib\net20\"
copy "..\Combinatorics.Net20\bin\*.pdb" "lib\net20\"
copy "..\Combinatorics.Net20\bin\*.xml" "lib\net20\"

copy "..\Combinatorics.Net40\bin\*.dll" "lib\net40\"
copy "..\Combinatorics.Net40\bin\*.pdb" "lib\net40\"
copy "..\Combinatorics.Net40\bin\*.xml" "lib\net40\"

copy "..\Combinatorics.Net40\bin\*.dll" "lib\sl40\"
copy "..\Combinatorics.Net40\bin\*.pdb" "lib\sl40\"
copy "..\Combinatorics.Net40\bin\*.xml" "lib\sl40\"