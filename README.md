# YiyoPlayer

![YiyoPlayer](https://img.shields.io/badge/version-v1.0.0-blue.svg)

A channel player, has the ability to grab a JSON list and load it into the program

## 1. Setup of a similar project
1. Create a project from the VSCode CLI:
    ```bash
    dotnet new wpf --name YiyoPlayer 
    ```
2. Access the project folder:
    ```bash
    cd YiyoPlayer
    ```
3. Compile the project:
    ```bash
    dotnet build
    ```
4. Add [LibVLCSharp](https://www.nuget.org/packages/LibVLCSharp), [LibVLCSharp.WPF](https://www.nuget.org/packages/LibVLCSharp.WPF) and the work complement [VideoLAN.LibVLC.Windows](https://www.nuget.org/packages/VideoLAN.LibVLC.Windows) for Windows:
    ```bash
    dotnet add package LibVLCSharp
    dotnet add package LibVLCSharp.WPF
    dotnet add package VideoLAN.LibVLC.Windows
    ```
5. After you have compiled, run the project to verify that it works correctly:
    ```bash
    dotnet run
    ```
6. Command on publish:
    ```bash
    dotnet publish -c Release -r win-x86 --self-contained true
    ```

## 2. Annotations

- ICOconvert ([web](https://www.icoconverter.com/)) was used to convert images to icons (_.ico_).
- Annotating async methods with [LibVLC](https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/docs/async_support.md)
- [Best practices](https://code.videolan.org/videolan/LibVLCSharp/blob/master/docs/best_practices.md) with LibVLC