using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace YiyoPlayer {
    /// <summary>
    /// Utility class that provides various helper methods for common tasks
    /// </summary>
    public static class Utils {

        /// <summary>
        /// This method checks whether an active internet connection is available or not
        /// It returns true if a network connection is available and false if it is not available
        /// </summary>
        /// <returns>True if a network connection is available, false otherwise</returns>
        public static bool IsInternetConnection(){
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        /// <summary>
        /// Retrieves the content from the specified URL using HTTP GET request
        /// </summary>
        /// <param name="url">The URL to retrieve the content from</param>
        /// <returns>The content of the URL as a string, or null if an error occurs</returns>
        public static string? GetContentFromURL(string url){
            // Create a new HttpClient instance
            using(var client = new HttpClient()){
                try {
                    // Set a timeout of 10 seconds for the request
                    var cts = new CancellationTokenSource(10000);

                    // Send an asynchronous GET request to the specified URL with a timeout cancellation token
                    var response = client.GetAsync(url, cts.Token).Result;

                    // Ensure that the response status code is a success code
                    response.EnsureSuccessStatusCode();

                    // Read the response content as a string
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Return the response body
                    return responseBody;
                } catch(HttpRequestException) {
                    // If a network error occurs, return null
                    return null;
                } catch(TaskCanceledException) {
                    // If the request times out, return null
                    return null;
                } catch(Exception){
                    // If any other error occurs, return null
                    return null;
                }
            }
        }

        /// <summary>
        /// Reads the contents of a file at the specified path and returns it as a string
        /// </summary>
        /// <param name="path">The path of the file to be read</param>
        /// <returns>The contents of the file as a string, or null if an exception occurs during the reading process</returns>
        public static string? GetContentFromFile(string path){
            try {
                // Reads the contents of the file at the specified path and returns it as a string
                return File.ReadAllText(path);
            } catch(Exception){
                // If an exception occurs during the reading process, returns null
                return null;
            }
        }

        /// <summary>
        /// A simple wrapper method around the MessageBox.Show method for displaying a message to the user
        /// </summary>
        /// <param name="message">The message to be displayed in the message box</param>
        /// <param name="title">The title of the message box. Defaults to an empty string if not specified</param>
        public static void SimpleMessageBox(string message, string title=""){
            // Displays a message box with the given message and title, using the OK button and an information icon
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Deserialize a JSON string to a string array, returning an empty array if the deserialization fails
        /// </summary>
        /// <param name="text">The JSON string to deserialize</param>
        /// <returns>A string array representing the deserialized JSON data</returns>
        public static string[] DeserializeStringToJSON(string text){
            try {
                // Deserialize the JSON string to a string array
                return JsonSerializer.Deserialize<string[]>(text)!;
            } catch(Exception){
                // If deserialization fails, return an empty string array
                return new string[0];
            }
        }
    }
}