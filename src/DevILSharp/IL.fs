namespace DevILSharp

open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open System
open System.Security
open System.IO

module IL =
    
    let mutable private initialized = false

    [<Literal>]
    let lib = "DevIL.dll"

    /// <summary>
    /// Sets the active image to a frame in an animation.
    /// </summary>
    [<DllImport(lib, EntryPoint="ilActiveFace"); SuppressUnmanagedCodeSecurity>]
    extern bool ActiveFace(int Number);

    [<DllImport(lib, EntryPoint="ilActiveImage"); SuppressUnmanagedCodeSecurity>]
    extern bool ActiveImage(int Number);

    [<DllImport(lib, EntryPoint="ilActiveLayer"); SuppressUnmanagedCodeSecurity>]
    extern bool ActiveLayer(int Number);

    [<DllImport(lib, EntryPoint="ilActiveMipmap"); SuppressUnmanagedCodeSecurity>]
    extern bool ActiveMipmap(int Number);

    [<DllImport(lib, EntryPoint="ilApplyPal"); SuppressUnmanagedCodeSecurity>]
    extern bool ApplyPal(string FileName);

    [<DllImport(lib, EntryPoint="ilApplyProfile"); SuppressUnmanagedCodeSecurity>]
    extern bool ApplyProfile(string InProfile, string OutProfile);

    [<DllImport(lib, EntryPoint="ilBindImage"); SuppressUnmanagedCodeSecurity>]
    extern void BindImage(int Image);

    [<DllImport(lib, EntryPoint="ilBlit"); SuppressUnmanagedCodeSecurity>]
    extern bool Blit(int Source, int DestX, int DestY, int DestZ, int SrcX, int SrcY, int SrcZ, int Width, int Height, int Depth);

    [<DllImport(lib, EntryPoint="ilClampNTSC"); SuppressUnmanagedCodeSecurity>]
    extern bool ClampNTSC();

    [<DllImport(lib, EntryPoint="ilClearColour"); SuppressUnmanagedCodeSecurity>]
    extern void ClearColour(float32 Red, float32 Green, float32 Blue, float32 Alpha);

    [<DllImport(lib, EntryPoint="ilClearImage"); SuppressUnmanagedCodeSecurity>]
    extern bool ClearImage();

    [<DllImport(lib, EntryPoint="ilCloneCurImage"); SuppressUnmanagedCodeSecurity>]
    extern int CloneCurImage();

    [<DllImport(lib, EntryPoint="ilCompressDXT"); SuppressUnmanagedCodeSecurity>]
    extern nativeint CompressDXT(nativeint Data, int Width, int Height, int Depth, CompressedDataFormat DXTCFormat, int& DXTCSize);

    [<DllImport(lib, EntryPoint="ilCompressFunc"); SuppressUnmanagedCodeSecurity>]
    extern bool CompressFunc(CompressMode Mode);

    [<DllImport(lib, EntryPoint="ilConvertImage"); SuppressUnmanagedCodeSecurity>]
    extern bool ConvertImage(ChannelFormat DestFormat, ChannelType DestType);

    [<DllImport(lib, EntryPoint="ilConvertPal"); SuppressUnmanagedCodeSecurity>]
    extern bool ConvertPal(PaletteType DestFormat);

    [<DllImport(lib, EntryPoint="ilCopyImage"); SuppressUnmanagedCodeSecurity>]
    extern bool CopyImage(int Src);

    [<DllImport(lib, EntryPoint="ilCopyPixels"); SuppressUnmanagedCodeSecurity>]
    extern int CopyPixels(int XOff, int YOff, int ZOff, int Width, int Height, int Depth, ChannelFormat Format, ChannelType Type, void *Data);

    [<DllImport(lib, EntryPoint="ilCreateSubImage"); SuppressUnmanagedCodeSecurity>]
    extern int CreateSubImage(SubImageType Type, int Num);

    [<DllImport(lib, EntryPoint="ilDefaultImage"); SuppressUnmanagedCodeSecurity>]
    extern bool DefaultImage();

    [<DllImport(lib, EntryPoint="ilDeleteImage"); SuppressUnmanagedCodeSecurity>]
    extern void DeleteImage(int Num);

    [<DllImport(lib, EntryPoint="ilDeleteImages"); SuppressUnmanagedCodeSecurity>]
    extern void DeleteImages(nativeint Num, int[] Images);

    [<DllImport(lib, EntryPoint="ilDetermineType"); SuppressUnmanagedCodeSecurity>]
    extern ImageType DetermineType(string FileName);

    [<DllImport(lib, EntryPoint="ilDetermineTypeF"); SuppressUnmanagedCodeSecurity>]
    extern ImageType DetermineTypeF(void* File);

    [<DllImport(lib, EntryPoint="ilDetermineTypeL"); SuppressUnmanagedCodeSecurity>]
    extern ImageType DetermineTypeL(void *Lump, int Size);

    [<DllImport(lib, EntryPoint="ilDisable"); SuppressUnmanagedCodeSecurity>]
    extern bool Disable(EnableCap Mode);

    [<DllImport(lib, EntryPoint="ilDxtcDataToImage"); SuppressUnmanagedCodeSecurity>]
    extern bool DxtcDataToImage();

    [<DllImport(lib, EntryPoint="ilDxtcDataToSurface"); SuppressUnmanagedCodeSecurity>]
    extern bool DxtcDataToSurface();

    [<DllImport(lib, EntryPoint="ilEnable"); SuppressUnmanagedCodeSecurity>]
    extern bool Enable(EnableCap Mode);

    [<DllImport(lib, EntryPoint="ilFlipSurfaceDxtcData"); SuppressUnmanagedCodeSecurity>]
    extern void FlipSurfaceDxtcData();

    [<DllImport(lib, EntryPoint="ilFormatFunc"); SuppressUnmanagedCodeSecurity>]
    extern bool FormatFunc(ChannelFormat Mode);

    [<DllImport(lib, EntryPoint="ilGenImages"); SuppressUnmanagedCodeSecurity>]
    extern void GenImages(nativeint Num, int[] Images);

    [<DllImport(lib, EntryPoint="ilGenImage"); SuppressUnmanagedCodeSecurity>]
    extern int GenImage();

    [<DllImport(lib, EntryPoint="ilGetAlpha"); SuppressUnmanagedCodeSecurity>]
    extern nativeint GetAlpha(ChannelType Type);

    [<DllImport(lib, EntryPoint="ilGetBoolean"); SuppressUnmanagedCodeSecurity>]
    extern bool GetBoolean(BooleanName Mode);

    [<DllImport(lib, EntryPoint="ilGetBooleanv"); SuppressUnmanagedCodeSecurity>]
    extern void GetBooleanv(BooleanName Mode, bool[] Param);
    
    [<DllImport(lib, EntryPoint="ilGetData"); SuppressUnmanagedCodeSecurity>]
    extern nativeint GetData();

    [<DllImport(lib, EntryPoint="ilGetDXTCData"); SuppressUnmanagedCodeSecurity>]
    extern int GetDXTCData(void *Buffer, int BufferSize, CompressedDataFormat DXTCFormat);

    [<DllImport(lib, EntryPoint="ilGetError"); SuppressUnmanagedCodeSecurity>]
    extern ErrorCode GetError();

    [<DllImport(lib, EntryPoint="ilGetInteger"); SuppressUnmanagedCodeSecurity>]
    extern int GetInteger(IntName Mode);

    [<DllImport(lib, EntryPoint="ilGetIntegerv"); SuppressUnmanagedCodeSecurity>]
    extern void GetIntegerv(IntName Mode, int[] Param);

    let GetFormat() =
        GetInteger(IntName.ImageFormat) |> unbox<ChannelFormat>

    let GetDataType() =
        GetInteger(IntName.ImageType) |> unbox<ChannelType>

    [<DllImport(lib, EntryPoint="ilGetLumpPos"); SuppressUnmanagedCodeSecurity>]
    extern int GetLumpPos();
    
    [<DllImport(lib, EntryPoint="ilGetPalette"); SuppressUnmanagedCodeSecurity>]
    extern nativeint GetPalette();

    [<DllImport(lib, EntryPoint="ilGetString"); SuppressUnmanagedCodeSecurity>]
    extern string GetString(StringName StringName);

