namespace DevILSharp

open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open System
open System.Security
open System.IO


module ILU =

    [<Literal>]
    let private lib = "ILU"

    type Info =
        struct
            /// <summary> the image's id </summary>
            val mutable public Id : int                   
            
            /// <summary> the image's data </summary>
            val mutable public Data : nativeint       
                
            /// <summary> the image's width </summary>
            val mutable public Width : int             
               
            /// <summary> the image's height </summary>
            val mutable public Height : int               

            /// <summary> the image's depth </summary>
            val mutable public Depth : int                

            /// <summary> bytes per pixel (not bits) of the image </summary>
            val mutable public Bpp : byte                 

            /// <summary> the total size of the data (in bytes) </summary>
            val mutable public SizeOfData : int           

            /// <summary> image format (in IL enum style) </summary>
            val mutable public Format : ChannelFormat     

            /// <summary> image type (in IL enum style) </summary>
            val mutable public Type : ChannelType         

            /// <summary> origin of the image </summary>
            val mutable public Origin : OriginMode        

            /// <summary> the image's palette </summary>
            val mutable public Palette : nativeint        

            /// <summary> palette type </summary>
            val mutable public PalType : PaletteType      

            /// <summary> palette size </summary>
            val mutable public PalSize : int              

            /// <summary> flags for what cube map sides are present </summary>
            val mutable public CubeFlags : CubeFace       

            /// <summary> number of images following </summary>
            val mutable public NumNext : int              

            /// <summary> number of mipmaps </summary>
            val mutable public NumMips : int              

            /// <summary> number of layers </summary>
            val mutable public NumLayers : int            

            override x.ToString() =
                let str = 
                    String.concat "; " [ 
                        sprintf "Id = %A" x.Id
                        sprintf "Width = %A" x.Width
                        sprintf "Height = %A" x.Height
                        sprintf "Depth = %A" x.Depth
                        sprintf "Bpp = %A" x.Bpp
                        sprintf "SizeOfData = %A" x.SizeOfData
                        sprintf "Type = %A" x.Type
                        sprintf "Origin = %A" x.Origin
                        sprintf "PalType = %A" x.PalType
                        sprintf "PalSize = %A" x.PalSize
                        sprintf "CubeFlags = %A" x.CubeFlags
                        sprintf "NumNext = %A" x.NumNext
                        sprintf "NumMips = %A" x.NumMips
                        sprintf "NumLayers = %A" x.NumLayers
                    ]
                sprintf "ILU.Info { %s }" str

        end

    type P2f =
        struct
            val mutable public x : float32
            val mutable public y : float32

            new(x,y) = { x = x; y = y }
        end

    type P2i =
        struct
            val mutable public x : int
            val mutable public y : int

            new(x,y) = { x = x; y = y }
        end

    [<DllImport(lib, EntryPoint="iluAlienify"); SuppressUnmanagedCodeSecurity>]
    extern bool Alienify();

    [<DllImport(lib, EntryPoint="iluBlurAvg"); SuppressUnmanagedCodeSecurity>]
    extern bool BlurAvg(int Iter);

    [<DllImport(lib, EntryPoint="iluBlurGaussian"); SuppressUnmanagedCodeSecurity>]
    extern bool BlurGaussian(int Iter);

    [<DllImport(lib, EntryPoint="iluBuildMipmaps"); SuppressUnmanagedCodeSecurity>]
    extern bool BuildMipmaps();

    [<DllImport(lib, EntryPoint="iluColoursUsed"); SuppressUnmanagedCodeSecurity>]
    extern int ColoursUsed();

    [<DllImport(lib, EntryPoint="iluCompareImage"); SuppressUnmanagedCodeSecurity>]
    extern bool CompareImage(int Comp);

    [<DllImport(lib, EntryPoint="iluContrast"); SuppressUnmanagedCodeSecurity>]
    extern bool Contrast(float32 Contrast);

    [<DllImport(lib, EntryPoint="iluCrop"); SuppressUnmanagedCodeSecurity>]
    extern bool Crop(int XOff, int YOff, int ZOff, int Width, int Height, int Depth);

    [<DllImport(lib, EntryPoint="iluDeleteImage"); SuppressUnmanagedCodeSecurity>]
    extern void DeleteImage(int Id); // Deprecated

    [<DllImport(lib, EntryPoint="iluEdgeDetectE"); SuppressUnmanagedCodeSecurity>]
    extern bool EdgeDetectE();

    [<DllImport(lib, EntryPoint="iluEdgeDetectP"); SuppressUnmanagedCodeSecurity>]
    extern bool EdgeDetectP();

    [<DllImport(lib, EntryPoint="iluEdgeDetectS"); SuppressUnmanagedCodeSecurity>]
    extern bool EdgeDetectS();

    [<DllImport(lib, EntryPoint="iluEmboss"); SuppressUnmanagedCodeSecurity>]
    extern bool Emboss();

    [<DllImport(lib, EntryPoint="iluEnlargeCanvas"); SuppressUnmanagedCodeSecurity>]
    extern bool EnlargeCanvas(int Width, int Height, int Depth);

    [<DllImport(lib, EntryPoint="iluEnlargeImage"); SuppressUnmanagedCodeSecurity>]
    extern bool EnlargeImage(float32 XDim, float32 YDim, float32 ZDim);

    [<DllImport(lib, EntryPoint="iluEqualize"); SuppressUnmanagedCodeSecurity>]
    extern bool Equalize();

    [<DllImport(lib, EntryPoint="iluErrorString"); SuppressUnmanagedCodeSecurity>]
    extern nativeint private ErrorStringInternal(ErrorCode Error);

    let ErrorString(code : ErrorCode) =
        let ptr = ErrorStringInternal(code)

        let rec readToEnd(ptr : nativeint) =
            let b = Marshal.ReadByte ptr
            if b <> 0uy then
                b::readToEnd(ptr + 1n)
            else
                []

        let data = readToEnd ptr |> List.toArray
        System.Text.ASCIIEncoding.ASCII.GetString(data)

    [<DllImport(lib, EntryPoint="iluConvolution"); SuppressUnmanagedCodeSecurity>]
    extern bool Convolution(int[] matrix, int scale, int bias);

    [<DllImport(lib, EntryPoint="iluFlipImage"); SuppressUnmanagedCodeSecurity>]
    extern bool FlipImage();

    [<DllImport(lib, EntryPoint="iluGammaCorrect"); SuppressUnmanagedCodeSecurity>]
    extern bool GammaCorrect(float32 Gamma);

    [<DllImport(lib, EntryPoint="iluGenImage"); SuppressUnmanagedCodeSecurity>]
    extern int GenImage(); // Deprecated

    [<DllImport(lib, EntryPoint="iluGetImageInfo"); SuppressUnmanagedCodeSecurity>]
    extern void GetImageInfo(Info& Info);

    [<DllImport(lib, EntryPoint="iluGetInteger"); SuppressUnmanagedCodeSecurity>]
    extern int GetInteger(IntName Mode);

    [<DllImport(lib, EntryPoint="iluGetIntegerv"); SuppressUnmanagedCodeSecurity>]
    extern void GetIntegerv(IntName Mode, int[] Param);

    [<DllImport(lib, EntryPoint="iluGetString"); SuppressUnmanagedCodeSecurity>]
    extern string GetString(StringName StringName);

    [<DllImport(lib, EntryPoint="iluImageParameter"); SuppressUnmanagedCodeSecurity>]
    extern void ImageParameter(ImageParameterName PName, int Param);

    [<DllImport(lib, EntryPoint="iluInit"); SuppressUnmanagedCodeSecurity>]
    extern void Init();

    [<DllImport(lib, EntryPoint="iluInvertAlpha"); SuppressUnmanagedCodeSecurity>]
    extern bool InvertAlpha();

    [<DllImport(lib, EntryPoint="iluLoadImage"); SuppressUnmanagedCodeSecurity>]
    extern int LoadImage(string FileName);

    [<DllImport(lib, EntryPoint="iluMirror"); SuppressUnmanagedCodeSecurity>]
    extern bool Mirror();

    [<DllImport(lib, EntryPoint="iluNegative"); SuppressUnmanagedCodeSecurity>]
    extern bool Negative();

    [<DllImport(lib, EntryPoint="iluNoisify"); SuppressUnmanagedCodeSecurity>]
    extern bool Noisify(float32 Tolerance);

    [<DllImport(lib, EntryPoint="iluPixelize"); SuppressUnmanagedCodeSecurity>]
    extern bool Pixelize(int PixSize);

    [<DllImport(lib, EntryPoint="iluRegionfv"); SuppressUnmanagedCodeSecurity>]
    extern void Regionfv(P2f[] Points, int n);

    [<DllImport(lib, EntryPoint="iluRegioniv"); SuppressUnmanagedCodeSecurity>]
    extern void Regioniv(P2i[] Points, int n);

    [<DllImport(lib, EntryPoint="iluReplaceColour"); SuppressUnmanagedCodeSecurity>]
    extern bool ReplaceColour(byte Red, byte Green, byte Blue, float32 Tolerance);

    [<DllImport(lib, EntryPoint="iluRotate"); SuppressUnmanagedCodeSecurity>]
    extern bool Rotate(float32 Angle);

    [<DllImport(lib, EntryPoint="iluRotate3D"); SuppressUnmanagedCodeSecurity>]
    extern bool Rotate3D(float32 x, float32 y, float32 z, float32 Angle);

    [<DllImport(lib, EntryPoint="iluSaturate1f"); SuppressUnmanagedCodeSecurity>]
    extern bool Saturate1f(float32 Saturation);

    [<DllImport(lib, EntryPoint="iluSaturate4f"); SuppressUnmanagedCodeSecurity>]
    extern bool Saturate4f(float32 r, float32 g, float32 b, float32 Saturation);

    [<DllImport(lib, EntryPoint="iluScale"); SuppressUnmanagedCodeSecurity>]
    extern bool Scale(int Width, int Height, int Depth);

    [<DllImport(lib, EntryPoint="iluScaleAlpha"); SuppressUnmanagedCodeSecurity>]
    extern bool ScaleAlpha(float32 scale);

    [<DllImport(lib, EntryPoint="iluScaleColours"); SuppressUnmanagedCodeSecurity>]
    extern bool ScaleColours(float32 r, float32 g, float32 b);

    [<DllImport(lib, EntryPoint="iluSetLanguage"); SuppressUnmanagedCodeSecurity>]
    extern bool SetLanguage(Language Language);

    [<DllImport(lib, EntryPoint="iluSharpen"); SuppressUnmanagedCodeSecurity>]
    extern bool Sharpen(float32 Factor, int Iter);

    [<DllImport(lib, EntryPoint="iluSwapColours"); SuppressUnmanagedCodeSecurity>]
    extern bool SwapColours();

    [<DllImport(lib, EntryPoint="iluWave"); SuppressUnmanagedCodeSecurity>]
    extern bool Wave(float32 Angle);

    let SetFilter(f : Filter) =
        ImageParameter(ImageParameterName.Filter, int f)

    let SetPlacement(p : Placement) =
        ImageParameter(ImageParameterName.Placement, int p)