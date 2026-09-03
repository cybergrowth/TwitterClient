using TwitterClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwitterClient
{
    /// <summary>
    /// the main program of the application
    /// </summary>
    internal static class TwitterClientMain
    {
        /// <summary>
        /// The twitter client.
        /// </summary>
        public static CommunicatorClient server;

        private static LoginPage loginPage;
        private static MainPage mainPage;

        public static string CurrentUsername;

        /// <summary>
        /// Setting all parts up
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            loginPage= new LoginPage();
            mainPage = new MainPage();
            UserProfilesManager.Setup();
            VideoHandler.Setup(mainPage);
            Application.Run(loginPage);
            
        }

        /// <summary>
        /// get a string from the server
        /// </summary>
        /// <returns></returns>
        public static string GetStringFromServer()
        {
            byte[] resData = server.ReadBytes();    
            string output = Encoding.UTF8.GetString(resData, 0, resData.Length);
            return output;
        }

        /// <summary>
        /// send a string to the server
        /// </summary>
        /// <param name="input"></param>
        public static void SendStringToServer(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            server.WriteBytes(data);
        }
        
        /// <summary>
        /// switching to the main page
        /// </summary>
        public static void SwitchToMainPage()
        {
            loginPage.Hide();
            mainPage.Show();
            mainPage.showMainPage();
            mainPage.TopMost=true;
        }
        /// <summary>
        /// switching to the login page
        /// </summary>
        public static void SwitchToLoginPage()
        {
            loginPage.Invoke(new Action(() =>
            {
                loginPage.Show();
                mainPage.Hide();
                mainPage.twitCreatorButton.BackgroundImage = global::TwitterClient.Properties.Resources.new_twit;
                mainPage.selectImageButton.Hide();
                mainPage.twitAttachment.SelectedIndex = 0;
                mainPage.repliesViewControls.Hide();
                mainPage.TwitRepliesPanel.Hide();
                mainPage.SearchButton.Enabled = true;
                VideoHandler.Reset();
                TwitImageHandler.Reset();
                mainPage.TwitsViewer.Controls.Clear();
                mainPage.userTwitsDisplay.Controls.Clear();
                mainPage.TwitRepliesPanel.Controls.Clear();
                VideoHandler.ResetFolder();
            }));
        }

        /// <summary>
        /// getting the main page instance
        /// </summary>
        public static MainPage GetMainPage() {
            return mainPage;
        }

        /// <summary>
        /// on server close
        /// </summary>
        public static void Close() {
            server.Close();
            loginPage.Close();

        }
    }
}
