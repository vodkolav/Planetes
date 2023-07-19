using System.Windows;

namespace PlanetesWPF
{
    /// <summary>
    /// Interaction logic for Billboard.xaml
    /// </summary>
    public partial class Billboard : Window
    {
        public Billboard() //TODO: repair
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.Manual;
        }

        public void Show(Window owner, string message)
        {
            Title = message;
            Left = owner.Left + 50;
            Top = owner.Top + 50;
            Show();            
        }
    }
}
