dotnet test
if %ERRORLEVEL% EQU 0 (
   echo Tests passed
   dotnet build UnityAuxiliaryTools/UnityAuxiliaryTools.csproj --no-dependencies --output build
) else (
   echo Tests failed
   exit /b %errorlevel%
)