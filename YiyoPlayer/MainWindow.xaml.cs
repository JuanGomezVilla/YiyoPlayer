using System;
using System.IO;
using System.Text.Json;
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e){
            //Verify that the config.json file exists
            if(File.Exists("config.json")){
                //Get the content of that file and convert it to an object of type Configuration
                var configuration = JsonSerializer.Deserialize<Configuration>(Utils.GetContentFromFile("config.json")!);

                //Verifica que la configuración no es un valor nulo, y existe un valor para servidor que no es nulo ni espacios
                if(configuration != null && !String.IsNullOrEmpty(configuration.server) && !String.IsNullOrWhiteSpace(configuration.server)){
                    //Le pasa a una función asíncrona el enlace al servidor
                    loadChannels(configuration.server);
                } else {
                    Utils.SimpleMessageBox("El archivo de configuración no contiene un parámetro para el servidor o está vacío", "Configuración");
                }
            } else {
                Utils.SimpleMessageBox("No existe un archivo de configuración", "Configuración");
            }



            
        }

        private void loadChannels(string urlPath){
            //Si existe conexión a internet
            if(Utils.IsInternetConnection()){
                //Obtiene el contenido de la URL y lo 
                string? contenido = Utils.GetContentFromURL(urlPath);
                if(contenido != null){
                    channels = Utils.DeserializeStringToJSON(contenido);
                    if(channels != null && channels.Length >= 1){
                        currentChannel = 0;
                        playCurrentChannel();
                    } else {
                        
                    }
                    
                } else {
                    Utils.SimpleMessageBox("No hay canales disponibles, lo siento. Puedes cargar los tuyos si lo deseas", "Información");
                }
            } else {
                Utils.SimpleMessageBox("Parece que no tienes conexión a Internet", "Información");
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
                AccionSiguienteCanal();
            } else if(keyPressed == Key.Left && channels.Length != 0){
                AccionAnteriorCanal();
            }
        }


        

        private void AccionSiguienteCanal(){
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
        }

        private void AccionAnteriorCanal(){
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

        //ACCIONES ÚTILES DE LA VENTANA
        private bool isFullscreen(){
            return WindowStyle == WindowStyle.None;
        }

        //ACCIONES DE LOS BOTONES Y LOS ITEMS
        private void menuItemCerrar_Click(object sender, RoutedEventArgs e){
            //Cierra la ventana, terminando el proceso
            this.Close();
        }

        private void menuItemPausar_Click(object sender, RoutedEventArgs e){
            //Cambia el estado del reproductor de reproduciendo a pausa o viceversa
            playerChannels.TogglePause();
        }

        private void menuItemSiguienteCanal_Click(object sender, RoutedEventArgs e){
            //Pasa al siguiente canal
            AccionSiguienteCanal();
        }

        private void menuItemAnteriorCanal_Click(object sender, RoutedEventArgs e){
            //Pasa al canal anterior
            AccionAnteriorCanal();
        }
    }
}
