namespace DevILSharp

open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open System
open System.IO

module IL =
    
    [<Literal>]
    let lib = "DevIL.dll"

    /// <summary>
    /// Sets the active image to a frame in an animation.
    /// </summary>
    [<DllImport(lib, EntryPoint="ilActiveFace")>]
    extern bool ActiveFace(int Number);

    [<DllImport(lib, EntryPoint="ilActiveImage")>]
    extern bool ActiveImage(int Number);

    [<DllImport(lib, EntryPoint="ilActiveLayer")>]
    extern bool ActiveLayer(int Number);

    [<DllImport(lib, EntryPoint="ilActiveMipmap")>]
    extern bool ActiveMipmap(int Number);

    [<DllImport(lib, EntryPoint="ilApplyPal")>]
    extern bool ApplyPal(string FileName);

    [<DllImport(lib, EntryPoint="ilApplyProfile")>]
    extern bool ApplyProfile(string InProfile, string OutProfile);

    [<DllImport(lib, EntryPoint="ilBindImage")>]
    extern void BindImage(int Image);

    [<DllImport(lib, EntryPoint="ilBlit")>]
    extern bool Blit(int Source, int DestX, int DestY, int DestZ, int SrcX, int SrcY, int SrcZ, int Width, int Height, int Depth);

    [<DllImport(lib, EntryPoint="ilClampNTSC")>]
    extern bool ClampNTSC();

    [<DllImport(lib, EntryPoint="ilClearColour")>]
    extern void ClearColour(float32 Red, float32 Green, float32 Blue, float32 Alpha);

    [<DllImport(lib, EntryPoint="ilClearImage")>]
    extern bool ClearImage();

    [<DllImport(lib, EntryPoint="ilCloneCurImage")>]
    extern int CloneCurImage();

    [<DllImport(lib, EntryPoint="ilCompressDXT")>]
    extern nativeint CompressDXT(nativeint Data, int Width, int Height, int Depth, CompressedDataFormat DXTCFormat, int& DXTCSize);

    [<DllImport(lib, EntryPoint="ilCompressFunc")>]
    extern bool CompressFunc(CompressMode Mode);

    [<DllImport(lib, EntryPoint="ilConvertImage")>]
    extern bool ConvertImage(ChannelFormat DestFormat, ChannelType DestType);

    [<DllImport(lib, EntryPoint="ilConvertPal")>]
    extern bool ConvertPal(PaletteType DestFormat);

    [<DllImport(lib, EntryPoint="ilCopyImage")>]
    extern bool CopyImage(int Src);

    [<DllImport(lib, EntryPoint="ilCopyPixels")>]
    extern int CopyPixels(int XOff, int YOff, int ZOff, int Width, int Height, int Depth, ChannelFormat Format, ChannelType Type, void *Data);

    [<DllImport(lib, EntryPoint="ilCreateSubImage")>]
    extern int CreateSubImage(SubImageType Type, int Num);

    [<DllImport(lib, EntryPoint="ilDefaultImage")>]
    extern bool DefaultImage();

    [<DllImport(lib, EntryPoint="ilDeleteImage")>]
    extern void DeleteImage(int Num);

    [<DllImport(lib, EntryPoint="ilDeleteImages")>]
    extern void DeleteImages(nativeint Num, int[] Images);

    [<DllImport(lib, EntryPoint="ilDetermineType")>]
    extern ImageType DetermineType(string FileName);

    [<DllImport(lib, EntryPoint="ilDetermineTypeF")>]
    extern ImageType DetermineTypeF(void* File);

    [<DllImport(lib, EntryPoint="ilDetermineTypeL")>]
    extern ImageType DetermineTypeL(void *Lump, int Size);

    [<DllImport(lib, EntryPoint="ilDisable")>]
    extern bool Disable(EnableCap Mode);

    [<DllImport(lib, EntryPoint="ilDxtcDataToImage")>]
    extern bool DxtcDataToImage();

    [<DllImport(lib, EntryPoint="ilDxtcDataToSurface")>]
    extern bool DxtcDataToSurface();

    [<DllImport(lib, EntryPoint="ilEnable")>]
    extern bool Enable(EnableCap Mode);

    [<DllImport(lib, EntryPoint="ilFlipSurfaceDxtcData")>]
    extern void FlipSurfaceDxtcData();

    [<DllImport(lib, EntryPoint="ilFormatFunc")>]
    extern bool FormatFunc(ChannelFormat Mode);

    [<DllImport(lib, EntryPoint="ilGenImages")>]
    extern void GenImages(nativeint Num, int[] Images);

    [<DllImport(lib, EntryPoint="ilGenImage")>]
    extern int GenImage();

    [<DllImport(lib, EntryPoint="ilGetAlpha")>]
    extern nativeint GetAlpha(ChannelType Type);

    [<DllImport(lib, EntryPoint="ilGetBoolean")>]
    extern bool GetBoolean(BooleanName Mode);

    [<DllImport(lib, EntryPoint="ilGetBooleanv")>]
    extern void GetBooleanv(BooleanName Mode, bool[] Param);
    
    [<DllImport(lib, EntryPoint="ilGetData")>]
    extern nativeint GetData();

    [<DllImport(lib, EntryPoint="ilGetDXTCData")>]
    extern int GetDXTCData(void *Buffer, int BufferSize, CompressedDataFormat DXTCFormat);

    [<DllImport(lib, EntryPoint="ilGetError")>]
    extern ErrorCode GetError();

    [<DllImport(lib, EntryPoint="ilGetInteger")>]
    extern int GetInteger(IntName Mode);

    [<DllImport(lib, EntryPoint="ilGetIntegerv")>]
    extern void GetIntegerv(IntName Mode, int[] Param);

    let GetFormat() =
        GetInteger(IntName.ImageFormat) |> unbox<ChannelFormat>

    let GetDataType() =
        GetInteger(IntName.ImageType) |> unbox<ChannelType>

    [<DllImport(lib, EntryPoint="ilGetLumpPos")>]
    extern int GetLumpPos();
    
    [<DllImport(lib, EntryPoint="ilGetPalette")>]
    extern nativeint GetPalette();

    [<DllImport(lib, EntryPoint="ilGetString")>]
    extern string GetString(StringName StringName);

