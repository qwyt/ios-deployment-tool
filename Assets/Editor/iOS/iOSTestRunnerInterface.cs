using UnityEngine;
using System.Collections;
using UnityEditor;

// used to interact with the builder (calls directly)
public  class iOSTestRunnerInterface : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	

	// create builder and serach for devices that are connected
	public static void InitEnvironment(){


		CleanEnv();
		//
		Debug.Log ("InitEnvironment called");

		if (iOSBuilder.instance == null) {
			new iOSBuilder ();
		}

		//check for the connected devices:
		iOSBuilder.instance.GetAllConnectedDevices(false);

		print (iOSBuilder.instance.devices.Count);  

		//

		iOSBuilder.instance.BuildProject ();
		iOSBuilder.instance.BuildAppInXcode (false);

		iOSBuilder.instance.DeployToAllDevices (true);



		EditorApplication.Exit(0);
	}

	static void CleanEnv() {
		
		var arg = new System.Diagnostics.ProcessStartInfo ("killall", "ios-deploy");	
		arg.RedirectStandardOutput = true;
		arg.UseShellExecute = false;
	
		var process = System.Diagnostics.Process.Start (arg);


	}
}
