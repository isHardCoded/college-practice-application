using System.Data;
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
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace WpfApp3
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Order> Orders { get; set; }

        DataBase DataBase = new DataBase();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Orders = new ObservableCollection<Order>();
            
        }

        private void auth_button(object sender, RoutedEventArgs e)
        {

            var log = login.Text;
            var pass = password.Text;
            int Role;

            string query = $"select user_id, role_id, username, password from Users where username = '{log}' and password ='{pass}'";

            SqlCommand cmd = new SqlCommand(query, DataBase.getConnection());
            adapter.SelectCommand = cmd;
            adapter.Fill(table);


            DataBase.openConnection();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read()) // Если запрос вернул результат
            {
                Role = Convert.ToInt32(reader["role_id"]); // Получаем значение role_id из результата
                MessageBox.Show($"Вы успешно вошли! Role ID: {Role}");

                if (Role == 1) // ID роли пользователя
                {
                    Info Info = new Info(log);
                    Info.Show();
                    this.Close();
                }
                if (Role == 2) // ID роли пользователя
                {
                    lists lists = new lists();
                    lists.Show();
                    this.Close();
                }
                if (Role == 3)
                {
                    WorkPage workPage = new WorkPage(log);
                    workPage.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Такого аккаунта нет");
            }
            reader.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            reg reg = new reg();
            reg.Show();
            this.Close();
        }
    }
}