//    [<DllImport(lib, EntryPoint="ilHint")>]
//    extern void Hint(ILenum Target, ILenum Mode);

    [<DllImport(lib, EntryPoint="ilInvertSurfaceDxtcDataAlpha")>]
    extern bool InvertSurfaceDxtcDataAlpha();

    [<DllImport(lib, EntryPoint="ilInit")>]
    extern void Init();


    [<DllImport(lib, EntryPoint="ilImageToDxtcData")>]
    extern bool ImageToDxtcData(CompressedDataFormat Format);

    [<DllImport(lib, EntryPoint="ilIsDisabled")>]
    extern bool IsDisabled(EnableCap Mode);

    [<DllImport(lib, EntryPoint="ilIsEnabled")>]
    extern bool IsEnabled(EnableCap Mode);

    [<DllImport(lib, EntryPoint="ilIsImage")>]
    extern bool IsImage(int Image);

    [<DllImport(lib, EntryPoint="ilIsValid")>]
    extern bool IsValid(ImageType Type, string FileName);

    [<DllImport(lib, EntryPoint="ilIsValidF")>]
    extern bool IsValidF(ImageType Type, void* File);

    [<DllImport(lib, EntryPoint="ilIsValidL")>]
    extern bool IsValidL(ImageType Type, void *Lump, int Size);

    [<DllImport(lib, EntryPoint="ilKeyColour")>]
    extern void KeyColour(float32 Red, float32 Green, float32 Blue, float32 Alpha);

    [<DllImport(lib, EntryPoint="ilLoad")>]
    extern bool Load(ImageType Type, string FileName);

    [<DllImport(lib, EntryPoint="ilLoadF")>]
    extern bool LoadF(ImageType Type, void* File);

    [<DllImport(lib, EntryPoint="ilLoadImage")>]
    extern bool LoadImage(string FileName);

    [<DllImport(lib, EntryPoint="ilLoadL")>]
    extern bool LoadL(ImageType Type, void *Lump, int Size);


    let LoadStreamWithType (imageType : ImageType, s : System.IO.Stream) =
        
        let bytes = Array.zeroCreate (int s.Length)
        let mutable read = 0
        while read < bytes.Length do
            let r = s.Read(bytes, read, bytes.Length - read)
            read <- read + r

        let gc = GCHandle.Alloc(bytes, GCHandleType.Pinned)
        let res = LoadL(imageType, gc.AddrOfPinnedObject(), bytes.Length)
        gc.Free()

        res

    let LoadStream (s : System.IO.Stream) =
        LoadStreamWithType(ImageType.Unkwown, s)

    [<DllImport(lib, EntryPoint="ilLoadPal")>]
    extern bool LoadPal(string FileName);

    [<DllImport(lib, EntryPoint="ilModAlpha")>]
    extern void ModAlpha(float AlphaValue);

    [<DllImport(lib, EntryPoint="ilOriginFunc")>]
    extern bool OriginFunc(OriginMode Mode);

    [<DllImport(lib, EntryPoint="ilOverlayImage")>]
    extern bool OverlayImage(int Source, int XCoord, int YCoord, int ZCoord);

    [<DllImport(lib, EntryPoint="ilPopAttrib")>]
    extern void PopAttrib();

    [<DllImport(lib, EntryPoint="ilPushAttrib")>]
    extern void PushAttrib(int Bits);

    [<DllImport(lib, EntryPoint="ilRegisterFormat")>]
    extern void RegisterFormat(ChannelFormat Format);

