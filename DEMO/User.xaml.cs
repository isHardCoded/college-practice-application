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
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : Window
    {
        DataBase dataBase = new DataBase();
        public User()
        {
            InitializeComponent();
            dataBase.openConnection();
            string cmd = "SELECT * FROM Application inner join Performers on Application.IDPerformer=Performers.ID where ID_User='" + DataBase.FIO + "'";
            SqlCommand createCommand = new SqlCommand(cmd, dataBase.getConnection());
            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable();
            dataAdp.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                dataGridView1.ItemsSource = dt.DefaultView;
                dataBase.closeConnection();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime currentDate = DateTime.Today;
            if (Equipment.Text != "" && Disrepair.Text != "" && Description.Text != "")
            {

                string querystrin = $"insert into Application(Date,Equipment,Disrepair,Description, ID_User, Status, IDPerformer) values('{currentDate}','{Equipment.Text}','{Disrepair.Text}','{Description.Text}','{DataBase.FIO}','В ожидание',{3})";
                SqlCommand comman = new SqlCommand(querystrin, dataBase.getConnection());
                dataBase.openConnection();
                if (comman.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Вы успешно добавили заявку");
                    dataBase.openConnection();
                    string cmd = "SELECT * FROM Application where ID_User='" + DataBase.FIO + "'";
                    SqlCommand createCommand = new SqlCommand(cmd, dataBase.getConnection());
                    SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
                    DataTable dt = new DataTable();
                    dataAdp.Fill(dt);
                    if (dt.Rows.Count != 0)
                    {
                        dataGridView1.ItemsSource = dt.DefaultView;
                        dataBase.closeConnection();
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста заполните все поля");
                }
                dataBase.closeConnection();
            }
        }
    }
}   
