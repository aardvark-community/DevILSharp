namespace DevILSharp

open System
open System.Runtime.InteropServices
open System.IO.MemoryMappedFiles

module private Memory =
    open System
    open System.Runtime.InteropServices

    module private MSVCRT =
        [<DllImport("msvcrt.dll")>]
        extern nativeint memcpy(nativeint dest, nativeint src, unativeint size)

    module private Libc =
        [<DllImport("libc")>]
        extern nativeint memcpy(nativeint dest, nativeint src, unativeint size)

    let memcpy (dest : nativeint, src : nativeint, size : unativeint) =
        match Environment.OSVersion.Platform with
            | PlatformID.Unix -> Libc.memcpy(dest, src, size)
            | PlatformID.MacOSX -> Libc.memcpy(dest, src, size)
            | _ -> MSVCRT.memcpy(dest, src, size)

type MirrorFlags =
    | None      = 0x0000
    | MirrorX   = 0x0001
    | MirrorY   = 0x0002

type Image private(handle : int) =

    static let l = obj()
    static let emptyInfo = ILU.Info(Id = -1)

    static let check (success : bool) =
        if not success then
            let code = IL.GetError()
            let str = ILU.ErrorString(code)
            failwith str

    let mutable handle = handle
    let mutable infoCache = emptyInfo

    let run (f : unit -> 'a) =
        if handle < 0 then
            raise <| System.ObjectDisposedException("image")

        lock l (fun () ->
            let old = IL.GetInteger(IntName.ActiveImage)
            try
                IL.BindImage(handle)
                f()
            finally
                IL.BindImage(old)
        )

    let getInfo() =
        if handle < 0 || infoCache.Id = handle then
            infoCache
        else
            run (fun () ->
                ILU.GetImageInfo(&infoCache)
                infoCache
            )


    member x.Handle = handle
    member x.Width = getInfo().Width
    member x.Height = getInfo().Height
    member x.ChannelFormat = getInfo().Format
    member x.ChannelType = getInfo().Type
    member x.Data = getInfo().Data
    member x.DataSize = getInfo().SizeOfData

    static member Load(file : string, imageType : ImageType) =
        let id = IL.GenImage()
        lock l (fun () ->
            let old = IL.GetInteger(IntName.CurrentImage)
            try
                IL.BindImage(id)
                IL.Load(imageType, file) |> check
            finally
                IL.BindImage(old)
        )
        new Image(id)

    static member Load(file : string) =
        let id = IL.GenImage()
        lock l (fun () ->
            let old = IL.GetInteger(IntName.CurrentImage)
            try
                IL.BindImage(id)
                IL.LoadImage(file) |> check
            finally
                IL.BindImage(old)
        )
        new Image(id)

    static member Load(arr : byte[], imageType : ImageType) =
        let id = IL.GenImage()
        lock l (fun () ->
            let old = IL.GetInteger(IntName.CurrentImage)
            let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)
            try
                IL.BindImage(id)
                let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)
                IL.LoadL(imageType, gc.AddrOfPinnedObject(), arr.Length) |> check
            finally
                gc.Free()
                IL.BindImage(old)
        )
        new Image(id)


    member x.Save(file : string, imageType : ImageType, overwrite : bool) =
        run (fun () ->
            if overwrite then IL.Enable(EnableCap.OverwriteExistingFile) |> check
            else IL.Disable(EnableCap.OverwriteExistingFile) |> check

            IL.Save(imageType, file) |> check
        )

    member x.Save(file : string, imageType : ImageType) =
        x.Save(file, imageType, true)

    member x.Save(file : string) =
        x.Save(file, ImageType.Unkwown, true)


    member x.Scale(newWidth : int, newHeight : int, filter : Filter) =
        run (fun () ->
            ILU.SetFilter(filter)
            ILU.Scale(newWidth, newHeight, 0) |> check
            infoCache <- emptyInfo
        )

    member x.Scale(factor : float, filter : Filter) =
        let newWidth = int (float x.Width * factor)
        let newHeight = int (float x.Height * factor)
        x.Scale(newWidth, newHeight, filter)

    member x.Scale(newWidth : int, newHeight : int) =
        x.Scale(newWidth, newHeight, Filter.Bilinear)

    member x.Scale(factor : float) =
        x.Scale(factor, Filter.Bilinear)


    member x.Rotate(angleInDegrees : float, filter : Filter) =
        run (fun () ->
            ILU.SetFilter(filter)
            IL.ClearColour(0.0f, 0.0f, 0.0f, 0.0f)
            ILU.Rotate(float32 angleInDegrees) |> check
            infoCache <- emptyInfo
        )

    member x.Rotate(angleInDegrees : float) =
        x.Rotate(angleInDegrees, Filter.Bilinear)

    member x.FlipX() =
        run (fun () ->
            ILU.Mirror() |> check
        )

    member x.FlipY() =
        run (fun () ->
            ILU.FlipImage() |> check
        )

    member x.Mirror (flags : MirrorFlags) =
        run (fun () ->
            if flags &&& MirrorFlags.MirrorY <> MirrorFlags.None then
                ILU.Mirror() |> check

            if flags &&& MirrorFlags.MirrorX <> MirrorFlags.None then
                ILU.FlipImage() |> check


            infoCache <- emptyInfo
        )

    member x.EdgeDetectSobel() =
        run (fun () ->
            ILU.EdgeDetectS() |> check
        )
        
    member x.EdgeDetectPrewitt() =
        run (fun () ->
            ILU.EdgeDetectP() |> check
        )

    member x.Sharpen(factor : float, iterations : int) =
        run (fun () ->
            ILU.Sharpen(float32 factor, iterations) |> check
        )

    member x.AddNoise(tolerance : float) =
        run (fun () ->
            ILU.Noisify(float32 tolerance) |> check
        )

    member x.Emboss() =
        run (fun () ->
            ILU.Emboss() |> check
        )

    member x.Pixelize(pixSize : int) =
        run (fun () ->
            ILU.Pixelize(pixSize) |> check
        )

    member x.Saturate(sat : float) =
        run (fun () ->
            ILU.Saturate1f(float32 sat) |> check
        )

    member x.Saturate(r : float, g : float, b : float, sat : float) =
        run (fun () ->
            ILU.Saturate4f(float32 r, float32 g, float32 b, float32 sat) |> check
        )

    member x.ScaleColors(r : float, g : float, b : float) =
        run (fun () ->
            ILU.ScaleColours(float32 r, float32 g, float32 b) |> check
        )


    member x.GammaCorrect(gamma : float) =
        run (fun () ->
            ILU.GammaCorrect(float32 gamma) |> check
            infoCache <- emptyInfo
        )

    member x.Contrast(contrast : float) =
        run (fun () ->
            ILU.Contrast(float32 contrast) |> check
            infoCache <- emptyInfo
        )

    member x.Enlarge(w : int, h : int, p : Placement) =
        run (fun () ->
            IL.ClearColour(0.0f, 0.0f, 0.0f, 0.0f)
            ILU.SetPlacement(p)
            ILU.EnlargeCanvas(w, h, 1) |> check
            infoCache <- emptyInfo
        )

    member x.Convert(f : ChannelFormat, t : ChannelType) =
        run(fun () ->
            IL.ConvertImage(f, t) |> check
            infoCache <- emptyInfo
        )

    member x.CopyTo(ptr : nativeint) =
        Memory.memcpy(ptr, x.Data, unativeint x.DataSize) |> ignore
  
    member x.CopyTo(arr : Array, startIndex : int) =
        let offset = 
            if startIndex = 0 then 0
            else
                let t = arr.GetType().GetElementType()
                let s = Marshal.SizeOf(t)
                startIndex * s

        let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)

        try
            Memory.memcpy(gc.AddrOfPinnedObject() + nativeint offset, x.Data, unativeint x.DataSize) |> ignore
        finally
            gc.Free()

    member x.CopyTo(arr : Array) =
        x.CopyTo(arr, 0)


//    member x.Crop(left : int, right : int, top : int, bottom : int) =
//        run (fun () ->
//            let newWidth = x.Width - left - right
//            let newHeight = x.Height - top - bottom
//
//            ILU.Crop(left, bottom, 0, newWidth, newHeight, 1) |> check
//            infoCache.Width <- newWidth
//            infoCache.Height <- newHeight
//        )
//
//    member x.Crop(margin : int) =
//        x.Crop(margin, margin, margin, margin)

    member x.Dispose() =
        if handle >= 0 then
            IL.DeleteImage(handle)
            handle <- -1
            infoCache <- emptyInfo

    interface IDisposable with
        member x.Dispose() = x.Dispose()