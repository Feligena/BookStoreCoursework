﻿using bookstore.View;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace bookstore.View
{
    /// <summary>
    /// Логика взаимодействия для AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        private DbbookstoreEntities _db = DbbookstoreEntities.GetContext();
        private books _currentBook = new books();
       
        public AddBook()
        {
            InitializeComponent();
            DataContext = _currentBook;

            PublishingComboBox.ItemsSource = _db.publishing_house.Where(a => a.is_deleted == false).ToList();
            genresComboBox.ItemsSource = _db.genres.Where(a => a.is_deleted == false).ToList();
            //authorComboBox.ItemsSource = _db.author.Where(a => a.is_deleted == false).Join(_db.human, a => a.id_human, h => h.id,
            //                                                (a, h) => new
            //                                                {
            //                                                    a.id,
            //                                                    nameauthor = h.last_name + " " + h.first_name + " " + h.patronymic,
            //                                                    a.id_human
            //                                                }).ToList();
            authorComboBox.ItemsSource = _db.author.Where(a => a.is_deleted == false).ToList();
        }

        private void SaveBookBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentBook.name_book))
                errors.AppendLine("Укажите название книги");

            if (authorComboBox.Text == null)
                errors.AppendLine("Укажите автора");

            if (_currentBook.publishing_house == null)
                errors.AppendLine("Выберите издательство");

            if (_currentBook.year_publishing == 0)
                errors.AppendLine("Укажите год издания");

            if (_currentBook.number_pages == 0)
                errors.AppendLine("Укажите количество страниц");

            if (_currentBook.genres == null)
                errors.AppendLine("Выберите жанр");

            if (_currentBook.cost_price == 0)
                errors.AppendLine("Укажите себестоимость");

            if (_currentBook.selling_price == 0)
                errors.AppendLine("Укажите стоимость продажи");

            if (_currentBook.amount == 0)
                errors.AppendLine("Укажите количество");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка!");
                return;
            }

            if(_db.books.Any(b => b.name_book == _currentBook.name_book 
                                && b.author.human.last_name == _currentBook.author.human.last_name
                                && b.author.human.first_name == _currentBook.author.human.first_name
                                && b.author.human.patronymic == _currentBook.author.human.patronymic
                                && b.publishing_house.name_pub_house == _currentBook.publishing_house.name_pub_house 
                                && b.year_publishing == _currentBook.year_publishing
                                && b.number_pages == _currentBook.number_pages 
                                && b.genres.name_genre == _currentBook.genres.name_genre
                                && b.is_deleted == false))
            {
                MessageBox.Show("Такая книга уже существует");
                return;
            }

            if (_currentBook.id == 0)
                _db.books.Add(_currentBook);

            try
            {
                _db.SaveChanges();
                MessageBox.Show("Добавлена новая книга");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                this.Close();
            }
        }

        private void AddGenreBtn_Click(object sender, RoutedEventArgs e)
        {
            var addGenre = new AddgenresWindow();
            addGenre.ShowDialog();
        }

        private void AddPubHouseBtn_Click(object sender, RoutedEventArgs e)
        {
            var addPubHouse = new AddPublishingHouseWindow();
            addPubHouse.ShowDialog();
        }

        private void AddauthorBtn_Click(object sender, RoutedEventArgs e)
        {
            var addauthor = new AddauthorWindow();
            addauthor.ShowDialog();
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
