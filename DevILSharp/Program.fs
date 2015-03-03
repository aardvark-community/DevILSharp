#if INTERACTIVE
#else
namespace DevILSharp.Demo
#endif

open System
open System.Runtime.InteropServices
open DevILSharp

module Demo = 
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
        let size = IL.GetInteger(GetName.ImageSizeOfData)
        let format = IL.GetFormat()
        let pixelType = IL.GetDataType()
        let width = IL.GetInteger(GetName.ImageWidth)
        let height = IL.GetInteger(GetName.ImageHeight)


    
        let arr : byte[] = Array.zeroCreate size
        Marshal.Copy(ptr, arr, 0, arr.Length)
        IL.BindImage(0)
        IL.DeleteImage(img)


        let img = IL.GenImage()
        IL.BindImage(img)
        let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)

        IL.TexImage(width, height, 1, 3uy, Format.RGB, ChannelType.UnsignedShort, gc.AddrOfPinnedObject()) |> IL.check "could not set image data"
        IL.ConvertImage(Format.RGB, ChannelType.UnsignedByte) |> IL.check "could not convert image"
    
        use s = new System.IO.FileStream("C:\\Users\\haaser\\Desktop\\test2.png", IO.FileMode.Create)
        IL.SaveStream(ImageType.Png, s)

        gc.Free()
        IL.BindImage(0)
        IL.DeleteImage(img)





        printfn "%A" arr

        IL.ShutDown()

