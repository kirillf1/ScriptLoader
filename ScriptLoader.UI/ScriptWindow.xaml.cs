using System.Windows;

namespace ScriptLoader.UI
{
    /// <summary>
    /// Логика взаимодействия для ScriptWindow.xaml
    /// </summary>
    public partial class ScriptWindow : Window
    {
        public ScriptWindow(string scriptBody)
        {
            InitializeComponent();
            ScriptBody = scriptBody;
            DataContext = this;
        }

        public string ScriptBody { get; }
    }
}
