@echo off

msbuild build.proj /t:Version /p:MajorVersion=1 /p:MinorVersion=2 /p:BuildNumber=3 /p:BranchName=master /verbosity:m /nologo
msbuild build.proj /t:Version /p:MajorVersion=1 /p:MinorVersion=2 /p:BuildNumber=3 /p:BranchName=develop /verbosity:m /nologo 
msbuild build.proj /t:Version /p:MajorVersion=1 /p:MinorVersion=2 /p:BuildNumber=3 /p:BranchName=release/1.2 /verbosity:m /nologo
msbuild build.proj /t:Version /p:MajorVersion=5 /p:MinorVersion=6 /p:BuildNumber=7 /p:BranchName=release/4.5.6 /verbosity:m /nologo
msbuild build.proj /t:Version /p:MajorVersion=1 /p:MinorVersion=2 /p:BuildNumber=3 /p:BranchName=feature/PHAK-1 /verbosity:m /nologo
msbuild build.proj /t:Version /p:MajorVersion=1 /p:MinorVersion=2 /p:BuildNumber=3 /p:BranchName=fake /verbosity:m /nologo