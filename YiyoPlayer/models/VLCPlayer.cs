using System;
using System.Windows.Media;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;

namespace YiyoPlayer {
    public class VLCPlayer {
        public LibVLC libVLC;
        public LibVLCSharp.Shared.MediaPlayer mediaPlayer;
        public VideoView yiyoPlayer;

        public VLCPlayer(VideoView yiyoPlayer){
            Core.Initialize();
            this.libVLC = new LibVLC();
            this.mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(this.libVLC);
            this.yiyoPlayer = yiyoPlayer;
            this.yiyoPlayer.MediaPlayer = this.mediaPlayer;
            this.yiyoPlayer.Background = Brushes.Black;
        }

        public void FinishStream(){
            this.mediaPlayer.Stop();
            this.yiyoPlayer.Background = new SolidColorBrush(Colors.Black);
        }

        public void Play(string url){
            this.mediaPlayer.Play(new Media(this.libVLC, new Uri(url)));
        }

        public void TogglePause(){
            this.mediaPlayer.Pause();
        }

        public bool IsPlaying(){
            return this.mediaPlayer.IsPlaying;
        }

    }
}