//    [<DllImport(lib, EntryPoint="ilHint"); SuppressUnmanagedCodeSecurity>]
//    extern void Hint(ILenum Target, ILenum Mode);

    [<DllImport(lib, EntryPoint="ilInvertSurfaceDxtcDataAlpha"); SuppressUnmanagedCodeSecurity>]
    extern bool InvertSurfaceDxtcDataAlpha();

    [<DllImport(lib, EntryPoint="ilInit"); SuppressUnmanagedCodeSecurity>]
    extern void private internalInit();

    let Init() =
        Bootstrap.Init()
        if not initialized then
            initialized <- true
            internalInit()


    [<DllImport(lib, EntryPoint="ilImageToDxtcData"); SuppressUnmanagedCodeSecurity>]
    extern bool ImageToDxtcData(CompressedDataFormat Format);

    [<DllImport(lib, EntryPoint="ilIsDisabled"); SuppressUnmanagedCodeSecurity>]
    extern bool IsDisabled(EnableCap Mode);

    [<DllImport(lib, EntryPoint="ilIsEnabled"); SuppressUnmanagedCodeSecurity>]
    extern bool IsEnabled(EnableCap Mode);

    [<DllImport(lib, EntryPoint="ilIsImage"); SuppressUnmanagedCodeSecurity>]
    extern bool IsImage(int Image);

    [<DllImport(lib, EntryPoint="ilIsValid"); SuppressUnmanagedCodeSecurity>]
    extern bool IsValid(ImageType Type, string FileName);

    [<DllImport(lib, EntryPoint="ilIsValidF"); SuppressUnmanagedCodeSecurity>]
    extern bool IsValidF(ImageType Type, void* File);

    [<DllImport(lib, EntryPoint="ilIsValidL"); SuppressUnmanagedCodeSecurity>]
    extern bool IsValidL(ImageType Type, void *Lump, int Size);

    [<DllImport(lib, EntryPoint="ilKeyColour"); SuppressUnmanagedCodeSecurity>]
    extern void KeyColour(float32 Red, float32 Green, float32 Blue, float32 Alpha);

    [<DllImport(lib, EntryPoint="ilLoad"); SuppressUnmanagedCodeSecurity>]
    extern bool Load(ImageType Type, string FileName);

    [<DllImport(lib, EntryPoint="ilLoadF"); SuppressUnmanagedCodeSecurity>]
    extern bool LoadF(ImageType Type, void* File);

    [<DllImport(lib, EntryPoint="ilLoadImage"); SuppressUnmanagedCodeSecurity>]
    extern bool LoadImage(string FileName);

    [<DllImport(lib, EntryPoint="ilLoadL"); SuppressUnmanagedCodeSecurity>]
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

    [<DllImport(lib, EntryPoint="ilLoadPal"); SuppressUnmanagedCodeSecurity>]
    extern bool LoadPal(string FileName);

    [<DllImport(lib, EntryPoint="ilModAlpha"); SuppressUnmanagedCodeSecurity>]
    extern void ModAlpha(float AlphaValue);

    [<DllImport(lib, EntryPoint="ilOriginFunc"); SuppressUnmanagedCodeSecurity>]
    extern bool OriginFunc(OriginMode Mode);

    [<DllImport(lib, EntryPoint="ilOverlayImage"); SuppressUnmanagedCodeSecurity>]
    extern bool OverlayImage(int Source, int XCoord, int YCoord, int ZCoord);

    [<DllImport(lib, EntryPoint="ilPopAttrib"); SuppressUnmanagedCodeSecurity>]
    extern void PopAttrib();

    [<DllImport(lib, EntryPoint="ilPushAttrib"); SuppressUnmanagedCodeSecurity>]
    extern void PushAttrib(int Bits);

    [<DllImport(lib, EntryPoint="ilRegisterFormat"); SuppressUnmanagedCodeSecurity>]
    extern void RegisterFormat(ChannelFormat Format);

