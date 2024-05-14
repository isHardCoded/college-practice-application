using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для reg.xaml
    /// </summary>
    public partial class reg : Window
    {
        DataBase DataBase = new DataBase();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public reg()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void reg_button(object sender, RoutedEventArgs e)
        {
            string log = login.Text;
            string pass = password.Password;
            string repass = repassword.Password;
            string mail = email.Text;

            if (repass != pass)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            var roles = role.Text;
            int Role;
            if (roles == "Администратор")
            {
                Role = 2;
            }
            else if (roles == "Пользователь")
            {
                Role = 1;
            }
            else 
            {
                Role = 3;
            }


            string querySelect = "SELECT COUNT(*) FROM Users WHERE username = @Username OR email = @Email";

            using (SqlCommand cmdSelect = new SqlCommand(querySelect, DataBase.getConnection()))
            {
                cmdSelect.Parameters.AddWithValue("@Username", log);
                cmdSelect.Parameters.AddWithValue("@Email", mail);

                try
                {
                    DataBase.openConnection();
                    int count = (int)cmdSelect.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Пользователь с таким логином или email уже зарегистрирован.");
                        return;
                    }

                    string queryInsert = "INSERT INTO Users (username, password, email, role_id) VALUES (@Username, @Password, @Email, @RoleId)";

                    using (SqlCommand cmdInsert = new SqlCommand(queryInsert, DataBase.getConnection()))
                    {
                        cmdInsert.Parameters.AddWithValue("@Username", log);
                        cmdInsert.Parameters.AddWithValue("@Password", pass);
                        cmdInsert.Parameters.AddWithValue("@Email", mail);
                        cmdInsert.Parameters.AddWithValue("@RoleId", Role);

                        int rowsAffected = cmdInsert.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Вы успешно зарегистрированы!");
                            if (roles == "Администратор")
                            {
                                lists lists = new lists();
                                lists.Show();
                                this.Close();
                            }
                            else if (roles == "Пользователь")
                            {
                                Info Info = new Info(log);
                                Info.Show();
                                this.Close();
                            }
                            else
                            {
                                WorkPage WorkPage = new WorkPage(log);
                                WorkPage.Show();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при регистрации.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
                finally
                {
                    DataBase.closeConnection();
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }
    }

}
