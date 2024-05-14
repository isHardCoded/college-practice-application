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
using QRCoder;
using System.Drawing.Imaging;
using System.IO;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для Info.xaml
    /// </summary>
    public partial class Info : Window
    {

        DataBase DataBase = new DataBase();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public string _username;

        public Info(string username)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _username = username;
            DataFill();

            usernameLabel.Text = _username;
        }
        private void ShowQrCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://docs.google.com/forms/d/e/1FAIpQLSc4NekKUvSkeh7ODPR4SC-CYe40c6jhrBo6MKK0dvueiF2HTA/viewform?usp=sf_link"; // URL, который будет использоваться для создания QR-кода

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            System.Drawing.Bitmap qrCodeImage = qrCode.GetGraphic(20); // Размер QR-кода

            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new MemoryStream(stream.ToArray());
                image.EndInit();

                QrCodeImageHolder.Source = image; // QrCodeImageHolder - элемент Image на форме WPF, где будет отображаться QR-код
            }
        }

        public void DataFill()
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
            {
                connection.Open();

                string query = $"SELECT u.username AS Логин, o.id AS ID_Заказа, o.client AS Ваш_ID, o.work_type AS Тип_работы, o.worker AS Тип_работника, o.date AS Дата, o.adress AS Адресс, o.state AS Состояние, o.performer Исполнитель_работ, o.comment AS Коментарий FROM Orders o JOIN users u ON o.client = u.user_id WHERE u.username = '{_username}'";
                SqlCommand command = new SqlCommand(query, DataBase.getConnection());

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dataTable);
            }

            GridList.ItemsSource = dataTable.DefaultView;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataBase.openConnection();
            var Desc = description.Text;
            var WorkMan = specialist.Text;
            var Target = adress.Text;
            var Data = Date.Text;

            string queryGetUserId = $"SELECT user_id FROM users WHERE username = '{_username}'";

            using (SqlCommand cmdGetUserId = new SqlCommand(queryGetUserId, DataBase.getConnection()))
            {
                int userId = (int)cmdGetUserId.ExecuteScalar();

                string queryInsert = "INSERT INTO [Orders] (client, work_type, worker, date, adress) VALUES (@UserId, @Description, @worker, @date, @adress)";

                using (SqlCommand cmdInsert = new SqlCommand(queryInsert, DataBase.getConnection()))
                {
                    cmdInsert.Parameters.AddWithValue("@UserId", userId);
                    cmdInsert.Parameters.AddWithValue("@Description", Desc);
                    cmdInsert.Parameters.AddWithValue("@worker", WorkMan);
                    cmdInsert.Parameters.AddWithValue("@date", Data);
                    cmdInsert.Parameters.AddWithValue("@adress", Target);

                    int rowsAffected = cmdInsert.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        DataFill();
                        MessageBox.Show("Заявка успешно отправлена!");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка.");
                    }
                }
            }

            DataBase.closeConnection();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Update(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = ((DataView)GridList.ItemsSource).Table;
            string connectionString = @"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.SelectCommand = new SqlCommand("SELECT * FROM Orders", connection);
                adapter.UpdateCommand = builder.GetUpdateCommand();

                adapter.Update(dataTable);
            }

            MessageBox.Show("Изменения успешно сохранены в базе данных.");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
