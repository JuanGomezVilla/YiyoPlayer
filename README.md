# YiyoPlayer
Un reproductor de listas de reproducción (HLS) en Python y en un futuro, en C#


pip install python-vlc
pip install pyinstaller

pyinstaller --hidden-import=python-vlc --noconsole --onefile -i="icon.ico" --name "YiyoPlayer" application.py
pyinstaller --hidden-import=python-vlc --noconsole -i="icon.ico" --name "YiyoPlayer" application.py


https://www.icoconverter.com/






dotnet add package Vlc.DotNet.Core.Interops --version 3.1.0



dotnet add package Vlc.DotNet.Wpf --version 3.1.0
dotnet add package VideoLAN.LibVLC.Windows --version 3.0.18


<vlc:VlcControl x:Name="vlcPlayer" />

using Vlc.DotNet.Core;
using Vlc.DotNet.Wpf;