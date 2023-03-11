dotnet test
if [ $? -eq 0 ]; then
   echo Tests passed
   dotnet build AsyncReactAwait/AsyncReactAwait.csproj --no-dependencies --output build
else
   echo Tests failed
   exit $?
fi