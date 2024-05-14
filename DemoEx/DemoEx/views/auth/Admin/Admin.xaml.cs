﻿using DemoEx.views.pagination;
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

namespace DemoEx.views.auth.Admin
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
        }

        public void GetAuth()
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"SELECT login, password FROM Admin WHERE login = '{Login.Text}' and password = '{Password.Password}'";
            SqlCommand command = new SqlCommand(querystring, database.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count == 1)
            {
                MessageBox.Show("Вы успешно вошли!");
                Manager.MainFrame.Navigate(new Uri("views/client/Admin/Admin.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Такого аккаунта нет");
            }
        }


        private void Auth_CLick(object sender, RoutedEventArgs e)
        {
            GetAuth();
        }
    }
}
