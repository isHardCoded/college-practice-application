using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для WorkPage.xaml
    /// </summary>
    public partial class WorkPage : Window
    {

        public ObservableCollection<NewOrder> Orders { get; set; }

        DataBase DataBase = new DataBase();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        public string _username;
        public WorkPage( string username)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Orders = new ObservableCollection<NewOrder>();
            _username = username;
            LoadOrdersFromDatabase(_username);
            DataContext = this;
            InitializeComponent();
            
             
           
        }
        // Метод для загрузки заказов из базы данных
        private void LoadOrdersFromDatabase(string username)
        {
           

            string query = $"SELECT id, client, work_type, worker, date, adress, state, comment FROM [Orders] WHERE performer = '{username}'";

            using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Orders.Add(new NewOrder
                        {
                            OrderNumber = reader["id"].ToString(),
                            CustomerName = reader["client"].ToString(),
                            WorkType = reader["work_type"].ToString(),
                            Worker = reader["worker"].ToString(),
                            Date = reader["date"].ToString(),
                            Address = reader["adress"].ToString(),
                            State = reader["state"].ToString(),
                            Comment = reader["comment"].ToString(),
                            

                        });
                    }
                }
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            NewOrder selectedOrder = ((FrameworkElement)sender).DataContext as NewOrder;
            if (selectedOrder != null)
            {
                selectedOrder.State = "Выполнен";

                using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
                {
                    connection.Open();
                    string query = $"UPDATE Orders SET state = 'Выполнено' WHERE id = {selectedOrder.OrderNumber}";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                // После обновления базы данных обновляем данные в коллекции и интерфейсе
                RefreshOrders();
            }
        }
        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            NewOrder selectedOrder = ((FrameworkElement)sender).DataContext as NewOrder;
            if (selectedOrder != null)
            {
                selectedOrder.State = "Выполнен";

                TextBox textBox = ((StackPanel)((Button)sender).Parent).Children.OfType<TextBox>().FirstOrDefault();
                if (textBox != null)
                {
                    string comment = textBox.Text;

                    using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
                    {
                        connection.Open();

                        string query = $"UPDATE Orders SET comment = '{comment}' WHERE id = {selectedOrder.OrderNumber}";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                    }

                    textBox.Text = ""; // Очистить текстовое поле сразу после добавления комментария
                }

                // После обновления базы данных обновляем данные в коллекции и интерфейсе
                RefreshOrders();
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            NewOrder selectedOrder = ((FrameworkElement)sender).DataContext as NewOrder;
            if (selectedOrder != null)
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
                {
                    connection.Open();
                    string query = $"DELETE FROM Orders WHERE id = {selectedOrder.OrderNumber}";
                    SqlCommand command = new SqlCommand(query, connection);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Заявка успешно удалена из базы данных.");
                        // После удаления обновляем данные в коллекции и интерфейсе
                        RefreshOrders();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить заявку из базы данных.");
                    }
                }
            }
        }

       
        private void RefreshOrders()
        {
            Orders.Clear();
            LoadOrdersFromDatabase(_username);
        }

    }

    public class NewOrder
    {
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string WorkType { get; set; }
        public string Worker { get; set; }
        public string Date { get; set; }
        public string Address { get; set; }
        public string State { get; set; }

        public string Comment { get; set; }
        
    }
}
   


    

