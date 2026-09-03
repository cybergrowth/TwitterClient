using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitterClient;

namespace TwitterClient
{
    /// <summary>
    /// handles all videos in twits and replies
    /// </summary>
    internal static class VideoHandler
    {
        static string videosFolder = Environment.CurrentDirectory + "\\videos\\";
        static Dictionary<int, AxWMPLib.AxWindowsMediaPlayer> awaitingVideos = new Dictionary<int, AxWMPLib.AxWindowsMediaPlayer>();
        static Stack<AxWMPLib.AxWindowsMediaPlayer> players = new Stack<AxWMPLib.AxWindowsMediaPlayer>();
        static MainPage mainPage;
        static Stack<int> tempVideos=new Stack<int>();
       
        /// <summary>
        /// sets up the video handler
        /// </summary>
        /// <param name="mainPage"></param>
        public static void Setup(MainPage mainPage)
        {
            System.IO.Directory.CreateDirectory(videosFolder);
            VideoHandler.mainPage = mainPage;
            ResetFolder();
        }

        /// <summary>
        /// deletes all existing content from the videos folder
        /// </summary>
        public static void ResetFolder() {
            System.IO.DirectoryInfo di = new DirectoryInfo(videosFolder);
            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                } 
                catch {
                    //means that file is currently used
                }
            }
        }

        /// <summary>
        /// add awaiting video
        /// </summary>
        /// <param name="id"></param>
        /// <param name="videoplayer"></param>
        /// <param name="isTemp"></param>
        public static void addAwaiting(int id, AxWMPLib.AxWindowsMediaPlayer videoplayer,bool isTemp) {
            if (!awaitingVideos.ContainsKey(id))
            {
                awaitingVideos.Add(id, videoplayer);
                players.Push(videoplayer);
                if (isTemp)
                {
                    tempVideos.Push(id);
                }
            }
        }
        
        /// <summary>
        /// reset all awaiting stuff
        /// </summary>
        public static void Reset()
        {
            tempVideos.Clear();
            awaitingVideos = new Dictionary<int, AxWMPLib.AxWindowsMediaPlayer>();

            while (players.Count != 0) { 
                AxWMPLib.AxWindowsMediaPlayer p = players.Pop();
                try
                {
                    mainPage.Invoke(new Action(() =>
                    {
                        try
                        {
                            p.Ctlcontrols.stop();
                            p.URL = null;
                        }
                        catch{ };
                    }));

                    p = null;
                }
                catch { players.Push(p);
                    Console.WriteLine("issue with player");
                }
            }

        }
        
        /// <summary>
        /// reset all temp videos of replies
        /// </summary>
        public static void ResetTempAwaiting()
        {

            while (tempVideos.Count != 0) {
                awaitingVideos.Remove(tempVideos.Pop());
            }
            StopAll();
        }

        /// <summary>
        /// check if a file exists in the videos folder
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool isExist(string name) {
            return File.Exists(videosFolder  + name);
        }

        /// <summary>
        /// adds a new recived video
        /// </summary>
        /// <param name="videoName"></param>
        /// <param name="data"></param>
        public static void addVideo(string videoName, byte[] data) {
            int id = Int32.Parse(videoName.Split('.')[0]);
            while(isExist(videoName)) {
                File.Delete(videosFolder + videoName);
                
            }
            if (awaitingVideos.ContainsKey(id))
            {
                using (FileStream f = File.Create(videosFolder + videoName))
                {
                    f.Write(data, 0, data.Length);
                }
                AxWMPLib.AxWindowsMediaPlayer video=awaitingVideos[id];
                Thread.Sleep(10);
                video.URL = videosFolder + videoName;
                video.Ctlcontrols.stop();
                awaitingVideos.Remove(id);
            }
        }

        /// <summary>
        /// get the videos folder
        /// </summary>
        /// <returns></returns>
        internal static string GetFolder() { return videosFolder; }

        /// <summary>
        /// stops all players
        /// </summary>
        public static void StopAll() {
            for (int i = 0; i < players.Count; i++) {
                try
                {
                    players.ElementAt(i).Ctlcontrols.stop();
                }
                catch { }
            }
        }
    }
}
