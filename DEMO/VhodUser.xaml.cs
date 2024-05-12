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
using System.Windows.Shapes;

namespace DEMO
{
    /// <summary>
    /// Логика взаимодействия для VhodUser.xaml
    /// </summary>
    public partial class VhodUser : Window
    {
        private string savedPassword = "";
        DataBase dataBase = new DataBase();
        public VhodUser()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select Name, Login, Password from [Users] where Login = '{Login.Text}' and Password = '{Password.Password}'";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Вы успешно вошли!");
                DataBase.FIO = table.Rows[0]["Name"].ToString();
                User user = new User();
                user.Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Такого аккаунта нет");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RegistrationUser user = new RegistrationUser();
            user.Show();
            Hide();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (Password.PasswordChar == '*')
            {
                savedPassword = Password.Password;
                Password.PasswordChar = '\0';
            }
            else
            {
                Password.PasswordChar = '*';
                Password.Password = savedPassword;
            }
        }
    }
}
