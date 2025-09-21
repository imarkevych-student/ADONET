using _02_CRUD_Interface;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _03_SportShopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SportShopDb db;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["SportShopDBConection"].ConnectionString;
            db = new SportShopDb(connectionString);
        }

      

        private void btnGetAllClients_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = db.GetAllClients();
        }

        private void btnGetAllEmployee_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = db.GetAllEmployees();
        }

        private void btnGetAllSales_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = db.GetAllSales();
        }
    }
}