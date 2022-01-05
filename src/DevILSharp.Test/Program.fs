open System
open System.Runtime.InteropServices
open DevILSharp
open System.Diagnostics
open System.IO

// module Demo = 

//     let probe (name : string) (f : unit -> 'a) =
//         let sw = Stopwatch()
//         sw.Start()
//         let res = f ()
//         sw.Stop()
//         printfn "%s took: %Ams" name sw.Elapsed.TotalMilliseconds
//         res

//     let imageTest() =
//         Bootstrap.Init()
//         IL.Init()
//         ILU.Init()
//         IL.Enable(EnableCap.AbsoluteOrigin) |> ignore
//         let img = 
//             probe "load" (fun () -> Image.Load (System.IO.File.ReadAllBytes(@"C:\Users\haaser\Desktop\ps_height_1k.png"), ImageType.Png))


//         probe "convert" (fun () -> img.Convert(ChannelFormat.BGRA, ChannelType.UnsignedByte))

//         //probe "flip" (fun () -> img.FlipY())

//         let data : byte[] = Array.zeroCreate img.DataSize
//         img.CopyTo(data :> System.Array)

//         probe "mirror" (fun () -> img.Mirror(MirrorFlags.MirrorX ||| MirrorFlags.MirrorY))


//         probe "copy" (fun () -> img.CopyTo(data :> System.Array))

//         probe "scale" (fun () -> img.Scale(0.25, Filter.Lanczos3))

//         probe "noise" (fun () -> img.AddNoise 0.2)

//         //printfn "img: %A %A %A" img.ChannelFormat img.ChannelType data

//         probe "save" (fun () -> img.Save @"C:\Users\haaser\Desktop\SixteenBitRGB2.png")
//         ()

//     let test() =
//         Bootstrap.Init()
//         IL.Init()
//         ILU.Init()
//         IL.Enable(EnableCap.OverwriteExistingFile) |> IL.check "could enable overwrite"
//         IL.Enable(EnableCap.AbsoluteOrigin) |> IL.check "could enable absolute origin"

//         let img = ILU.GenImage()
//         let mutable info = ILU.Info()
//         IL.BindImage(img)
//         IL.LoadImage(@"C:\Users\schorsch\Desktop\SixteenBitRGB.tif") |> printfn "load: %A"
//         ILU.GetImageInfo(&info)
//         ILU.Scale(1024, 768, 0) |> printfn "blur: %A"
//         IL.SaveImage("C:\\Users\\schorsch\\Desktop\\test.png") |> printfn "save: %A"
//         printfn "%A" info

//     let run() = 
//         Bootstrap.Init()
//         IL.Init()
//         printfn "inited"
//         IL.Enable(EnableCap.OverwriteExistingFile) |> IL.check "could enable overwrite"
//         IL.Enable(EnableCap.AbsoluteOrigin) |> IL.check "could enable absolute origin"
//         let img = IL.GenImage()

//         printfn "asdasdasd"

//         IL.BindImage(img)
//         use s = new System.IO.FileStream(@"C:\Users\haaser\Desktop\SixteenBitRGB.tif", IO.FileMode.Open)
//         IL.LoadStream(s) |> IL.check "could not load image"

//         let ptr = IL.GetData()
//         let size = IL.GetInteger(IntName.ImageSizeOfData)
//         let format = IL.GetFormat()
//         let pixelType = IL.GetDataType()
//         let width = IL.GetInteger(IntName.ImageWidth)
//         let height = IL.GetInteger(IntName.ImageHeight)


    
//         let arr : byte[] = Array.zeroCreate size
//         Marshal.Copy(ptr, arr, 0, arr.Length)
//         IL.BindImage(0)
//         IL.DeleteImage(img)


//         let img = IL.GenImage()
//         IL.BindImage(img)
//         let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)

//         IL.TexImage(width, height, 1, 3uy, ChannelFormat.RGB, ChannelType.UnsignedShort, gc.AddrOfPinnedObject()) |> IL.check "could not set image data"
//         IL.ConvertImage(ChannelFormat.RGB, ChannelType.UnsignedByte) |> IL.check "could not convert image"
    
//         use s = new System.IO.FileStream("C:\\Users\\haaser\\Desktop\\test2.png", IO.FileMode.Create)
//         IL.SaveStream(ImageType.Png, s) |> IL.check "could not save stream"

//         gc.Free()
//         IL.BindImage(0)
//         IL.DeleteImage(img)





//         printfn "%A" arr

//         IL.ShutDown()

type Marker = class end

[<EntryPoint>]
let main argv =

    let paths = [Path.GetDirectoryName(typeof<Marker>.Assembly.Location)]
    let formats = ["{0}.dylib"; "lib{0}.dylib"]

    let tryLoad (name : string) =
        formats |> List.tryPick (fun fmt ->
            let name = String.Format(fmt, name)
            paths |> List.tryPick (fun path ->
                let p = Path.Combine(path, name)
                if File.Exists p then
                    let ptr = NativeLibrary.Load p
                    match NativeLibrary.TryLoad p with
                    | (true, l) -> Some l
                    | _ -> None
                else
                    None

            )
        )

    NativeLibrary.SetDllImportResolver(typeof<DevILSharp.IntName>.Assembly, DllImportResolver(fun name _ _ ->
        match tryLoad name with
        | Some p -> p
        | None -> 0n
    ))

    System.Runtime.Loader.AssemblyLoadContext.Default.add_ResolvingUnmanagedDll(Func<_,_,_>(fun a b ->
        match tryLoad b with
        | Some p -> p
        | None -> 0n
    ))

    IL.Init()

    let img = IL.GenImage()
    IL.BindImage(img)
    IL.TexImage(1024, 768, 1, 4uy, ChannelFormat.BGRA, ChannelType.UnsignedByte, 0n) |> ignore
    IL.ClearColour(1.0f, 0.0f, 0.0f, 1.0f)
    IL.ClearImage() |> ignore

    let dst = Path.Combine(Environment.GetFolderPath Environment.SpecialFolder.Desktop, "test.png")
    IL.SaveImage dst |> ignore

    




    0

