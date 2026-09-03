using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitterClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace TwitterClient
{
    /// <summary>
    /// the main page of twitter
    /// </summary>
    public partial class MainPage : Form
    {
        bool isTwitCreater;
        bool isRepliesPage;
        string currentUserPage;
        Button selectedReply;
        bool notSelectedTwitAttachment;
        bool notSelectedReplyAttachment;
        bool isTwitSearch;
        JsonObject tempScroll;

        /// <summary>
        /// starts the main page after login is complete
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            TwitCreater.Hide();
            isTwitCreater = false;
            isRepliesPage=false;
            TwitRepliesPanel.Hide();
            repliesViewControls.Hide();
            selectImageButton.Hide();
            selectedReplyAttachment.Hide();
            fileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg";
            replyFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg";
            TwitsViewer.AutoScroll = true;
            TwitsViewer.MouseWheel += TwitMouseScroll;
            TwitsViewer.Scroll += TwitScrollSideBar;
            userTwitsDisplay.AutoScroll=true;
            userTwitsDisplay.MouseWheel += TwitMouseScroll;
            userTwitsDisplay.Scroll += TwitScrollSideBar;
            TwitRepliesPanel.AutoScroll = true;
            TwitRepliesPanel.MouseWheel += TwitMouseScroll;
            TwitRepliesPanel.Scroll += TwitScrollSideBar;
            twitAttachment.SelectedIndex = 0;
            replyAttachmentType.SelectedIndex = 0;
            SearchTypeBox.SelectedIndex = 0;
            selectedReply = null;
            notSelectedReplyAttachment = true;
            notSelectedTwitAttachment = true;
        }

        /// <summary>
        /// when the page is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPage_Closed(object sender, EventArgs e)
        {
            TwitterClientMain.Close();
        }

        /// <summary>
        /// show the main page
        /// </summary>
        public void showMainPage()
        {
            userPagePanel.Hide();
            TwitRepliesPanel.Hide();
            this.currentUserPage = "";
            this.Text = "Twitter | Logged in as " + TwitterClientMain.CurrentUsername;
            tagsLabel.Text = "tags:";
            isTwitSearch = true;
            SearchTypeBox.SelectedIndex = 0;
            tempScroll=  new JsonObject
            {
                ["followed"] = 0,
                ["maxId"] = -1,
                ["tags"] = ""
            };
            //setting up an empty search
            resetEmptySearch();
        }

        

        /// <summary>
        /// enable scrolling with mouse to the twits panels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwitMouseScroll(object sender, MouseEventArgs e) {
            int scrollAmount = e.Delta; // positive = up, negative = down
            FlowLayoutPanel panel= sender as FlowLayoutPanel;
            panel.AutoScrollPosition = new System.Drawing.Point(
                -panel.AutoScrollPosition.X,
                -panel.AutoScrollPosition.Y - scrollAmount
            );
            //for loading more stuff in
            int scrollThreshold = panel.VerticalScroll.Maximum - panel.ClientSize.Height;

            if (panel.VerticalScroll.Value >= scrollThreshold && !HandleServerResponse.LoadedAllTwits && !HandleServerResponse.midSearch)
            {
                HandleServerResponse.resetView = false;
                HandleServerResponse.midSearch = true;
                if (isTwitSearch)
                {
                    tempScroll["maxId"] = HandleServerResponse.maxId;
                    TwitterClientMain.SendStringToServer("0" + tempScroll.ToJsonString());
                }
                else {
                    tempScroll["maxId"] = HandleServerResponse.maxId;
                    TwitterClientMain.SendStringToServer("5" + tempScroll.ToJsonString());
                }
            }
        }
        /// <summary>
        /// enable scrolling with sideBar to the twits panels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwitScrollSideBar(object sender, ScrollEventArgs e) {
            FlowLayoutPanel panel = sender as FlowLayoutPanel;
            int scrollThreshold = panel.VerticalScroll.Maximum - panel.ClientSize.Height;

            if (panel.VerticalScroll.Value >= scrollThreshold && !HandleServerResponse.LoadedAllTwits  && !HandleServerResponse.midSearch)
            {
                HandleServerResponse.resetView = false;
                HandleServerResponse.midSearch = true;
                if (isTwitSearch)
                {
                    tempScroll["maxId"] = HandleServerResponse.maxId;
                    TwitterClientMain.SendStringToServer("0" + tempScroll.ToJsonString());
                }
                else
                {
                    tempScroll["maxId"] = HandleServerResponse.maxId;
                    TwitterClientMain.SendStringToServer("5" + tempScroll.ToJsonString());
                }
            }
        }

        

        /// <summary>
        /// start a search request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_Click(object sender, EventArgs e)
        {   
            SearchButton.Enabled = false;
            HandleServerResponse.resetView = true;
            userPagePanel.Hide();
            TwitCreater.Hide();
            TwitsViewer.Show();
            isTwitCreater = false;
            TwitImageHandler.Reset();
            UserProfilesManager.Reset();
            if (SearchTypeBox.SelectedIndex == 0)
            {//twits
                isTwitSearch = true;
                JsonObject main = new JsonObject
                {
                    ["followed"] = (OnlyFollowedCheckBox.Checked ? 1 : 0),
                    ["maxId"] = -1,
                    ["tags"] = tagsBox.Text
                };

                tempScroll = main;
                TwitterClientMain.SendStringToServer("0" + main.ToJsonString());
                
            }
            else {//users
                isTwitSearch = false;
                TwitterClientMain.SendStringToServer("a" + (OnlyFollowedCheckBox.Checked ? 1 : 0) + tagsBox.Text);
            }
        }

        /// <summary>
        /// create a twit view switch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void twitCreatorButton_Click(object sender, EventArgs e)
        {
            if (isTwitCreater)
            {
                TwitCreater.Hide();
                if (isRepliesPage)
                {
                    repliesViewControls.Show();
                    TwitRepliesPanel.Show();
                }
                else if (currentUserPage.Equals(""))
                {
                    TwitsViewer.Show();
                }
                else
                {
                    userPagePanel.Show();
                }
                twitCreatorButton.BackgroundImage = global::TwitterClient.Properties.Resources.new_twit;
                isTwitCreater = false;
            }
            else
            {
                userPagePanel.Hide();
                repliesViewControls.Hide();
                TwitRepliesPanel.Hide();
                TwitsViewer.Hide();
                TwitCreater.Show();
                if (isRepliesPage) {
                    twitCreatorButton.BackgroundImage = global::TwitterClient.Properties.Resources.backToTwit;
                }
                else if (currentUserPage.Equals(""))
                {
                    twitCreatorButton.BackgroundImage = global::TwitterClient.Properties.Resources.mainPageIcon;
                }
                else
                {
                    twitCreatorButton.BackgroundImage = global::TwitterClient.Properties.Resources.BackToUser;
                }
                isTwitCreater = true;
            }
            
        }

        /// <summary>
        /// create a new twit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateTwit_Click(object sender, EventArgs e)
        {
            //attached image
            string image = "0";
            byte[] imageBytes=new byte[0];

            try
            {
                if (!notSelectedTwitAttachment)
                {
                    if (twitAttachment.SelectedIndex == 1 && selectedTwitImage.CheckFileExists)
                    {
                        image = "1";
                        using (var ms = new MemoryStream())
                        {
                            Image img = Image.FromFile(selectedTwitImage.FileName);
                            img.Save(ms, img.RawFormat);
                            imageBytes = ms.ToArray();
                        }
                        image += imageBytes.Length;
                    }
                    else if (twitAttachment.SelectedIndex == 2 && selectedTwitImage.CheckFileExists)
                    {
                        image = "2";

                        using (FileStream f = File.OpenRead(selectedTwitImage.FileName))
                        {
                            imageBytes = new byte[(int)f.Length];

                            f.Read(imageBytes, 0, (int)f.Length);
                        }
                        image += imageBytes.Length;
                    }
                }
            }
            catch { image = "0"; }

            JsonObject main = new JsonObject
            {
                ["tags"] = newTwitTags.Text,
                ["content"] = newTwitContent.Text,
                ["image"] = image
            };

            TwitterClientMain.SendStringToServer("1" + main.ToJsonString());

            if (image[0] != '0') {
                TwitterClientMain.server.WriteBytes(imageBytes);
            }
            
            TwitCreater.Hide();
            userPagePanel.Hide();
            TwitRepliesPanel.Hide();
            repliesViewControls.Hide();
            selectedReply = null;
            TwitsViewer.Show();
            currentUserPage = "";
            isTwitCreater = false;
            
            newTwitTags.Text = "";
            newTwitContent.Text = "";
            tagsBox.Text = "";
            twitCreatorButton.BackgroundImage = global::TwitterClient.Properties.Resources.new_twit;
            resetEmptySearch();
            
        }

        /// <summary>
        /// reset the search to empty
        /// </summary>
        private void resetEmptySearch() {
            TwitImageHandler.Reset();
            HandleServerResponse.resetView = true;
            JsonObject main = new JsonObject
            {
                ["followed"] = 0,
                ["maxId"] = -1,
                ["tags"] = ""
            };
            tempScroll = main;
            TwitterClientMain.server.WriteBytes(Encoding.UTF8.GetBytes("0"+ main.ToJsonString()));
        }

        /// <summary>
        /// like a twit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void LikeButtonClick(object sender, EventArgs e) {
            Button button = sender as Button;
            string id=button.Name.Split(':')[1];

            System.Windows.Forms.Label likeLabel = button.Parent.Controls
                .OfType<System.Windows.Forms.Label>()
                    .FirstOrDefault(t => t.Name.Split(':')[0] == "likeLabel");

            string currentLikes = likeLabel.Name.Split(':')[1];

            if (currentLikes[0] == '0')
            {
                button.BackgroundImage = global::TwitterClient.Properties.Resources.Liked;
                likeLabel.Name = "likeLabel:1" + (Int32.Parse(currentLikes.Substring(1))+1);
                likeLabel.Text = "" + (Int32.Parse(currentLikes.Substring(1)) + 1);
                TwitterClientMain.SendStringToServer("20" + id);
            }
            else {
                button.BackgroundImage = global::TwitterClient.Properties.Resources.NotLiked;
                likeLabel.Name = "likeLabel:0" + (Int32.Parse(currentLikes.Substring(1)) - 1);
                likeLabel.Text = "" + (Int32.Parse(currentLikes.Substring(1)) - 1);
                TwitterClientMain.SendStringToServer("21" + id);

            }
        }

        /// <summary>
        /// logout request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logout_Click(object sender, EventArgs e)
        {
            TwitsViewer.Show();
            TwitCreater.Hide();
            userPagePanel.Hide();
            TwitterClientMain.server.Close();
            TwitterClientMain.SwitchToLoginPage();
            VideoHandler.Reset();

        }

        /// <summary>
        /// try to delte a twit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void delete_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string id= button.Name.Split(':')[1];
            TwitterClientMain.SendStringToServer("3"+id);

            //stop any running videos
            AxWMPLib.AxWindowsMediaPlayer player= button.Parent.Controls.OfType<AxWMPLib.AxWindowsMediaPlayer>().FirstOrDefault();
            if (player != null) {player.Ctlcontrols.stop(); player.URL = null; }

            Control parenttemp = button.Parent.Parent;
            FlowLayoutPanel parent;
            if (Int32.Parse(id) < 0)
            {
                parent = parenttemp.Parent as FlowLayoutPanel;
            }
            else {
                parent = parenttemp as FlowLayoutPanel;
            }

            //to check if the context is in the search results twits or the twit replies viewer
            if (Int32.Parse(id) >= 0)
                parent.Controls.Remove(button.Parent);
            else {
                //update the reply amount label
                System.Windows.Forms.Label likeLabel = button.Parent.Parent.Parent.Controls
                    .OfType<System.Windows.Forms.Panel>()
                        .FirstOrDefault().Controls
                    .OfType<System.Windows.Forms.Label>()
                        .FirstOrDefault(t => t.Name == "repliesNumLabel");

                likeLabel.Text = "" + (Int32.Parse(likeLabel.Text) - 1);
                
                //remove the reply
                button.Parent.Parent.Parent.Controls.Remove(button.Parent.Parent);
            }
            if (parent.Name[4] == 'R')
            {
                while (parent != TwitRepliesPanel)
                {   
                    parent.Size = new System.Drawing.Size(parent.Width, 3 + parent.Controls[parent.Controls.Count - 1].Height + parent.Controls[parent.Controls.Count - 1].Location.Y);
                    parent = parent.Parent as FlowLayoutPanel;
                }
            }

            //maybe make it remove without reloading


        }

        /// <summary>
        /// request a user page via twit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void TwitUserClick(object sender, EventArgs e) {
            VideoHandler.Reset();
            TwitImageHandler.Reset();
            Label label = sender as Label;
            currentUserPage = label.Text.Split('\n')[0].Substring(7);
            TwitsViewer.Hide();
            repliesViewControls.Hide();
            TwitRepliesPanel.Hide();
            userPagePanel.Show();
            isTwitSearch = false;
            userNameLabel.Text = currentUserPage;
            followButton.Location=new Point(userNameLabel.Left+userNameLabel.Width+20,followButton.Top);
            HandleServerResponse.resetView = true;

            JsonObject res = new JsonObject()
            {
                ["user"] = currentUserPage,
                ["maxId"] = -1
            };
            
            TwitterClientMain.SendStringToServer("5"+res.ToJsonString());
            tempScroll = res;
        }

        /// <summary>
        /// request a user page via user search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void UserSearchNameClick(object sender, EventArgs e) {
            VideoHandler.Reset();
            TwitImageHandler.Reset();
            Label label = sender as Label;
            currentUserPage = label.Text;
            TwitsViewer.Hide();
            repliesViewControls.Hide();
            TwitRepliesPanel.Hide();
            userPagePanel.Show();
            userNameLabel.Text = currentUserPage;
            followButton.Location = new Point(userNameLabel.Left + userNameLabel.Width + 20, followButton.Top);
            HandleServerResponse.resetView = true;
            JsonObject res = new JsonObject()
            {
                ["user"] = currentUserPage,
                ["maxId"] = -1
            };
            TwitterClientMain.SendStringToServer("5" + res.ToJsonString());
        }

        /// <summary>
        /// switch back to main page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userToMainButton_Click(object sender, EventArgs e)
        {
            if (currentUserPage != "") { resetEmptySearch(); }
            currentUserPage = "";
            isTwitSearch = true;
            userPagePanel.Hide();
            TwitsViewer.Show();
            VideoHandler.Reset();
            TwitImageHandler.Reset();
        }

        /// <summary>
        /// Switching to a twits reply page
        /// </summary>
        public void SwitchToRepliesPage() {
            currentUserPage = "";
            userPagePanel.Hide();
            TwitsViewer.Hide();
            TwitRepliesPanel.Show();
            repliesViewControls.Show();
            VideoHandler.StopAll();
        }

        /// <summary>
        /// follow a user request
        /// </summary>
        private void followButton_Click(object sender, EventArgs e)
        {
            if (followButton.Text[0] == 'f')
            {
                followButton.Text = "unfollow";
                followersCount.Text="followers: "+(Int32.Parse(followersCount.Text.Substring(11))+1);
                TwitterClientMain.SendStringToServer("40" + currentUserPage);
            }
            else {
                followButton.Text = "follow";
                followersCount.Text = "followers: " + (Int32.Parse(followersCount.Text.Substring(11)) - 1);
                TwitterClientMain.SendStringToServer("41" + currentUserPage);
            }
        }

        /// <summary>
        /// full twit request
        /// </summary>
        internal void GetFullTwitRequest(object sender, EventArgs e) {
            selectedReply = null;
            repbut.Enabled = false;
            repbut.BackgroundImage = global::TwitterClient.Properties.Resources.replyDisabled;
            isRepliesPage = true;
            Control pan= sender as Control;
            string id = pan.Name.Split(':')[1];
            TwitterClientMain.SendStringToServer("7"+id);
            VideoHandler.StopAll();
        }
        
        private void profilePickButton_Click(object sender, EventArgs e)
        {
            fileDialog.ShowDialog();
        }

        /// <summary>
        /// after new profile picture is chosen
        /// </summary>
        private void fileDialog_FileOk(object sender, CancelEventArgs e)
        {
            string[] splited = fileDialog.SafeFileName.Split('.');
            if (new string[3] {"png","jpg","jpeg" }.Contains( splited[splited.Length - 1]))
            {
                //new profile chosen
                TwitterClientMain.SendStringToServer("60"+ splited[splited.Length - 1]);

                //send to server
                Image image = Image.FromFile(fileDialog.FileName);
                byte[] data;

                

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    data= ms.ToArray();
                }

                TwitterClientMain.server.WriteBytes(data);
            }
            else {
                MessageBox.Show("Invalid file format");
            }
        }

        /// <summary>
        /// going back from the replies view to the main page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToMainPage_Click(object sender, EventArgs e)
        {
            VideoHandler.ResetTempAwaiting();
            TwitImageHandler.ResetTempAwaiting();
            isRepliesPage= false;
            TwitRepliesPanel.Hide();
            repliesViewControls.Hide();
            TwitsViewer.Show();
            if (currentUserPage != "") {
                resetEmptySearch();
            }
        }

        /// <summary>
        /// when a reply is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void selectReply_Click(object sender, EventArgs e) {
            Button button = sender as Button;
            if (selectedReply == null)
            {
                repbut.Enabled = true;
                repbut.BackgroundImage = global::TwitterClient.Properties.Resources.replyEnabled;

            }
            else {
                selectedReply.BackgroundImage = global::TwitterClient.Properties.Resources.replyUnselected;
            }
            button.BackgroundImage = global::TwitterClient.Properties.Resources.replySelected;
            selectedReply = button;
        }

        /// <summary>
        /// reply to selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repbut_Click(object sender, EventArgs e)
        {
            //attached image
            string image = "0";
            byte[] imageBytes = new byte[0];
            if (!notSelectedReplyAttachment)
            {
                if (replyAttachmentType.SelectedIndex == 1 && replyFileDialog.CheckFileExists)
                {
                    image = "1";
                    
                    using (var fs = new FileStream(replyFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    using (var ms = new MemoryStream())
                    {
                        Image img = Image.FromStream(fs);

                        img.Save(ms, img.RawFormat);
                        imageBytes = ms.ToArray();
                    }
                    
                    image += imageBytes.Length;
                }
                else if (replyAttachmentType.SelectedIndex == 2 && replyFileDialog.CheckFileExists)
                {
                    image = "2";

                    using (FileStream f = File.OpenRead(replyFileDialog.FileName))
                    {
                        imageBytes = new byte[(int)f.Length];

                        f.Read(imageBytes, 0, (int)f.Length);
                    }
                    image += imageBytes.Length;
                }
            }

            JsonObject post = new JsonObject() {
                ["id"] = selectedReply.Name.Substring(12),
                ["content"] = repContent.Text,
                ["image"]=image
            };
            TwitterClientMain.SendStringToServer("8"+post.ToJsonString());
            NewReplyData.parent=selectedReply.Parent.Parent as FlowLayoutPanel;
            NewReplyData.content=repContent.Text;
            NewReplyData.time = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            repContent.Text = "";

            if (image[0] != '0')
            {
                TwitterClientMain.server.WriteBytes(imageBytes);
            }

            notSelectedReplyAttachment = true;
            replyAttachmentType.SelectedIndex = 0;
        }

        /// <summary>
        /// load your own page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goToMyPageButton_Click(object sender, EventArgs e)
        {
            VideoHandler.StopAll();
            currentUserPage = TwitterClientMain.CurrentUsername;
            TwitsViewer.Hide();
            repliesViewControls.Hide();
            TwitRepliesPanel.Hide();
            userPagePanel.Show();
            if (!UserProfilesManager.hasImage(TwitterClientMain.CurrentUsername))
            {
                if (!UserProfilesManager.HasRequested(TwitterClientMain.CurrentUsername))
                {
                    TwitterClientMain.SendStringToServer("61" + TwitterClientMain.CurrentUsername);
                }
                UserProfilesManager.addAwaiting(userPageProfile, TwitterClientMain.CurrentUsername);
            }
            else
            {
                //render image now
                userPageProfile.Image = UserProfilesManager.GetImage(TwitterClientMain.CurrentUsername);
            }
            HandleServerResponse.resetView = true;
            isTwitSearch = false;
            userNameLabel.Text = currentUserPage;
            followButton.Location = new Point(userNameLabel.Left + userNameLabel.Width + 20, followButton.Top);
            JsonObject res = new JsonObject()
            {
                ["user"] = currentUserPage,
                ["maxId"] = -1
            };

            TwitterClientMain.SendStringToServer("5" + res.ToJsonString());
            tempScroll = res;
        }

        /// <summary>
        /// twit attachment changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TwitAttachmentChanged(object sender, EventArgs e)
        {
            notSelectedTwitAttachment=true;
            switch (twitAttachment.SelectedIndex) {
                case 0:
                    //None
                    selectImageButton.Hide();
                    selectedTwitImage.Reset();
                    break;
                case 1:
                    //select image
                    selectedTwitImage.Reset();
                    selectImageButton.Show();
                    selectImageButton.Text = "select image";
                    selectedTwitImage.Filter = "Image Files|*.png;*.jpg;*.jpeg";
                    break;
                case 2:
                    //select video
                    selectedTwitImage.Reset();
                    selectImageButton.Show();
                    selectImageButton.Text = "select video";
                    selectedTwitImage.Filter = "Video Files|*.mp4";
                    break;
            
            }
        }

        /// <summary>
        /// open the file dialog for twitImage selecting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectImageButton_Click(object sender, EventArgs e)
        {
            selectedTwitImage.ShowDialog();
        }

        /// <summary>
        /// update the selected twit image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectedTwitImage_FileOk(object sender, CancelEventArgs e)
        {
            selectImageButton.Text="selected "+selectedTwitImage.SafeFileName;
            notSelectedTwitAttachment = false;
        }

        /// <summary>
        /// reply attachment change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replyAttachmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            notSelectedReplyAttachment=true;
            switch(replyAttachmentType.SelectedIndex )
            {
                case 0:
                    selectedReplyAttachment.Hide();
                    break;
                case 1:
                    selectedReplyAttachment.Show();
                    selectedReplyAttachment.Text = "select image";
                    replyFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg";
                    break;
                case 2:
                    selectedReplyAttachment.Show();
                    selectedReplyAttachment.Text = "select video";
                    replyFileDialog.Filter = "Video Files|*.mp4";
                    break;
            }
            
        }

        /// <summary>
        /// update the selected reply image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectedReplyAttachment_Click(object sender, EventArgs e)
        {
            replyFileDialog.ShowDialog();
        }

        /// <summary>
        /// update the selected twit image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replyFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            selectedReplyAttachment.Text = "selected " + replyFileDialog.SafeFileName;
            notSelectedReplyAttachment = false;
        }

        /// <summary>
        /// when the search type is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SearchTypeBox.SelectedIndex == 0)
            {
                tagsLabel.Text = "tags:";  
            }
            else {

                tagsLabel.Text = "user:";     
            }
        }
    }
}
