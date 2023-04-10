using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace YiyoPlayer {
    public partial class MainWindow : Window {
        private VLCPlayer channelPlayer;
        private string[] channels;
        private int currentChannel;

        public MainWindow(){
            InitializeComponent();
            // Creates a channel player from the one that exists in the XAML document
            channelPlayer = new VLCPlayer(this.YiyoPlayer);
            // Start channel list
            channels = new string[0];
            // For now there is no channel working, it receives a negative value
            currentChannel = -1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e){
            // Verify that the config.json file exists
            if(File.Exists("config.json")){
                // Get the content of that file and convert it to an object of type Configuration
                Configuration? configuration = null;
                try {
                    configuration = JsonSerializer.Deserialize<Configuration>(Utils.GetContentFromFile("config.json")!);
                } catch(Exception){

                }

                // Verifies that the setting is not a null value, and a value exists for the server that is not null and does not contain spaces
                if (configuration != null && !String.IsNullOrEmpty(configuration.server) && !String.IsNullOrWhiteSpace(configuration.server)){
                    // Passes the link to the server to an asynchronous function
                    loadChannels(configuration.server);
                } else {
                    Utils.SimpleMessageBox("El archivo de configuración no contiene un parámetro para el servidor o está vacío", "Configuración");
                }
            } else {
                // Creates a JSON configuration file, an object for configurations, and serializes it to the file
                FileStream archivo = File.Create("config.json");
                Configuration settings = new Configuration();
                var optionsSerializer = new JsonSerializerOptions { WriteIndented = true };
                JsonSerializer.SerializeAsync(archivo, settings, optionsSerializer);
                archivo.Close();
            }
        }

        private void loadChannels(string urlPath){
            // Gets the content of the URL
            if(Utils.IsInternetConnection()){
                // Gets the content of the URL
                string? contenido = Utils.GetContentFromURL(urlPath);
                // Verify that content exists
                if(contenido != null){
                    // The content converts it to an array of type string
                    channels = Utils.DeserializeStringToJSON(contenido);
                    // If there are channels
                    if(channels != null && channels.Length >= 1){
                        // Sets the current channel to the first value in the list and updates with the function
                        currentChannel = 0;
                        playCurrentChannel();
                    } else {
                        Utils.SimpleMessageBox("Ahora no hay canales disponibles, lo siento", "Información");
                    }
                } else {
                    Utils.SimpleMessageBox("Ahora no hay canales disponibles, lo siento", "Información");
                }
            } else {
                Utils.SimpleMessageBox("Parece que no tienes conexión a Internet", "Información");
            }
        }

        /// <summary>
        /// Plays the currently selected channel in the channel list
        /// </summary>
        private void playCurrentChannel(){
            // Calls the Play method of the channelPlayer object, passing in the URL of the current channel
            channelPlayer.Play(channels[currentChannel]);
        }

        /// <summary>
        /// Toggles between fullscreen and normal mode for the application window
        /// </summary>
        private void toggleFullscreen(){
            // Check if the screen is in fullscreen
            if(isFullscreen()){
                // If it is, the window and border are changed to normal
                WindowStyle = WindowStyle.SingleBorderWindow;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            } else {
                // Border is removed and becomes maximized window
                WindowStyle = WindowStyle.None;
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            // If fullscreen, remove the toolbar, otherwise a default of double
            this.navigationTools.Height = isFullscreen() ? 0 : double.NaN;
        }
        
        /// <summary>
        /// Handles key press events and performs different actions depending on which key is pressed
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void OnKeyDownHandler(object sender, KeyEventArgs e){
            // Gets the key pressed
            var keyPressed = e.Key;

            // If the key pressed is F or is in Fullscreen and the Escape key has been pressed
            if(keyPressed == Key.F || (isFullscreen() && keyPressed == Key.Escape)){
                // Change full screen state
                toggleFullscreen();
            } else if(keyPressed == Key.Space){
                // If the key pressed is space, the pause state is changed
                channelPlayer.TogglePause();
            } else if(keyPressed == Key.Right && channels.Length != 0){
                // If the right key has been pressed and the number of channels is not 0, it goes to the next channel
                // IMPORTANT: when executing the action it will also check if it is possible to not pass the range
                ActionNextChannel();
            } else if(keyPressed == Key.Left && channels.Length != 0){
                // If the left key has been pressed and the number of channels is not 0, it goes to the previous channel
                // IMPORTANT: when executing the action it will also check if it is possible to not pass the range
                ActionPreviousChannel();
            }
        }

        /// <summary>
        /// Action that moves to the next channel in the array
        /// </summary>
        private void ActionNextChannel(){
            // The current channel has to be different from the maximum allowed minus one to advance
            if(currentChannel != channels.Length - 1){
                // Ends the stream, removes the height of the player, advances in the current channel, updates the player and resets the height of the player
                channelPlayer.FinishStream();
                this.YiyoPlayer.Height = 0;
                currentChannel++;
                playCurrentChannel();
                this.YiyoPlayer.Height = double.NaN;
            } else {
                // Status message
                Utils.SimpleMessageBox("Has llegado al final de la lista de canales", "Información");
            }
        }

        /// <summary>
        /// Action to go back in the channel array
        /// </summary>
        private void ActionPreviousChannel(){
            // Ends the stream, removes the height of the player
            channelPlayer.FinishStream();
            this.YiyoPlayer.Height = 0;

            // Check that the current channel is not the last one in the list, in this case, repeat
            if(currentChannel != 0) currentChannel--;
            else Utils.SimpleMessageBox("Ya estás en el primer canal, será recargado", "Información");
            
            // Refreshes the player and resets the height of the player
            playCurrentChannel();
            this.YiyoPlayer.Height = double.NaN;
        }

        // USEFUL WINDOW ACTIONS

        /// <summary>
        /// Checks if the application window is currently in fullscreen mode
        /// </summary>
        /// <returns>True if the window is in fullscreen mode, false otherwise</returns>
        private bool isFullscreen(){
            // Returns true if the window style is set to None (borderless)
            return WindowStyle == WindowStyle.None;
        }

        // TOOLBAR ACTIONS
        /// <summary>
        /// Handles the click event for the Close menu item
        /// Closes the application window, ending the process
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void menuItemClose_Click(object sender, RoutedEventArgs e){
            // Close the window, ending the process
            this.Close();
        }

        /// <summary>
        /// Handles the click event for the Pause menu item
        /// Changes the state of the player from playing to paused or vice versa
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void menuItemPause_Click(object sender, RoutedEventArgs e){
            //Change the state of the player from playing to paused or vice versa
            channelPlayer.TogglePause();
        }

        /// <summary>
        /// Handles the click event for the Next Channel menu item
        /// Goes to the next channel in the channel list
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void menuItemNextChannel_Click(object sender, RoutedEventArgs e){
            // Go to the next channel
            ActionNextChannel();
        }
        
        /// <summary>
        /// Handles the click event for the Previous Channel menu item
        /// Goes to the previous channel in the channel list
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void menuItemPreviousChannel_Click(object sender, RoutedEventArgs e){
            // Go to the previous channel
            ActionPreviousChannel();
        }

        /// <summary>
        /// Allows user to reload current channels
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void menuItemReloadChannels_Click(object sender, RoutedEventArgs e){
            try {
                // Allows user to reload current channels
                Configuration? settings = JsonSerializer.Deserialize<Configuration>(Utils.GetContentFromFile("config.json")!);
                
                // If there are configurations or the name of the server
                if(settings != null && settings.server != null){
                    // Finish a possible channel playback, and reloads the channels value
                    channelPlayer.FinishStream();
                    loadChannels(settings.server);
                }
            } catch(Exception){
                // Pass
            }
        }

        /// <summary>
        /// Gives the user the ability to change the server address
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void menuItemChangeServer_Click(object sender, RoutedEventArgs e){
            // A window appears to change the server
            InputBox inputDialog = new InputBox("Servidor de canales:", "", "Cambiar servidor");
            bool? newValueServer = inputDialog.ShowDialog();

            // Check if the dialog value is OK
            if(newValueServer == true){
                // Display a confirmation message to perform the action
                MessageBoxResult resultConfirm = MessageBox.Show(
                    "La operación que vas a realizar no es revertible. ¿Estás seguro?",
                    "Confirmar acción",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation
                );

                // If the user confirms to perform the action
                if(resultConfirm == MessageBoxResult.Yes){
                    // Gets the content of the configuration file, if it doesn't exist it creates it
                    var valorArchivo = Utils.GetContentFromFile("config.json");
                    Configuration settings = new Configuration();
                    if(valorArchivo == null) File.Create("config.json").Close();

                    // Open the file, set the server value, and tab the code
                    FileStream archivo = File.OpenWrite("config.json");
                    settings.server = inputDialog.Answer;
                    var optionsSerializer = new JsonSerializerOptions { WriteIndented = true };

                    // Write asynchronously with options with the values ​​and close the file
                    JsonSerializer.SerializeAsync(archivo, settings, optionsSerializer);
                    archivo.Close();

                    // Stop a possible playback and load the new channels
                    channelPlayer.FinishStream();
                    loadChannels(settings.server);
                }
            }
        }
    }
}
