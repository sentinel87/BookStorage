using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookStorage.Helpers;
using BookStorage.Models;
using System.Collections.ObjectModel;

namespace BookStorage.ViewModels
{
    public class BookViewModel: BaseViewModel
    {
        public RelayCommand AddEditCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        private ObservableCollection<string> _locations;

        public ObservableCollection<string> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }


        private Book book;

        public Book Book
        {
            get { return book; }
            set
            {
                book = value;
                OnPropertyChanged("Book");
            }
        }


        private string _windowType;

        public string WindowType
        {
            get { return _windowType; }
            set
            {
                _windowType = value;
                OnPropertyChanged("WindowType");
            }
        }

        private void AddEditCommandExecute(object obj)
        {
            var window = obj as Window;
            if (window != null)
            {
                window.DialogResult = true;
                window.Close();
            }
        }

        private void CancelCommandExecute(object obj)
        {
            var window = obj as Window;
            if (window != null)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        public BookViewModel(WindowType type,Book book)
        {
            Locations = new ObservableCollection<string>();
            Locations.Add("Jaworzno");
            Locations.Add("Miedźno");
            AddEditCommand = new RelayCommand(AddEditCommandExecute);
            CancelCommand = new RelayCommand(CancelCommandExecute);
            if(type==Models.WindowType.NewRecord)
            {
                Book = new Book();
                WindowType = "Nowa książka";
                Book.OwnershipDate = DateTime.Now;
                Book.Location = "Jaworzno";
            }
            else
            {
                WindowType = "Edycja książki";
                Book = book;
            }
        }
    }
}
