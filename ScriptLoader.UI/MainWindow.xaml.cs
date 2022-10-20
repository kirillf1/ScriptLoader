using ScriptLoader.UI.ViewModels;
using System;
using System.Windows;

namespace ScriptLoader.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ScriptLoaderViewModel scriptLoaderViewModel;

        public MainWindow(ScriptLoaderViewModel scriptLoaderViewModel)
        {
            InitializeComponent();
            DataContext = scriptLoaderViewModel;
            this.scriptLoaderViewModel = scriptLoaderViewModel;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await scriptLoaderViewModel.LoadScripts();
            }
            catch(Exception ex)
            {
                string messageBoxText = $"Script loading failed! Error message:{ex.Message}";
                string caption = "Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
        }

        private void ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var script = ((FrameworkElement)e.OriginalSource).DataContext as ScriptViewModel;
            if (script != null)
            {
                new ScriptWindow(script.ScriptBody).ShowDialog();
            }
        }
    }
}
