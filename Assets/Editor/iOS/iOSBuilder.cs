using UnityEngine;
using System;
using System.Collections;
using UnityEditor;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

[InitializeOnLoad]
public class iOSBuilder  {

	//

	public static iOSBuilder instance;

	//
	public enum iosLocalDeviceStatus { Free, Deploying, Running, Suceeded, Failed };

  public bool BuilderOccupied = false;

	// Contains information stattus of local device
	public class iosLocalDeviceInfo{
		
		public string name;
		public string udid;
		public iosLocalDeviceStatus status;

		public string StatusString(){


			if ( status == iosLocalDeviceStatus.Failed)
				return "    <color=#ff0000ff>Failed</color>";
			else if ( status == iosLocalDeviceStatus.Free)
				return "    <color=#00ff00ff>Free</color>";
			else if ( status == iosLocalDeviceStatus.Deploying)
				return "    <color=#ffa500ff>Deploying</color>";
			else if ( status == iosLocalDeviceStatus.Running)
				return "    <color=#0000ffff>Running</color>";
			else if ( status == iosLocalDeviceStatus.Suceeded)
				return "    <color=#008000ff>Suceeded</color>";

			return Enum.GetName(typeof(iosLocalDeviceStatus), status);
		}
	}

  public void SetBuildStatusOccupied( bool occupied ){
    this.BuilderOccupied = occupied;
  }


	//
	public  Dictionary<string, iosLocalDeviceInfo> devices;

	public  bool appAlreadyBuilt {
		get {
			return appPath != null && appPath.Contains(".app");
		}
	}

	//
	public  int sessionID;


	//cause Unity restarts mono after build there is no way to make sure that it's built (yet TODO check if the files really exist) so let's just assume that for now...
	public  string xcodeProjectPath = "Builds/project";
	public  string appPath = "Builds/project/build/Debug-iphoneos/ProductName.app";

	//


	//

	public iOSBuilder(bool useAsync = true)
	{

		sessionID = new System.Random ().Next (0, 1000);
		devices = new Dictionary<string, iosLocalDeviceInfo> ();

		instance = this;
	}


	




	// - - - - 
	public  void BuildProject(){

		var scenes  =  new string[]{"Assets/CollectData.unity"};
		var path = "Builds/project";

		xcodeProjectPath =  path;

		// Build player.
		BuildPipeline.BuildPlayer(scenes,path, BuildTarget.iOS, BuildOptions.None);
			

	}


	public void ExecuteDelegate(ThreadStart del, bool useAsync){

		Debug.Log ("Invoking : " + del.ToString () +" useAsync: "+ useAsync);

		if (useAsync) {
			var newThread = new Thread (del);
			newThread.Start ();
		}
		else {
			del.Invoke ();
		}
	}

	// Build .app in XcodeBuild
	public  void BuildAppInXcode(bool useAsync = true){

    var appName = PlayerSettings.bundleIdentifier.Split ('.') [2];

		ThreadStart ths = new ThreadStart (delegate() { 

      this.SetBuildStatusOccupied(true);

			var path = string.Format ("{0}{1}", xcodeProjectPath,	"/Unity-iPhone.xcodeproj");

			var argStr = string.Format ("-project {0} -configuration Debug build", path);

			Debug.Log ("Building app in xcodeBuild with: " + argStr);

      var arg = new System.Diagnostics.ProcessStartInfo ("/Applications/Xcode.app/Contents/Developer/usr/bin/xcodebuild", argStr);
			arg.RedirectStandardOutput = true;
			arg.UseShellExecute = false;

			using (var process = System.Diagnostics.Process.Start (arg)) {

				// read chunk-wise while process is running.
				while (!process.HasExited) {
					Debug.Log (process.StandardOutput.ReadToEnd ());
				}

				// make sure not to miss out on any remaindings.
				Debug.Log (process.StandardOutput.ReadToEnd ());

				// ...
			}

      this.SetBuildStatusOccupied(false);

			appPath = xcodeProjectPath + "/build/Debug-iphoneos/" + appName + ".app";
		});
			
		ExecuteDelegate (ths, useAsync);
	}

	
	


	public  void DeployToAllDevices(bool useAsync = true){


		foreach (var d in devices) {
			DeployApp (d.Value, useAsync);
		}
	}

