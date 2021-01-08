//tabs=4
// --------------------------------------------------------------------------------
//
// ASCOM Telescope driver for iOptron_iEQ45_8406HC
//
// Description:	As the 8406HC is not implemented using the modern ASCOM features as multi-connect
//              I decided to program my own implementation.
//              The main goal is to connect to the iEQ45 - 8600 HC mount from PHD2 and APT simultaneously
////
// Implements:	ASCOM Telescope interface version: <To be completed by driver developer>
// Author:		Martjan van der Spek martjan.vanderspek@gmail.com
//
// Edit Log:
//
// Date			Who	Vers	Description
// -----------	---	-----	-------------------------------------------------------
// 08-Dec-2020	SPJ	1.0.0	Initial edit, created from ASCOM driver template
// --------------------------------------------------------------------------------
//


// This is used to define code in the template that is specific to one class implementation
// unused code can be deleted and this definition removed.
#define Telescope

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ASCOM.iOptron_iEQ45_8406HC
{
    //
    // Your driver's DeviceID is ASCOM.iOptron_iEQ45_8406HC.Telescope
    //
    // The Guid attribute sets the CLSID for ASCOM.iOptron_iEQ45_8406HC.Telescope
    // The ClassInterface/None attribute prevents an empty interface called
    // _iOptron_iEQ45_8406HC from being created and used as the [default] interface
    //
    // TODO Replace the not implemented exceptions with code to implement the function or
    // throw the appropriate ASCOM exception.
    //

    /// <summary>
    /// ASCOM Telescope Driver for iOptron_iEQ45_8406HC.
    /// </summary>
    [Guid("33db4d17-1683-4758-8922-cfc962312455")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Telescope : ITelescopeV3
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.iOptron_iEQ45_8406HC.Telescope";
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "iOptron iEQ45 with 8406HC.";

        internal static string comPortProfileName = "COM Port"; // Constants used for Profile persistence
        internal static string comPortDefault = "COM3";
        internal static string traceStateProfileName = "Trace Level";
        internal static string traceStateDefault = "true";
        internal static double SiderealRate = 0.004178074624;

        internal static string comPort; // Variables to hold the current device configuration
//        internal static double Latitude;
//        internal static double Longitude;
        internal static bool SetLocation;
        internal static bool SetTime;

        private static double targetRightAscension = 0.0;
        private static bool targetRightAcensionWasSet = false;
        private static double targetDeclination = 0.0;
        private static bool targetDeclinationWasSet = false;
        private static bool slewing = false;
        private static DateTime endPulseGuiding = DateTime.MinValue;
        private static bool tracking = false;
        private static bool parked = false;
        private static DriveRates trackingRate = DriveRates.driveSidereal;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal TraceLogger tl;

        /// <summary>
        /// Initializes a new instance of the <see cref="iOptron_iEQ45_8406HC"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Telescope()
        {
            tl = new TraceLogger("iOptron_iEQ45_8406HC");
            //tl.LogFilePath = @"%userprofile%\Documents\ASCOM\";
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl.LogMessage("Telescope", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro-utilities object
                                               //TODO: Implement your additional construction here

            tl.LogMessage("Telescope", "Completed initialisation");
        }

        //
        // PUBLIC COM INTERFACE ITelescopeV3 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm(tl))
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
                return new ArrayList();
            }
        }

        public string Action(string actionName, string actionParameters)
        {
            LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
            throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        public void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response
            LogMessage("CommandBlind", "command {0}, raw {1} not implemented", command, raw);
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            // string retString = CommandString(command, raw); // Send the command and wait for the response
            // bool retBool = XXXXXXXXXXXXX; // Parse the returned string and create a boolean True / False value
            // return retBool; // Return the boolean value to the client
            LogMessage("CommandBool", "command {0}, raw {1} not implemented", command, raw);
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            LogMessage("CommandString", "command {0}, raw {1} not implemented", command, raw);
            throw new ASCOM.MethodNotImplementedException("CommandString");
        }

        public void Dispose()
        {
            // Clean up the trace logger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                //tl.Enabled = true;
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

                if (value)
                {
                    connectedState = true;
                    LogMessage("Connected Set", "Connecting to port {0}", comPort);
                    SerPort = new ASCOM.Utilities.Serial();
                    //objSerial.Port = Convert.ToInt32(comPort.Replace("COM", ""));
                    SerPort.PortName = comPort;
                    SerPort.Speed = ASCOM.Utilities.SerialSpeed.ps9600;
                    SerPort.Connected = true;

                    //Set date and time
                    //:SG sHH#  (Set offset from UTC)
                    TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                    bool isDaylightsaving = TimeZone.CurrentTimeZone.IsDaylightSavingTime(DateTime.Now);
                    SerPort.ClearBuffers();
                    if (utcOffset >= new TimeSpan(0, 0, 0))
                        SerPort.Transmit(string.Format(":SG +{0}:{1}#", utcOffset.Hours, utcOffset.Minutes));
                    else
                        SerPort.Transmit(string.Format(":SG -{0}:{1}#", utcOffset.Hours, utcOffset.Minutes));
                    string strResponse = SerPort.Receive();
                    if (strResponse != "1")
                    {
                        LogMessage("Connected Set", "Error response set UTC offset {0}", strResponse);
                        throw new ASCOM.ValueNotSetException("Connected set time");
                    }

                    //:SC MM/DD/YY# (Set local date)
                    SerPort.ClearBuffers();
                    SerPort.Transmit(string.Format(":SC {0:00}/{1:00}/{2:00}#", DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year % 100));
                    strResponse = SerPort.ReceiveTerminated("#");
                    //if (strResponse != "                                #                                #")
                    if (strResponse != "                                #")
                    {
                        LogMessage("Connected Set", "Error response set date {0}", strResponse);
                        throw new ASCOM.ValueNotSetException("Connected set date");
                    }
                    //:SL HH:MM:SS# (set local time)
                    SerPort.ClearBuffers();
                    SerPort.Transmit(string.Format(":SL {0:00}:{1:00}:{2:00}#", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
                    strResponse = SerPort.Receive();
                    if (strResponse != " ")
                    {
                        LogMessage("Connected Set", "Error response set time {0}", strResponse);
                        throw new ASCOM.ValueNotSetException("Connected set time");
                    }
                    //SerPort.Transmit(":EW#");
                    //tl.LogMessage("Connected Set", "Switch East and West button");
                }
                else
                {
                    connectedState = false;
                    LogMessage("Connected Set", "Disconnecting from port {0}", comPort);
                    // disconnect from the device
                    if (SerPort.Connected)
                    {
                        SerPort.ClearBuffers();
                        SerPort.Connected = false;
                        SerPort.Dispose();
                        SerPort = null;
                    }
                }
            }
        }

        public string Description
        {
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description
                string driverInfo = "Information about the driver itself. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "3");
                return Convert.ToInt16("3");
            }
        }

        public string Name
        {
            get
            {
                string name = "iEQ45_8406HC";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region ITelescope Implementation
        public void AbortSlew()
        {
            if (parked)
                throw new ASCOM.ParkedException("AbortSlew");
            tl.LogMessage("AbortSlew", "Abort Slew");
            SerPort.Transmit(":Q#");
            slewing = false;
        }

        public AlignmentModes AlignmentMode
        {
            get
            {
                tl.LogMessage("AlignmentMode Get", "Alignment German Polar");
                return AlignmentModes.algGermanPolar;
            }
        }

        public double Altitude
        {
            get
            {
                SerPort.ClearBuffers();
                SerPort.Transmit(":GA#");
                string straltitude = SerPort.ReceiveTerminated("#");
                straltitude = straltitude.Replace("#", "");
                tl.LogMessage("Altitude Get", straltitude);
                double altitude = utilities.DMSToDegrees(straltitude);
                //tl.LogMessage("Altitude", "Get - " + utilities.DegreesToDMS(altitude, ":", ":"));
                return altitude;
            }
        }

        public double ApertureArea
        {
            get
            {
                tl.LogMessage("ApertureArea Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ApertureArea", false);
            }
        }

        public double ApertureDiameter
        {
            get
            {
                tl.LogMessage("ApertureDiameter Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("ApertureDiameter", false);
            }
        }

        public bool AtHome
        {
            get
            {
                tl.LogMessage("AtHome", "Get - " + false.ToString());
                return false;
            }
        }

        public bool AtPark
        {
            get
            {
                double az = Azimuth;
                double altError = Math.Abs(SiteLatitude - Altitude);
                if ((az < 2 || az > 358) && (altError < 2) && !tracking && parked)
                {
                    tl.LogMessage("AtPark", "Get - " + true.ToString());
                    return true;
                }
                else
                {
                    tl.LogMessage("AtPark", "Get - " + false.ToString());
                    return false;
                }
            }
        }

        public IAxisRates AxisRates(TelescopeAxes Axis)
        {
            tl.LogMessage("AxisRates", "Get - " + Axis.ToString());
            return new AxisRates(Axis);
        }

        public double Azimuth
        {
            get
            {
                SerPort.ClearBuffers();
                SerPort.Transmit(":GZ#");
                string strAzimuth = SerPort.ReceiveTerminated("#");
                strAzimuth = strAzimuth.Replace("#", "");
                tl.LogMessage("Azimuth Get", strAzimuth);
                double azimuth = utilities.DMSToDegrees(strAzimuth);
                //tl.LogMessage("Azimuth", "Get - " + utilities.DegreesToDMS(azimuth, ":", ":"));
                return azimuth;
            }
        }

        public bool CanFindHome
        {
            get
            {
                tl.LogMessage("CanFindHome", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanMoveAxis(TelescopeAxes Axis)
        {
            tl.LogMessage("CanMoveAxis", "Get - " + Axis.ToString());
            switch (Axis)
            {
                case TelescopeAxes.axisPrimary: return true;
                case TelescopeAxes.axisSecondary: return true;
                case TelescopeAxes.axisTertiary: return false;
                default:
                    {
                        LogMessage("CanMoveAxis", "Unknown axis");
                        throw new InvalidValueException("CanMoveAxis", Axis.ToString(), "0 to 2");
                    }
            }
        }

        public bool CanPark
        {
            get
            {
                tl.LogMessage("CanPark", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanPulseGuide
        {
            get
            {
                tl.LogMessage("CanPulseGuide", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSetDeclinationRate
        {
            get
            {
                tl.LogMessage("CanSetDeclinationRate", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetGuideRates
        {
            get
            {
                tl.LogMessage("CanSetGuideRates", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetPark
        {
            get
            {
                tl.LogMessage("CanSetPark", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetPierSide
        {
            get
            {
                tl.LogMessage("CanSetPierSide", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetRightAscensionRate
        {
            get
            {
                tl.LogMessage("CanSetRightAscensionRate", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSetTracking
        {
            get
            {
                tl.LogMessage("CanSetTracking", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSlew
        {
            get
            {
                tl.LogMessage("CanSlew", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSlewAltAz
        {
            get
            {
                tl.LogMessage("CanSlewAltAz", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSlewAltAzAsync
        {
            get
            {
                tl.LogMessage("CanSlewAltAzAsync", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanSlewAsync
        {
            get
            {
                tl.LogMessage("CanSlewAsync", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSync
        {
            get
            {
                tl.LogMessage("CanSync", "Get - " + true.ToString());
                return true;
            }
        }

        public bool CanSyncAltAz
        {
            get
            {
                tl.LogMessage("CanSyncAltAz", "Get - " + false.ToString());
                return false;
            }
        }

        public bool CanUnpark
        {
            get
            {
                tl.LogMessage("CanUnpark", "Get - " + true.ToString());
                return true;
            }
        }

        public double Declination
        {
            get
            {
                SerPort.ClearBuffers();
                SerPort.Transmit(":GD#");
                string strDeclination = SerPort.ReceiveTerminated("#");
                strDeclination = strDeclination.Replace("#", "");
                tl.LogMessage("Declination Get", strDeclination);
                double declination = utilities.DMSToDegrees(strDeclination);
                //tl.LogMessage("Declination", "Get - " + utilities.DegreesToDMS(declination, ":", ":"));
                return declination;
            }
        }

        public double DeclinationRate
        {
            get
            {
                double declination = 0.0;
                tl.LogMessage("DeclinationRate", "Get - " + declination.ToString());
                return declination;
            }
            set
            {
                tl.LogMessage("DeclinationRate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("DeclinationRate", true);
            }
        }

        public PierSide DestinationSideOfPier(double RightAscension, double Declination)
        {
            PierSide pierSide = PierSide.pierUnknown;
            double hourAngle = SiderealTime - RightAscension;
            if (hourAngle < -12.0)
                hourAngle += 24.0;
            if (hourAngle < 0.0)
                pierSide = PierSide.pierWest;
            else
                pierSide = PierSide.pierEast;

            //if (hourAngle < 0.0)
            //{
            //    if (hourAngle >= -6.0)
            //        pierSide = PierSide.pierWest;
            //    else
            //        pierSide = PierSide.pierEast;
            //}
            //else
            //{
            //    if (hourAngle <= 6.0)
            //        pierSide = PierSide.pierEast;
            //    else
            //        pierSide = PierSide.pierWest;
            //}

            LogMessage("DestinationSideOfPier", "RA {0} DEC {1} hourAngle {2} PierSide {3}", RightAscension, Declination, hourAngle, pierSide);
            return pierSide;
        }

        public bool DoesRefraction
        {
            get
            {
                tl.LogMessage("DoesRefraction Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("DoesRefraction", false);
            }
            set
            {
                tl.LogMessage("DoesRefraction Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("DoesRefraction", true);
            }
        }

        public EquatorialCoordinateType EquatorialSystem
        {
            get
            {
                EquatorialCoordinateType equatorialSystem = EquatorialCoordinateType.equTopocentric;
                tl.LogMessage("DeclinationRate", "Get - " + equatorialSystem.ToString());
                return equatorialSystem;
            }
        }

        public void FindHome()
        {
            tl.LogMessage("FindHome", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("FindHome");
        }

        public double FocalLength
        {
            get
            {
                tl.LogMessage("FocalLength Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("FocalLength", false);
            }
        }

        public double GuideRateDeclination
        {
            get
            {
                tl.LogMessage("GuideRateDeclination Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GuideRateDeclination", false);
            }
            set
            {
                tl.LogMessage("GuideRateDeclination Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GuideRateDeclination", true);
            }
        }

        public double GuideRateRightAscension
        {
            get
            {
                tl.LogMessage("GuideRateRightAscension Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GuideRateRightAscension", false);
            }
            set
            {
                tl.LogMessage("GuideRateRightAscension Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("GuideRateRightAscension", true);
            }
        }

        public bool IsPulseGuiding
        {
            get
            {
                LogMessage("IsPulseGuiding Get", "Time: {0} ExpectedEndTime {1}", DateTime.Now, endPulseGuiding);
                return DateTime.Now < endPulseGuiding;
            }
        }

        public void MoveAxis(TelescopeAxes Axis, double Rate)
        {
            if (Axis == TelescopeAxes.axisTertiary) //Rotator
            {
                tl.LogMessage("MoveAxis", "Invalid axis");
                throw new ASCOM.InvalidValueException("MoveAxis");
            }
            if (IsConnected)
            {
                if (parked)
                {
                    tl.LogMessage("MoveAxis", "Scope is parked");
                    throw new ASCOM.ParkedException("MoveAxis");
                }
                double rate = Rate;
                //If RA axis, convert rate to relative to sidereal rate  for more convenient operations
                if (Axis == TelescopeAxes.axisPrimary)
                {
                    if (Math.Abs(Rate) > 0.0004)
                        rate = Rate - SiderealRate;
                }
                double xSiderealRate = Math.Round((Math.Abs(rate) / SiderealRate), 2);

                LogMessage("MoveAxis", "Axis {0} Rate {1} °/s. {2} x Sidereal", Axis, rate, xSiderealRate);

                if (xSiderealRate <= 0.2)
                {
                    SerPort.Transmit(":Q#");
                    //reset guiderate to Default 0.5x Sidereal
                    SerPort.Transmit(":RG1#");
                }
                else if (xSiderealRate >= 0.2 && xSiderealRate < 0.4)
                {
                    SerPort.Transmit(":RG0#"); //0.25x
                    LogMessage("MoveAxis", ":RG0#");
                }
                else if (xSiderealRate >= 0.4 && xSiderealRate < 0.8)
                {
                    SerPort.Transmit(":RG1#"); //0.5x
                    LogMessage("MoveAxis", ":RG1#");
                }
                else if (xSiderealRate >= 0.8 && xSiderealRate <= 255.0)
                {
                    SerPort.Transmit(string.Format(":Rc{0:000}#", Convert.ToInt32(xSiderealRate)));
                    LogMessage("MoveAxis", ":Rc{0:000}#", Convert.ToInt32(xSiderealRate));
                }
                else if (xSiderealRate > 255.0 && xSiderealRate <= 800.0)
                {
                    SerPort.Transmit(":RC2#"); // 600x
                    LogMessage("MoveAxis", ":RC2#");
                }
                else if (xSiderealRate > 800.0 && xSiderealRate <= 1200.0)
                {
                    SerPort.Transmit(":RC3#"); // 1200x
                    LogMessage("MoveAxis", ":RC3#");
                }
                else
                {
                    tl.LogMessage("MoveAxis", string.Format("Invalid rate {0}", Rate));
                    throw new ASCOM.InvalidValueException("MoveAxis");
                }

                //When rate is slower than .2 x Sidereal, a stop command was sent.
                if (xSiderealRate > 0.2)
                {
                    switch (Axis)
                    {
                        case TelescopeAxes.axisPrimary: //RA
                            if (rate > 0)
                            {
                                SerPort.Transmit(":Me#");
                                LogMessage("MoveAxis", ":Me#");
                            }
                            else
                            {
                                SerPort.Transmit(":Mw#");
                                LogMessage("MoveAxis", ":Mw#");
                            }
                            break;
                        case TelescopeAxes.axisSecondary: //Dec
                            if (rate > 0)
                            {
                                SerPort.Transmit(":Mn#");
                                LogMessage("MoveAxis", ":Mn#");
                            }
                            else
                            {
                                SerPort.Transmit(":Ms#");
                                LogMessage("MoveAxis", ":Ms#");
                            }
                            break;
                        case TelescopeAxes.axisTertiary: //Rotator
                            tl.LogMessage("MoveAxis", "Invalid axis");
                            throw new ASCOM.InvalidValueException("MoveAxis");
                        default:
                            tl.LogMessage("MoveAxis", "Invalid axis");
                            throw new ASCOM.InvalidValueException("MoveAxis");
                    }
                }
            }
            else
            {
                tl.LogMessage("MoveAxis", "NotConnected");
                throw new ASCOM.NotConnectedException("MoveAxis");
            }
        }

        public void Park()
        {
            tl.LogMessage("Park", "");
            SerPort.Transmit(":KA#");
            tracking = false;
            slewing = true;
            parked = true;
            int seconds = 0;
            while (Slewing && seconds < 30)
            {
                Thread.Sleep(1000);
                seconds++;
            }
            if (seconds == 30)
                LogMessage("Park", "End on Timeout {0}", seconds);
        }

        public void PulseGuide(GuideDirections Direction, int Duration)
        {
            if (parked)
                throw new ASCOM.ParkedException("PulseGuide");

            if (IsConnected)
            {
                SerPort.ClearBuffers();
                switch (Direction)
                {
                    case GuideDirections.guideNorth:
                        LogMessage("PulseGuide", "Direction N Duration {0} ms", Duration);
                        SerPort.Transmit(string.Format(":Mn{0}#", Duration));
                        break;
                    case GuideDirections.guideSouth:
                        LogMessage("PulseGuide", "Direction S Duration {0} ms", Duration);
                        SerPort.Transmit(string.Format(":Ms{0}#", Duration));
                        break;
                    case GuideDirections.guideEast:
                        LogMessage("PulseGuide", "Direction E Duration {0} ms", Duration);
                        SerPort.Transmit(string.Format(":Me{0}#", Duration));
                        break;
                    case GuideDirections.guideWest:
                        LogMessage("PulseGuide", "Direction W Duration {0} ms", Duration);
                        SerPort.Transmit(string.Format(":Mw{0}#", Duration));
                        break;
                    default:
                        LogMessage("PulseGuide", "Direction Unknown Duration {0} ms", Duration);
                        break;
                }
                endPulseGuiding = DateTime.Now.AddMilliseconds(Duration);
            }
            else
                throw new ASCOM.NotConnectedException("PulseGuide");
        }

        public double RightAscension
        {
            get
            {
                SerPort.ClearBuffers();
                SerPort.Transmit(":GR#");
                string strRightAscencion = SerPort.ReceiveTerminated("#");
                strRightAscencion = strRightAscencion.Replace("#", "");
                tl.LogMessage("RightAscension Get", strRightAscencion);
                double rightAscension = utilities.HMSToHours(strRightAscencion);
                //tl.LogMessage("RightAscension", "Get - " + utilities.HoursToHMS(rightAscension));
                return rightAscension;
            }
        }

        public double RightAscensionRate
        {
            get
            {
                double rightAscensionRate = 0.0;
                tl.LogMessage("RightAscensionRate", "Get - " + rightAscensionRate.ToString());
                return rightAscensionRate;
            }
            set
            {
                tl.LogMessage("RightAscensionRate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("RightAscensionRate", true);
            }
        }

        public void SetPark()
        {
            tl.LogMessage("SetPark", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SetPark");
        }

        public PierSide SideOfPier
        {
            /* TODO
            SideofPier ISSUE    pierEast is returned when the mount is observing at an hour angle between -6.0 and 0.0
            SideofPier INFO ASCOM has adopted a convention that, for German Equatorial mounts, pierWest must be returned when observing at hour angles from -6.0 to -0.0 
            and that pierEast must be returned at hour angles from 0.0 to +6.0.
            SideofPier ISSUE    pierWest is returned when the mount is observing at an hour angle between 0.0 and +6.0
            SideofPier INFO ASCOM has adopted a convention that, for German Equatorial mounts, pierWest must be returned when observing at hour angles from -6.0 to -0.0 
            and that pierEast must be returned at hour angles from 0.0 to +6.0.
            */

            get
            {
                PierSide pierSide = PierSide.pierUnknown;
                double hourAngle = SiderealTime - RightAscension;
                if (hourAngle < -12.0)
                    hourAngle += 24.0;
                if (hourAngle < 0.0)
                    pierSide = PierSide.pierWest;
                else
                    pierSide = PierSide.pierEast;
                tl.LogMessage("SideOfPier Get", "");
                return pierSide;
            }
            set
            {
                tl.LogMessage("SideOfPier Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SideOfPier", true);
            }
        }

        public double SiderealTime
        {
            get
            {
                // Now using NOVAS 3.1
                double siderealTime = 0.0;
                using (var novas = new ASCOM.Astrometry.NOVAS.NOVAS31())
                {
                    var jd = utilities.DateUTCToJulian(DateTime.UtcNow);
                    novas.SiderealTime(jd, 0, novas.DeltaT(jd),
                        ASCOM.Astrometry.GstType.GreenwichApparentSiderealTime,
                        ASCOM.Astrometry.Method.EquinoxBased,
                        ASCOM.Astrometry.Accuracy.Reduced, ref siderealTime);
                }

                // Allow for the longitude
                siderealTime += SiteLongitude / 360.0 * 24.0;

                // Reduce to the range 0 to 24 hours
                siderealTime = astroUtilities.ConditionRA(siderealTime);

                tl.LogMessage("SiderealTime", "Get - " + siderealTime.ToString());
                return siderealTime;
            }
        }

        public double SiteElevation
        {
            get
            {
                tl.LogMessage("SiteElevation Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteElevation", false);
            }
            set
            {
                tl.LogMessage("SiteElevation Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteElevation", true);
            }
        }

        public double SiteLatitude
        {
            get
            {
                SerPort.ClearBuffers();
                SerPort.Transmit(":Gt#");
                string strLatitude = SerPort.ReceiveTerminated("#");
                strLatitude = strLatitude.Replace("#", "");
                tl.LogMessage("SiteLatitude Get", strLatitude);
                double Latitude = utilities.DMSToDegrees(strLatitude);
                //tl.LogMessage("SiteLatitude Get", Latitude.ToString());
                return Latitude;
            }
            set
            {
                tl.LogMessage("SiteLatitude Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteLatitude", true);
            }
        }

        public double SiteLongitude
        {
            get
            {
                SerPort.ClearBuffers();
                SerPort.Transmit(":Gg#");
                string strLongitude = SerPort.ReceiveTerminated("#");
                strLongitude = strLongitude.Replace("#", "");
                tl.LogMessage("SiteLongitude Get", strLongitude);
                double Longitude = utilities.DMSToDegrees(strLongitude);
                //tl.LogMessage("SiteLongitude Get", Longitude.ToString());
                return Longitude;
            }
            set
            {
                tl.LogMessage("SiteLongitude Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SiteLongitude", true);
            }
        }

        public short SlewSettleTime
        {
            get
            {
                tl.LogMessage("SlewSettleTime Get", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SlewSettleTime", false);
            }
            set
            {
                tl.LogMessage("SlewSettleTime Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("SlewSettleTime", true);
            }
        }

        public void SlewToAltAz(double Azimuth, double Altitude)
        {
            tl.LogMessage("SlewToAltAz", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SlewToAltAz");
        }

        public void SlewToAltAzAsync(double Azimuth, double Altitude)
        {
            tl.LogMessage("SlewToAltAzAsync", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SlewToAltAzAsync");
        }

        public void SlewToCoordinates(double RightAscension, double Declination)
        {
            if (parked)
                throw new ASCOM.ParkedException("SlewToCoordinates");

            LogMessage("SlewToCoordinates", "RA {0} Dec {1}",utilities.HoursToHMS(RightAscension), utilities.DegreesToDMS(Declination));
            TargetRightAscension = RightAscension;
            TargetDeclination = Declination;
            SlewToTarget();
            int seconds = 0;
            while (Slewing && seconds < 30)
            {
                Thread.Sleep(1000);
                seconds++;
            }
            if (seconds == 30)
                tl.LogMessage("SlewToCoordinates", string.Format("End on Timeout {0}", seconds));
        }

        public void SlewToCoordinatesAsync(double RightAscension, double Declination)
        {
            if (parked)
                throw new ASCOM.ParkedException("SlewToCoordinatesAsync");

            LogMessage("SlewToCoordinatesAsync", "RA {0} Dec {1}", utilities.HoursToHMS(RightAscension), utilities.DegreesToDMS(Declination));
            TargetRightAscension = RightAscension;
            TargetDeclination = Declination;
            SlewToTargetAsync();
        }

        public void SlewToTarget()
        {
            if (parked)
                throw new ASCOM.ParkedException("SlewToTarget");

            if (IsConnected)
            {
                LogMessage("SlewToTarget", "Target RA {0} Target Dec {1}", utilities.HoursToHMS(TargetRightAscension), utilities.DegreesToDMS(TargetDeclination));
                SerPort.ClearBuffers();
                SerPort.Transmit(":MS#");
                slewing = true;
                string strResponse = SerPort.Receive();
                if (strResponse != "0" || strResponse == "1Object is below horizon        #")
                {
                    LogMessage("SlewToTarget", "Expected Response: 0 Response: {0}", strResponse);
                    throw new ASCOM.ValueNotSetException("SlewToTarget Not Slewing");
                }
                int seconds = 0;
                while (Slewing && seconds < 30)
                {
                    Thread.Sleep(1000);
                    seconds++;
                }
                if (seconds == 30)
                    LogMessage("SlewToTarget", "End on Timeout {0}", seconds);
            }
            else 
                throw new ASCOM.NotConnectedException("SlewToTarget");
        }

        public void SlewToTargetAsync()
        {
            if (parked)
                throw new ASCOM.ParkedException("SlewToCoordinatesAsync");
            if (IsConnected)
            {
                LogMessage("SlewToTargetAsync", "Target RA {0} Target Dec {1}", utilities.HoursToHMS(TargetRightAscension), utilities.DegreesToDMS(TargetDeclination));
                SerPort.ClearBuffers();
                SerPort.Transmit(":MS#");
                slewing = true;
                string strResponse = SerPort.Receive();
                if (strResponse != "0" || strResponse == "1Object is below horizon        #")
                {
                    LogMessage("SlewToTargetAsync", "Expected Response: 0 Response: {0}", strResponse);
                    throw new ASCOM.ValueNotSetException("SlewToTargetAsync Not Slewing");
                }
            }
            else
                throw new ASCOM.NotConnectedException("SlewToTargetAsync");
        }

        public bool Slewing
        {
            get
            {
                double ra = RightAscension;
                double dec = Declination;
                double raErrorInArcSec = Math.Abs(ra - targetRightAscension) * 15 * 60 * 60;
                double decErrorInArcSec = Math.Abs(dec - targetDeclination) * 60 * 60;
                if (slewing)
                {
                    //End slewing when RA and Dec within 100 arcSec. of target.
                    if (raErrorInArcSec < 100 && decErrorInArcSec < 100)
                        slewing = false;
                    if (parked && AtPark)
                        slewing = false;
                }

                LogMessage("Slewing Get", "Slewing {0} RA error {1} Dec error {2}", slewing.ToString(), raErrorInArcSec, decErrorInArcSec);
                return slewing;
            }
        }

        public void SyncToAltAz(double Azimuth, double Altitude)
        {
            tl.LogMessage("SyncToAltAz", "Not implemented");
            throw new ASCOM.MethodNotImplementedException("SyncToAltAz");
        }

        public void SyncToCoordinates(double RA, double Dec)
        {
            LogMessage("SyncToCoordinates", "RA {0} Dec {1}", utilities.HoursToHMS(RA), utilities.DegreesToDMS(Dec));
            this.TargetRightAscension = RA;
            this.TargetDeclination = Dec;
            SyncToTarget();
        }

        public void SyncToTarget()
        {
            if (parked)
                throw new ASCOM.ParkedException("SlewToTarget");

            tl.LogMessage("SyncToTarget", "");
            SerPort.ClearBuffers();
            SerPort.Transmit(":CM#");
            string strResponse = SerPort.ReceiveTerminated("#");
            if (strResponse != "Coordinates     matched.        #")
            {
                LogMessage("SyncToTarget", "Expected Response: Coordinates     matched.        # Response: {0}", strResponse);
                throw new ASCOM.ValueNotSetException("SyncToTarget");
            }
        }

        public double TargetDeclination
        {
            get
            {
                if (targetDeclinationWasSet)
                {
                    LogMessage("TargetDeclination Get", "{0}", utilities.DegreesToDMS(targetDeclination));
                    return targetDeclination;
                }
                else
                    throw new ASCOM.ValueNotSetException("TargetDeclination");
            }
            set
            {
                if (value > 90.0 || value < -90.0)
                    throw new ASCOM.InvalidValueException("TargetDeclination");
                //:Sd +24*07:00#
                tl.LogMessage("TargetDeclination Set", value.ToString());
                targetDeclination = value;
                if (IsConnected)
                {
                    string strTargetDEC = utilities.DegreesToDMS(value, "*", ":", "");
                    //Add sign if positive, negative is automatic
                    if (value > 0)
                        strTargetDEC = "+" + strTargetDEC;
                    tl.LogMessage("TargetDeclination Set", "Set - " + strTargetDEC);
                    SerPort.ClearBuffers();
                    SerPort.Transmit(string.Format(":Sd {0}#", strTargetDEC));
                    string strResponse = SerPort.Receive();
                    if (strResponse != "1")
                    {
                        LogMessage("TargetDeclination Set", "Expected Response: 1 Response {0}", strResponse);
                        throw new ASCOM.ValueNotSetException("TargetDeclination not Set");
                    }
                    targetDeclinationWasSet = true;
                }
                else
                {
                    tl.LogMessage("TargetDeclination Set", "Not connected");
                    throw new ASCOM.NotConnectedException();
                }
            }
        }

        public double TargetRightAscension
        {
            get
            {
                if (targetRightAcensionWasSet)
                {
                    LogMessage("TargetRightAscension Get", "{0}", utilities.HoursToHMS(targetRightAscension));
                    return targetRightAscension;
                }
                else
                    throw new ASCOM.ValueNotSetException("TargetRightAscension");
            }
            set
            {
                if (value >= 25.0 || value < 0.0)
                    throw new ASCOM.InvalidValueException("TargetRightAscension");
                targetRightAscension = value;
                //:Sr 03:47:00#
                tl.LogMessage("TargetRightAscension Set",  value.ToString());
                if (IsConnected)
                {
                    string strTargetRA = utilities.HoursToHMS(value, ":", ":");
                    tl.LogMessage("TargetRightAscension", "Set - " + strTargetRA);
                    SerPort.ClearBuffers();
                    SerPort.Transmit(string.Format(":Sr {0}#", strTargetRA));
                    string strResponse = SerPort.Receive();
                    if (strResponse != "1")
                    {
                        LogMessage("TargetRightAscension Set", "Expected Response: 1 Response {0}", strResponse);
                        throw new ASCOM.ValueNotSetException("TargetRightAscension not Set");
                    }
                    targetRightAcensionWasSet = true;
                }
                else
                {
                    tl.LogMessage("TargetRightAscension Set", "Not connected");
                    throw new ASCOM.NotConnectedException();
                }
            }
        }

        public bool Tracking
        {
            get
            {
                tl.LogMessage("Tracking", "Get - " + tracking.ToString());
                return tracking;
            }
            set
            {
                tracking = value; 
                tl.LogMessage("Tracking Set", tracking.ToString());
                //lunar (: RT0#), solar (:RT1#), sidereal (:RT2#), or zero (:RT9#)
                if (!value)
                    SerPort.Transmit(string.Format(":RT9#"));
                else
                    TrackingRate = trackingRate; //set to previous or default state
            }
        }

        public DriveRates TrackingRate
        {
            get
            {
                tl.LogMessage("TrackingRate Get", trackingRate.ToString());
                return trackingRate;
            }
            set
            {
                tracking = true;
                trackingRate = value;
                tl.LogMessage("TrackingRate Set", trackingRate.ToString());
                SerPort.ClearBuffers();
                switch (trackingRate)
                {
                    //lunar (: RT0#), solar (:RT1#), sidereal (:RT2#), or zero (:RT9#)
                    case DriveRates.driveLunar:
                        SerPort.Transmit(string.Format(":RT0#"));
                        break;
                    case DriveRates.driveSolar:
                        SerPort.Transmit(string.Format(":RT1#"));
                        break;
                    case DriveRates.driveSidereal:
                        SerPort.Transmit(string.Format(":RT2#"));
                        break;
                    default:
                        {
                            tl.LogMessage("TrackingRate Set", "Unknown DriveRate");
                            throw new ASCOM.InvalidValueException("TrackingRate");
                        }
                }
            }
        }

        public ITrackingRates TrackingRates
        {
            get
            {
                ITrackingRates trackingRates = new TrackingRates();
                foreach (DriveRates driveRate in trackingRates)
                {
                    tl.LogMessage("TrackingRates", "Get - " + driveRate.ToString());
                }
                return trackingRates;
            }
        }

        public DateTime UTCDate
        {
            get
            {
                DateTime utcDate = DateTime.UtcNow;
                LogMessage("UTCDate",  "Get - {0} {1}", utcDate.ToShortDateString(), utcDate.ToShortTimeString());
                return utcDate;
            }
            set
            {
                tl.LogMessage("UTCDate Set", "Not implemented");
                throw new ASCOM.PropertyNotImplementedException("UTCDate", true);
            }
        }

        public void Unpark()
        {
            tl.LogMessage("Unpark", "");
            if(IsConnected)
            {
                //:PO#
                SerPort.Transmit(string.Format(":PO#"));
                parked = false;
            }
            else
            {
                tl.LogMessage("Unpark", "Not connected");
                throw new ASCOM.NotConnectedException("Unpark");
            }
        }

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Telescope";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion
        private ASCOM.Utilities.Serial SerPort;

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Telescope";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Telescope";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
