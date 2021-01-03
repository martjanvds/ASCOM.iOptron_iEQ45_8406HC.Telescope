using System;
using System.Windows.Forms;

namespace ASCOM.TestTelescope
{
    public partial class Form1 : Form
    {

        private ASCOM.DriverAccess.Telescope driver;

        public Form1()
        {
            InitializeComponent();
            SetUIState();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConnected)
                driver.Connected = false;

            Properties.Settings.Default.Save();
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DriverId = ASCOM.DriverAccess.Telescope.Choose(Properties.Settings.Default.DriverId);
            SetUIState();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                driver.Connected = false;
            }
            else
            {
                try
                {
                    driver = new ASCOM.DriverAccess.Telescope(Properties.Settings.Default.DriverId);
                    driver.Connected = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            SetUIState();
        }

        private void SetUIState()
        {
            buttonConnect.Enabled = !string.IsNullOrEmpty(Properties.Settings.Default.DriverId);
            buttonChoose.Enabled = !IsConnected;
            buttonConnect.Text = IsConnected ? "Disconnect" : "Connect";
        }

        private bool IsConnected
        {
            get
            {
                return ((this.driver != null) && (driver.Connected == true));
            }
        }

        private void btnSetUTC_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                driver.UTCDate = DateTime.UtcNow;
            }
        }

        private void btnGoToT1_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                try
                {
                    driver.TargetRightAscension = 10.16667;
                    driver.TargetDeclination = 17.5;
                    driver.SlewToTarget();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnGoToT2_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                try
                {
                    driver.TargetRightAscension = 12.16667;
                    driver.TargetDeclination = 15.5;
                    driver.SlewToTarget();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                chkSlewing.Checked = driver.Slewing;
            }
        }

        private void btnTrackingRates_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                string strTrackingRates = "";
                foreach (var trRate in driver.TrackingRates)
                {
                    strTrackingRates += trRate.ToString() + "\r\n";
                }
                MessageBox.Show(strTrackingRates);
            }
        }

        private void btnN_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                try
                {
                    driver.MoveAxis(DeviceInterface.TelescopeAxes.axisSecondary, 0.02);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void btnE_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                try
                {
                    driver.MoveAxis(DeviceInterface.TelescopeAxes.axisPrimary, -0.02);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void btnS_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                try
                {
                    driver.MoveAxis(DeviceInterface.TelescopeAxes.axisSecondary, -0.02);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void btnW_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                try
                {
                    driver.MoveAxis(DeviceInterface.TelescopeAxes.axisPrimary, 0.02);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnQ_Click(object sender, EventArgs e)
        {

            if (IsConnected)
            {
                try
                {
                    driver.AbortSlew();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
    
