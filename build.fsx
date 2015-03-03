#I @"packages/FAKE/tools/"
#r @"FakeLib.dll"

open Fake
open System
open System.IO

let projects = !!"**/*.fsproj"

Target "Restore" (fun () ->

    let packageConfigs = !!"src/**/packages.config" |> Seq.toList

    let defaultNuGetSources = RestorePackageHelper.RestorePackageDefaults.Sources
    for pc in packageConfigs do
        RestorePackage (fun p -> { p with OutputPath = "packages" }) pc


)

Target "Clean" (fun () ->
    CleanDir "bin"
)

Target "Projects" (fun () ->
    MSBuildRelease "bin/Release" "Build" projects |> ignore
)



Target "CreatePackage" (fun () ->
    let branch = Fake.Git.Information.getBranchName "."
    let releaseNotes = Fake.Git.Information.getCurrentHash()

    if branch = "master" then
        let tag = Fake.Git.Information.getLastTag()
        NuGetPack (fun p -> 
            { p with OutputPath = "bin"; 
                     Version = tag; 
                     ReleaseNotes = releaseNotes; 
                     WorkingDir = "bin"
            }) "bin/DevILSharp.nuspec"
    
    else 
        traceError (sprintf "cannot create package for branch: %A" branch)
)

Target "Deploy" (fun () ->

    let accessKeyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ssh", "nuget.key")
    let accessKey =
        if File.Exists accessKeyPath then Some (File.ReadAllText accessKeyPath)
        else None

    let branch = Fake.Git.Information.getBranchName "."
    let releaseNotes = Fake.Git.Information.getCurrentHash()
    if branch = "master" then
        let tag = Fake.Git.Information.getLastTag()
        match accessKey with
            | Some accessKey ->
                try
                    NuGetPublish (fun p -> 
                        { p with 
                            Project = "DevILSharp"
                            OutputPath = "bin"
                            Version = tag; 
                            ReleaseNotes = releaseNotes; 
                            WorkingDir = "bin"
                            AccessKey = accessKey
                            Publish = true
                        })
                with e ->
                    ()
            | None ->
                ()
     else 
        traceError (sprintf "cannot deploy branch: %A" branch)
)


"Projects" ==>
    "CreatePackage" ==>
    "Deploy"

// start build
RunTargetOrDefault "Projects"

