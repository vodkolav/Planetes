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
        }

        public Billboard(Window owner) : this()
        {
            Owner = owner;
        }      

        public static void Show(string message)
        {
            Billboard b = new Billboard();
            b.Title = message;
            b.Show();            
        }
    }
}
