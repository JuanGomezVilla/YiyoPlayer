using System.Windows;
using System.Windows.Input;

namespace YiyoPlayer {
    public partial class MainWindow : Window {
        private VLCPlayer playerChannels;
        private string[] channels;
        private int currentChannel;

        public MainWindow(){
            InitializeComponent();
            playerChannels = new VLCPlayer(this.YiyoPlayer);
            channels = new string[0];
            currentChannel = -1;
            
            //Captura el contenido de la URL
            /*
            
            //Si el contenido es diferente de null
            if(contenido != null){
                //Deserializa las URLS y carga la cantidad de canales existentes
                string[] urls = Utils.deserializeStringToJSON(contenido);
                MessageBox.Show(urls.Length.ToString());
            } else {
                MessageBox.Show("No hay datos");
            }*/
        }

        private void Window_Loaded(object sender, RoutedEventArgs e){
            if(Utils.IsInternetConnection()){
                string? contenido = Utils.GetContentFromFile("prueba.json");
                if(contenido != null){
                    channels = Utils.DeserializeStringToJSON(contenido);
                    if(channels != null && channels.Length >= 1){
                        currentChannel = 0;
                        playCurrentChannel();
                    } else {
                        MessageBox.Show("No hay canales disponibles, lo siento. Puedes cargar los tuyos si lo deseas", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    
                } else {
                    MessageBox.Show("No hay canales disponibles, lo siento. Puedes cargar los tuyos si lo deseas", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            } else {
                MessageBox.Show("Parece que no tienes conexión a Internet", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void playCurrentChannel(){
            playerChannels.Play(channels[currentChannel]);
        }

        private void toggleFullscreen(){
            //Verificamos si está en fullscreen o no
            if(isFullscreen()){
                //Si lo está, cambiamos a normal el borde y la ventana a normal
                WindowStyle = WindowStyle.SingleBorderWindow;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            } else {
                WindowStyle = WindowStyle.None;
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }

            this.navigationTools.Height = isFullscreen() ? 0 : double.NaN;
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e){
            var keyPressed = e.Key;


            if(keyPressed == Key.F || (isFullscreen() && keyPressed == Key.Escape)){
                toggleFullscreen();
            } else if(keyPressed == Key.Space){
                playerChannels.TogglePause();
            } else if(keyPressed == Key.Right && channels.Length != 0){
                if(currentChannel != channels.Length - 1){
                    playerChannels.FinishStream();
                    this.YiyoPlayer.Height = 0;
                    currentChannel++;
                    playCurrentChannel();
                    this.YiyoPlayer.Height = double.NaN;
                } else {
                    MessageBox.Show(
                        "Has llegado al final de la lista de canales",
                        "Canales",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                
            } else if(keyPressed == Key.Left && channels.Length != 0){
                //1. Terminar el stream y reducir el tamaño del reproductor
                playerChannels.FinishStream();
                this.YiyoPlayer.Height = 0;

                //2. Comprobar que no es el ultimo de la lista, en ese caso, repetir
                if(currentChannel != 0){
                    currentChannel--;
                } else {
                    MessageBox.Show(
                        "Ya estás en el primer canal, será recargado",
                        "Canales",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                playCurrentChannel();
                this.YiyoPlayer.Height = double.NaN;
            }
        }

        private bool isFullscreen(){
            return WindowStyle == WindowStyle.None;
        }
    }
}
