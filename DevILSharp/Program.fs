//#if INTERACTIVE
//#r @"..\bin\Debug\DevilSharp.dll"
//#else
//namespace DevILSharp.Demo
//#endif

open System
open System.Runtime.InteropServices
open DevILSharp

module Demo = 
    let imageTest() =
        Bootstrap.Init()
        IL.Init()
        ILU.Init()
        IL.Enable(EnableCap.AbsoluteOrigin) |> ignore
        let img = Image.Load (System.IO.File.ReadAllBytes(@"C:\Users\schorsch\Desktop\small.png"), ImageType.Png)
        img.Convert(ChannelFormat.BGRA, ChannelType.UnsignedByte)
        img.FlipY()

        let data : byte[] = Array.zeroCreate img.DataSize
        img.CopyTo(data :> System.Array)

        printfn "img: %A %A %A" img.ChannelFormat img.ChannelType data

        img.Save @"C:\Users\schorsch\Desktop\SixteenBitRGB2.png"
        ()

    let test() =
        Bootstrap.Init()
        IL.Init()
        ILU.Init()
        IL.Enable(EnableCap.OverwriteExistingFile) |> IL.check "could enable overwrite"
        IL.Enable(EnableCap.AbsoluteOrigin) |> IL.check "could enable absolute origin"

        let img = ILU.GenImage()
        let mutable info = ILU.Info()
        IL.BindImage(img)
        IL.LoadImage(@"C:\Users\schorsch\Desktop\SixteenBitRGB.tif") |> printfn "load: %A"
        ILU.GetImageInfo(&info)
        ILU.Scale(1024, 768, 0) |> printfn "blur: %A"
        IL.SaveImage("C:\\Users\\schorsch\\Desktop\\test.png") |> printfn "save: %A"
        printfn "%A" info

    let run() = 
        Bootstrap.Init()
        IL.Init()
        IL.Enable(EnableCap.OverwriteExistingFile) |> IL.check "could enable overwrite"
        IL.Enable(EnableCap.AbsoluteOrigin) |> IL.check "could enable absolute origin"
        let img = IL.GenImage()

        IL.BindImage(img)
        use s = new System.IO.FileStream(@"C:\Users\haaser\Desktop\SixteenBitRGB.tif", IO.FileMode.Open)
        IL.LoadStream(s) |> IL.check "could not load image"

        let ptr = IL.GetData()
        let size = IL.GetInteger(IntName.ImageSizeOfData)
        let format = IL.GetFormat()
        let pixelType = IL.GetDataType()
        let width = IL.GetInteger(IntName.ImageWidth)
        let height = IL.GetInteger(IntName.ImageHeight)


    
        let arr : byte[] = Array.zeroCreate size
        Marshal.Copy(ptr, arr, 0, arr.Length)
        IL.BindImage(0)
        IL.DeleteImage(img)


        let img = IL.GenImage()
        IL.BindImage(img)
        let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)

        IL.TexImage(width, height, 1, 3uy, ChannelFormat.RGB, ChannelType.UnsignedShort, gc.AddrOfPinnedObject()) |> IL.check "could not set image data"
        IL.ConvertImage(ChannelFormat.RGB, ChannelType.UnsignedByte) |> IL.check "could not convert image"
    
        use s = new System.IO.FileStream("C:\\Users\\haaser\\Desktop\\test2.png", IO.FileMode.Create)
        IL.SaveStream(ImageType.Png, s) |> IL.check "could not save stream"

        gc.Free()
        IL.BindImage(0)
        IL.DeleteImage(img)





        printfn "%A" arr

        IL.ShutDown()

[<EntryPoint>]
let main argv =
    Demo.imageTest()
    0

