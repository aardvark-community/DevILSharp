namespace DevILSharp

open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open System
open System.IO


module ILU =

    [<Literal>]
    let private lib = "ILU.dll"

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

    [<DllImport(lib, EntryPoint="iluAlienify")>]
    extern bool Alienify();

    [<DllImport(lib, EntryPoint="iluBlurAvg")>]
    extern bool BlurAvg(int Iter);

    [<DllImport(lib, EntryPoint="iluBlurGaussian")>]
    extern bool BlurGaussian(int Iter);

    [<DllImport(lib, EntryPoint="iluBuildMipmaps")>]
    extern bool BuildMipmaps();

    [<DllImport(lib, EntryPoint="iluColoursUsed")>]
    extern int ColoursUsed();

    [<DllImport(lib, EntryPoint="iluCompareImage")>]
    extern bool CompareImage(int Comp);

    [<DllImport(lib, EntryPoint="iluContrast")>]
    extern bool Contrast(float32 Contrast);

    [<DllImport(lib, EntryPoint="iluCrop")>]
    extern bool Crop(int XOff, int YOff, int ZOff, int Width, int Height, int Depth);

    [<DllImport(lib, EntryPoint="iluDeleteImage")>]
    extern void DeleteImage(int Id); // Deprecated

    [<DllImport(lib, EntryPoint="iluEdgeDetectE")>]
    extern bool EdgeDetectE();

    [<DllImport(lib, EntryPoint="iluEdgeDetectP")>]
    extern bool EdgeDetectP();

    [<DllImport(lib, EntryPoint="iluEdgeDetectS")>]
    extern bool EdgeDetectS();

    [<DllImport(lib, EntryPoint="iluEmboss")>]
    extern bool Emboss();

    [<DllImport(lib, EntryPoint="iluEnlargeCanvas")>]
    extern bool EnlargeCanvas(int Width, int Height, int Depth);

    [<DllImport(lib, EntryPoint="iluEnlargeImage")>]
    extern bool EnlargeImage(float32 XDim, float32 YDim, float32 ZDim);

    [<DllImport(lib, EntryPoint="iluEqualize")>]
    extern bool Equalize();

    [<DllImport(lib, EntryPoint="iluErrorString")>]
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

    [<DllImport(lib, EntryPoint="iluConvolution")>]
    extern bool Convolution(int[] matrix, int scale, int bias);

    [<DllImport(lib, EntryPoint="iluFlipImage")>]
    extern bool FlipImage();

    [<DllImport(lib, EntryPoint="iluGammaCorrect")>]
    extern bool GammaCorrect(float32 Gamma);

    [<DllImport(lib, EntryPoint="iluGenImage")>]
    extern int GenImage(); // Deprecated

    [<DllImport(lib, EntryPoint="iluGetImageInfo")>]
    extern void GetImageInfo(Info& Info);

    [<DllImport(lib, EntryPoint="iluGetInteger")>]
    extern int GetInteger(IntName Mode);

    [<DllImport(lib, EntryPoint="iluGetIntegerv")>]
    extern void GetIntegerv(IntName Mode, int[] Param);

    [<DllImport(lib, EntryPoint="iluGetString")>]
    extern string GetString(StringName StringName);

    [<DllImport(lib, EntryPoint="iluImageParameter")>]
    extern void ImageParameter(ImageParameterName PName, int Param);

    [<DllImport(lib, EntryPoint="iluInit")>]
    extern void Init();

    [<DllImport(lib, EntryPoint="iluInvertAlpha")>]
    extern bool InvertAlpha();

    [<DllImport(lib, EntryPoint="iluLoadImage")>]
    extern int LoadImage(string FileName);

    [<DllImport(lib, EntryPoint="iluMirror")>]
    extern bool Mirror();

    [<DllImport(lib, EntryPoint="iluNegative")>]
    extern bool Negative();

    [<DllImport(lib, EntryPoint="iluNoisify")>]
    extern bool Noisify(float32 Tolerance);

    [<DllImport(lib, EntryPoint="iluPixelize")>]
    extern bool Pixelize(int PixSize);

    [<DllImport(lib, EntryPoint="iluRegionfv")>]
    extern void Regionfv(P2f[] Points, int n);

    [<DllImport(lib, EntryPoint="iluRegioniv")>]
    extern void Regioniv(P2i[] Points, int n);

    [<DllImport(lib, EntryPoint="iluReplaceColour")>]
    extern bool ReplaceColour(byte Red, byte Green, byte Blue, float32 Tolerance);

    [<DllImport(lib, EntryPoint="iluRotate")>]
    extern bool Rotate(float32 Angle);

    [<DllImport(lib, EntryPoint="iluRotate3D")>]
    extern bool Rotate3D(float32 x, float32 y, float32 z, float32 Angle);

    [<DllImport(lib, EntryPoint="iluSaturate1f")>]
    extern bool Saturate1f(float32 Saturation);

    [<DllImport(lib, EntryPoint="iluSaturate4f")>]
    extern bool Saturate4f(float32 r, float32 g, float32 b, float32 Saturation);

    [<DllImport(lib, EntryPoint="iluScale")>]
    extern bool Scale(int Width, int Height, int Depth);

    [<DllImport(lib, EntryPoint="iluScaleAlpha")>]
    extern bool ScaleAlpha(float32 scale);

    [<DllImport(lib, EntryPoint="iluScaleColours")>]
    extern bool ScaleColours(float32 r, float32 g, float32 b);

    [<DllImport(lib, EntryPoint="iluSetLanguage")>]
    extern bool SetLanguage(Language Language);

    [<DllImport(lib, EntryPoint="iluSharpen")>]
    extern bool Sharpen(float32 Factor, int Iter);

    [<DllImport(lib, EntryPoint="iluSwapColours")>]
    extern bool SwapColours();

    [<DllImport(lib, EntryPoint="iluWave")>]
    extern bool Wave(float32 Angle);

    let SetFilter(f : Filter) =
        ImageParameter(ImageParameterName.Filter, int f)

    let SetPlacement(p : Placement) =
        ImageParameter(ImageParameterName.Placement, int p)