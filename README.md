# ASCOM.iOptron_iEQ45_8406HC.Telescope

I wrote this ASCOM driver for the iOptron iEQ45 mount with 8406 handcontroller as the driver supplied by iOptron did not function properly.

The following functionalities have been implemented:
- Connect to mount. As the computer time is probably more accurate than the internal clock the local time, and GMT offset will be set.
- Read Latitude and Longitude. Setting these values has not yet been implemented.
- Read RightAscension, Declination, Altitude and Azimuth and related properties as LocalSiderealTime and SideOfPier.
- Set TargetRightAscension and TargetDeclination
- SlewToTarget, SlewToCoordinates, SlewToTargetAsync, SlewToCoordinatesAsync
