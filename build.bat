dotnet test
if %ERRORLEVEL% EQU 0 (
   echo Tests passed
   dotnet build AsyncReactAwait/AsyncReactAwait.csproj --no-dependencies --output build
) else (
   echo Tests failed
   exit /b %errorlevel%
)