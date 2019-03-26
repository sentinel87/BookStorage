using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookStorage.Helpers;
using System.ComponentModel;
using BookStorage.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BookStorage.Views;

namespace BookStorage.ViewModels
{
    public class MainViewModel: BaseViewModel
    {
        private bool dataRetrieve = false;
        private DatabaseConnection Connection { get; set; }

        private int _startPage=1;

        public int StartPage
        {
            get { return _startPage; }
            set
            {
                _startPage = value;
                OnPropertyChanged("StartPage");
            }
        }

        private int _pagesTotal;

        public int PagesTotal
        {
            get { return _pagesTotal; }
            set
            {
                _pagesTotal = value;
                OnPropertyChanged("PagesTotal");
            }
        }

        private int _booksTotal;

        public int BooksTotal
        {
            get { return _booksTotal; }
            set
            {
                _booksTotal = value;
                OnPropertyChanged("BooksTotal");
            }
        }

        private string _searchedText;

        public string SearchedText
        {
            get { return _searchedText; }
            set
            {
                _searchedText = value;
                OnPropertyChanged("SearchedText");
            }
        }

        private int overduedBooks;

        public int OverduedBooks
        {
            get { return overduedBooks; }
            set
            {
                overduedBooks = value;
                OnPropertyChanged("OverduedBooks");
            }
        }


        private ObservableCollection<Book> _books { get; set; }

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                OnPropertyChanged("Books");
            }
        }

        private Book _selectedBook;

        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }

        private Cursor _mouseCursor;

        public Cursor MouseCursor
        {
            get { return _mouseCursor; }
            set
            {
                _mouseCursor = value;
                OnPropertyChanged("MouseCursor");
            }
        }



        public RelayCommand NewBookCommand { get; set; }

        public RelayCommand RemoveCommand { get; set; }

        public RelayCommand UpdateCommand { get; set; }

        public RelayCommand SearchCommand { get; set; }

        public RelayCommand FirstCommand { get; set; }

        public RelayCommand NextCommand { get; set; }

        public RelayCommand PreviousCommand { get; set; }

        public RelayCommand LastCommand { get; set; }


        private bool CanNextExecute()
        {
            return StartPage < PagesTotal;
        }

        private void NextCommandExecute(object obj)
        {
            StartPage += 1;
            getData();
        }

        private bool CanPreviousExecute()
        {
            return StartPage > 1;
        }

        private void PreviousCommandExecute(object obj)
        {
            StartPage -= 1;
            getData();
        }

        private void FirstCommandExecute(object obj)
        {
            StartPage = 1;
            getData();
        }

        private void LastCommandExecute(object obj)
        {
            StartPage = PagesTotal;
            getData();
        }

        private bool CanNewBookCommandExecute()
        {
            return !dataRetrieve;
        }

        private void NewBookCommandExecute(object obj)
        {
            BookView bookView = new BookView();
            BookViewModel model = new BookViewModel(WindowType.NewRecord, null);
            bookView.DataContext = model;
            bookView.ShowDialog();
            if (bookView.DialogResult == true)
            {
                Connection.addBookToDB(model.Book);
                getData();
            }
        }

        private bool CanRemoveCommandExecute()
        {
            return !dataRetrieve;
        }

        private void RemoveCommandExecute(object obj)
        {
            Connection.removeBookFromDB(SelectedBook);
            StartPage = 1;
            getData();
        }

        private bool CanUpdateCommandExecute()
        {
            return SelectedBook != null;
        }

        private void UpdateCommandExecute(object obj)
        {
            BookView bookView = new BookView();
            BookViewModel model=new BookViewModel(WindowType.EditRecord, SelectedBook);
            bookView.DataContext = model;
            bookView.ShowDialog();
            if(bookView.DialogResult==true)
            {
                Connection.updateBookInDB(model.Book);
                getData();
            }
        }

        private void getPages()
        {
            BooksTotal=Connection.getRecordCount(SearchedText);
            OverduedBooks = Connection.getOverduedRecordCount(SearchedText);
            PagesTotal = BooksTotal / 10;
            if (BooksTotal % 10 > 0)
                PagesTotal++;
            if (PagesTotal == 0)
                PagesTotal = 1;
        }

        private bool CanSearchCommandExecute()
        {
            return !dataRetrieve;
        }

        private void SearchCommandExecute(object obj)
        {
            StartPage = 1;
            getData();
        }

        public MainViewModel()
        {
            Connection = new DatabaseConnection();
            Books = new ObservableCollection<Book>();
            SearchCommand = new RelayCommand(SearchCommandExecute, CanSearchCommandExecute);
            NewBookCommand = new RelayCommand(NewBookCommandExecute, CanNewBookCommandExecute);
            RemoveCommand = new RelayCommand(RemoveCommandExecute, CanRemoveCommandExecute);
            UpdateCommand = new RelayCommand(UpdateCommandExecute, CanUpdateCommandExecute);
            NextCommand = new RelayCommand(NextCommandExecute,CanNextExecute);
            PreviousCommand = new RelayCommand(PreviousCommandExecute,CanPreviousExecute);
            FirstCommand = new RelayCommand(FirstCommandExecute, CanPreviousExecute);
            LastCommand = new RelayCommand(LastCommandExecute, CanNextExecute);
            //Connection.fillDB();
            getData();
        }

        private async void getData()
        {
            getPages();           
            MouseCursor = Cursors.Wait;
            dataRetrieve = true;
            int startRecord = (StartPage - 1) * 10;
            List<Book>bookList= await Connection.getDBRecords(startRecord,SearchedText);
            Books = new ObservableCollection<Book>(bookList);
            dataRetrieve = false;
            MouseCursor = Cursors.Arrow;
        }
    }
}
