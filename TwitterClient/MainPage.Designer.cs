namespace TwitterClient
{
    partial class MainPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            this.SearchPanel = new System.Windows.Forms.Panel();
            this.SearchTypeBox = new System.Windows.Forms.ComboBox();
            this.OnlyFollowedCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tagsLabel = new System.Windows.Forms.Label();
            this.tagsBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.TwitsViewer = new System.Windows.Forms.FlowLayoutPanel();
            this.TwitCreater = new System.Windows.Forms.Panel();
            this.selectImageButton = new System.Windows.Forms.Button();
            this.twitAttachment = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.createTwitButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.newTwitTags = new System.Windows.Forms.TextBox();
            this.newTwitContent = new System.Windows.Forms.RichTextBox();
            this.logout = new System.Windows.Forms.Button();
            this.userPagePanel = new System.Windows.Forms.Panel();
            this.userPageProfile = new System.Windows.Forms.PictureBox();
            this.followersCount = new System.Windows.Forms.Label();
            this.followButton = new System.Windows.Forms.Button();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.userTwitsDisplay = new System.Windows.Forms.FlowLayoutPanel();
            this.userToMainButton = new System.Windows.Forms.Button();
            this.profilePickButton = new System.Windows.Forms.Button();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.TwitRepliesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.repliesViewControls = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.selectedReplyAttachment = new System.Windows.Forms.Button();
            this.replyAttachmentType = new System.Windows.Forms.ComboBox();
            this.repContent = new System.Windows.Forms.RichTextBox();
            this.repbut = new System.Windows.Forms.Button();
            this.RepliesToMainButton = new System.Windows.Forms.Button();
            this.goToMyPageButton = new System.Windows.Forms.Button();
            this.selectedTwitImage = new System.Windows.Forms.OpenFileDialog();
            this.replyFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.twitCreatorButton = new System.Windows.Forms.Button();
            this.SearchPanel.SuspendLayout();
            this.TwitCreater.SuspendLayout();
            this.userPagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userPageProfile)).BeginInit();
            this.repliesViewControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchPanel
            // 
            this.SearchPanel.Controls.Add(this.SearchTypeBox);
            this.SearchPanel.Controls.Add(this.OnlyFollowedCheckBox);
            this.SearchPanel.Controls.Add(this.label2);
            this.SearchPanel.Controls.Add(this.tagsLabel);
            this.SearchPanel.Controls.Add(this.tagsBox);
            this.SearchPanel.Controls.Add(this.SearchButton);
            this.SearchPanel.Location = new System.Drawing.Point(12, 12);
            this.SearchPanel.Name = "SearchPanel";
            this.SearchPanel.Size = new System.Drawing.Size(960, 35);
            this.SearchPanel.TabIndex = 0;
            // 
            // SearchTypeBox
            // 
            this.SearchTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SearchTypeBox.FormattingEnabled = true;
            this.SearchTypeBox.Items.AddRange(new object[] {
            "Twit Search",
            "User Search"});
            this.SearchTypeBox.Location = new System.Drawing.Point(4, 9);
            this.SearchTypeBox.Name = "SearchTypeBox";
            this.SearchTypeBox.Size = new System.Drawing.Size(107, 21);
            this.SearchTypeBox.TabIndex = 5;
            this.SearchTypeBox.SelectedIndexChanged += new System.EventHandler(this.SearchTypeBox_SelectedIndexChanged);
            // 
            // OnlyFollowedCheckBox
            // 
            this.OnlyFollowedCheckBox.AutoSize = true;
            this.OnlyFollowedCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OnlyFollowedCheckBox.Location = new System.Drawing.Point(766, 12);
            this.OnlyFollowedCheckBox.Name = "OnlyFollowedCheckBox";
            this.OnlyFollowedCheckBox.Size = new System.Drawing.Size(15, 14);
            this.OnlyFollowedCheckBox.TabIndex = 4;
            this.OnlyFollowedCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(635, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "only followed:";
            // 
            // tagsLabel
            // 
            this.tagsLabel.AutoSize = true;
            this.tagsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagsLabel.Location = new System.Drawing.Point(117, 6);
            this.tagsLabel.Name = "tagsLabel";
            this.tagsLabel.Size = new System.Drawing.Size(49, 24);
            this.tagsLabel.TabIndex = 2;
            this.tagsLabel.Text = "tags:";
            // 
            // tagsBox
            // 
            this.tagsBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tagsBox.Location = new System.Drawing.Point(172, 4);
            this.tagsBox.Name = "tagsBox";
            this.tagsBox.Size = new System.Drawing.Size(457, 26);
            this.tagsBox.TabIndex = 1;
            // 
            // SearchButton
            // 
            this.SearchButton.BackgroundImage = global::TwitterClient.Properties.Resources.searchLogo1;
            this.SearchButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SearchButton.Location = new System.Drawing.Point(859, 4);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SearchButton.Size = new System.Drawing.Size(98, 28);
            this.SearchButton.TabIndex = 0;
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // TwitsViewer
            // 
            this.TwitsViewer.Location = new System.Drawing.Point(12, 54);
            this.TwitsViewer.Name = "TwitsViewer";
            this.TwitsViewer.Size = new System.Drawing.Size(860, 545);
            this.TwitsViewer.TabIndex = 1;
            // 
            // TwitCreater
            // 
            this.TwitCreater.Controls.Add(this.selectImageButton);
            this.TwitCreater.Controls.Add(this.twitAttachment);
            this.TwitCreater.Controls.Add(this.label4);
            this.TwitCreater.Controls.Add(this.createTwitButton);
            this.TwitCreater.Controls.Add(this.label3);
            this.TwitCreater.Controls.Add(this.newTwitTags);
            this.TwitCreater.Controls.Add(this.newTwitContent);
            this.TwitCreater.Location = new System.Drawing.Point(12, 54);
            this.TwitCreater.Name = "TwitCreater";
            this.TwitCreater.Size = new System.Drawing.Size(860, 545);
            this.TwitCreater.TabIndex = 2;
            // 
            // selectImageButton
            // 
            this.selectImageButton.Location = new System.Drawing.Point(225, 439);
            this.selectImageButton.Name = "selectImageButton";
            this.selectImageButton.Size = new System.Drawing.Size(291, 23);
            this.selectImageButton.TabIndex = 6;
            this.selectImageButton.Text = "select image";
            this.selectImageButton.UseVisualStyleBackColor = true;
            this.selectImageButton.Click += new System.EventHandler(this.selectImageButton_Click);
            // 
            // twitAttachment
            // 
            this.twitAttachment.DisplayMember = "0";
            this.twitAttachment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.twitAttachment.FormattingEnabled = true;
            this.twitAttachment.Items.AddRange(new object[] {
            "None",
            "Image",
            "Video"});
            this.twitAttachment.Location = new System.Drawing.Point(98, 439);
            this.twitAttachment.Name = "twitAttachment";
            this.twitAttachment.Size = new System.Drawing.Size(121, 21);
            this.twitAttachment.TabIndex = 5;
            this.twitAttachment.SelectedIndexChanged += new System.EventHandler(this.TwitAttachmentChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 442);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "attachment:";
            // 
            // createTwitButton
            // 
            this.createTwitButton.BackgroundImage = global::TwitterClient.Properties.Resources.createTwitSymbol;
            this.createTwitButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.createTwitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createTwitButton.Location = new System.Drawing.Point(19, 474);
            this.createTwitButton.Name = "createTwitButton";
            this.createTwitButton.Size = new System.Drawing.Size(820, 57);
            this.createTwitButton.TabIndex = 3;
            this.createTwitButton.UseVisualStyleBackColor = true;
            this.createTwitButton.Click += new System.EventHandler(this.CreateTwit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "tags:";
            // 
            // newTwitTags
            // 
            this.newTwitTags.Location = new System.Drawing.Point(74, 33);
            this.newTwitTags.Name = "newTwitTags";
            this.newTwitTags.Size = new System.Drawing.Size(756, 20);
            this.newTwitTags.TabIndex = 1;
            // 
            // newTwitContent
            // 
            this.newTwitContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newTwitContent.Location = new System.Drawing.Point(19, 63);
            this.newTwitContent.Name = "newTwitContent";
            this.newTwitContent.Size = new System.Drawing.Size(820, 359);
            this.newTwitContent.TabIndex = 0;
            this.newTwitContent.Text = "";
            // 
            // logout
            // 
            this.logout.BackColor = System.Drawing.Color.Gray;
            this.logout.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.logout.Location = new System.Drawing.Point(882, 515);
            this.logout.Margin = new System.Windows.Forms.Padding(2);
            this.logout.Name = "logout";
            this.logout.Size = new System.Drawing.Size(90, 35);
            this.logout.TabIndex = 4;
            this.logout.Text = "logout";
            this.logout.UseVisualStyleBackColor = false;
            this.logout.Click += new System.EventHandler(this.logout_Click);
            // 
            // userPagePanel
            // 
            this.userPagePanel.Controls.Add(this.userPageProfile);
            this.userPagePanel.Controls.Add(this.followersCount);
            this.userPagePanel.Controls.Add(this.followButton);
            this.userPagePanel.Controls.Add(this.userNameLabel);
            this.userPagePanel.Controls.Add(this.userTwitsDisplay);
            this.userPagePanel.Controls.Add(this.userToMainButton);
            this.userPagePanel.Location = new System.Drawing.Point(12, 53);
            this.userPagePanel.Name = "userPagePanel";
            this.userPagePanel.Size = new System.Drawing.Size(861, 546);
            this.userPagePanel.TabIndex = 5;
            // 
            // userPageProfile
            // 
            this.userPageProfile.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.userPageProfile.Location = new System.Drawing.Point(4, 17);
            this.userPageProfile.Name = "userPageProfile";
            this.userPageProfile.Size = new System.Drawing.Size(60, 55);
            this.userPageProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.userPageProfile.TabIndex = 5;
            this.userPageProfile.TabStop = false;
            // 
            // followersCount
            // 
            this.followersCount.AutoSize = true;
            this.followersCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.followersCount.Location = new System.Drawing.Point(580, 32);
            this.followersCount.Name = "followersCount";
            this.followersCount.Size = new System.Drawing.Size(94, 24);
            this.followersCount.TabIndex = 4;
            this.followersCount.Text = "followers: ";
            // 
            // followButton
            // 
            this.followButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.followButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.followButton.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.followButton.Location = new System.Drawing.Point(194, 17);
            this.followButton.Name = "followButton";
            this.followButton.Size = new System.Drawing.Size(92, 64);
            this.followButton.TabIndex = 3;
            this.followButton.Text = "follow";
            this.followButton.UseVisualStyleBackColor = false;
            this.followButton.Click += new System.EventHandler(this.followButton_Click);
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userNameLabel.Location = new System.Drawing.Point(70, 17);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(118, 55);
            this.userNameLabel.TabIndex = 2;
            this.userNameLabel.Text = "user";
            // 
            // userTwitsDisplay
            // 
            this.userTwitsDisplay.Location = new System.Drawing.Point(4, 87);
            this.userTwitsDisplay.Name = "userTwitsDisplay";
            this.userTwitsDisplay.Size = new System.Drawing.Size(853, 456);
            this.userTwitsDisplay.TabIndex = 1;
            // 
            // userToMainButton
            // 
            this.userToMainButton.BackgroundImage = global::TwitterClient.Properties.Resources.mainPageIcon;
            this.userToMainButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.userToMainButton.Location = new System.Drawing.Point(780, 4);
            this.userToMainButton.Name = "userToMainButton";
            this.userToMainButton.Size = new System.Drawing.Size(77, 77);
            this.userToMainButton.TabIndex = 0;
            this.userToMainButton.UseVisualStyleBackColor = true;
            this.userToMainButton.Click += new System.EventHandler(this.userToMainButton_Click);
            // 
            // profilePickButton
            // 
            this.profilePickButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.profilePickButton.Location = new System.Drawing.Point(899, 179);
            this.profilePickButton.Name = "profilePickButton";
            this.profilePickButton.Size = new System.Drawing.Size(50, 50);
            this.profilePickButton.TabIndex = 6;
            this.profilePickButton.Text = "switch profile";
            this.profilePickButton.UseVisualStyleBackColor = true;
            this.profilePickButton.Click += new System.EventHandler(this.profilePickButton_Click);
            // 
            // fileDialog
            // 
            this.fileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.fileDialog_FileOk);
            // 
            // TwitRepliesPanel
            // 
            this.TwitRepliesPanel.Location = new System.Drawing.Point(12, 53);
            this.TwitRepliesPanel.Name = "TwitRepliesPanel";
            this.TwitRepliesPanel.Size = new System.Drawing.Size(861, 458);
            this.TwitRepliesPanel.TabIndex = 8;
            // 
            // repliesViewControls
            // 
            this.repliesViewControls.Controls.Add(this.label5);
            this.repliesViewControls.Controls.Add(this.selectedReplyAttachment);
            this.repliesViewControls.Controls.Add(this.replyAttachmentType);
            this.repliesViewControls.Controls.Add(this.repContent);
            this.repliesViewControls.Controls.Add(this.repbut);
            this.repliesViewControls.Controls.Add(this.RepliesToMainButton);
            this.repliesViewControls.Location = new System.Drawing.Point(12, 511);
            this.repliesViewControls.Name = "repliesViewControls";
            this.repliesViewControls.Size = new System.Drawing.Size(860, 90);
            this.repliesViewControls.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(487, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "attachment";
            // 
            // selectedReplyAttachment
            // 
            this.selectedReplyAttachment.Location = new System.Drawing.Point(553, 33);
            this.selectedReplyAttachment.Name = "selectedReplyAttachment";
            this.selectedReplyAttachment.Size = new System.Drawing.Size(121, 52);
            this.selectedReplyAttachment.TabIndex = 5;
            this.selectedReplyAttachment.Text = "select";
            this.selectedReplyAttachment.UseVisualStyleBackColor = true;
            this.selectedReplyAttachment.Click += new System.EventHandler(this.selectedReplyAttachment_Click);
            // 
            // replyAttachmentType
            // 
            this.replyAttachmentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.replyAttachmentType.FormattingEnabled = true;
            this.replyAttachmentType.Items.AddRange(new object[] {
            "None",
            "Image",
            "Video"});
            this.replyAttachmentType.Location = new System.Drawing.Point(553, 6);
            this.replyAttachmentType.Name = "replyAttachmentType";
            this.replyAttachmentType.Size = new System.Drawing.Size(121, 21);
            this.replyAttachmentType.TabIndex = 4;
            this.replyAttachmentType.SelectedIndexChanged += new System.EventHandler(this.replyAttachmentType_SelectedIndexChanged);
            // 
            // repContent
            // 
            this.repContent.Location = new System.Drawing.Point(42, 6);
            this.repContent.MaxLength = 5000;
            this.repContent.Name = "repContent";
            this.repContent.Size = new System.Drawing.Size(431, 81);
            this.repContent.TabIndex = 3;
            this.repContent.Text = "";
            // 
            // repbut
            // 
            this.repbut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.repbut.BackgroundImage = global::TwitterClient.Properties.Resources.replyDisabled;
            this.repbut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.repbut.Location = new System.Drawing.Point(691, 4);
            this.repbut.Name = "repbut";
            this.repbut.Size = new System.Drawing.Size(90, 81);
            this.repbut.TabIndex = 2;
            this.repbut.UseVisualStyleBackColor = false;
            this.repbut.Click += new System.EventHandler(this.repbut_Click);
            // 
            // RepliesToMainButton
            // 
            this.RepliesToMainButton.BackgroundImage = global::TwitterClient.Properties.Resources.mainPageIcon;
            this.RepliesToMainButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RepliesToMainButton.Location = new System.Drawing.Point(787, 0);
            this.RepliesToMainButton.Name = "RepliesToMainButton";
            this.RepliesToMainButton.Size = new System.Drawing.Size(71, 85);
            this.RepliesToMainButton.TabIndex = 0;
            this.RepliesToMainButton.UseVisualStyleBackColor = true;
            this.RepliesToMainButton.Click += new System.EventHandler(this.ToMainPage_Click);
            // 
            // goToMyPageButton
            // 
            this.goToMyPageButton.Location = new System.Drawing.Point(882, 249);
            this.goToMyPageButton.Name = "goToMyPageButton";
            this.goToMyPageButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.goToMyPageButton.Size = new System.Drawing.Size(87, 22);
            this.goToMyPageButton.TabIndex = 11;
            this.goToMyPageButton.Text = "my page";
            this.goToMyPageButton.UseVisualStyleBackColor = true;
            this.goToMyPageButton.Click += new System.EventHandler(this.goToMyPageButton_Click);
            // 
            // selectedTwitImage
            // 
            this.selectedTwitImage.FileName = "selectedTwitImage";
            this.selectedTwitImage.FileOk += new System.ComponentModel.CancelEventHandler(this.selectedTwitImage_FileOk);
            // 
            // replyFileDialog
            // 
            this.replyFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.replyFileDialog_FileOk);
            // 
            // twitCreatorButton
            // 
            this.twitCreatorButton.BackgroundImage = global::TwitterClient.Properties.Resources.new_twit;
            this.twitCreatorButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.twitCreatorButton.Location = new System.Drawing.Point(879, 54);
            this.twitCreatorButton.Name = "twitCreatorButton";
            this.twitCreatorButton.Size = new System.Drawing.Size(90, 80);
            this.twitCreatorButton.TabIndex = 3;
            this.twitCreatorButton.UseVisualStyleBackColor = true;
            this.twitCreatorButton.Click += new System.EventHandler(this.twitCreatorButton_Click);
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 611);
            this.Controls.Add(this.goToMyPageButton);
            this.Controls.Add(this.profilePickButton);
            this.Controls.Add(this.TwitRepliesPanel);
            this.Controls.Add(this.logout);
            this.Controls.Add(this.repliesViewControls);
            this.Controls.Add(this.userPagePanel);
            this.Controls.Add(this.TwitsViewer);
            this.Controls.Add(this.twitCreatorButton);
            this.Controls.Add(this.TwitCreater);
            this.Controls.Add(this.SearchPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1000, 650);
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "MainPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Twitter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainPage_Closed);
            this.SearchPanel.ResumeLayout(false);
            this.SearchPanel.PerformLayout();
            this.TwitCreater.ResumeLayout(false);
            this.TwitCreater.PerformLayout();
            this.userPagePanel.ResumeLayout(false);
            this.userPagePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userPageProfile)).EndInit();
            this.repliesViewControls.ResumeLayout(false);
            this.repliesViewControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel SearchPanel;
        internal System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox tagsBox;
        private System.Windows.Forms.CheckBox OnlyFollowedCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label tagsLabel;
        public System.Windows.Forms.FlowLayoutPanel TwitsViewer;
        private System.Windows.Forms.Panel TwitCreater;
        internal System.Windows.Forms.Button twitCreatorButton;
        private System.Windows.Forms.Button createTwitButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox newTwitTags;
        private System.Windows.Forms.RichTextBox newTwitContent;
        private System.Windows.Forms.Button logout;
        private System.Windows.Forms.Panel userPagePanel;
        public System.Windows.Forms.Label userNameLabel;
        public System.Windows.Forms.FlowLayoutPanel userTwitsDisplay;
        private System.Windows.Forms.Button userToMainButton;
        public System.Windows.Forms.Button followButton;
        public System.Windows.Forms.Label followersCount;
        private System.Windows.Forms.Button profilePickButton;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        public System.Windows.Forms.PictureBox userPageProfile;
        public System.Windows.Forms.FlowLayoutPanel TwitRepliesPanel;
        public System.Windows.Forms.Panel repliesViewControls;
        private System.Windows.Forms.Button RepliesToMainButton;
        private System.Windows.Forms.Button goToMyPageButton;
        internal System.Windows.Forms.Button selectImageButton;
        internal System.Windows.Forms.ComboBox twitAttachment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog selectedTwitImage;
        private System.Windows.Forms.Button repbut;
        private System.Windows.Forms.RichTextBox repContent;
        private System.Windows.Forms.ComboBox replyAttachmentType;
        private System.Windows.Forms.Button selectedReplyAttachment;
        private System.Windows.Forms.OpenFileDialog replyFileDialog;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox SearchTypeBox;
    }
}