//    [<DllImport(lib, EntryPoint="ilRegisterLoad"); SuppressUnmanagedCodeSecurity>]
//    extern bool RegisterLoad(string Ext, IL_LOADPROC Load);

    [<DllImport(lib, EntryPoint="ilRegisterMipNum"); SuppressUnmanagedCodeSecurity>]
    extern bool RegisterMipNum(int Num);

    [<DllImport(lib, EntryPoint="ilRegisterNumFaces"); SuppressUnmanagedCodeSecurity>]
    extern bool RegisterNumFaces(int Num);

    [<DllImport(lib, EntryPoint="ilRegisterNumImages"); SuppressUnmanagedCodeSecurity>]
    extern bool RegisterNumImages(int Num);

    [<DllImport(lib, EntryPoint="ilRegisterOrigin"); SuppressUnmanagedCodeSecurity>]
    extern void RegisterOrigin(OriginMode Origin);

    [<DllImport(lib, EntryPoint="ilRegisterPal"); SuppressUnmanagedCodeSecurity>]
    extern void RegisterPal(void *Pal, int Size, PaletteType Type);

//    [<DllImport(lib, EntryPoint="ilRegisterSave"); SuppressUnmanagedCodeSecurity>]
//    extern bool RegisterSave(string Ext, IL_SAVEPROC Save);

    [<DllImport(lib, EntryPoint="ilRegisterType"); SuppressUnmanagedCodeSecurity>]
    extern void RegisterType(ChannelType Type);

    [<DllImport(lib, EntryPoint="ilRemoveLoad"); SuppressUnmanagedCodeSecurity>]
    extern bool RemoveLoad(string Ext);

    [<DllImport(lib, EntryPoint="ilRemoveSave"); SuppressUnmanagedCodeSecurity>]
    extern bool RemoveSave(string Ext);

    [<DllImport(lib, EntryPoint="ilResetMemory"); SuppressUnmanagedCodeSecurity>]
    extern void ResetMemory(); // Deprecated

    [<DllImport(lib, EntryPoint="ilResetRead"); SuppressUnmanagedCodeSecurity>]
    extern void ResetRead();

    [<DllImport(lib, EntryPoint="ilResetWrite"); SuppressUnmanagedCodeSecurity>]
    extern void ResetWrite();

    [<DllImport(lib, EntryPoint="ilSave"); SuppressUnmanagedCodeSecurity>]
    extern bool Save(ImageType Type, string FileName);

    [<DllImport(lib, EntryPoint="ilSaveF"); SuppressUnmanagedCodeSecurity>]
    extern int SaveF(ImageType Type, void* File);

    [<DllImport(lib, EntryPoint="ilSaveImage"); SuppressUnmanagedCodeSecurity>]
    extern bool SaveImage(string FileName);

    [<DllImport(lib, EntryPoint="ilSaveL"); SuppressUnmanagedCodeSecurity>]
    extern int SaveL(ImageType Type, void *Lump, int Size);


    [<DllImport(lib, EntryPoint="ilSavePal"); SuppressUnmanagedCodeSecurity>]
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


    [<DllImport(lib, EntryPoint="ilSetAlpha"); SuppressUnmanagedCodeSecurity>]
    extern bool SetAlpha(float AlphaValue);

    [<DllImport(lib, EntryPoint="ilSetData"); SuppressUnmanagedCodeSecurity>]
    extern bool SetData(void *Data);

    [<DllImport(lib, EntryPoint="ilSetDuration"); SuppressUnmanagedCodeSecurity>]
    extern bool SetDuration(int Duration);

    [<DllImport(lib, EntryPoint="ilSetInteger"); SuppressUnmanagedCodeSecurity>]
    extern void SetInteger(IntName Mode, int Param);

