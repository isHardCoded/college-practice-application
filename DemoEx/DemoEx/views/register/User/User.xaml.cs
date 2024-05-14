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
using DemoEx.views.pagination;

namespace DemoEx.views.register.User
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

        public void PostRegister()
        {
            if (Name.Text != "" && Surname.Text != "" && Login.Text != "" && Password.Password != "")
            {
                if (CheckLogin())
                {
                    string querystrin = $"INSERT INTO [User] (Name, Surname, Login, Password) VALUES ('{Name.Text}','{Surname.Text}','{Login.Text}','{Password.Password}')";
                    SqlCommand comman = new SqlCommand(querystrin, dataBase.getConnection());
                    dataBase.openConnection();
                    if (comman.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы успешно зарегистроровались. Теперь вы можете войти");
                        Manager.MainFrame.Navigate(new Uri("views/auth/User/User.xaml", UriKind.Relative));
                    }
                }

            }
            else
            {
                MessageBox.Show("Пожалуйста заполните все поля");
            }

            dataBase.closeConnection();
        }

        public bool CheckLogin()
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string querystring = $"SELECT login from [User] WHERE login = '{Login.Text}'";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Такой логин уже зарегистрирован");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void Register_CLick(object sender, RoutedEventArgs e)
        {
            PostRegister();
        }
    }
}
