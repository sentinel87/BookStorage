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
using System.Windows.Shapes;

namespace BookStorage.Views
{
    /// <summary>
    /// Interaction logic for BookView.xaml
    /// </summary>
    public partial class BookView : Window
    {
        public BookView()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string key= e.Key.ToString();
            if (key.Length > 1)
                key = " ";
            if (e.Key == Key.Back)
                key = "";
            string phrase = textBox.Text + key;
            if (phrase.Length > 100)
                e.Handled = true;
        }

        private void TextBox_PreviewKeyDown_2(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string key = e.Key.ToString();
            if (key.Length > 1)
                key = " ";
            if (e.Key == Key.Back)
                key = "";
            string phrase = textBox.Text + key;
            if (phrase.Length > 60)
                e.Handled = true;
        }

        private void TextBox_PreviewKeyDown_3(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string key = e.Key.ToString();
            if (key.Length > 1)
                key = " ";
            if (e.Key == Key.Back)
                key = "";
            string phrase = textBox.Text + key;
            if (phrase.Length > 13)
                e.Handled = true;
        }
    }
}