//    [<DllImport(lib, EntryPoint="ilSetMemory"); SuppressUnmanagedCodeSecurity>]
//    extern void SetMemory(mAlloc, mFree);

    [<DllImport(lib, EntryPoint="ilSetPixels"); SuppressUnmanagedCodeSecurity>]
    extern void SetPixels(int XOff, int YOff, int ZOff, int Width, int Height, int Depth, ChannelFormat Format, ChannelType Type, void *Data);

//    [<DllImport(lib, EntryPoint="ilSetRead"); SuppressUnmanagedCodeSecurity>]
//    extern void SetRead(fOpenRProc, fCloseRProc, fEofProc, fGetcProc, fReadProc, fSeekRProc, fTellRProc);

    [<DllImport(lib, EntryPoint="ilSetString"); SuppressUnmanagedCodeSecurity>]
    extern void SetString(StringName Mode,  string String);

//    [<DllImport(lib, EntryPoint="ilSetWrite"); SuppressUnmanagedCodeSecurity>]
//    extern void SetWrite(fOpenWProc, fCloseWProc, fPutcProc, fSeekWProc, fTellWProc, fWriteProc);

    [<DllImport(lib, EntryPoint="ilShutDown"); SuppressUnmanagedCodeSecurity>]
    extern void ShutDown();

    [<DllImport(lib, EntryPoint="ilSurfaceToDxtcData"); SuppressUnmanagedCodeSecurity>]
    extern bool SurfaceToDxtcData(CompressedDataFormat Format);

    [<DllImport(lib, EntryPoint="ilTexImage"); SuppressUnmanagedCodeSecurity>]
    extern bool TexImage(int Width, int Height, int Depth, byte NumChannels, ChannelFormat Format, ChannelType Type, void *Data);

    [<DllImport(lib, EntryPoint="ilTexImageDxtc"); SuppressUnmanagedCodeSecurity>]
    extern bool TexImageDxtc(int w, int h, int d, CompressedDataFormat DxtFormat, nativeint data);

    [<DllImport(lib, EntryPoint="ilTypeFromExt"); SuppressUnmanagedCodeSecurity>]
    extern ImageType TypeFromExt(string FileName);

    [<DllImport(lib, EntryPoint="ilTypeFunc"); SuppressUnmanagedCodeSecurity>]
    extern bool TypeFunc(ChannelType Mode);

    [<DllImport(lib, EntryPoint="ilLoadData"); SuppressUnmanagedCodeSecurity>]
    extern bool LoadData(string FileName, int Width, int Height, int Depth, byte Bpp);

    [<DllImport(lib, EntryPoint="ilLoadDataF"); SuppressUnmanagedCodeSecurity>]
    extern bool LoadDataF(void* File, int Width, int Height, int Depth, byte Bpp);

    [<DllImport(lib, EntryPoint="ilLoadDataL"); SuppressUnmanagedCodeSecurity>]
    extern bool LoadDataL(void *Lump, int Size, int Width, int Height, int Depth, byte Bpp);

    [<DllImport(lib, EntryPoint="ilSaveData"); SuppressUnmanagedCodeSecurity>]
    extern bool SaveData(string FileName);


    let check (error : string) (v : bool) =
        if not v then
            let err = GetError()
            printfn "ERROR %A: %s" err error