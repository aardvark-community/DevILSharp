namespace DevILSharp

open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open System
open System.IO
open System.Reflection
open System.IO.Compression


/// <summary>
/// Bootstrap copies the needed native libraries to the current folder
/// according to the operating system and architecture of the running process.
/// </summary>
module Bootstrap =

    let mutable private initialized = false

    // since Devil.dll is called libIL.so on linux system we need to
    // write a config file remapping the dll name.
    let private config = 
        "<configuration>\r\n" +
        "  <dllmap os=\"linux\" dll=\"DevIL.dll\" target=\"libIL.so\"/>\r\n" +
        "  <dllmap os=\"linux\" dll=\"ILU.dll\" target=\"libILU.so\"/>\r\n" +
        "  <dllmap os=\"linux\" dll=\"ILUT.dll\" target=\"libILUT.so\"/>\r\n" +
        "</configuration>\r\n"

    // utility function saving one ZipEntry to the current directory
    let private save (entry : ZipArchiveEntry) bytesWriter =
        if entry.Length > 0L && not <| File.Exists entry.Name then
            let name = entry.Name
            use stream = entry.Open()
            let arr = Array.zeroCreate (int entry.Length)
            let mutable read = 0
            while read < arr.Length do
                let r = stream.Read(arr, read, arr.Length - read)
                read <- read + r
            bytesWriter arr name

    /// <summary>
    /// initialize devil by copying all needed native libraries to the
    /// current directory allowing them to be subsequently loaded.
    /// </summary>
    let Init() =
        if not initialized then
            initialized <- true

            let ass = Assembly.GetExecutingAssembly()

            let location = Path.GetDirectoryName(ass.Location)
            let writeBytesSafe (bytes : byte[]) (name : string) =
                let path = Path.Combine(location,name) 
                if File.Exists path |> not then 
                    File.WriteAllBytes(path,bytes)

            use stream = ass.GetManifestResourceStream("DevIL.zip")
            use archive = new ZipArchive(stream)

            let configPath = Path.Combine(location,"DevILSharp.dll.config")
            if File.Exists configPath |> not then
                File.WriteAllText(configPath, config)

            let os =
                match Environment.OSVersion.Platform with
                    | PlatformID.Unix -> "Linux"
                    | PlatformID.MacOSX -> "Mac"
                    | _ -> "Windows"

            let arch =
                if IntPtr.Size = 4 then "x86"
                else "AMD64"

            let prefix = sprintf "%s_%s/" os arch

            let entries = archive.Entries |> Seq.filter (fun a -> a.FullName.StartsWith prefix) |> Seq.toList
        
            for e in entries do
                save e writeBytesSafe

                