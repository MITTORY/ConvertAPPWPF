using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Drawing;
using Path = System.IO.Path;
using System.Drawing.Imaging;

namespace ConvertAPPWPF
{
    public partial class MainWindow : Window
    {
        private List<string> _imagePaths = new List<string>();
        private int _currentImageIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            Left.IsEnabled = false;
            Right.IsEnabled = false;
        }

        public void Open_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Multiselect = true;
            op.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp;*.gif)|*.png;*.jpeg;*.jpg;*.bmp;*.gif";
            if (op.ShowDialog() == true)
            {
                Image.Source = new BitmapImage(new Uri(op.FileName));
                Path1.Text = op.FileName;
                _imagePaths = op.FileNames.ToList();
                _currentImageIndex= 0;
                if (_imagePaths.Count > 1)
                {
                    Left.IsEnabled = true;
                    Right.IsEnabled = true;
                }
                else
                {
                    Left.IsEnabled = false;
                    Right.IsEnabled = false;
                }
            }
        }

        private void Save_File(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = Path1.Text;
                string extension = ((ComboBoxItem)ListConvert.SelectedItem).Content.ToString();

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = $"Image files (*{extension})|*{extension}|Icon files (*.ico)|*.ico";


                if (saveFileDialog.ShowDialog() == true)
                {
                    BitmapEncoder encoder;
                    if (ListConvert.SelectedIndex != -1)
                    {
                        int selectedIndex = ListConvert.SelectedIndex;
                        switch (selectedIndex)
                        {
                            case 0:
                                encoder = new PngBitmapEncoder();
                                break;
                            case 1:
                                encoder = new JpegBitmapEncoder();
                                break;
                            case 2:
                                encoder = new BmpBitmapEncoder();
                                break;
                            case 3:
                                encoder = new GifBitmapEncoder();
                                break;
                            default:
                                MessageBox.Show("Invalid selection");
                                return;
                        }

                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)Image.Source));
                        try
                        {
                            using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                            {
                                encoder.Save(stream);
                                MessageBox.Show($"Конвертация в {extension.ToUpper()} прошла успешно!");

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error: {ex.Message}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите формат конвертации из списка");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        } 
        private void Left_Click(object sender, RoutedEventArgs e)
        {
            _currentImageIndex--;
            if (_currentImageIndex < 0)
            {
                _currentImageIndex = _imagePaths.Count - 1;
            }
            Image.Source = new BitmapImage(new Uri(_imagePaths[_currentImageIndex]));
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            _currentImageIndex++;
            if (_currentImageIndex >= _imagePaths.Count)
            {
                _currentImageIndex = 0;
            }
            Image.Source = new BitmapImage(new Uri(_imagePaths[_currentImageIndex]));
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UpdateImage(object sender, RoutedEventArgs e)
        {

        }
    }
}
