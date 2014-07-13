#r "packages/Fake.3.2.0/tools/FakeLib.dll"
open Fake
open System

let NugetOutputDirectory = currentDirectory @@ @"..\build\packages"

let msbuild targets properties = 
    let setParams (defaults:MSBuildParams) = {
        defaults with
            Verbosity = Some(Minimal)
            Targets = targets            
            Properties = List.append properties [ "Configuration","Release" ]
    }

    build setParams "Trigger.NET.sln"

let pack nuspec = 
    let csproj = System.IO.Path.ChangeExtension(nuspec, ".csproj")
     
    let nuget = findNuget (currentDirectory @@ "tools" @@ "NuGet")

    let args = [
        "pack"
        "\"" + csproj + "\""
        "-Properties Configuration=Release"
        "-OutputDirectory \"" + NugetOutputDirectory + "\""
        "-Verbosity quiet"
    ]

    ExecProcess (fun info ->
        info.FileName <- nuget
        info.Arguments <- String.concat " " args
    ) (TimeSpan.FromMinutes(5.0)) |> ignore    