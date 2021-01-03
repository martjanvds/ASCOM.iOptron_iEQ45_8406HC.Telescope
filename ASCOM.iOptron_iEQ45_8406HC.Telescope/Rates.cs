using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ASCOM.iOptron_iEQ45_8406HC
{
    #region Rate class
    //
    // The Rate class implements IRate, and is used to hold values
    // for AxisRates. You do not need to change this class.
    //
    // The Guid attribute sets the CLSID for ASCOM.iOptron_iEQ45_8406HC.Rate
    // The ClassInterface/None attribute prevents an empty interface called
    // _Rate from being created and used as the [default] interface
    //
    [Guid("a7cbde17-e1f4-495d-9880-d8c9b88ef453")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Rate : ASCOM.DeviceInterface.IRate
    {
        private double maximum = 0;
        private double minimum = 0;

        //
        // Default constructor - Internal prevents public creation
        // of instances. These are values for AxisRates.
        //
        internal Rate(double minimum, double maximum)
        {
            this.maximum = maximum;
            this.minimum = minimum;
        }

        #region Implementation of IRate

        public void Dispose()
        {
            // TODO Add any required object clean-up here
        }

        public double Maximum
        {
            get { return this.maximum; }
            set { this.maximum = value; }
        }

        public double Minimum
        {
            get { return this.minimum; }
            set { this.minimum = value; }
        }

        #endregion
    }
    #endregion

    #region AxisRates
    //
    // AxisRates is a strongly-typed collection that must be enumerable by
    // both COM and .NET. The IAxisRates and IEnumerable interfaces provide
    // this polymorphism. 
    //
    // The Guid attribute sets the CLSID for ASCOM.iOptron_iEQ45_8406HC.AxisRates
    // The ClassInterface/None attribute prevents an empty interface called
    // _AxisRates from being created and used as the [default] interface
    //
    [Guid("9b01025e-1541-4b28-9a24-e0efe11a7f8d")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]

    public class AxisRates : IAxisRates, IEnumerable
    {
        private TelescopeAxes axis;
        private readonly Rate[] rates;
        private const double SiderealRate = 0.004178074624;
        //
        // Constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal AxisRates(TelescopeAxes axis)
        {
            this.axis = axis;
            //
            // This collection must hold zero or more Rate objects describing the 
            // rates of motion ranges for the Telescope.MoveAxis() method
            // that are supported by your driver. It is OK to leave this 
            // array empty, indicating that MoveAxis() is not supported.
            //
            // Note that we are constructing a rate array for the axis passed
            // to the constructor. Thus we switch() below, and each case should 
            // initialize the array for the rate for the selected axis.
            //
            switch (axis)
            {
                case TelescopeAxes.axisPrimary:
                    this.rates = new Rate[] {
                        new Rate(SiderealRate/4, SiderealRate/4),   //GuideSpeed slow
                        new Rate(SiderealRate/2, SiderealRate/2),   //GuideSpeed
                        new Rate(SiderealRate, 5.0)                 //1x - 1200x
                    };
                    break;
                case TelescopeAxes.axisSecondary:
                    this.rates = new Rate[] {
                        new Rate(SiderealRate/4, SiderealRate/4),   //GuideSpeed slow
                        new Rate(SiderealRate/2, SiderealRate/2),   //GuideSpeed
                        new Rate(SiderealRate, 5.0)                 //1x - 1200x
                        //new Rate(0.001389, 0.001389),   //Used by APT
                        //new Rate(0.004167, 0.004167),   //1x
                        //new Rate(0.008333, 0.008333),   //2x
                        //new Rate(0.016667, 0.016667),   //4x Used by APT
                        //new Rate(0.033333, 0.033333),   //8x
                        //new Rate(0.066667, 0.066667),   //16x
                        //new Rate(0.266667, 0.266667),   //64x
                        //new Rate(0.533333, 0.533333),   //128x
                        //new Rate(1.066667, 1.066667),   //256x
                        //new Rate(2.133333, 2.133333),   //512x
                        //new Rate(5.0, 5.0)              //MAX 1200x
                    };
                    break;
                case TelescopeAxes.axisTertiary:
                    this.rates = new Rate[0];
                    break;
            }
        }

        #region IAxisRates Members

        public int Count
        {
            get { return this.rates.Length; }
        }

        public void Dispose()
        {
            // TODO Add any required object clean-up here
        }

        public IEnumerator GetEnumerator()
        {
            return rates.GetEnumerator();
        }

        public IRate this[int index]
        {
            get { return this.rates[index - 1]; }   // 1-based
        }

        #endregion
    }
    #endregion

    #region TrackingRates
    //
    // TrackingRates is a strongly-typed collection that must be enumerable by
    // both COM and .NET. The ITrackingRates and IEnumerable interfaces provide
    // this polymorphism. 
    //
    // The Guid attribute sets the CLSID for ASCOM.iOptron_iEQ45_8406HC.TrackingRates
    // The ClassInterface/None attribute prevents an empty interface called
    // _TrackingRates from being created and used as the [default] interface
    //
    // This class is implemented in this way so that applications based on .NET 3.5
    // will work with this .NET 4.0 object.  Changes to this have proved to be challenging
    // and it is strongly suggested that it isn't changed.
    //
    [Guid("ab0aa87f-80e7-4c69-bc5d-3f4d10af5032")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class TrackingRates : ITrackingRates, IEnumerable, IEnumerator
    {
        private readonly DriveRates[] trackingRates;

        // this is used to make the index thread safe
        private readonly ThreadLocal<int> pos = new ThreadLocal<int>(() => { return -1; });
        private static readonly object lockObj = new object();

        //
        // Default constructor - Internal prevents public creation
        // of instances. Returned by Telescope.AxisRates.
        //
        internal TrackingRates()
        {
            //
            // This array must hold ONE or more DriveRates values, indicating
            // the tracking rates supported by your telescope. The one value
            // (tracking rate) that MUST be supported is driveSidereal!
            //
            this.trackingRates = new[]
            {
            DriveRates.driveLunar,
            DriveRates.driveSolar,
            DriveRates.driveSidereal
        };
        }

        #region ITrackingRates Members

        public int Count
        {
            get { return this.trackingRates.Length; }
        }

        public IEnumerator GetEnumerator()
        {
            pos.Value = -1;
            return this as IEnumerator;
        }

        public void Dispose()
        {
            // TODO Add any required object clean-up here
        }

        public DriveRates this[int index]
        {
            get { return this.trackingRates[index - 1]; }   // 1-based
        }

        #endregion

        #region IEnumerable members

        public object Current
        {
            get
            {
                lock (lockObj)
                {
                    if (pos.Value < 0 || pos.Value >= trackingRates.Length)
                    {
                        throw new System.InvalidOperationException();
                    }
                    return trackingRates[pos.Value];
                }
            }
        }

        public bool MoveNext()
        {
            lock (lockObj)
            {
                if (++pos.Value >= trackingRates.Length)
                {
                    return false;
                }
                return true;
            }
        }

        public void Reset()
        {
            pos.Value = -1;
        }
        #endregion
    }
    #endregion
}
