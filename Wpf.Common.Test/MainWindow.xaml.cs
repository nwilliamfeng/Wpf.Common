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

namespace Wpf.Common.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
           
            InitializeComponent();
            this.DataContext = new CommandTest();
            
            this.Loaded += MainWindow_Loaded;
            this.textBox.DragEnter += TextBox_DragEnter;
            this.textBox.DragLeave += TextBox_DragLeave;
            this.textBox.DragOver += TextBox_DragOver;
            this.textBox.Drop += TextBox_Drop;
         //   this.textBox.AllowDrop = true;
            
            this.textBox.TextChanged += TextBox_TextChanged;

            this.textBox.PreviewDragEnter += TextBox_PreviewDragEnter;
            this.textBox.PreviewDragLeave += TextBox_PreviewDragLeave;
            this.textBox.PreviewDragOver += TextBox_PreviewDragOver;
            this.textBox.PreviewDrop += TextBox_PreviewDrop;
            
    
        }

        private void TextBox_PreviewDrop(object sender, DragEventArgs e)
        {
          
            // throw new NotImplementedException();
            Console.WriteLine("TextBox_PreviewDrop");
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_PreviewDragLeave(object sender, DragEventArgs e)
        {
            // throw new NotImplementedException();
            Console.WriteLine("TextBox_PreviewDragLeave");
        }

        private void TextBox_PreviewDragEnter(object sender, DragEventArgs e)
        {
            //   e.Data.
            (sender as TextBox).Cursor = System.Windows.Input.Cursors.Hand;
            Console.WriteLine("TextBox_PreviewDragEnter");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("TextBox_TextChanged");
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
           var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            StringBuilder sb = new StringBuilder();
           
            
            files.ToList().ForEach(x => sb.AppendLine());
            (sender as TextBox).Text = files.FirstOrDefault();
            //e.Data.GetFormats().ToList().ForEach(x =>
            //{
               
            //    if (x == "FileDrop")
            //    {
            //        var result= e.Data.GetData(x) as string[];
            //        result.ToList().ForEach(a => Console.WriteLine(a));
                     
            //    }

            //});
            Console.WriteLine("TextBox_Drop");
        }

        private void TextBox_DragOver(object sender, DragEventArgs e)
        {
            Console.WriteLine("TextBox_DragOver");
        }

        private void TextBox_DragLeave(object sender, DragEventArgs e)
        {
            Console.WriteLine("TextBox_DragLeave");
        }

        private void TextBox_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("TextBox_DragEnter");
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.SetSingleInstance(); 
        }
    }
}
