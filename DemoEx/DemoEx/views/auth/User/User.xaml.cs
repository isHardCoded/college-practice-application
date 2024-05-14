using DemoEx.views.pagination;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoEx.views.auth.User
{
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : Page
    {
        Database dataBase = new Database();

        public User()
        {
            InitializeComponent();
        }

        public void GetAuth()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"SELECT login, password FROM [User] WHERE login = '{Login.Text}' and password = '{Password.Password}'";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Вы успешно вошли!");
                Manager.MainFrame.Navigate(new Uri("views/client/User/User.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Такого аккаунта нет");
            }
        }

        private void Auth_CLick(object sender, RoutedEventArgs e)
        {
            GetAuth();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Uri("views/register/User/User.xaml", UriKind.Relative));
        }
    }
}
