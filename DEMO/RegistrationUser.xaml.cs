using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для RegistrationUser.xaml
    /// </summary>
    public partial class RegistrationUser : Window
    {
        DataBase dataBase = new DataBase();
        public RegistrationUser()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataBase.FIO = FIO.Text;
            if (FIO.Text != "" && Number.Text != "" && Login.Text != "" && Password.Password != "")
            {
                if (chek())
                {
                    string querystrin = $"insert into [User](Name,Number,Login,Password) values('{FIO.Text}','{Number.Text}','{Login.Text}','{Password.Password}')";
                    SqlCommand comman = new SqlCommand(querystrin, dataBase.getConnection());
                    dataBase.openConnection();
                    if (comman.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Вы успешно зарегистроровались");
                        User user = new User();
                        user.Show();
                        Hide();
                    }
                }

            }
            else
            {
                MessageBox.Show("Пожалуйста заполните все поля");
            }
            dataBase.closeConnection();
           
        }
        private Boolean chek()
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();
            string querystring = $"select Login from [User] where Name = '{Login.Text}'";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            dataAdapter.SelectCommand = command;
            dataAdapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                MessageBox.Show("Такая почта уже зарегистрирована");
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
