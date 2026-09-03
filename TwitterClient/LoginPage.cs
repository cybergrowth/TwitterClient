using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TwitterClient;

namespace TwitterClient
{
    /// <summary>
    /// the page where you can login,register and change your password
    /// </summary>
    public partial class LoginPage : Form
    {
        private static string ip = "127.0.0.1";
        private static int port = 500;

        public LoginPage()
        {
            InitializeComponent();
            registerPanel.Hide();
            resetPasswordPanel.Hide();
            forcedPassChangePanel.Hide();
        }
        /// <summary>
        /// start register request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void registerButton_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(IPAddress.Parse(ip), port);
            }
            catch
            {
                MessageBox.Show("Failed to connect to server");
                return;
            }

            TwitterClientMain.server = new CommunicatorClient(tcpClient);
            JsonObject res = new JsonObject()
            {
                ["username"] = usernameRegister.Text,
                ["password"] = passwordRegister.Text,
                ["email"] = emailRegister.Text
            };
            TwitterClientMain.SendStringToServer('1'+res.ToJsonString() );
            string output =TwitterClientMain.GetStringFromServer();
            bool success = false;
            switch (output[0]){
                case '0':
                    //switching to auth page
                    loginModeCheckBox.Hide();
                    resetPassCheckBox.Hide();
                    registerPanel.Hide();
                    loginPanel.Hide();
                    resetPasswordPanel.Hide();
                    authPanel.Show();
                    emailLabel.Text = "Sent a code to " + output.Substring(1);


                    byte[] imgBytes = TwitterClientMain.server.ReadBytes();
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        captchaBox.Image = Image.FromStream(ms);
                    }
                    success = true;
                    break;
                case '1':
                    MessageBox.Show("Username taken");
                    break;
                case '2':
                    if (output[1] == '0')
                        MessageBox.Show("Invalid characters:' and , ");
                    else if (output[1] == '1')
                        MessageBox.Show("Invalid email address");
                    else if (output[1] == '2')
                        MessageBox.Show("Username should be at least 1 char ");
                    else
                    {
                        MessageBox.Show("Password at least 8 chars long,\n and include lower,upper,number,special\n type letters");
                    }
                    break;
                }
            if (!success) {
                TwitterClientMain.server.Close();
            }
        }


        /// <summary>
        /// start login request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginButton_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(IPAddress.Parse(ip), port);
            }
            catch {
                MessageBox.Show("Failed to connect to server");
                return;
            }
            TwitterClientMain.server = new CommunicatorClient(tcpClient);

            JsonObject res = new JsonObject() {
                ["username"]= loginUser.Text,
                ["password"]= loginPassword.Text
            };
            TwitterClientMain.SendStringToServer('0'+ res.ToJsonString() );
            TwitterClientMain.CurrentUsername = loginUser.Text;

            
            string output = TwitterClientMain.GetStringFromServer();

            if (output[0] == '1') { MessageBox.Show("Username or password wrong"); }
            else {
                loginModeCheckBox.Hide();
                loginModeCheckBox.Hide();
                resetPassCheckBox.Hide();
                resetPasswordPanel.Hide();
                loginPanel.Hide();
                authPanel.Show();
                emailLabel.Text = "Sent a code to " + output.Substring(1);

                byte[] imgBytes=null;
                while (imgBytes == null)
                {
                    //await the image
                    imgBytes = TwitterClientMain.server.ReadBytes();
                }
                using (MemoryStream ms = new MemoryStream(imgBytes))
                {
                    captchaBox.Image = Image.FromStream(ms);
                }
            }
        }

        /// <summary>
        /// switch between login and register mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginModeBox_CheckedChanged(object sender, EventArgs e)
        {
            loginPanel.Visible = !loginModeCheckBox.Checked;
            registerPanel.Visible = loginModeCheckBox.Checked;
        }

        /// <summary>
        /// Authentication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void codeButton_Click(object sender, EventArgs e)
        {
            TwitterClientMain.SendStringToServer(codeTextBox.Text+":"+captchaText.Text);
            string res = TwitterClientMain.GetStringFromServer();
            if (res.Equals("0"))
            {
                if (resetPassCheckBox.Checked)
                {
                    MessageBox.Show("Password changed");
                    resetPassCheckBox.Show();
                    resetPasswordPanel.Show();
                    authPanel.Hide();
                    codeTextBox.Text = "";
                    captchaText.Text = "";
                    resetPassUser.Text = "";
                    resetPassEmail.Text = "";
                    resetPassPassword.Text = "";
                    TwitterClientMain.server.Close();
                }
                else if (loginModeCheckBox.Checked)
                {//register
                    MessageBox.Show("registion successfull");
                    authPanel.Hide();
                    registerButton.Show();
                    loginModeCheckBox.Show();
                    registerPanel.Show();
                    codeTextBox.Text = "";
                    captchaText.Text = "";
                    usernameRegister.Text = "";
                    passwordRegister.Text = "";
                    emailRegister.Text = "";
                    resetPassCheckBox.Show();
                    TwitterClientMain.server.Close();
                }
                else
                {//login
                    MessageBox.Show("You have logged in");
                    TwitterClientMain.server.StartReading();
                    TwitterClientMain.SwitchToMainPage();

                    resetPassCheckBox.Show();
                    authPanel.Hide();
                    loginPanel.Show();
                    loginModeCheckBox.Show();
                    codeTextBox.Text = "";
                    captchaText.Text = "";
                    loginUser.Text = "";
                    loginPassword.Text = "";
                }
            }
            else if (res.Equals("2"))
            {
                //auth failure
                MessageBox.Show("Authentication failed,please try again");
                authPanel.Hide();
                if (resetPassCheckBox.Checked)
                {
                    resetPassCheckBox.Show();
                    resetPasswordPanel.Show();
                }
                else if (loginModeCheckBox.Checked)
                {
                    registerPanel.Show();
                    loginModeCheckBox.Show();
                }
                else
                {
                    loginPanel.Show();
                    loginModeCheckBox.Show();
                }
                resetPassCheckBox.Show();
                codeTextBox.Text = "";
                captchaText.Text = "";
                loginUser.Text = "";
                loginPassword.Text = "";
                usernameRegister.Text = "";
                passwordRegister.Text = "";
                emailRegister.Text = "";
            }
            else if (res.Equals("3")) {
                authPanel.Hide();
                forcedPassChangePanel.Show();
            }
            else { MessageBox.Show("Wrong code or captcha"); }
        }

        /// <summary>
        /// when the reset password checkbox is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetPassCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (resetPassCheckBox.Checked)
            {
                loginModeCheckBox.Hide();
                loginPanel.Hide();
                registerPanel.Hide();
                resetPasswordPanel.Show();
            }
            else {
                resetPasswordPanel.Hide();
                loginModeCheckBox.Show();
                if (loginModeCheckBox.Checked)
                {
                    registerPanel.Show();
                }
                else {
                    loginPanel.Show();
                }
            }
        }

        /// <summary>
        /// clicking on the reset password button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reset_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect(IPAddress.Parse(ip), port);
            }
            catch
            {
                MessageBox.Show("Failed to connect to server");
                return;
            }
            TwitterClientMain.server = new CommunicatorClient(tcpClient);
            JsonObject res=new JsonObject() {
                ["username"]=resetPassUser.Text,
                ["email"]=resetPassEmail.Text,
                ["password"]=resetPassPassword.Text
            };
            TwitterClientMain.SendStringToServer("2"+res.ToJsonString());

            string responce = TwitterClientMain.GetStringFromServer();
            if (responce[0] == '0')
            {
                //success
                loginModeCheckBox.Hide();
                loginModeCheckBox.Hide();
                resetPassCheckBox.Hide();
                resetPasswordPanel.Hide();
                loginPanel.Hide();
                authPanel.Show();
                emailLabel.Text = "Sent a code to " + responce.Substring(1);


                byte[] imgBytes = TwitterClientMain.server.ReadBytes();
                using (MemoryStream ms = new MemoryStream(imgBytes))
                {
                    captchaBox.Image = Image.FromStream(ms);
                }
            }
            else {
                //fail
                MessageBox.Show(responce.Substring(1));
                TwitterClientMain.server.Close();
            }

        }

        /// <summary>
        /// the server forces you to change password on set periods
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForcedNewPassContinue_Click(object sender, EventArgs e)
        {
            TwitterClientMain.SendStringToServer(forcedNewPassBox.Text);
            string responce = TwitterClientMain.GetStringFromServer();
            if (responce.Equals("0"))
            {
                //success
                forcedPassChangePanel.Hide();
                MessageBox.Show("You have logged in");
                TwitterClientMain.server.StartReading();
                TwitterClientMain.SwitchToMainPage();

                resetPassCheckBox.Show();
                loginPanel.Show();
                loginModeCheckBox.Show();
                codeTextBox.Text = "";
                captchaText.Text = "";
                loginUser.Text = "";
                loginPassword.Text = "";
                forcedNewPassBox.Text = "";
            }
            else {
                MessageBox.Show("Password at least 8 chars long,\n and include lower,upper,number,special\n type letters\nand be different from the last one");
            }
        }

        private void IPBox_TextChanged(object sender, EventArgs e)
        {
            ip=IPBox.Text;
        }

        private void portBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                port = Int32.Parse(portBox.Text);
            }
            catch { }
        }
    }
}
