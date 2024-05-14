using System;
using System.Collections.Generic;
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
using LiveCharts;
using LiveCharts.Wpf;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Data;


namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для lists.xaml
    /// </summary>
    public partial class lists : Window
    {
        public ObservableCollection<Order> Orders { get; set; }

        DataBase DataBase = new DataBase();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();
        private int acceptedCount = 0;
        private int rejectedCount = 0;
        private int completedCount = 0;

        public SeriesCollection PieSeriesCollection { get; set; }

        public lists()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Orders = new ObservableCollection<Order>();
            LoadOrdersFromDatabase();
            DataContext = this;
            CalculateAcceptedRejectedCompletedCount();
            BuildPieChart();

        }





        private void CalculateAcceptedRejectedCompletedCount()
        {
            foreach (Order order in Orders)
            {
                if (order.State.Equals("Принят"))
                {
                    acceptedCount++;
                }
                else if (order.State.Equals("Отклонен"))
                {
                    rejectedCount++;
                }
                else if (order.State.Equals("Выполнено"))
                {
                    completedCount++;
                }
            }

            // Далее вы можете использовать переменные acceptedCount, rejectedCount и completedCount в вашем коде для дальнейшего использования.
        }


        private void BuildPieChart()
        {
            PieSeriesCollection = new SeriesCollection
    {
        new PieSeries
        {
            Title = "Принятые",
            Values = new ChartValues<int> { acceptedCount },
            DataLabels = true
        },
        new PieSeries
        {
            Title = "Отклоненные",
            Values = new ChartValues<int> { rejectedCount },
            DataLabels = true
        },
        new PieSeries
        {
            Title = "Выполненные",
            Values = new ChartValues<int> { completedCount },
            DataLabels = true
        }
    };

            DataContext = this;
        }



        // Метод для загрузки заказов из базы данных
        private void LoadOrdersFromDatabase()
        {
            
            string query = "SELECT id, client,work_type,worker,date,adress,state FROM [Orders]";

            using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
            {
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Orders.Add(new Order
                        {
                            OrderNumber = reader["id"].ToString(),
                            CustomerName = reader["client"].ToString(),
                            WorkType = reader["work_type"].ToString(),
                            Worker = reader["worker"].ToString(),
                            Date = reader["date"].ToString(),
                            Address = reader["adress"].ToString(),
                            State = reader["state"].ToString()
                        }); ;
                    }
                }
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {

            Order selectedOrder = ((FrameworkElement)sender).DataContext as Order;
            if (selectedOrder != null)
            {
                selectedOrder.State = "Принят";
                BuildPieChart();

                using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
                {
                    connection.Open();
                    string query = $"UPDATE Orders SET state = 'Принят' WHERE id = {selectedOrder.OrderNumber}";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                // После обновления базы данных обновляем данные в коллекции и интерфейсе
                RefreshOrders();
                

            }
           
        }


        private void AddCommentButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = ((FrameworkElement)sender).DataContext as Order;
            if (selectedOrder != null)
            {
                selectedOrder.State = "Принят";

                TextBox textBox = ((StackPanel)((Button)sender).Parent).Children.OfType<TextBox>().FirstOrDefault();
                if (textBox != null)
                {
                    string comment = textBox.Text;

                    using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
                    {
                        connection.Open();

                        string query = $"UPDATE Orders SET performer = '{comment}' WHERE id = {selectedOrder.OrderNumber}";
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
            Order selectedOrder = ((FrameworkElement)sender).DataContext as Order;
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

        // Обновление данных после отклонения заказа
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            Order selectedOrder = ((FrameworkElement)sender).DataContext as Order;
            if (selectedOrder != null)
            {
                selectedOrder.State = "Отклонен";
                

                using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
                {
                    connection.Open();
                    string query = $"UPDATE Orders SET state = 'Отклонен' WHERE id = {selectedOrder.OrderNumber}";
                    SqlCommand command = new SqlCommand(query, connection);
                    BuildPieChart();
                    command.ExecuteNonQuery();
                }

                // После обновления базы данных обновляем данные в коллекции и интерфейсе
                RefreshOrders();
               

            }
           
        }


       

        // Метод для обновления данных в коллекции и интерфейсе
        private void RefreshOrders()
        {
            Orders.Clear();
            LoadOrdersFromDatabase();
        }

        private void PieChart_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Order
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
   

