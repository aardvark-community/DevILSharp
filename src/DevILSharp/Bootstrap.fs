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

    /// <summary>
    /// initialize devil by copying all needed native libraries to the
    /// current directory allowing them to be subsequently loaded.
    /// </summary>
    let Init() = ()
