using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures;
using Vlc.DotNet.Wpf;

namespace YiyoPlayer {
    public partial class MainWindow : Window {
        
        private bool isPlaying = true;

        public MainWindow(){
            InitializeComponent();
            
            //DETECCIÓN DE EVENTOS
            this.YiyoPlayer.MouseDoubleClick += VlcControl_MouseDoubleClick;
            this.YiyoPlayer.KeyDown += VlcControl_PreviewKeyDown;

            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));

            this.YiyoPlayer.SourceProvider.CreatePlayer(libDirectory);
            this.YiyoPlayer.SourceProvider.MediaPlayer.Play(new Uri("https://linear-489.frequency.stream/dist/vix/489/hls/master/playlist.m3u8"));
            isPlaying = true;
        }

        private void VlcControl_MouseDoubleClick(object sender, MouseButtonEventArgs e){
            //this.YiyoPlayer.SourceProvider.MediaPlayer.Pause();
            MessageBox.Show("hola");
        }

        private void VlcControl_PreviewKeyDown(object sender, KeyEventArgs e){
            MessageBox.Show("Stopeado");
        }
    }
}
