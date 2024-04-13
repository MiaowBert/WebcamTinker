/*
    Copyright 2024Nathan Krone

   Licensed under the Creative Commons Attribution-NonCommercial 4.0 International License (CC BY-NC 4.0).
   You may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       https://creativecommons.org/licenses/by-nc/4.0/

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using System;
using System.Drawing.Imaging;
using AForge.Video;
using AForge.Video.DirectShow;

namespace ComputerVisionFormProject {

    public partial class Form1 : Form {

        AForge.Video.DirectShow.FilterInfoCollection videoDevices;

        VideoCaptureDevice videoDevice;

        private VideoCaptureBuffer buffer = new(bufferSize: 64, enableScaling: true);

        private bool connected = false;

        CancellationTokenSource cancelVideoToken;

        protected EventHandler FrameAddHandler;

        public Form1() {
            InitializeComponent();

        }

        protected void WriteLine(string text) {
            lst_debug.Items.Add(text);

            if (lst_debug.Items.Count > 0) {
                lst_debug.SelectedIndex = lst_debug.Items.Count - 1;
                lst_debug.ClearSelected(); // Optional: Clear selection after scrolling
            }
        }

        private async void Form1_Load(object sender, EventArgs e) {
            // Get a list of video capture devices
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            //make sure one is listed
            if (videoDevices.Count == 0) {
                WriteLine("No video capture devices found.");

            } else {
                lst_devices.Items.Clear();

                WriteLine("Capture Device Found.");

                for (int i = 0; i < videoDevices.Count; i++) {
                    lst_devices.Items.Add($"|{i} - {videoDevices[i].MonikerString,40} |");
                }

            }

        }

        private void VideoDevice_NewFrame(object sender, NewFrameEventArgs args) {
            if (args.Frame != null) {
                var bmp = args.Frame.Clone(new Rectangle(0, 0, args.Frame.Width, args.Frame.Height), PixelFormat.Format24bppRgb);

                buffer.push(bmp);

            }
        }

        private void DEBUG() {
            //value of index, if frame is null or not. 

            WriteLine($"INDEX: {buffer.Index}");
        }


        /// <summary>
        /// continuously take the images in the image buffer and display them in the picturebox
        /// </summary>
        /// <param name="token"> allows the user to cancel the procedure </param>
        /// <returns></returns>
        protected Task UpdatePicture(CancellationTokenSource token) {
            bool res = false;

            while (!token.IsCancellationRequested) {

                try {

                    pic_video.Invoke((Action)(() => {
                        Bitmap bmp = buffer.pop();

                        if (bmp != null) pic_video.Image = bmp;

                    }));

                    int i = 0;

                } catch (Exception e) {

                }
            }

            if (token.IsCancellationRequested) this.Invoke((Action)(() => { this.WriteLine("Cancellation via token requested"); }));

            this.Invoke((Action)(() => { this.WriteLine(""); }));

            return new Task<bool>(() => { return res; });
        }

        private async void lst_devices_SelectedIndexChanged(object sender, EventArgs e) {
            if (lst_devices.Items.Count > 0) {
                int i = lst_devices.SelectedIndex;
                string s = "";

                if (i != -1) s = lst_devices.Items[i].ToString();

                if (!string.IsNullOrEmpty(s)) {
                    WriteLine(" Selecting device...");

                    // Select the first device (you might need to provide options for the user to choose)
                    videoDevice = new VideoCaptureDevice(videoDevices[0].MonikerString);

                    if (videoDevice != null) {

                        WriteLine("Created object..");

                        videoDevice.Start();
                        WriteLine("Started device");

                        videoDevice.NewFrame += VideoDevice_NewFrame;
                        WriteLine("Subscribed to event");

                        try {

                            this.Invoke((Action)(() => {

                                this.lbl_connectStatus.Text = "Status: Online";
                                cancelVideoToken = new();

                            }));

                            this.Invoke((Action)(() => {
                                this.toggleConnectStatus();

                            }));


                            await Task.Run(() => {
                                UpdatePicture(cancelVideoToken);
                            });

                            this.Invoke((Action)(() => {
                                this.toggleConnectStatus();
                            }));

                        } catch (Exception ex) {
                            WriteLine("Something happened");

                        }


                    } else {
                        WriteLine("video failed to connect");
                    }
                }

            }

        }

        private void toggleConnectStatus() {
            connected = !connected;
            btn_disconnect.Enabled = connected;

            if (connected) {//
                lbl_connectStatus.Text = "Status: Online";
                pictureBox1.Image = Image.FromFile("Images/CameraOn.png");

            } else {//
                lbl_connectStatus.Text = "Status: Offline";
                pictureBox1.Image = Image.FromFile("Images/CameraOff.png");
                pic_video.Image = null;

            }

        }

        private void btn_disconnect_Click(object sender, EventArgs e) {
            if (cancelVideoToken != null && cancelVideoToken.IsCancellationRequested == false) {
                lbl_connectStatus.Text = "Status: Request Cancel";
                cancelVideoToken.Cancel();
            }

        }

        private void num_scaleFactor_ValueChanged(object sender, EventArgs e) {
            int num = (int)(num_scaleFactor.Value);

            if( num <= 0 ) {
                num = 1;

            }else if( num >= 21 ) { 
                num = 20;    

            }

            if( num == 1) {
                buffer.scalesImages = false;
            } else {
                buffer.scalesImages = true;
                Config.scaleX = num;
                Config.scaleY = num;
            }

        }
    }

}
