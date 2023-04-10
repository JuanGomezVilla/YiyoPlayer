using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace YiyoPlayer {
    public partial class InputBox : Window {
		public InputBox(string question, string defaultAnswer = "", string title = ""){
			InitializeComponent();
			lblQuestion.Content = question;
			txtAnswer.Text = defaultAnswer;
			this.Title = title;
		}

		private void btnDialogOk_Click(object sender, RoutedEventArgs e){
			this.DialogResult = true;
		}

		private void Window_ContentRendered(object sender, EventArgs e){
			txtAnswer.SelectAll();
			txtAnswer.Focus();
		}

		public string Answer {
			get { return txtAnswer.Text; }
		}
	}
}