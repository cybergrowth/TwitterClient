using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitterClient
{
    /// <summary>
    /// manages all images of twits and replies currently loaded
    /// </summary>
     class TwitImageHandler
    {
        static Dictionary< int,PictureBox> awaitingImages=new Dictionary<int,PictureBox>();
        static Stack<int> tempImages = new Stack<int>();

        /// <summary>
        /// add an awaiting image
        /// </summary>
        /// <param name="pictureBox"></param>
        /// <param name="id"></param>
        /// <param name="isTemp"></param>
        public static void addAwaiting(PictureBox pictureBox,int id,bool isTemp) {
            if (awaitingImages.Keys.Contains(id)) {
                if (!awaitingImages[id].IsDisposed)
                {
                    return;
                }
                else {
                    lock (awaitingImages)
                    {
                        awaitingImages.Remove(id);
                    }
                }
            }
            awaitingImages.Add( id,pictureBox);
            tempImages.Push(id);
        }

        /// <summary>
        /// reset all the image data
        /// </summary>
        public static void Reset() {
            awaitingImages = new Dictionary< int,PictureBox>();
        }

        /// <summary>
        /// reset all of the temporary  replies awaiting
        /// </summary>
        public static void ResetTempAwaiting()
        {

            while (tempImages.Count != 0)
            {
                awaitingImages.Remove(tempImages.Pop());
            }
        }

        /// <summary>
        /// Handles a new Image recived
        /// </summary>
        /// <param name="image"></param>
        /// <param name="name"></param>
        public static void NewImageRecived(Image image, int name) {
            if (awaitingImages.Keys.Contains(name)) {
                if (!awaitingImages[name].IsDisposed)
                {
                    //checking for dupes
                    awaitingImages[name].Image = image;
                    awaitingImages[name].SizeMode = PictureBoxSizeMode.StretchImage;
                    awaitingImages.Remove(name);
                }
                
            }
        }

    }
}
