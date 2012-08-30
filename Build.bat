@echo off
devenv Combinatorics.sln /Clean
devenv Combinatorics.sln /Build Debug

if exist Nuget\lib rmdir /S /Q Nuget\lib

mkdir Nuget\lib
mkdir Nuget\lib\net20
mkdir Nuget\lib\net40
mkdir Nuget\lib\sl40

copy "Combinatorics.Net20\bin\*.dll" "Nuget\lib\net20\"
copy "Combinatorics.Net20\bin\*.pdb" "Nuget\lib\net20\"
copy "Combinatorics.Net20\bin\*.xml" "Nuget\lib\net20\"
									  
copy "Combinatorics.Net40\bin\*.dll" "Nuget\lib\net40\"
copy "Combinatorics.Net40\bin\*.pdb" "Nuget\lib\net40\"
copy "Combinatorics.Net40\bin\*.xml" "Nuget\lib\net40\"
									  
copy "Combinatorics.Net40\bin\*.dll" "Nuget\lib\sl40\"
copy "Combinatorics.Net40\bin\*.pdb" "Nuget\lib\sl40\"
copy "Combinatorics.Net40\bin\*.xml" "Nuget\lib\sl40\"