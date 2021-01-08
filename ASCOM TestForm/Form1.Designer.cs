
namespace ASCOM.TestTelescope
{
    partial class Form1
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
            this.buttonChoose = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelDriverId = new System.Windows.Forms.Label();
            this.btnSetUTC = new System.Windows.Forms.Button();
            this.btnSetRA10100 = new System.Windows.Forms.Button();
            this.btnGoToT2 = new System.Windows.Forms.Button();
            this.btnSlewing = new System.Windows.Forms.Button();
            this.chkSlewing = new System.Windows.Forms.CheckBox();
            this.btnTrackingRates = new System.Windows.Forms.Button();
            this.btnN = new System.Windows.Forms.Button();
            this.btnW = new System.Windows.Forms.Button();
            this.btnS = new System.Windows.Forms.Button();
            this.btnE = new System.Windows.Forms.Button();
            this.btnQ = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnDeclGet = new System.Windows.Forms.Button();
            this.lblDeclination = new System.Windows.Forms.Label();
            this.btnSlewToCoords = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonChoose
            // 
            this.buttonChoose.Location = new System.Drawing.Point(309, 10);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(72, 23);
            this.buttonChoose.TabIndex = 0;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(309, 39);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(72, 23);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelDriverId
            // 
            this.labelDriverId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDriverId.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.TestTelescope.Properties.Settings.Default, "DriverId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.labelDriverId.Location = new System.Drawing.Point(12, 40);
            this.labelDriverId.Name = "labelDriverId";
            this.labelDriverId.Size = new System.Drawing.Size(291, 21);
            this.labelDriverId.TabIndex = 2;
            this.labelDriverId.Text = global::ASCOM.TestTelescope.Properties.Settings.Default.DriverId;
            this.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSetUTC
            // 
            this.btnSetUTC.Location = new System.Drawing.Point(309, 80);
            this.btnSetUTC.Name = "btnSetUTC";
            this.btnSetUTC.Size = new System.Drawing.Size(72, 23);
            this.btnSetUTC.TabIndex = 3;
            this.btnSetUTC.Text = "SetUTC";
            this.btnSetUTC.UseVisualStyleBackColor = true;
            this.btnSetUTC.Click += new System.EventHandler(this.btnSetUTC_Click);
            // 
            // btnSetRA10100
            // 
            this.btnSetRA10100.Location = new System.Drawing.Point(309, 131);
            this.btnSetRA10100.Name = "btnSetRA10100";
            this.btnSetRA10100.Size = new System.Drawing.Size(72, 23);
            this.btnSetRA10100.TabIndex = 4;
            this.btnSetRA10100.Text = "GoTo T1";
            this.btnSetRA10100.UseVisualStyleBackColor = true;
            this.btnSetRA10100.Click += new System.EventHandler(this.btnGoToT1_Click);
            // 
            // btnGoToT2
            // 
            this.btnGoToT2.Location = new System.Drawing.Point(309, 160);
            this.btnGoToT2.Name = "btnGoToT2";
            this.btnGoToT2.Size = new System.Drawing.Size(72, 23);
            this.btnGoToT2.TabIndex = 5;
            this.btnGoToT2.Text = "GoTo T2";
            this.btnGoToT2.UseVisualStyleBackColor = true;
            this.btnGoToT2.Click += new System.EventHandler(this.btnGoToT2_Click);
            // 
            // btnSlewing
            // 
            this.btnSlewing.Location = new System.Drawing.Point(309, 210);
            this.btnSlewing.Name = "btnSlewing";
            this.btnSlewing.Size = new System.Drawing.Size(72, 23);
            this.btnSlewing.TabIndex = 6;
            this.btnSlewing.Text = "Upd Slew";
            this.btnSlewing.UseVisualStyleBackColor = true;
            this.btnSlewing.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkSlewing
            // 
            this.chkSlewing.AutoSize = true;
            this.chkSlewing.Location = new System.Drawing.Point(195, 210);
            this.chkSlewing.Name = "chkSlewing";
            this.chkSlewing.Size = new System.Drawing.Size(63, 17);
            this.chkSlewing.TabIndex = 7;
            this.chkSlewing.Text = "Slewing";
            this.chkSlewing.UseVisualStyleBackColor = true;
            // 
            // btnTrackingRates
            // 
            this.btnTrackingRates.Location = new System.Drawing.Point(309, 239);
            this.btnTrackingRates.Name = "btnTrackingRates";
            this.btnTrackingRates.Size = new System.Drawing.Size(72, 23);
            this.btnTrackingRates.TabIndex = 8;
            this.btnTrackingRates.Text = "TrackingRates";
            this.btnTrackingRates.UseVisualStyleBackColor = true;
            this.btnTrackingRates.Click += new System.EventHandler(this.btnTrackingRates_Click);
            // 
            // btnN
            // 
            this.btnN.Location = new System.Drawing.Point(88, 104);
            this.btnN.Name = "btnN";
            this.btnN.Size = new System.Drawing.Size(28, 23);
            this.btnN.TabIndex = 9;
            this.btnN.Text = "N";
            this.btnN.UseVisualStyleBackColor = true;
            this.btnN.Click += new System.EventHandler(this.btnN_Click);
            // 
            // btnW
            // 
            this.btnW.Location = new System.Drawing.Point(122, 133);
            this.btnW.Name = "btnW";
            this.btnW.Size = new System.Drawing.Size(28, 23);
            this.btnW.TabIndex = 10;
            this.btnW.Text = "W";
            this.btnW.UseVisualStyleBackColor = true;
            this.btnW.Click += new System.EventHandler(this.btnW_Click);
            // 
            // btnS
            // 
            this.btnS.Location = new System.Drawing.Point(88, 160);
            this.btnS.Name = "btnS";
            this.btnS.Size = new System.Drawing.Size(28, 23);
            this.btnS.TabIndex = 11;
            this.btnS.Text = "S";
            this.btnS.UseVisualStyleBackColor = true;
            this.btnS.Click += new System.EventHandler(this.btnS_Click);
            // 
            // btnE
            // 
            this.btnE.Location = new System.Drawing.Point(54, 131);
            this.btnE.Name = "btnE";
            this.btnE.Size = new System.Drawing.Size(28, 23);
            this.btnE.TabIndex = 12;
            this.btnE.Text = "E";
            this.btnE.UseVisualStyleBackColor = true;
            this.btnE.Click += new System.EventHandler(this.btnE_Click);
            // 
            // btnQ
            // 
            this.btnQ.Location = new System.Drawing.Point(88, 131);
            this.btnQ.Name = "btnQ";
            this.btnQ.Size = new System.Drawing.Size(28, 23);
            this.btnQ.TabIndex = 13;
            this.btnQ.Text = "Q";
            this.btnQ.UseVisualStyleBackColor = true;
            this.btnQ.Click += new System.EventHandler(this.btnQ_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            // 
            // btnDeclGet
            // 
            this.btnDeclGet.Location = new System.Drawing.Point(309, 280);
            this.btnDeclGet.Name = "btnDeclGet";
            this.btnDeclGet.Size = new System.Drawing.Size(72, 23);
            this.btnDeclGet.TabIndex = 14;
            this.btnDeclGet.Text = "Declination Get";
            this.btnDeclGet.UseVisualStyleBackColor = true;
            this.btnDeclGet.Click += new System.EventHandler(this.btnDeclGet_Click);
            // 
            // lblDeclination
            // 
            this.lblDeclination.AutoSize = true;
            this.lblDeclination.Location = new System.Drawing.Point(195, 289);
            this.lblDeclination.Name = "lblDeclination";
            this.lblDeclination.Size = new System.Drawing.Size(35, 13);
            this.lblDeclination.TabIndex = 15;
            this.lblDeclination.Text = "label1";
            // 
            // btnSlewToCoords
            // 
            this.btnSlewToCoords.Location = new System.Drawing.Point(170, 133);
            this.btnSlewToCoords.Name = "btnSlewToCoords";
            this.btnSlewToCoords.Size = new System.Drawing.Size(133, 23);
            this.btnSlewToCoords.TabIndex = 16;
            this.btnSlewToCoords.Text = "SlewToCoordAsync(0,0)";
            this.btnSlewToCoords.UseVisualStyleBackColor = true;
            this.btnSlewToCoords.Click += new System.EventHandler(this.btnSlewToCoords_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 380);
            this.Controls.Add(this.btnSlewToCoords);
            this.Controls.Add(this.lblDeclination);
            this.Controls.Add(this.btnDeclGet);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnQ);
            this.Controls.Add(this.btnE);
            this.Controls.Add(this.btnS);
            this.Controls.Add(this.btnW);
            this.Controls.Add(this.btnN);
            this.Controls.Add(this.btnTrackingRates);
            this.Controls.Add(this.chkSlewing);
            this.Controls.Add(this.btnSlewing);
            this.Controls.Add(this.btnGoToT2);
            this.Controls.Add(this.btnSetRA10100);
            this.Controls.Add(this.btnSetUTC);
            this.Controls.Add(this.labelDriverId);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonChoose);
            this.Name = "Form1";
            this.Text = "TEMPLATEDEVICETYPE Test";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelDriverId;
        private System.Windows.Forms.Button btnSetUTC;
        private System.Windows.Forms.Button btnSetRA10100;
        private System.Windows.Forms.Button btnGoToT2;
        private System.Windows.Forms.Button btnSlewing;
        private System.Windows.Forms.CheckBox chkSlewing;
        private System.Windows.Forms.Button btnTrackingRates;
        private System.Windows.Forms.Button btnN;
        private System.Windows.Forms.Button btnW;
        private System.Windows.Forms.Button btnS;
        private System.Windows.Forms.Button btnE;
        private System.Windows.Forms.Button btnQ;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnDeclGet;
        private System.Windows.Forms.Label lblDeclination;
        private System.Windows.Forms.Button btnSlewToCoords;
    }
}

