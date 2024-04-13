/*
    Copyright 2024 Nathan Krone

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

namespace ComputerVisionFormProject {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pic_video = new PictureBox();
            lst_debug = new ListBox();
            lst_devices = new ListBox();
            lbl_connectStatus = new Label();
            btn_disconnect = new Button();
            pictureBox1 = new PictureBox();
            num_scaleFactor = new NumericUpDown();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pic_video).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_scaleFactor).BeginInit();
            SuspendLayout();
            // 
            // pic_video
            // 
            pic_video.BackColor = Color.Transparent;
            pic_video.BackgroundImage = (Image)resources.GetObject("pic_video.BackgroundImage");
            pic_video.BackgroundImageLayout = ImageLayout.Zoom;
            pic_video.BorderStyle = BorderStyle.Fixed3D;
            pic_video.Location = new Point(12, 12);
            pic_video.Name = "pic_video";
            pic_video.Size = new Size(489, 367);
            pic_video.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_video.TabIndex = 0;
            pic_video.TabStop = false;
            // 
            // lst_debug
            // 
            lst_debug.BackColor = Color.PaleTurquoise;
            lst_debug.Enabled = false;
            lst_debug.FormattingEnabled = true;
            lst_debug.ItemHeight = 15;
            lst_debug.Location = new Point(507, 163);
            lst_debug.Name = "lst_debug";
            lst_debug.Size = new Size(268, 214);
            lst_debug.TabIndex = 1;
            // 
            // lst_devices
            // 
            lst_devices.BackColor = Color.Azure;
            lst_devices.FormattingEnabled = true;
            lst_devices.ItemHeight = 15;
            lst_devices.Location = new Point(507, 78);
            lst_devices.Name = "lst_devices";
            lst_devices.Size = new Size(268, 79);
            lst_devices.TabIndex = 2;
            lst_devices.SelectedIndexChanged += lst_devices_SelectedIndexChanged;
            // 
            // lbl_connectStatus
            // 
            lbl_connectStatus.BorderStyle = BorderStyle.Fixed3D;
            lbl_connectStatus.Location = new Point(507, 4);
            lbl_connectStatus.Name = "lbl_connectStatus";
            lbl_connectStatus.Size = new Size(155, 23);
            lbl_connectStatus.TabIndex = 3;
            lbl_connectStatus.Text = "Status: Offline";
            lbl_connectStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btn_disconnect
            // 
            btn_disconnect.Location = new Point(700, 4);
            btn_disconnect.Name = "btn_disconnect";
            btn_disconnect.Size = new Size(75, 23);
            btn_disconnect.TabIndex = 4;
            btn_disconnect.Text = "Disconnect";
            btn_disconnect.UseVisualStyleBackColor = true;
            btn_disconnect.Click += btn_disconnect_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(668, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(26, 23);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // num_scaleFactor
            // 
            num_scaleFactor.Location = new Point(621, 30);
            num_scaleFactor.Name = "num_scaleFactor";
            num_scaleFactor.Size = new Size(41, 23);
            num_scaleFactor.TabIndex = 6;
            num_scaleFactor.Value = new decimal(new int[] { 1, 0, 0, 0 });
            num_scaleFactor.ValueChanged += num_scaleFactor_ValueChanged;
            // 
            // label1
            // 
            label1.BorderStyle = BorderStyle.Fixed3D;
            label1.Location = new Point(507, 30);
            label1.Name = "label1";
            label1.Size = new Size(108, 23);
            label1.TabIndex = 7;
            label1.Text = "Scale By";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.oris_conjabe;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(785, 385);
            Controls.Add(label1);
            Controls.Add(num_scaleFactor);
            Controls.Add(pictureBox1);
            Controls.Add(btn_disconnect);
            Controls.Add(lbl_connectStatus);
            Controls.Add(lst_devices);
            Controls.Add(lst_debug);
            Controls.Add(pic_video);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pic_video).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_scaleFactor).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pic_video;
        private ListBox lst_debug;
        private ListBox lst_devices;
        private Label lbl_connectStatus;
        private Button btn_disconnect;
        private PictureBox pictureBox1;
        private NumericUpDown num_scaleFactor;
        private Label label1;
    }
}
