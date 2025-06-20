dotnet build src/AsyncReactAwait/AsyncReactAwait.csproj --no-dependencies --output build
copy build\AsyncReactAwait.dll AraUnityProj\Assets\Packages\com.kekchpek.ara\AsyncReactAwait.dll
pause