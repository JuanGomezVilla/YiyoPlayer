# YiyoPlayer

![YiyoPlayer](https://img.shields.io/badge/version-v1.0.0-blue.svg)

A player of channels in the cloud, such as m3u8 (HLS) in C# using LibVLCSharp

## 1. Configuración de un proyecto similar
1. Crea un proyecto desde el CLI de VSCode:
    ```bash
    dotnet new wpf --name YiyoPlayer 
    ```
2. Acceder a la carpeta del proyecto:
    ```bash
    cd YiyoPlayer
    ```
3. Compilar el proyecto:
    ```bash
    dotnet build
    ```
4. Añadir [LibVLCSharp](https://www.nuget.org/packages/LibVLCSharp), [LibVLCSharp.WPF](https://www.nuget.org/packages/LibVLCSharp.WPF) y el complemento de trabajo [VideoLAN.LibVLC.Windows](https://www.nuget.org/packages/VideoLAN.LibVLC.Windows) para Windows:
    ```bash
    dotnet add package LibVLCSharp
    dotnet add package LibVLCSharp.WPF
    dotnet add package VideoLAN.LibVLC.Windows
    ```
5. Tras haber compilado, ejecutar el proyecto para comprobar que funciona correctamente:
    ```bash
    dotnet run
    ```

<TrimMode>CopyUsed</TrimMode>
dotnet publish -c Release -r win-x86 --self-contained true

## 2. Anotaciones

- Se utilizó https://www.icoconverter.com/ para convertir imágenes a iconos.
- Anotaciones de métodos asíncronos con [LibVLC](https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/docs/async_support.md)
- [Best practices](https://code.videolan.org/videolan/LibVLCSharp/blob/master/docs/best_practices.md) with LibVLC  
- Para crear un ejecutable en Python, utiliza el siguiente comando:
    ```bash
    pyinstaller --hidden-import=python-vlc --noconsole --onefile -i="icon.ico" --name "YiyoPlayer" application.py
    ```
