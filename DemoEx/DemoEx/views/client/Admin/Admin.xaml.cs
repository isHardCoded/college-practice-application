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

namespace DemoEx.views.client.Admin
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        Database database = new Database();

        public Admin()
        {
            InitializeComponent();
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            database.openConnection();
            string cmd = "SELECT * FROM Tasks";
            SqlCommand createCommand = new SqlCommand(cmd, database.getConnection());
            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable();
            dataAdp.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                dataGridApp.ItemsSource = dt.DefaultView;
                database.closeConnection();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGridApp.SelectedItem;

            if (selectedRow != null)
            {
                int id = Convert.ToInt32(selectedRow["ID"]);

                DataView dv = (DataView)dataGridApp.ItemsSource;
                DataTable dt = dv.Table;

                DataRow rowToDelete = dt.Select($"ID = {id}").FirstOrDefault();

                if (rowToDelete != null)
                {
                    database.openConnection();
                    string cmd = "DELETE FROM Tasks WHERE ID = @ID";
                    SqlCommand createCommand = new SqlCommand(cmd, database.getConnection());
                    createCommand.Parameters.AddWithValue("@ID", id);
                    createCommand.ExecuteNonQuery();
                    dt.Rows.Remove(rowToDelete);
                }
                database.closeConnection();
            }
        }
    }
}
