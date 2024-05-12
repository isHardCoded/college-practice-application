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
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        DataBase dataBase = new DataBase();
        public Admin()
        {
            InitializeComponent();
            RefreshDataGrid();


        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGridView1.SelectedItem;
            if (Status.SelectedIndex == 0)
            {
                
                if (selectedRow != null)
                {
                    int id = Convert.ToInt32(selectedRow["ID_Application"]);

                    DataView dv = (DataView)dataGridView1.ItemsSource;
                    DataTable dt = dv.Table;

                    DataRow rowToDelete = dt.Select($"ID_Application = {id}").FirstOrDefault();

                    if (rowToDelete != null)
                    {
                        dataBase.openConnection();
                        string cmd = $"UPDATE Application SET status = @Status WHERE ID_Application = @ID";
                        SqlCommand createCommand = new SqlCommand(cmd, dataBase.getConnection());
                        createCommand.Parameters.AddWithValue("@Status", "в работе");
                        createCommand.Parameters.AddWithValue("@ID", id);
                        createCommand.ExecuteNonQuery();
                        RefreshDataGrid();
                    }
                    dataBase.closeConnection();
                }
               
                
            }
            else if (Status.SelectedIndex == 1)
            {
                if (selectedRow != null)
                {
                    int id = Convert.ToInt32(selectedRow["ID_Application"]);

                    DataView dv = (DataView)dataGridView1.ItemsSource;
                    DataTable dt = dv.Table;

                    DataRow rowToDelete = dt.Select($"ID_Application = {id}").FirstOrDefault();

                    if (rowToDelete != null)
                    {
                        dataBase.openConnection();
                        string cmd = $"UPDATE Application SET status = @Status WHERE ID_Application = @ID";
                        SqlCommand createCommand = new SqlCommand(cmd, dataBase.getConnection());
                        createCommand.Parameters.AddWithValue("@Status", "Выполнено");
                        createCommand.Parameters.AddWithValue("@ID", id);
                        createCommand.ExecuteNonQuery();
                        RefreshDataGrid();
                    }
                    dataBase.closeConnection();
                }
            }
            if (Performer.SelectedIndex==0)
            {
                if (selectedRow != null)
                {
                    int id = Convert.ToInt32(selectedRow["ID_Application"]);

                    DataView dv = (DataView)dataGridView1.ItemsSource;
                    DataTable dt = dv.Table;

                    DataRow rowToDelete = dt.Select($"ID_Application = {id}").FirstOrDefault();

                    if (rowToDelete != null)
                    {
                        dataBase.openConnection();
                        string cmd = $"UPDATE Application SET IDPerformer = @IDPerformer WHERE ID_Application = @ID";
                        SqlCommand createCommand = new SqlCommand(cmd, dataBase.getConnection());
                        createCommand.Parameters.AddWithValue("@IDPerformer", 1);
                        createCommand.Parameters.AddWithValue("@ID", id);
                        createCommand.ExecuteNonQuery();
                        RefreshDataGrid();
                    }
                    dataBase.closeConnection();
                }
            }
            else
            {

                if (selectedRow != null)
                {
                    int id = Convert.ToInt32(selectedRow["ID_Application"]);

                    DataView dv = (DataView)dataGridView1.ItemsSource;
                    DataTable dt = dv.Table;

                    DataRow rowToDelete = dt.Select($"ID_Application = {id}").FirstOrDefault();

                    if (rowToDelete != null)
                    {
                        dataBase.openConnection();
                        string cmd = $"UPDATE Application SET IDPerformer = @IDPerformer WHERE ID_Application = @ID";
                        SqlCommand createCommand = new SqlCommand(cmd, dataBase.getConnection());
                        createCommand.Parameters.AddWithValue("@IDPerformer", 2);
                        createCommand.Parameters.AddWithValue("@ID", id);
                        createCommand.ExecuteNonQuery();
                        RefreshDataGrid();
                    }
                    dataBase.closeConnection();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)dataGridView1.SelectedItem;

            if (selectedRow != null)
            {
                int id = Convert.ToInt32(selectedRow["ID_Application"]);

                DataView dv = (DataView)dataGridView1.ItemsSource;
                DataTable dt = dv.Table;

                DataRow rowToDelete = dt.Select($"ID_Application = {id}").FirstOrDefault();

                if (rowToDelete != null)
                {
                    dataBase.openConnection();
                    string cmd = "DELETE FROM Application WHERE ID_Application = @ID";
                    SqlCommand createCommand = new SqlCommand(cmd, dataBase.getConnection());
                    createCommand.Parameters.AddWithValue("@ID", id);
                    createCommand.ExecuteNonQuery();
                    dt.Rows.Remove(rowToDelete);
                }
                dataBase.closeConnection();
            }
        }

       
        private void RefreshDataGrid()
        {
            dataBase.openConnection();
            string cmd = "SELECT * FROM Application inner join Performers on Application.IDPerformer=Performers.ID ";
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
    }
}