	//will projectdeploy to any connected device using ios-deploy
	public void DeployApp(iosLocalDeviceInfo device, bool useAsync = true){

    var fAppPath = Directory.GetCurrentDirectory () +  appPath;


    if (!appAlreadyBuilt) {
      UnityEngine.Debug.LogError (" App not yet built! \n please build the app before trying to deploy it");
      return;
    }

//    else if (!File.Exists (fAppPath)) {
//      UnityEngine.Debug.LogError (fAppPath + ": App file not found!!"); 
//      return;
//    }

		Debug.Log (" Deploying Project ");


		Debug.Log (appPath);

		System.Diagnostics.Process process = null;

		var argStr = string.Format("--debug --bundle {0} --id  {1} --uninstall --no-wifi --justlaunch --noninteractive ", appPath, device.udid);

		ThreadStart ths = new ThreadStart(delegate() { 


			//TODO add timeout i.e if app has not responded for >10 seconds just say that it failed.

			var standardOutput = new List<string> ();


			var arg = new System.Diagnostics.ProcessStartInfo ("Assets/Editor/Tools/ios-deploy", argStr);	
			Debug.Log (arg.Arguments);
			//	var arg = new ProcessStartInfo ("Assets/Tools/ios-deploy", "-c");
			arg.RedirectStandardOutput = true;
			arg.UseShellExecute = false;

			device.status = iosLocalDeviceStatus.Deploying;


			DeployTimeoutChecker(process, device, 7.0f);

			using (process = System.Diagnostics.Process.Start(arg))
			{
		      
        standardOutput.Add("Starting deployment on : "  +device.udid);
					// read chunk-wise while process is running.
					while (!process.HasExited)
					{
          var ln = process.StandardOutput.ReadLine();
          standardOutput.Add(ln);
          Debug.Log("   ios-deploy   " + ln);
					}
		
					// make sure not to miss out on any remaindings.
          var endLn = process.StandardOutput.ReadToEnd();
          standardOutput.Add(endLn);
          Debug.Log("   ios-deploy   " + endLn);

			}

        standardOutput.Add("App deployed on : "  +device.udid);

					// ...
		
			if( AppDeployedSuccesfully (device, standardOutput)){
				device.status = iosLocalDeviceStatus.Running;
			}
			else{
				device.status = iosLocalDeviceStatus.Failed;
			}


		
		});

		ExecuteDelegate (ths, useAsync);



	}

	public void DeployTimeoutChecker( System.Diagnostics.Process proc, iosLocalDeviceInfo device,  float timeout){

		ThreadStart ths = new ThreadStart (delegate() { 

			int timeStep = 500;
			int msTimeout = (int)(timeout * 1000);
			int it = 0;

			while (proc == null || proc.HasExited) {

				if (proc != null && it > msTimeout) {

					Debug.LogWarning ("Deployment process timed out!");
					device.status = iosLocalDeviceStatus.Failed;
					proc.Kill ();
				}

				Thread.Sleep (timeStep);
				it += timeStep;
			}
		});

		var monitorThread = new Thread(ths);
		monitorThread.Start();
	}

	//goes through the ios-deploy log to determine whether it was sucessful
	public  bool AppDeployedSuccesfully(iosLocalDeviceInfo device, List<string> output){

		bool status = false;

		foreach (var s in output) {
			Debug.Log ( string.Format("[{0}]:", device.name)+ s);

      if (s.Contains ("process launch failed") || s.Contains ("Application has not been launched") || s.Contains ("Error")) {
        device.status = iosLocalDeviceStatus.Failed;
        return false;
      } else if (s.Contains ("success")) {
        
        device.status = iosLocalDeviceStatus.Running;
      }

		}

		return status;
	}


	public  void  GetAllConnectedDevices(bool useAsync = true){
			var standardOutput = new StringBuilder();

		ThreadStart ths = new ThreadStart (delegate() { 
			
			var process = new System.Diagnostics.Process
			{
				StartInfo = new System.Diagnostics.ProcessStartInfo
				{
					FileName = "Assets/Editor/Tools/ios-deploy",
					Arguments = "-c",
					UseShellExecute = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
				}
			};

		

			process.OutputDataReceived += (sender, args) => LogMessage(args.Data);
			process.ErrorDataReceived += (sender, args) => LogMessage(args.Data);

			process.Start();
			process.BeginOutputReadLine();
			process.BeginErrorReadLine();

			process.WaitForExit(); //you need this in order to flush the output buffer
		});

		ExecuteDelegate (ths, useAsync);

	}

	public  void LogMessage(string str){

		Debug.Log (str);
		try {

			var expr = "\\(.+\\)";
			var matchUDID = Regex.Matches (str, "\\(.+\\)");
			var matchName = Regex.Matches (str, "Found .+ \\(");

			for (var it = 0; it < matchUDID.Count; it ++){
				
				var uuid = matchUDID [it].ToString();
				uuid = uuid.Replace("(", "");
				uuid = uuid.Replace(")", "");

				AddDevice(uuid, matchName [it].ToString ().Replace ("Found", "").Replace ("(", ""));
			}

		} 
		catch (Exception ex) {
		//	Debug.Log (str);
		}
	
	}


	//

	public   void AddDevice (string udid, string name ){
	
		if (devices.ContainsKey (udid)) {
			//TODO upate info and remove all devices that were disconnected
		}
		else {

			var info = new iosLocalDeviceInfo ();
			info.name = name;
			info.udid = udid;
			info.status = iosLocalDeviceStatus.Free;

			devices [udid] = info;

		}
	}
}
