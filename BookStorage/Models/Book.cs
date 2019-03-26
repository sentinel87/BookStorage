using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookStorage.Helpers;

namespace BookStorage.Models
{
    [Serializable]
    public class Book: BaseViewModel
    {
        private int _id;

        [ExpandedAttributes(HeaderName ="ID")]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _title;

        [ExpandedAttributes(HeaderName = "Tytuł książki")]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        private string _author;

        [ExpandedAttributes(HeaderName = "Autor")]
        public string Author
        {
            get { return _author; }
            set
            {
                _author = value;
                OnPropertyChanged("Author");
            }
        }

        private DateTime _ownershipDate;

        [ExpandedAttributes(HeaderName = "Data nabycia")]
        public DateTime OwnershipDate
        {
            get { return _ownershipDate; }
            set
            {
                _ownershipDate = value;
                OnPropertyChanged("OwnershipDate");
            }
        }

        private string _location;

        [ExpandedAttributes(HeaderName = "Aktualna lokalizacja")]
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                OnPropertyChanged("Location");
            }
        }

        private string _isbn;

        [ExpandedAttributes(HeaderName = "ISBN")]
        public string Isbn
        {
            get { return _isbn; }
            set
            {
                _isbn = value;
                OnPropertyChanged("Isbn");
            }
        }

    }
}
