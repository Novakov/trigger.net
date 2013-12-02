$base = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Path)

$output = "$($base)\build\packages"

if(-not (Test-Path $output)) {
	mkdir $output | Out-Null
}

Set-Alias nuget (Get-Item "$($base)\src\packages\NuGet.*\tools\nuget.exe").Fullname

$projects = Get-ChildItem "$($base)\src" -Recurse -Include *.csproj | where {Test-Path ([IO.Path]::ChangeExtension($_.FullName, '.nuspec'))}

$projects | % {
	echo "Building NuGet package for $($_)"
	nuget pack $_ -Build -OutputDirectory $output -Properties Configuration=Release -Verbosity quiet
	nuget pack $_ -Build -OutputDirectory $output -Properties Configuration=Release -Verbosity quiet -Symbols
}
