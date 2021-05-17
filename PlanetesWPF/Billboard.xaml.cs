using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