//    [<DllImport(lib, EntryPoint="ilRegisterLoad")>]
//    extern bool RegisterLoad(string Ext, IL_LOADPROC Load);

    [<DllImport(lib, EntryPoint="ilRegisterMipNum")>]
    extern bool RegisterMipNum(int Num);

    [<DllImport(lib, EntryPoint="ilRegisterNumFaces")>]
    extern bool RegisterNumFaces(int Num);

    [<DllImport(lib, EntryPoint="ilRegisterNumImages")>]
    extern bool RegisterNumImages(int Num);

    [<DllImport(lib, EntryPoint="ilRegisterOrigin")>]
    extern void RegisterOrigin(OriginMode Origin);

    [<DllImport(lib, EntryPoint="ilRegisterPal")>]
    extern void RegisterPal(void *Pal, int Size, PaletteType Type);

//    [<DllImport(lib, EntryPoint="ilRegisterSave")>]
//    extern bool RegisterSave(string Ext, IL_SAVEPROC Save);

    [<DllImport(lib, EntryPoint="ilRegisterType")>]
    extern void RegisterType(ChannelType Type);

    [<DllImport(lib, EntryPoint="ilRemoveLoad")>]
    extern bool RemoveLoad(string Ext);

    [<DllImport(lib, EntryPoint="ilRemoveSave")>]
    extern bool RemoveSave(string Ext);

    [<DllImport(lib, EntryPoint="ilResetMemory")>]
    extern void ResetMemory(); // Deprecated

    [<DllImport(lib, EntryPoint="ilResetRead")>]
    extern void ResetRead();

    [<DllImport(lib, EntryPoint="ilResetWrite")>]
    extern void ResetWrite();

    [<DllImport(lib, EntryPoint="ilSave")>]
    extern bool Save(ImageType Type, string FileName);

    [<DllImport(lib, EntryPoint="ilSaveF")>]
    extern int SaveF(ImageType Type, void* File);

    [<DllImport(lib, EntryPoint="ilSaveImage")>]
    extern bool SaveImage(string FileName);

    [<DllImport(lib, EntryPoint="ilSaveL")>]
    extern int SaveL(ImageType Type, void *Lump, int Size);


    [<DllImport(lib, EntryPoint="ilSavePal")>]
    extern bool SavePal(string FileName);

    let SaveStream(imageType : ImageType, stream : Stream) =
        let size = SaveL(imageType, 0n, 0)
        if size = 0 then 
            false
        else
            let arr : byte[] = Array.zeroCreate size


            let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)
            let dataSize = SaveL(imageType, gc.AddrOfPinnedObject(), size)
            stream.Write(arr, 0, dataSize)

            gc.Free()
            true


    [<DllImport(lib, EntryPoint="ilSetAlpha")>]
    extern bool SetAlpha(float AlphaValue);

    [<DllImport(lib, EntryPoint="ilSetData")>]
    extern bool SetData(void *Data);

    [<DllImport(lib, EntryPoint="ilSetDuration")>]
    extern bool SetDuration(int Duration);

    [<DllImport(lib, EntryPoint="ilSetInteger")>]
    extern void SetInteger(IntName Mode, int Param);

