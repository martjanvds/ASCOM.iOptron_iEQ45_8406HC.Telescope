# ASCOM Telescope driver for iOptron iEQ45 with 8406HC

I wrote this ASCOM driver for the iOptron iEQ45 mount with 8406 handcontroller as the driver supplied by iOptron did not function properly.

The following functionalities have been implemented:
- Connect to mount. As the computer time is probably more accurate than the internal clock the local time, and GMT offset will be set.
- Read SiteLatitude and SiteLongitude. Reading or writing SiteElevation has not been implemented. Setting SiteLatitude and SiteLongitude has not yet been implemented.
- Read RightAscension, Declination, Altitude and Azimuth and related properties as LocalSiderealTime, SideOfPier and DestinationSideOfPier
- Read UTCDate (+ Time)
- Read IsParked, Tracking, Slewing properties
- Read and set TargetRightAscension and TargetDeclination
- Read supported TrackingRates (Sidereal, Lunar, Solar)

- Start and stop Tracking
- Read and set TrackingRate
- Park and Unpark. Setting the park position has to be done via the handcontroller.
- SlewToTarget, SlewToCoordinates, SlewToTargetAsync, SlewToCoordinatesAsync and AbortSlew. Note that SlewAltAz and SlewAltAzAsync have not been implemented.
- SyncToTarget, SyncToCoordinates. SyncToAltAz has not been implemented.
- MoveAxis at 0.25, 0,5 guiderates and 1 - 1200x sidereal AxisRates for RA and Dec Axis.
- PulseGuide Setting the GuideRate has to be done via the handcontroller.

The setup of the driver can be downloaded here:
[download setup](files//ASCOM.iOptron_iEQ45_8406HC.Telescope/iEQ45%20with%208406HC%20Telescope%20driver%20Setup.exe)

LICENSE:
This driver is free to use and modify, both for personal or commercial usage. Any improvements are welcome via a pull request.
