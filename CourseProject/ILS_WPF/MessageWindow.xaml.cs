using System.Windows;
using System.Windows.Input;
namespace ILS_WPF
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public string MessageTitle { get; set; }
        public string MessageText { get; set; }
        public MessageWindow(string title, string text)
        {
            MessageTitle = title;
            MessageText = text;
            DataContext = this;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
