using Kaffeeplan.App.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kaffeeplan.App;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Handle Delete globally even when a child control (z. B. TextBox) has focus
        if (e.Key == Key.Delete && Keyboard.Modifiers == ModifierKeys.None)
        {
            if (DataContext is MainViewModel vm && vm.MitarbeiterEntfernenCommand.CanExecute(null))
            {
                vm.MitarbeiterEntfernenCommand.Execute(null);
                e.Handled = true;
            }
        }
    }
}
