dotnet test
if [ $? -eq 0 ]; then
   echo Tests passed
   dotnet build src/AsyncReactAwait/AsyncReactAwait.csproj --no-dependencies --output src/build
else
   echo Tests failed
   exit $?
fi