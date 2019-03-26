using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookStorage.Helpers;
using BookStorage.Models;

namespace BookStorage.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void MainDatagrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string Name = null;
            var Descriptor = ((PropertyDescriptor)e.PropertyDescriptor);
            AttributeCollection coll = Descriptor.Attributes;
            foreach (Attribute att in coll)
            {
                Type type = att.GetType();

                if (type.Name == "ExpandedAttributes")
                {
                    ExpandedAttributes prpAtt = (ExpandedAttributes)att;
                    Name = prpAtt.HeaderName;
                }
            }
        }

        private void MainGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Book book = (Book)e.Row.Item;
            if(book.OwnershipDate<=DateTime.Now.AddDays(-183))
            {
                e.Row.Background = new SolidColorBrush(Colors.LightGreen);
            }
        }
    }
}
