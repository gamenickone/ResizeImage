using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using Image = System.Drawing.Image;
using Size = System.Drawing.Size;

namespace ResizeImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] paths;
        public MainWindow()
        {
            InitializeComponent();
            Status.Content = "";
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                // Lay duong dan
                paths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                Thread process = new Thread(Process);
                process.Start();
            }
        }

        private void Process()
        {
            string fileName;
            string fileExtention;
            string folderName;
            int width, height;
            foreach (var path in paths)
            {
                folderName = Path.GetDirectoryName(path) + "/Resize";
                if (!Directory.Exists(folderName))
                    Directory.CreateDirectory(folderName);
                fileExtention = Path.GetExtension(path);
                if (fileExtention == ".jpg" || fileExtention == ".png")
                {
                    fileName = Path.GetFileName(path);
                    var name = fileName;
                    Image img = Image.FromFile(path);

                    width = (int)Math.Round(((float)img.Width / 4), MidpointRounding.ToEven) * 4;
                    height = (int)Math.Round(((float)img.Height / 4), MidpointRounding.ToEven) * 4;
                    img = new Bitmap(img, new Size(width, height));
                    try
                    {
                        img.Save(folderName + "/" + fileName);
                    }
                    catch (Exception)
                    {

                    }
                    Dispatcher.Invoke(() =>
                    {
                        Status.Content = name;
                    });
                }
            }
            Dispatcher.Invoke(() =>
            {
                Status.Content = "Done";
            });
        }
    }
}
