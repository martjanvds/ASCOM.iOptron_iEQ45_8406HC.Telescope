//*** CHECK THIS ProgID ***
var X = new ActiveXObject("ASCOM.iEQ45 with 8406HC Telescope driver.Telescope");
WScript.Echo("This is " + X.Name + ")");
// You may want to uncomment this...
// X.Connected = true;
X.SetupDialog();
