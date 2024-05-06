using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace WPF_Webcam
{
    public partial class MainWindow : Window
    {
        private BackgroundWorker backgroundWorker;
        public Cloudinary cloudinary;
        public const string CLOUND_NAME = "dan0stbfi";
        public const string API_KEY = "687237422157452";
        public const string API_SECRET = "XQbEo1IhkXxbC24rHvpdNJ5BHNw";
        public string imagePath;

        private String folder_path;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            Closed += Window_Closed;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += background_DoWork;
            backgroundWorker.RunWorkerCompleted += background_RunCompleted;
        }



        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load available video devices
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevices.Count > 0)
            {
                // Select the first available video device
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
            }
            else
            {
                System.Windows.MessageBox.Show("No video devices found.");
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Update the frame in the UI
            Dispatcher.Invoke(() =>
            {
                frameCamera.Content = new System.Windows.Controls.Image() { Source = BitmapToImageSource(eventArgs.Frame) };
            });
        }

        private BitmapSource BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Stop the video source when the window is closed
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }



        private void btnBrowes_Click(object sender, RoutedEventArgs e)
        {
            using (var folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = folderBrowserDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    // Lấy đường dẫn thư mục được chọn
                    string folderPath = folderBrowserDialog.SelectedPath;

                    // Set đường dẫn vào TextBox
                    txtBrowes.Text = folderPath;
                    folder_path = folderPath;
                }
            }
        }

        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            // Ensure that the video source is running
            if (videoSource != null && videoSource.IsRunning)
            {
                // Capture the current frame
                BitmapSource capturedBitmapSource = ((System.Windows.Controls.Image)frameCamera.Content).Source as BitmapSource;

                if (capturedBitmapSource != null)
                {
                    // Convert BitmapSource to Bitmap
                    Bitmap capturedBitmap = BitmapFromSource(capturedBitmapSource);

                    // Generate a unique file name (you can use your logic here)
                    string fileName = $"capture_{DateTime.Now:yyyyMMddHHmmss}.png";

                    // Combine the file name with the desired path
                    string filePath = Path.Combine(txtBrowes.Text, fileName);

                    try
                    {
                        // Save the captured bitmap to the specified file path in PNG format
                        capturedBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                        System.Windows.MessageBox.Show($"Capture saved to {filePath}");

                        // Add the captured image and filename to the ListView
                        AddImageToListView(filePath, capturedBitmap);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Error saving capture: {ex.Message}");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Error capturing frame.");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Video source is not running.");
            }
        }

        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder(); // Use PNG format
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        private void AddImageToListView(string filePath, Bitmap capturedBitmap)
        {
            // Create a data structure to hold image information
            var imageInfo = new
            {
                ImageSource = BitmapToImageSource(capturedBitmap),
                FileName = Path.GetFileName(filePath)
            };

            // Add the data structure to the ListView
            listView1.Items.Add(imageInfo);
        }


        private void frameCamera_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem != null)
            {
                // Lấy thông tin về hình ảnh được chọn trong ListView
                dynamic selectedImage = listView1.SelectedItem;

                // Lấy tên file và đường dẫn
                string fileName = selectedImage.FileName;
                string filePath = Path.Combine(txtBrowes.Text, fileName);

                // Gọi hàm xóa hình ảnh từ ổ đĩa cục bộ
                DeleteImageLocal(filePath);

                // Xóa item khỏi ListView
                listView1.Items.Remove(listView1.SelectedItem);
            }
            else
            {
                System.Windows.MessageBox.Show("Please select an image to delete.");
            }
        }


        private void DeleteImageLocal(string filePath)
        {
            try
            {
                // Kiểm tra xem file có tồn tại không
                if (File.Exists(filePath))
                {
                    // Xóa file từ ổ đĩa cục bộ
                    File.Delete(filePath);
                    System.Windows.MessageBox.Show("Image deleted successfully.");
                }
                else
                {
                    System.Windows.MessageBox.Show("Image file not found.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error deleting image locally: {ex.Message}");
            }
        }

        private void btnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image | *.jpg;*.jpeg;*.png";
            DialogResult result = dlg.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFilePath = dlg.FileName;

                // Hiển thị hình ảnh trên đối tượng Image trong XAML
                BitmapImage bitmapImage = new BitmapImage(new Uri(selectedFilePath));
                image.Source = bitmapImage;
                imagePath = selectedFilePath;
            }
        }

        private void cloudinaryStorage()
        {
            Account account = new Account(CLOUND_NAME,API_KEY,API_SECRET);
            cloudinary = new Cloudinary(account);
            uploadImage(imagePath);
        }

        private void uploadImage(String path)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(path),
            };
            cloudinary.Upload(uploadParams);
        }

        private void background_DoWork(object sender, DoWorkEventArgs e)
        {
            cloudinaryStorage();
        }

        private void background_RunCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // An error occurred during the background operation
                System.Windows.MessageBox.Show($"Error: {e.Error.Message}");
            }
            else
            {
                // Background operation completed successfully
                System.Windows.MessageBox.Show("OKE");
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                System.Windows.MessageBox.Show("The background worker is currently busy. Please wait for it to complete.");
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Turn off the camera when the checkbox is checked
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Turn on the camera when the checkbox is unchecked
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
            }
            else
            {
                System.Windows.MessageBox.Show("No video devices found.");
            }
        }

        private void OnCopyAndResizeButtonClick(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected in the ListView
            if (listView1.SelectedItem != null)
            {
                try
                {
                    dynamic selectedImage = listView1.SelectedItem;
                    // Get the selected image from the ListView

                    // Assuming YourImageItemType has a property representing the image source
                    BitmapImage imageSource = selectedImage.ImageSource;

                    // Convert the BitmapImage to a System.Drawing.Bitmap
                    Bitmap sourceBitmap = BitmapFromBitmapImage(imageSource);

                    // Resize the image
                    int newWidth = 640;
                    int newHeight = 360;
                    Bitmap resizedBitmap = ResizeImage(sourceBitmap, newWidth, newHeight);

                    // Get the folder path where you want to save the image
                    string folderPath = folder_path;// Replace with your actual folder path

                    // Generate a new file name with "_capture" appended
                    string originalFileName = Path.GetFileNameWithoutExtension(selectedImage.FileName);
                    string fileExtension = Path.GetExtension(selectedImage.FileName);
                    string newFileName = $"{originalFileName}_capture{fileExtension}";

                    // Combine the folder path and new file name to get the full file path
                    string filePath = Path.Combine(folderPath, newFileName);

                    // Save the resized image to the specified path
                    SaveImage(resizedBitmap, filePath);

                    System.Windows.MessageBox.Show("Image copied, resized, and saved successfully.");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error: {ex.Message}");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select an image from the ListView first.");
            }
        }


        private Bitmap BitmapFromBitmapImage(BitmapImage bitmapImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);

                Bitmap bitmap = new Bitmap(memoryStream);
                return new Bitmap(bitmap);
            }
        }

        private Bitmap ResizeImage(Bitmap sourceImage, int newWidth, int newHeight)
        {
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(sourceImage, 0, 0, newWidth, newHeight);
            }
            return resizedImage;
        }

        private void SaveImage(Bitmap image, string filePath)
        {
            image.Save(filePath, ImageFormat.Jpeg);
        }

    }
}
