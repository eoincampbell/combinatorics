@echo off
nuget pack Nuget\Combinatorics.nuspec
nuget push %1