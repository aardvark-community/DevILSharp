// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System
open System.Runtime.InteropServices
open DevILSharp

[<EntryPoint>]
let main argv = 
    IL.Init()
    IL.Enable(EnableCap.OverwriteExistingFile) |> IL.check "could enable overwrite"
    IL.Enable(EnableCap.AbsoluteOrigin) |> IL.check "could enable absolute origin"
    let img = IL.GenImage()

    IL.BindImage(img)
    IL.LoadImage(@"C:\Users\Schorsch\Desktop\SixteenBitRGB.tif") |> IL.check "could not load image"

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
    
    IL.SaveImage("C:\\Users\\schorsch\\Desktop\\test.png") |> IL.check "could not save image"

    gc.Free()
    IL.BindImage(0)
    IL.DeleteImage(img)





    printfn "%A" arr

    IL.ShutDown()




    printfn "%A" argv
    0 // return an integer exit code
