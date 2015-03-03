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


// start build
RunTargetOrDefault "Projects"

