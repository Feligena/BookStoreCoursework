﻿using bookstore.View;
using BookStore.View.MVVM.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace bookstore.View.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для WriteOffsView.xaml
    /// </summary>
    public partial class WriteOffsView : UserControl
    {
        private DbBookStoreEntities _db = DbBookStoreEntities.GetContext();
        public WriteOffsView()
        {
            InitializeComponent();
            WriteOffsDataGrid.ItemsSource = _db.write_offs.ToList();
        }

        private void SearchWriteOffsBtn_CLick(object sender, RoutedEventArgs e)
        {
            //SearchWriteOffsText.Text
            WriteOffsDataGrid.ItemsSource = _db.write_offs.Where(b => b.book.name_book.Contains(SearchWriteOffsText.Text)
                                                                || b.book.author.human.first_name.Contains(SearchWriteOffsText.Text)
                                                                || b.book.author.human.last_name.Contains(SearchWriteOffsText.Text)
                                                                || b.book.genre.name_genre.Contains(SearchWriteOffsText.Text)
                                                                || b.employee.human.first_name.Contains(SearchWriteOffsText.Text)
                                                                || b.employee.human.last_name.Contains(SearchWriteOffsText.Text)
                                                                || b.book.publishing_house.name_pub_house.Contains(SearchWriteOffsText.Text))
                                                          .ToList();
        }
    }
}
