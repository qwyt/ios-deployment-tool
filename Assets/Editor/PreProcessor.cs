using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class PreProcessor {
    [PostProcessScene]
    public static void OnPostprocessScene()
    { 
        ScriptingImplementation backend = new ScriptingImplementation();
        string renderingPath = PlayerSettings.renderingPath.ToString();

        #if UNITY_5_0
        	Debug.LogError("Unity 5.0 is not supported. Only Unity 5.1 and later is supported.");
        #endif

        #if UNITY_IOS
			backend = (ScriptingImplementation) UnityEditor.PlayerSettings.GetPropertyInt("ScriptingBackend",BuildTargetGroup.iOS);
		#elif UNITY_tvOS
			backend = (ScriptingImplementation) UnityEditor.PlayerSettings.GetPropertyInt("ScriptingBackend",BuildTargetGroup.tvOS);
		#elif UNITY_ANDROID
			backend = (ScriptingImplementation) UnityEditor.PlayerSettings.GetPropertyInt("ScriptingBackend",BuildTargetGroup.Android);
		#elif UNITY_WSA
			backend = (ScriptingImplementation) UnityEditor.PlayerSettings.GetPropertyInt("ScriptingBackend",BuildTargetGroup.WSA);
		#elif UNITY_WP8 
			backend = (ScriptingImplementation) UnityEditor.PlayerSettings.GetPropertyInt("ScriptingBackend",BuildTargetGroup.WP8Player);
		#elif UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE
			backend = (ScriptingImplementation) UnityEditor.PlayerSettings.GetPropertyInt("ScriptingBackend",BuildTargetGroup.Standalone);
		#elif UNITY_WEBGL
			backend = (ScriptingImplementation) UnityEditor.PlayerSettings.GetPropertyInt("ScriptingBackend",BuildTargetGroup.WebGL);
		#endif

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		if(Directory.Exists(Application.streamingAssetsPath)==false)
		{
			Directory.CreateDirectory(Application.streamingAssetsPath);
		}

		string scripting_backend = Application.streamingAssetsPath+"/scripting_backend.txt";

		if(File.Exists(scripting_backend))
		{
			File.Delete(scripting_backend);
		}

		string rendering_path = Application.streamingAssetsPath+"/rendering_path.txt";
		if(File.Exists(rendering_path))
		{
			File.Delete(rendering_path);
		}

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		StreamWriter sw = new StreamWriter(scripting_backend);
		sw.Write(backend);
		sw.Flush();
		sw.Close();

		StreamWriter sw2 = new StreamWriter(rendering_path);
		sw2.Write(renderingPath);
		sw2.Flush();
		sw2.Close();

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
    }
}