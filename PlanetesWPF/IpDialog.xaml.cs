using System.Windows;

namespace PlanetesWPF
{
    /// <summary>
    /// Interaction logic for IpDialog.xaml
    /// </summary>
    public partial class IpDialog : Window
    {

        public IpDialog()
        {
            InitializeComponent();
            IP = "192.168.1.11";
        }

        public string IP { 
            get; 
            internal set; 
        }
        public string URL { get =>  "http://" + tbxIP.Text + ":" + tbxPort.Text; }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
