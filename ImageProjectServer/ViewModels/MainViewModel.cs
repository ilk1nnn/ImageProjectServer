using ImageProjectServer.Commands;
using ImageProjectServer.Models;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ImageProjectServer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand StartCommand { get; set; }

        private ObservableCollection<Item> allpaths2;

        public ObservableCollection<Item> AllPaths2
        {
            get { return allpaths2; }
            set { allpaths2 = value; OnPropertyChanged(); }
        }

        public Bitmap stringToImage(string inputString)
        {
            byte[] imageBytes = Encoding.Unicode.GetBytes(inputString);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                return new Bitmap(ms);
            }
        }

        public BitmapImage ToImage(byte[] array)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new System.IO.MemoryStream(array);
            image.EndInit();
            return image;
        }


        //public System.Drawing.Image Base64ToImage(string base64String)
        //{
        //    // Convert Base64 String to byte[]
        //    byte[] imageBytes = Convert.FromBase64CharArray(base64String);
        //    MemoryStream ms = new MemoryStream(imageBytes, 0,
        //      imageBytes.Length);

        //    // Convert byte[] to Image
        //    ms.Write(imageBytes, 0, imageBytes.Length);
        //    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
        //    return image;
        //}



        private Item item;

        public Item Item
        {
            get { return item; }
            set { item = value; OnPropertyChanged(); }
        }

        public string IP_Address { get; set; }

        private ImageBrush img;

        public ImageBrush Img
        {
            get { return img; }
            set { img = value; OnPropertyChanged(); }
        }


        public System.Windows.Shapes.Rectangle rectangle { get; set; }



        public MainViewModel()
        {

            AllPaths2 = new ObservableCollection<Item>();

            StartCommand = new RelayCommand(s =>
            {
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                Console.WriteLine(hostName);
                // Get the IP
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                IP_Address = myIP;
                var ipAddress = IPAddress.Parse($"{IP_Address}"); // Change IP
                var port = 27001; // doesn't work, use 80;
                using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    var endPoint = new IPEndPoint(ipAddress, port);
                    socket.Bind(endPoint);
                    socket.Listen(10);
                    MessageBox.Show($"Listen Over {socket.LocalEndPoint}");

                    //var client = socket.Accept();

                    //var length = 0;
                    //var bytes = new byte[500000];

                    //length = client.Receive(bytes);
                    //var a = ToImage(bytes);
                    //ImageBrush imageBrush = new ImageBrush();
                    //imageBrush.ImageSource = a;
                    //AllPaths2.Add(new Item { Image = a });
                    //var msg = Encoding.UTF8.GetString(bytes);
                    //MessageBox.Show("Sended");



                    while (true)
                    {

                        var client = socket.Accept();



                        var length = 0;
                        var bytes = new byte[500000];
                        length = client.Receive(bytes);
                        var img = ToImage(bytes);
                        var a = img;
                        AllPaths2.Add(new Item { Image = img });
                        MessageBox.Show("Sended");




                    }

















                }
            });
        }

    }
}
