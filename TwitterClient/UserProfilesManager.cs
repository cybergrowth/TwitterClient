using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitterClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TwitterClient
{
    /// <summary>
    /// handles reciving all the user profiles
    /// </summary>
    static class UserProfilesManager
    {
        static string userProfilesFolder = Environment.CurrentDirectory + "\\userProfiles\\";
        static Dictionary<PictureBox,string> awaitingImages;

        /// <summary>
        /// setup the class
        /// </summary>
        public static void Setup()
        {
            System.IO.Directory.CreateDirectory(userProfilesFolder);

            //clear previous
            System.IO.DirectoryInfo di = new DirectoryInfo(userProfilesFolder);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            awaitingImages = new Dictionary<PictureBox, string>();
        }
        
        /// <summary>
        /// adding a new picture from data
        /// </summary>
        public static void addPicture(string pictureName,string Ftype, byte[] data)
        {
            try
            {
                
                if (Ftype.Equals("png"))
                {//png
                    Image image;
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        image = Image.FromStream(ms);
                    }
                    ImageFormat imageFormat = ImageFormat.Png;


                    image.Save(userProfilesFolder + pictureName + "." + Ftype, imageFormat);
                }
                else {//jpeg
                    using (MemoryStream ms = new MemoryStream(data))
                    using (Bitmap bmp = new Bitmap(ms))
                    {
                        string path = Path.Combine(userProfilesFolder, pictureName + "." + Ftype);

                        if (Ftype.ToLower() == "jpeg" || Ftype.ToLower() == "jpg")
                        {
                            // Explicitly get the JPEG encoder
                            ImageCodecInfo jpegEncoder = ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
                            EncoderParameters encoderParams = new EncoderParameters(1);
                            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);

                            bmp.Save(path, jpegEncoder, encoderParams);
                        }
                        else
                        {
                            bmp.Save(path);
                        }
                    }
                }
            }
            catch {
                //if there is a problem or the file is corrupted
                Console.WriteLine(pictureName + " profile is corrupted or has issues");
            }
        }

        /// <summary>
        /// removes unneccecary requesting
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool HasRequested(string name) {
            return awaitingImages.Values.Contains(name);
        }

         

        /// <summary>
        /// checking if we need to request an image or we allready have the profile
        /// </summary>
        /// <returns></returns>
        public static bool hasImage(string pictureName)
        {
            return Directory.GetFiles(userProfilesFolder, pictureName + ".*").Length != 0;
        }

        /// <summary>
        /// get a requested profile picture by username
        /// </summary>
        /// <returns></returns>
        public static Image GetImage(string pictureName){

            string filePath = Directory.GetFiles(userProfilesFolder, pictureName + ".*")[0];
            byte[] bytes;
            try
            {
                bytes = File.ReadAllBytes(filePath);
            }
            catch {
                return global::TwitterClient.Properties.Resources.defaultProfile;
            }
            try
            {
                Image res = Image.FromStream(new MemoryStream(bytes));
                return res;
            }
            catch {
                File.Delete(filePath);
                return global::TwitterClient.Properties.Resources.defaultProfile; 
            }
            
        }

        /// <summary>
        /// reset the images wanting to be updated and removed anything previous
        /// </summary>
        public static void Reset()
        {
            awaitingImages.Clear();
            foreach (string file in Directory.GetFiles(userProfilesFolder))
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
            }
            
        }
        /// <summary>
        /// add a new image to the awaiting
        /// </summary>
        public static void addAwaiting(PictureBox image,string name) {
            awaitingImages[image] = name;
        }


        /// <summary>
        /// update all awaited
        /// </summary>
        public static void updateAwaited(string recived) {
            List<PictureBox> remove = new List<PictureBox>();
            lock (awaitingImages)
            {  
                PictureBox image;
                for (int i = 0; i < awaitingImages.Keys.Count; i++) { 
                    image=awaitingImages.Keys.ElementAt(i);
                    if (awaitingImages[image].Equals(recived))
                    {
                        image.Image = GetImage(recived);
                        remove.Add(image);
                    }
                }
                for(int i = 0; i < remove.Count; i++) {
                    awaitingImages.Remove(remove[i]);
                }
            }
        }
    }
}