//    [<DllImport(lib, EntryPoint="ilSetMemory")>]
//    extern void SetMemory(mAlloc, mFree);

    [<DllImport(lib, EntryPoint="ilSetPixels")>]
    extern void SetPixels(int XOff, int YOff, int ZOff, int Width, int Height, int Depth, ChannelFormat Format, ChannelType Type, void *Data);

//    [<DllImport(lib, EntryPoint="ilSetRead")>]
//    extern void SetRead(fOpenRProc, fCloseRProc, fEofProc, fGetcProc, fReadProc, fSeekRProc, fTellRProc);

    [<DllImport(lib, EntryPoint="ilSetString")>]
    extern void SetString(StringName Mode,  string String);

//    [<DllImport(lib, EntryPoint="ilSetWrite")>]
//    extern void SetWrite(fOpenWProc, fCloseWProc, fPutcProc, fSeekWProc, fTellWProc, fWriteProc);

    [<DllImport(lib, EntryPoint="ilShutDown")>]
    extern void ShutDown();

    [<DllImport(lib, EntryPoint="ilSurfaceToDxtcData")>]
    extern bool SurfaceToDxtcData(CompressedDataFormat Format);

    [<DllImport(lib, EntryPoint="ilTexImage")>]
    extern bool TexImage(int Width, int Height, int Depth, byte NumChannels, ChannelFormat Format, ChannelType Type, void *Data);

    [<DllImport(lib, EntryPoint="ilTexImageDxtc")>]
    extern bool TexImageDxtc(int w, int h, int d, CompressedDataFormat DxtFormat, nativeint data);

    [<DllImport(lib, EntryPoint="ilTypeFromExt")>]
    extern ImageType TypeFromExt(string FileName);

    [<DllImport(lib, EntryPoint="ilTypeFunc")>]
    extern bool TypeFunc(ChannelType Mode);

    [<DllImport(lib, EntryPoint="ilLoadData")>]
    extern bool LoadData(string FileName, int Width, int Height, int Depth, byte Bpp);

    [<DllImport(lib, EntryPoint="ilLoadDataF")>]
    extern bool LoadDataF(void* File, int Width, int Height, int Depth, byte Bpp);

    [<DllImport(lib, EntryPoint="ilLoadDataL")>]
    extern bool LoadDataL(void *Lump, int Size, int Width, int Height, int Depth, byte Bpp);

    [<DllImport(lib, EntryPoint="ilSaveData")>]
    extern bool SaveData(string FileName);


    let check (error : string) (v : bool) =
        if not v then
            let err = GetError()
            printfn "ERROR %A: %s" err error