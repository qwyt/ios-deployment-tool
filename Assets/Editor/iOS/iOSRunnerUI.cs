using UnityEngine;
using System.Collections;
using UnityEditor;

// exposes the iOSBuilder to the Editor UI 
public class iOSRunnerUI : EditorWindow {
	string myString = "Hello World";
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Deployment/iOS/Manager")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		iOSRunnerUI window = (iOSRunnerUI)EditorWindow.GetWindow (typeof (iOSRunnerUI));
		window.Show();
	}

	void OnGUI () {

    Rect BuilderInfo = EditorGUILayout.BeginVertical ("box");
    {
      GUILayout.Label ("iOSDeployInterface : " + iOSBuilder.instance == null ? "null" : "initialized"); 
//
//			if (iOSDeployScript.instance == null) {
//				Rect buildInfo = EditorGUILayout.BeginHorizontal ("box");
//				GUILayout.Label ("Initialize");
//				if (GUILayout.Button ("Build")) {
//					iOSDeployScript.instance = new iOSDeployScript ();
//				}
//				EditorGUILayout.EndHorizontal ();
//			}

    }
    EditorGUILayout.EndVertical ();
 

    if (iOSBuilder.instance == null) {
      new iOSBuilder ();
    }

    if (iOSBuilder.instance.BuilderOccupied) {
      //TODO add beter visual indicator i.e grey out stuff bellow and a rotating circle..
      GUILayout.Label ("Building....");
    } else {

      Rect ProjectBuilder = EditorGUILayout.BeginVertical ("box");
      {

        Rect xcodeBuilder = EditorGUILayout.BeginHorizontal ("box");
        GUILayout.Label ("Build Xcode Project in Unity");
        if (GUILayout.Button ("Build")) {
          iOSBuilder.instance.BuildProject ();
        }
        EditorGUILayout.EndHorizontal ();

        Rect appBuilder = EditorGUILayout.BeginHorizontal ("box");
        GUILayout.Label ("Build App in xcodebuild");
        if (GUILayout.Button ("Build")) {
          iOSBuilder.instance.BuildAppInXcode ();
        }
        EditorGUILayout.EndHorizontal ();

        Rect appDeploy = EditorGUILayout.BeginHorizontal ("box");
        GUILayout.Label ("Deploy to all conected devices");
        if (GUILayout.Button ("Deploy")) {
          //		iOSBuilder.instance.DeployProjectToAllDevices(true);
        }
        EditorGUILayout.EndHorizontal ();

        if (GUILayout.Button ("Build and Deploy to all Devices")) {
          //	iOSBuilder.instance.DeployProjectToAllDevices ();
        }


      }

      EditorGUILayout.EndVertical ();

      GUILayout.Label ("sessionID : " + iOSBuilder.instance.sessionID);

      GUILayout.Label ("App is build and ready for deploment : " + iOSBuilder.instance.appAlreadyBuilt);
      GUILayout.Label ("Xcode project path : " + iOSBuilder.instance.xcodeProjectPath);
      GUILayout.Label ("iOS App path : " + iOSBuilder.instance.appPath);
		

      // -- 
      GUILayout.Label (string.Format ("Connected Devices ({0}):", 
        iOSBuilder.instance.devices.Count), EditorStyles.boldLabel);

      var buildLabel = "Build&Deploy";

      // if the app is already built only deploy it
      if (iOSBuilder.instance.appAlreadyBuilt) {
        buildLabel = "Deploy";
      }

      Rect DevicesInfo = EditorGUILayout.BeginVertical ("box");
      {

        GUIStyle style = new GUIStyle ();
        style.richText = true;

        foreach (var d in iOSBuilder.instance.devices) {

          Rect CurrentDevice = EditorGUILayout.BeginHorizontal ("box");
          GUILayout.Label ("<color=silver>" + d.Value.name + "</color>" + d.Value.StatusString (), style); 

          if (GUILayout.Button (buildLabel)) {
            iOSBuilder.instance.DeployApp (d.Value);
          }

          EditorGUILayout.EndHorizontal ();
        }

        if (GUILayout.Button ("Refresh")) {
          iOSBuilder.instance.GetAllConnectedDevices ();
        }
      }
			
      EditorGUILayout.EndVertical ();
    }
  }

}
