using System.Windows;

namespace PlanetesWPF
{
    /// <summary>
    /// Interaction logic for Billboard.xaml
    /// </summary>
    public partial class Billboard : Window
    {
        public Billboard() 
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.Manual;
        }

        public void Show(Window owner, string message)
        {
            Title = message;
            Left = owner.Left + 0.25 * owner.Width;
            Top = owner.Top + 0.25 * owner.Height;
            Show();            
        }
    }
}
