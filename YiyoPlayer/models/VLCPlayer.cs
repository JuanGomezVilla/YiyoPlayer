using System;
using System.Windows.Media;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;

namespace YiyoPlayer {

    /// <summary>
    /// Initializes a new instance of the VLCPlayer class with the specified VideoView control
    /// </summary>
    /// <param name="yiyoPlayer">The VideoView control to be used for playback</param>
    public class VLCPlayer {
        public LibVLC libVLC;
        public LibVLCSharp.Shared.MediaPlayer mediaPlayer;
        public VideoView yiyoPlayer;

        public VLCPlayer(VideoView yiyoPlayer){
            // Initializes the LibVLC library and sets up the MediaPlayer and VideoView controls
            Core.Initialize();
            this.libVLC = new LibVLC("--verbose=4"); //, "--reset-plugins-cache"
            this.mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(this.libVLC);
            this.yiyoPlayer = yiyoPlayer;
            this.yiyoPlayer.MediaPlayer = this.mediaPlayer;
            this.yiyoPlayer.Background = Brushes.Black;
        }

        /// <summary>
        /// Stops playback and resets the VideoView background to black
        /// </summary>
        public void FinishStream(){
            // Stops playback and resets the VideoView background to black
            this.mediaPlayer.Stop();
            this.yiyoPlayer.Background = new SolidColorBrush(Colors.Black);
        }
        
        /// <summary>
        /// Plays the video at the specified URL
        /// </summary>
        /// <param name="url">The URL of the video to be played</param>
        public void Play(string url){
            // Plays the video at the specified URL
            this.mediaPlayer.Play(new Media(this.libVLC, new Uri(url)));
        }

        /// <summary>
        /// Pauses or resumes playback, depending on the current state of the MediaPlayer
        /// </summary>
        public void TogglePause(){
            // Pauses or resumes playback, depending on the current state of the MediaPlayer
            this.mediaPlayer.Pause();
        }
    }
}