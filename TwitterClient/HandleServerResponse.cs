using AxWMPLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using TwitterClient;
using TwitterClient.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace TwitterClient
{
    /// <summary>
    /// The class handles all of string messages recived by the server
    /// </summary>
    static class HandleServerResponse
    {
        private static CommunicatorClient communicatorClient;
        private static MainPage mainPage;
        private static string twitSeparator;
        private const int lineLength = 158;
        internal static bool resetView = true;
        internal static int maxId = -1;
        internal static bool midSearch = false;
        

        /// <summary>
        /// Sets up the handeling class, must be run before any handling
        /// </summary>
        /// <param name="communicatorClient"></param>
        /// <param name="mainPage"></param>
        public static void Setup(CommunicatorClient communicatorClient, MainPage mainPage) {
            HandleServerResponse.communicatorClient=communicatorClient;
            HandleServerResponse.mainPage=mainPage;

            twitSeparator = "";
            for (int i = 0; i < 264; i++) { twitSeparator += "-"; }

        }

        /// <summary>
        /// sort the messages using their headers into their different functions 
        /// </summary>
        /// <param name="responce"></param>
        public static void Handle(string responce) {
            switch (responce[0]) {
                case '0':
                    SearchResponce(responce.Substring(1));
                    break;
                case '1':
                    UserPageResponce(responce.Substring(1));
                    break;
                case '2':
                    ReciveUserProfile(responce.Substring(1));
                    break;
                case '3':
                    ReciveFullTwit(responce.Substring(1));
                    break;
                case '4':
                    displayNewReply(responce.Substring(1));
                    break;
                case '5':
                    UpdateTwitAttachment(responce.Substring(1));
                    break;
                case '6':
                    MessageBox.Show("server error: "+responce.Substring(1), "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
                case '7':
                    UserSearchResponce(responce.Substring(1));
                    break;
            }
        }

        /// <summary>
        /// handles search responces
        /// </summary>
        /// <param name="responce"></param>
        private static void SearchResponce(string responce) {
            //adding the responces -need to clear prev
            JsonDocument resp=JsonDocument.Parse(responce);
            JsonElement root=resp.RootElement;

            DisplayTwits(root, mainPage.TwitsViewer);
            mainPage.Invoke(new Action(() =>
            {
                mainPage.SearchButton.Enabled = true;
            }));
        }

        /// <summary>
        /// Handles the responce for a user page
        /// </summary>
        /// <param name="responce"></param>
        private static void UserPageResponce(string responce) {
            JsonDocument jsonDocument= JsonDocument.Parse(responce);
            JsonElement jsonElement= jsonDocument.RootElement;
            int isFollowed = jsonElement.GetProperty("isfollowed").GetInt32();
            int followersNumber = jsonElement.GetProperty("followers").GetInt32();
            JsonElement twits = jsonElement.GetProperty("twits");

            //check if we have the profile picture or get it -- need to add not exist responce
            if (!UserProfilesManager.hasImage(mainPage.userNameLabel.Text))
            {
                TwitterClientMain.SendStringToServer("61" + mainPage.userNameLabel.Text);
                UserProfilesManager.addAwaiting(mainPage.userPageProfile, mainPage.userNameLabel.Text);
                mainPage.userPageProfile.Image = global::TwitterClient.Properties.Resources.defaultProfile;
            }
            else
            {
                //render image now
                mainPage.userPageProfile.Image = UserProfilesManager.GetImage(mainPage.userNameLabel.Text);
            }

            if (isFollowed == 0)
            {
                mainPage.Invoke(new Action(() =>
                {
                    mainPage.followButton.Text = "follow";
                }));
            }
            else {
                mainPage.Invoke(new Action(() =>
                {
                    mainPage.followButton.Text = "unfollow";
                }));
            }//Twits...
            mainPage.Invoke(new Action(() =>
            {
                mainPage.followersCount.Text = "followers: " + followersNumber;
            }));

            DisplayTwits(twits, mainPage.userTwitsDisplay);
        }

        /// <summary>
        /// display all the requested twits in the view
        /// </summary>
        private static void DisplayTwits(JsonElement root, FlowLayoutPanel panel) {
            //clear previous
            if (resetView)
            {
                VideoHandler.Reset();
                mainPage.Invoke(new Action(() =>
                {
                    panel.Controls.Clear();
                    mainPage.repliesViewControls.Hide();
                    mainPage.TwitRepliesPanel.Hide();
                }));
                UserProfilesManager.Reset();
                LoadedAllTwits = false;
                maxId = int.MaxValue;
            }
            twitsMainElement = root;
            twitsMainPanel = panel;
            //add new search results for every twit
            DisplayTwitsAmount(7);
            midSearch = false;
        }

        public static bool LoadedAllTwits;
        static int start;
        static JsonElement twitsMainElement;
        static FlowLayoutPanel twitsMainPanel;
        /// <summary>
        /// display an amount of twits
        /// </summary>
        /// <param name="amount"></param>
        public static void DisplayTwitsAmount( int amount) {
            foreach (JsonProperty prop in twitsMainElement.EnumerateObject())
            {
                
                
                //registering
                Twit twitData = JsonSerializer.Deserialize<Twit>(prop.Value.ToString());

                maxId = Math.Min(maxId, twitData.id);
                
                //creating visual stuff
                Panel twitPan = new Panel();
                twitPan.Location = new System.Drawing.Point(3, 3);
                twitPan.Name = "twitPan:" + twitData.id;
                twitPan.Size = new System.Drawing.Size(800, 25);
                twitPan.TabIndex = 0;
                twitPan.BackColor = System.Drawing.Color.LightBlue;
                twitPan.Click += mainPage.GetFullTwitRequest;

                twitObjects(twitPan, twitData.id, twitData.username, twitData.content, twitData.time, twitData.Likes, twitData.tags,
                    twitData.replies, twitData.isonline, twitData.image, true, false, "" + twitData.replies);

                mainPage.Invoke(new Action(() => { twitsMainPanel.Controls.Add(twitPan); }));

                
            }
            LoadedAllTwits = twitsMainElement.EnumerateObject().Count() == 0;
        }

        /// <summary>
        /// handles reciving a user profile picture
        /// </summary>
        /// <param name="responce"></param>
        private static void ReciveUserProfile(string responce) {
            string fileType = responce.Split(':')[0];
            string target = responce.Substring(fileType.Length + 1);
            byte[] imgBytes;
            lock (communicatorClient.GetTcpClient().GetStream())
            {
                imgBytes = communicatorClient.ReadBytes();
            }
            UserProfilesManager.addPicture(target,fileType, imgBytes);
            UserProfilesManager.updateAwaited(target);
        }

        /// <summary>
        /// creating all required objects for a twit or reply
        /// </summary>
        /// <param name="twitPan"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="content"></param>
        /// <param name="time"></param>
        /// <param name="Likes"></param>
        /// <param name="tags"></param>
        /// <param name="replies"></param>
        /// <param name="isonline"></param>
        /// <param name="image"></param>
        private static System.Windows.Forms.Label twitObjects(Panel twitPan,int id,string username,string content,string time,
            string Likes,string tags,int numreplies ,bool isonline,int image,bool isMainPage,bool isMainTwit,string repliesNumber) {
            //profile picture
            PictureBox twitterProfile = new PictureBox();
            twitterProfile.Location = new System.Drawing.Point(3, 3);
            twitterProfile.Name = "twitProf:" + id;
            twitterProfile.Size = new System.Drawing.Size(17, 17);
            twitterProfile.TabIndex = 0;
            twitterProfile.Image = global::TwitterClient.Properties.Resources.defaultProfile;
            twitterProfile.BackgroundImageLayout = ImageLayout.Stretch;
            twitterProfile.SizeMode = PictureBoxSizeMode.StretchImage;
            twitPan.Controls.Add(twitterProfile);



            //check if we have the profile picture or get it -- need to add not exist responce
            if (!UserProfilesManager.hasImage(username))
            {
                if (!UserProfilesManager.HasRequested(username))
                {
                    TwitterClientMain.SendStringToServer("61" + username);
                }
                UserProfilesManager.addAwaiting(twitterProfile, username);
            }
            else
            {
                //render image now
                twitterProfile.Image = UserProfilesManager.GetImage(username);
            }

            //add delete button if you are the owner (there is a server side check too)
            if (username.Equals(TwitterClientMain.CurrentUsername) && (isMainPage || !isMainTwit))
            {
                System.Windows.Forms.Button delButton = new System.Windows.Forms.Button();
                delButton.AutoSize = true;
                delButton.BackColor = System.Drawing.Color.White;
                delButton.Location = new System.Drawing.Point(twitPan.Width - 80, -3);
                delButton.Name = "delBut:" + id;
                delButton.BackgroundImage = global::TwitterClient.Properties.Resources.DeleteLogo;
                delButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                delButton.TabIndex = 0;
                delButton.Click += mainPage.delete_Click;
                twitPan.Controls.Add(delButton);
            }

            //the user label
            System.Windows.Forms.Label userLabel = new System.Windows.Forms.Label();
            userLabel.AutoSize = true;
            userLabel.Location = new System.Drawing.Point(0, 0);
            userLabel.Name = "userLab:" + id;
            userLabel.Size = new System.Drawing.Size(800, 13);
            userLabel.TabIndex = 0;
            userLabel.Text = "       " + username + "\n" + twitSeparator + "\n";
            userLabel.Click += mainPage.TwitUserClick;

            //isonlineLabel
            System.Windows.Forms.Label isOnlineLabel = new System.Windows.Forms.Label();
            isOnlineLabel.AutoSize = true;
            isOnlineLabel.Location = new System.Drawing.Point((int)((username).Length * 4.125)+55, 0);
            isOnlineLabel.TabIndex = 0;
            if (isonline)
            {
                isOnlineLabel.Text = "online";
                isOnlineLabel.BackColor = System.Drawing.Color.LimeGreen;
            }
            else
            {
                isOnlineLabel.Text = "offline";
                isOnlineLabel.BackColor = System.Drawing.Color.LightGray;
            }
            twitPan.Controls.Add(isOnlineLabel);
            twitPan.Controls.Add(userLabel);



            //the content label
            System.Windows.Forms.Label contentLabel = new System.Windows.Forms.Label();
            contentLabel.AutoSize = true;
            contentLabel.Location = new System.Drawing.Point(0, twitPan.Height);
            contentLabel.Name = "contentLab:" + id;
            contentLabel.Size = new System.Drawing.Size(800, 13);
            contentLabel.TabIndex = 0;
            if (isMainPage)
            {
                contentLabel.Click += mainPage.GetFullTwitRequest;
            }

            string resText = content;
            string adjusted = "";
            //add new lines for the size

            foreach (string line in resText.Split('\n'))
            {
                int amount = line.Length / lineLength;
                for (int i = 0; i < amount; i++)
                {
                    adjusted += line.Substring(i * lineLength, lineLength) + "\n";
                }
                if (line.Length % 264 != 0)
                {
                    adjusted += line.Substring(lineLength * amount, line.Length % lineLength);
                }
                adjusted += "\n";
            }
            contentLabel.Text = adjusted + "\n";



            twitPan.Controls.Add(contentLabel);
            if (isMainPage ||isMainTwit)
                twitPan.Height = Math.Max(contentLabel.Height + twitPan.Height, 80);
            
            else
                twitPan.Height = contentLabel.Height + twitPan.Height + 25;
                

            if (image == 1)
            {
                //Twit image
                PictureBox twitImage = new PictureBox();
                twitImage.Location = new System.Drawing.Point(3, twitPan.Height);
                twitImage.Name = "twitProf:" + id;
                twitImage.Size = new System.Drawing.Size(366, 246);
                twitImage.TabIndex = 0;
                twitImage.BackColor = Color.White;
                twitImage.BackgroundImageLayout = ImageLayout.Stretch;
                twitterProfile.SizeMode = PictureBoxSizeMode.StretchImage;
                twitPan.Controls.Add(twitImage);
                twitPan.Height += twitImage.Height;
                TwitImageHandler.addAwaiting(twitImage, id, !(isMainTwit || isMainPage));
                TwitterClientMain.SendStringToServer("91" + id);
            }
            else if (image == 2)
            {
                //video
                mainPage.Invoke(new Action(() =>
                {
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
                    AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
                    axWindowsMediaPlayer1.Enabled = true;
                    axWindowsMediaPlayer1.Location = new System.Drawing.Point(3, twitPan.Height);
                    axWindowsMediaPlayer1.Name = "play" + id;
                    axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
                    axWindowsMediaPlayer1.Size = new System.Drawing.Size(366, 246);
                    axWindowsMediaPlayer1.TabIndex = 0;
                    twitPan.Height += axWindowsMediaPlayer1.Height;
                    twitPan.Controls.Add(axWindowsMediaPlayer1);


                    VideoHandler.addAwaiting(id, axWindowsMediaPlayer1,!(isMainTwit || isMainPage));
                    TwitterClientMain.SendStringToServer("92" + id);

                }));

            }

            //the like button
            System.Windows.Forms.Button likeButton = new System.Windows.Forms.Button();
            likeButton.AutoSize = false;
            likeButton.Location = new System.Drawing.Point(10, twitPan.Height);
            likeButton.Name = "LikeBut:" + id;
            likeButton.Size = new System.Drawing.Size(25, 25);
            likeButton.TabIndex = 0;
            likeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            likeButton.FlatStyle = FlatStyle.Flat;
            likeButton.FlatAppearance.BorderSize = 0;

            //coloring and writing Like/Unlike
            if (Likes[0] == '1')
            {
                likeButton.BackgroundImage = global::TwitterClient.Properties.Resources.Liked;
            }
            else
            {
                likeButton.BackgroundImage = global::TwitterClient.Properties.Resources.NotLiked;
            }


            likeButton.Click += mainPage.LikeButtonClick;

            twitPan.Controls.Add(likeButton);
            twitPan.Height += likeButton.Height;

            //showes the number of likes a tweet has
            System.Windows.Forms.Label LikeLabel=new System.Windows.Forms.Label();
            LikeLabel.AutoSize = true;
            LikeLabel.Location = new System.Drawing.Point(likeButton.Location.X+likeButton.Width+1 , likeButton.Location.Y + (int)(likeButton.Height / 4));
            LikeLabel.Name = "likeLabel:"+Likes;
            LikeLabel.TabIndex = 0;
            LikeLabel.Text = Likes.Substring(1);
            twitPan.Controls.Add(LikeLabel);

            //replies logo
            PictureBox repliesNumberLogo=new PictureBox();
            repliesNumberLogo.Size = new System.Drawing.Size(25, 25);
            repliesNumberLogo.Image = global::TwitterClient.Properties.Resources.commentsLogo;
            repliesNumberLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            repliesNumberLogo.Location = new System.Drawing.Point(LikeLabel.Location.X + LikeLabel.Width + 5, likeButton.Location.Y);
            twitPan.Controls.Add(repliesNumberLogo);

            //replies number
            System.Windows.Forms.Label repliesNumLabel = new System.Windows.Forms.Label();
            repliesNumLabel.AutoSize = true;
            repliesNumLabel.Location = new System.Drawing.Point(repliesNumberLogo.Location.X + repliesNumberLogo.Width + 1, likeButton.Location.Y + (int)(likeButton.Height / 4));
            repliesNumLabel.TabIndex = 0;
            repliesNumLabel.Name = "repliesNumLabel";
            repliesNumLabel.Text = repliesNumber;
            twitPan.Controls.Add(repliesNumLabel);

            //date label (also tags)
            System.Windows.Forms.Label dateLabel = new System.Windows.Forms.Label();
            dateLabel.AutoSize = true;
            dateLabel.Location = new System.Drawing.Point(repliesNumLabel.Location.X+ repliesNumLabel.Width + 60, likeButton.Location.Y + (int)(likeButton.Height / 4));
            dateLabel.Name = "dateLab:" + id;
            dateLabel.TabIndex = 0;
            dateLabel.Text = "|  " + time + "  |";
            if (!tags.Equals(""))
            {
                dateLabel.Text += "                  |  " + tags + "  |";
            }



            twitPan.Controls.Add(dateLabel);
            return dateLabel;
        }


        /// <summary>
        /// recive a full twit
        /// </summary>
        /// <param name="responce"></param>
        private static void ReciveFullTwit(string responce) {
            mainPage.Invoke(new Action(() => { mainPage.TwitRepliesPanel.Controls.Clear(); }));
            
            //Console.WriteLine(responce);
            mainPage.Invoke(new Action(()=>{ mainPage.SwitchToRepliesPage(); }));
            JsonDocument jsonDocument = JsonDocument.Parse(responce);
            JsonElement jsonElement = jsonDocument.RootElement;

            // twitPageMainTwit
            Panel twitPageMainTwit=new Panel();
            twitPageMainTwit.Location = new System.Drawing.Point(3, 3);
            twitPageMainTwit.Name = "twitPageMainTwit";
            twitPageMainTwit.Size = new System.Drawing.Size(835 , 25);
            twitPageMainTwit.TabIndex = 0;
            twitPageMainTwit.BackColor = System.Drawing.Color.LightBlue;

            int id = Int32.Parse(jsonElement.GetProperty("id").ToString());
            string username=jsonElement.GetProperty("username").ToString();
            string content=jsonElement.GetProperty("content").ToString();
            bool hasLiked = jsonElement.GetProperty("hasLiked").GetBoolean();
            string likes = jsonElement.GetProperty("likes").GetString();
            string time = jsonElement.GetProperty("time").GetString();
            bool isOnline = jsonElement.GetProperty("isonline").GetBoolean();
            int image = Int32.Parse(jsonElement.GetProperty("image").ToString());
            

            System.Windows.Forms.Label timeLabel = twitObjects(twitPageMainTwit, id, username, content, time, Convert.ToInt32(hasLiked).ToString() + likes, "", 0, isOnline,image,false,true, ""+jsonElement.GetProperty("replies").EnumerateArray().Count());
                //mainTwitData(twitPageMainTwit, id,username,isOnline, content,hasLiked,likes,time);
               

            //reply button
            Button replyButton = new Button();
            replyButton.Size = new System.Drawing.Size(25, 25);
            replyButton.AutoSize = false;
            replyButton.Location = new Point(timeLabel.Location.X + timeLabel.Width, timeLabel.Location.Y - 3);
            replyButton.BackgroundImage = global::TwitterClient.Properties.Resources.replyUnselected;
            replyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            replyButton.Name = "replyButton:" + id ;
            replyButton.TabIndex = 1;
            replyButton.Click += mainPage.selectReply_Click;
            twitPageMainTwit.Controls.Add(replyButton);

           
            mainPage.Invoke(new Action(() => { mainPage.TwitRepliesPanel.Controls.Add(twitPageMainTwit); }));
            
            foreach (JsonElement item in jsonElement.GetProperty("replies").EnumerateArray()) {
                createReply(item, mainPage.TwitRepliesPanel,0);
            }


            
        }
        
        private static System.Drawing.Color[] colorDepth = new System.Drawing.Color[3] { System.Drawing.Color.Gray, System.Drawing.Color.LightGray, System.Drawing.Color.LightCyan};
        
        /// <summary>
        /// create every reply panel and its child replies
        /// </summary>
        /// <param name="jsonElement"></param>
        /// <param name="parent"></param>
        /// <param name="level"></param>
        private static void createReply(JsonElement jsonElement,FlowLayoutPanel parent,int level) {
            FlowLayoutPanel childReplies = new FlowLayoutPanel();
            childReplies.Name = parent.Name+".";
            childReplies.Size = new System.Drawing.Size(835-7*level, 0);
            childReplies.TabIndex = 0;
            childReplies.BackColor = colorDepth[level % 3];
            if (level != 0)
            {
                childReplies.Margin = new Padding(7, 0, 3, 3);
            }

            //mainReply
            Panel mainReply = new Panel();
            mainReply.Size = new Size(835-7*level,25);
            childReplies.Controls.Add(mainReply);

            string name= jsonElement.GetProperty("username").ToString();
            string id = jsonElement.GetProperty("id").ToString();
            string time = jsonElement.GetProperty("time").ToString();
            bool isOnline = jsonElement.GetProperty("isonline").GetBoolean();
            string content = jsonElement.GetProperty("content").ToString();
            string likes = jsonElement.GetProperty("likes").ToString();
            bool hasLiked = jsonElement.GetProperty("hasLiked").GetBoolean();
            int image = Int32.Parse(jsonElement.GetProperty("image").ToString());



            System.Windows.Forms.Label timeLabel = twitObjects(mainReply, -(Int32.Parse(id) + 1),name,content,time, Convert.ToInt32(hasLiked).ToString() +likes, "",0,isOnline,image,false,false, ""+jsonElement.GetProperty("replies").EnumerateArray().Count());


            //reply button
            Button replyButton = new Button();
            replyButton.Size = new System.Drawing.Size(25, 25);
            replyButton.AutoSize = false;
            replyButton.Location = new Point(timeLabel.Location.X + timeLabel.Width, timeLabel.Location.Y-3);
            replyButton.Name = "replyButton:" + (-Int32.Parse(jsonElement.GetProperty("id").ToString()) - 1);
            replyButton.TabIndex = 1;
            replyButton.BackgroundImage = global::TwitterClient.Properties.Resources.replyUnselected;
            replyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            replyButton.Click += mainPage.selectReply_Click;
            mainReply.Controls.Add(replyButton);

            //child replies
            foreach (JsonElement item in jsonElement.GetProperty("replies").EnumerateArray())
            {
                createReply(item, childReplies,level+1);
            }
            childReplies.Size = new System.Drawing.Size(childReplies.Width,3+ childReplies.Controls[childReplies.Controls.Count - 1].Height + childReplies.Controls[childReplies.Controls.Count - 1].Location.Y);

            if (level == 0)
            {
                mainPage.Invoke(new Action(() => { parent.Controls.Add(childReplies); }));
            }
            else {
                parent.Controls.Add(childReplies);
            }

        }
        

        /// <summary>
        /// display a new reply
        /// </summary>
        /// <param name="responce"></param>
        private static void displayNewReply(string responce) {
            int id=Int32.Parse(responce.Split(':')[0]);
            int image= Int32.Parse(responce.Split(':')[1]);
            FlowLayoutPanel parent =NewReplyData.parent;
            int level=parent.Name.Substring(16).Length;

            FlowLayoutPanel childReplies = new FlowLayoutPanel();
            childReplies.Name = parent.Name + ".";
            childReplies.Size = new System.Drawing.Size(835 - 3 * level, 0);
            childReplies.TabIndex = 0;
            childReplies.BackColor = colorDepth[level % 3];

            //mainReply
            Panel mainReply = new Panel();
            mainReply.Size = new Size(835 - 3 * level, 25);
            childReplies.Controls.Add(mainReply);

            

            System.Windows.Forms.Label timeLabel= twitObjects(mainReply, -(id + 1), TwitterClientMain.CurrentUsername, NewReplyData.content, NewReplyData.time,"00", "", 0, true, image, false, false,"0");
            //mainTwitData(mainReply,-id-1,Program.CurrentUsername,true,NewTwitData.content,false,"0",NewTwitData.time);
            //reply button
            Button replyButton = new Button();
            replyButton.Size = new System.Drawing.Size(25, 25);
            replyButton.AutoSize = false;
            replyButton.Location = new Point(timeLabel.Location.X + timeLabel.Width, timeLabel.Location.Y - 3);
            replyButton.BackgroundImage = global::TwitterClient.Properties.Resources.replyUnselected;
            replyButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            replyButton.Name = "replyButton:" + (-id - 1); ;
            replyButton.TabIndex = 1;
            replyButton.BackColor = System.Drawing.Color.LightCyan;
            replyButton.Click += mainPage.selectReply_Click;
            mainReply.Controls.Add(replyButton);

            childReplies.Size = new System.Drawing.Size(childReplies.Width, 3 + childReplies.Controls[childReplies.Controls.Count - 1].Height + childReplies.Controls[childReplies.Controls.Count - 1].Location.Y);

            mainPage.Invoke(new Action(() => {
                parent.Controls.Add(childReplies);
                while (parent != mainPage.TwitRepliesPanel)
                {
                    parent.Size = new System.Drawing.Size(parent.Width, 3 + parent.Controls[parent.Controls.Count - 1].Height + parent.Controls[parent.Controls.Count - 1].Location.Y);
                    parent = parent.Parent as FlowLayoutPanel ;
                }
            }));

            System.Windows.Forms.Label likeLabel = childReplies.Parent.Controls
                .OfType<System.Windows.Forms.Panel>()
                    .FirstOrDefault().Controls
                .OfType<System.Windows.Forms.Label>()
                    .FirstOrDefault(t => t.Name == "repliesNumLabel");
            mainPage.Invoke(new Action(() =>
            {
                likeLabel.Text = "" + (Int32.Parse(likeLabel.Text) + 1);
            }));
        }

        /// <summary>
        /// update a twit's attachment
        /// </summary>
        /// <param name="responce"></param>
        private static void UpdateTwitAttachment(string responce) {
            byte[] imgBytes=null;
            int type = Int32.Parse(""+responce[0]);
            string name=responce.Substring(1);
            lock (communicatorClient.GetTcpClient().GetStream())
            {
                while (imgBytes == null)
                {
                    imgBytes = communicatorClient.ReadBytes();
                }
            }
            if (type == 1)
            {
                Image res;
                using (var ms = new MemoryStream(imgBytes))
                {
                    res = Image.FromStream(ms);

                }
                TwitImageHandler.NewImageRecived(res, Int32.Parse(name.Split('.')[0]));
            }
            else {
                VideoHandler.addVideo(name,imgBytes);
            }
        }

        /// <summary>
        /// display the responce for a user search request
        /// </summary>
        /// <param name="responce"></param>
        private static void UserSearchResponce(string responce) {
            JsonDocument resp=JsonDocument.Parse(responce);
            JsonElement root =resp.RootElement;
            List<string> users = root.GetProperty("users").EnumerateArray().Select(u => u.GetString()).ToList();
            mainPage.Invoke(new Action(() => {
                mainPage.TwitsViewer.Controls.Clear();
                mainPage.repliesViewControls.Hide();
                mainPage.TwitRepliesPanel.Hide();
            }));
            VideoHandler.Reset();
            for (int i = 0; i < users.Count; i++) {
                Panel panel = new Panel();
                panel.Size = new Size(800, 40);
                panel.BackColor = Color.LightBlue;

                PictureBox profilePic = new PictureBox();
                profilePic.Location = new Point(3, 3);
                profilePic.Size = new Size(34, 34);
                profilePic.Image = global::TwitterClient.Properties.Resources.defaultProfile;
                profilePic.SizeMode = PictureBoxSizeMode.StretchImage;

                if (!UserProfilesManager.hasImage(users[i]))
                {
                    if (!UserProfilesManager.HasRequested(users[i]))
                    {
                        TwitterClientMain.SendStringToServer("61" + users[i]);
                    }
                    UserProfilesManager.addAwaiting(profilePic, users[i]);
                }
                else
                {
                    //render image now
                    profilePic.Image = UserProfilesManager.GetImage(users[i]);
                }
                panel.Controls.Add(profilePic);
                
                System.Windows.Forms.Label nameLabel=new System.Windows.Forms.Label();
                nameLabel.Size = new Size(700, 40);
                nameLabel.Text = users[i];
                nameLabel.Font= new Font("Arial", 24, FontStyle.Bold);
                nameLabel.Location=new Point(profilePic.Location.X + profilePic.Width + 3, 0);
                nameLabel.Click += mainPage.UserSearchNameClick;


                panel.Controls.Add(nameLabel);

                mainPage.Invoke(new Action(() => {
                    mainPage.TwitsViewer.Controls.Add(panel);
                }));
            }
            mainPage.Invoke(new Action(() =>
            {
                mainPage.SearchButton.Enabled = true;
            }));
        }
    }

    /// <summary>
    /// every twit is put into this one
    /// </summary>
    internal class Twit {

        public int id { get; set; }
        public string username { get; set; }
        public string content { get; set; }
        public string time { get; set; }
        public string Likes { get; set; }
        public string tags { get; set; }
        public int replies { get; set; }
        public bool isonline { get; set; }
        public int image { get; set; }
           
    }

    /// <summary>
    /// remembers the data for a new reply
    /// </summary>
    internal static class NewReplyData{
        public static FlowLayoutPanel parent;
        public static string content;
        public static string time;
    }
}
