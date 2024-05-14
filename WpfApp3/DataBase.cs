using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WpfApp3
{
    class DataBase
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False");

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }


       
            public void UpdateOrderState(string orderNumber, string status)
            {
                string query = $"UPDATE Orders SET state = '{status}' WHERE id = '{orderNumber}'";

                using (SqlConnection connection = new SqlConnection(@"Data Source=ISHARDCODED;Initial Catalog=DemoDB;Integrated Security=True;Encrypt=False"))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

 }
