using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ManualBase64;

public class CollectData : MonoBehaviour {

	public class DeviceInfo
	{
		public string deviceModel;
		public string deviceName;
		public string deviceType;
		public string deviceUniqueIdentifier;
		public string graphicsDeviceID;
		public string graphicsDeviceName;
		public string graphicsDeviceType;
		public string graphicsDeviceVendor;
		public string graphicsDeviceVendorID;
		public string graphicsDeviceVersion;
		public string graphicsMemorySize;
		public string operatingSystem;
		public string processorType;
		public string processorCount;
		public string systemMemorySize;
		public string runningResolution;
		public string runningUnityVersion;
		public string runningPlatform;
		public string targetFrameRate;
		public string scriptingBackend;
		public string renderingPath;
		public Texture2D screenshot;
		public byte[] screenshotBinary;
		public string screenshotBinaryFull = "";

		public DeviceInfo() {}
	}

	private List<DeviceInfo> m_deviceList = new List<DeviceInfo>();
	private bool isCollecting;
	private bool isSubmitting;
	private bool isCapturingScreen;
	private bool CaptureScreen;
	private bool isSubmitted;
	private bool isConnectionGood;
	private bool isMakingScreenshot;
	private Texture2D lastScreenshot;

	private void Start()
	{
		Application.targetFrameRate = 60;
		Application.runInBackground = true;
		isSubmitting = false;
		CaptureScreen = true;
		isCapturingScreen = false;
		isSubmitted = false;
		isMakingScreenshot = false;
		StartCoroutine(Collect());
	}

	private IEnumerator Collect()
	{
		isCollecting = true;
		yield return new WaitForSeconds(1.0f);

		m_deviceList.Add(new DeviceInfo());

		m_deviceList[0].deviceModel = SystemInfo.deviceModel;
		Debug.Log("deviceModel: "+m_deviceList[0].deviceModel);

		m_deviceList[0].deviceName = SystemInfo.deviceName;
		Debug.Log("deviceName: "+m_deviceList[0].deviceName);

		m_deviceList[0].deviceType = SystemInfo.deviceType.ToString();
		Debug.Log("deviceType: "+m_deviceList[0].deviceType);

		m_deviceList[0].deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
		Debug.Log("deviceUniqueIdentifier: "+m_deviceList[0].deviceUniqueIdentifier);

		m_deviceList[0].graphicsDeviceID = SystemInfo.graphicsDeviceID.ToString();
		Debug.Log("graphicsDeviceID: "+m_deviceList[0].graphicsDeviceID);

		m_deviceList[0].graphicsDeviceName = SystemInfo.graphicsDeviceName;
		Debug.Log("graphicsDeviceName: "+m_deviceList[0].graphicsDeviceName);

		m_deviceList[0].graphicsDeviceType = SystemInfo.graphicsDeviceType.ToString();
		Debug.Log("graphicsDeviceType: "+m_deviceList[0].graphicsDeviceType);

		m_deviceList[0].graphicsDeviceVendor = SystemInfo.graphicsDeviceVendor;
		Debug.Log("graphicsDeviceVendor: "+m_deviceList[0].graphicsDeviceVendor);

		m_deviceList[0].graphicsDeviceVendorID = SystemInfo.graphicsDeviceVendorID.ToString();
		Debug.Log("graphicsDeviceVendorID: "+m_deviceList[0].graphicsDeviceVendorID);

		m_deviceList[0].graphicsDeviceVersion = SystemInfo.graphicsDeviceVersion;
		Debug.Log("graphicsDeviceVersion: "+m_deviceList[0].graphicsDeviceVersion);

		m_deviceList[0].graphicsMemorySize = SystemInfo.graphicsMemorySize.ToString();
		Debug.Log("graphicsMemorySize: "+m_deviceList[0].graphicsMemorySize);

		m_deviceList[0].operatingSystem = SystemInfo.operatingSystem;
		Debug.Log("operatingSystem: "+m_deviceList[0].operatingSystem);

		m_deviceList[0].processorType = SystemInfo.processorType;
		Debug.Log("processorType: "+m_deviceList[0].processorType);

		m_deviceList[0].processorCount = SystemInfo.processorCount.ToString();
		Debug.Log("processorCount: "+m_deviceList[0].processorCount);

		m_deviceList[0].systemMemorySize = SystemInfo.systemMemorySize.ToString();
		Debug.Log("systemMemorySize: "+m_deviceList[0].systemMemorySize);

		m_deviceList[0].runningResolution = Screen.width.ToString()+"x"+Screen.height.ToString();
		Debug.Log("runningResolution: "+m_deviceList[0].runningResolution);

		m_deviceList[0].runningUnityVersion = Application.unityVersion.ToString();
		Debug.Log("runningUnityVersion: "+m_deviceList[0].runningUnityVersion);

		m_deviceList[0].runningPlatform = Application.platform.ToString();
		Debug.Log("runningPlatform: "+m_deviceList[0].runningPlatform);

		m_deviceList[0].targetFrameRate = Application.targetFrameRate.ToString();
		Debug.Log("targetFrameRate: "+m_deviceList[0].targetFrameRate);

		yield return StartCoroutine(ReadPreProcessData());
//		ScriptingImplementation asd = new ScriptingImplementation();
//		asd = (ScriptingImplementation) UnityEditor.PlayerSettings.GetPropertyInt("ScriptingBackend",BuildTargetGroup.iOS);

//		Debug.Log(asd);
		yield return 0;

		isCollecting = false;

		if(CaptureScreen)
		{
			isCapturingScreen = true;

		//	string filename = "screenshot.png";
 		//	string path = Path.Combine(Application.persistentDataPath, filename);

 		//	#if UNITY_IOS
 		//	path = "screenshot.png";
 		//	#endif

		//	Application.CaptureScreenshot(path);
			
		//	yield return new WaitForSeconds(1.0f);

			yield return new WaitForSeconds(0.3f);

			isMakingScreenshot = true;

			while(isMakingScreenshot)
			{
				yield return 0;
			}

//        	using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
  //      	{
        		byte[] imgBytes;
        		imgBytes = lastScreenshot.EncodeToPNG();//reader.ReadBytes(int.MaxValue);
        		ManualBase64Encoder be = new ManualBase64Encoder(imgBytes);
        		char[] re = be.Encode();
        		string s = new string(re);
        		//string s = System.Convert.ToBase64String(imgBytes);
        	/*	StreamWriter sw4 = new StreamWriter(Application.streamingAssetsPath+"/test3.txt");
        		sw4.Write(s);
        		sw4.Flush();
        		sw4.Close();*/
        		//Base64Encoder en = new Base64Encoder(imgBytes);
        		//char[] res = en.GetEncoded();
        		//string s = new string(res);
        		m_deviceList[0].screenshotBinaryFull = s;

        		//for(int i=0;i<res.Length;i++)
        		{
        		//	m_deviceList[0].screenshotBinaryFull += res[i].ToString();
        		}
        		//string full = reader.ReadString();
        		//m_deviceList[0].screenshotBinaryFull = full;
        	//	for(int i=0;i<imgBytes.Length;i++)
        		{
        	//		m_deviceList[0].screenshotBinaryFull += imgBytes[i].ToString()+":";
        		}

        	//	Debug.Log(m_deviceList[0].screenshotBinaryFull);
           		//m_deviceList[0].screenshotBinary = imgBytes;

        	//	tex.LoadImage(imgBytes);
        	//}

        	//using (StreamReader reader = new StreamReader(path))
        	{
        		//m_deviceList[0].screenshotBinaryFull = reader.ReadToEnd();
        	}
           	
           	Debug.Log("IMAGE DATA: "+m_deviceList[0].screenshotBinaryFull);

           	if(lastScreenshot!=null)
           	{
           		m_deviceList[0].screenshot = lastScreenshot;
           	}
           	else
           	{
           		Debug.LogError("Screenshot is null");
           	}

           	isCapturingScreen = false;
		}

		yield return 0;
	}

	private IEnumerator ReadPreProcessData()
	{
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "scripting_backend.txt");

		if (filePath.Contains("://"))
		{
			WWW www = new WWW(filePath);
            yield return www;
            m_deviceList[0].scriptingBackend = www.text;
		}
		else
		{
#if UNITY_WSA
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
#else
            string fs = filePath;
#endif
            StreamReader sr = new StreamReader(fs);
			m_deviceList[0].scriptingBackend = sr.ReadToEnd();
#if UNITY_WSA
            sr.Dispose();
#else
            sr.Close();
#endif
        }

		Debug.Log("scriptingBackend: "+m_deviceList[0].scriptingBackend);
		
		string filePath2 = System.IO.Path.Combine(Application.streamingAssetsPath, "rendering_path.txt");

		if(filePath2.Contains("://"))
		{
			WWW www = new WWW(filePath2);
            yield return www;
            m_deviceList[0].renderingPath = www.text;
		}
		else
        {
#if UNITY_WSA
            FileStream fs2 = new FileStream(filePath2, FileMode.Open, FileAccess.Read);
#else
            string fs2 = filePath2;
#endif
            StreamReader sr2 = new StreamReader(fs2);
			m_deviceList[0].renderingPath = sr2.ReadToEnd();
#if UNITY_WSA 
            sr2.Dispose();
#else
            sr2.Close();
#endif
        }

		Debug.Log("renderingPath: "+m_deviceList[0].renderingPath);

		yield return 0;
	}

	private IEnumerator CheckInternet()
	{
		WWW www = new WWW("http://unityqakit.us.lt/performance/www/internet_check.php");
		yield return www;

		isConnectionGood = true;

		if (!string.IsNullOrEmpty(www.error))
		{
            Debug.Log("[WWW] "+www.error);
            isConnectionGood = false;
		}

		if(isConnectionGood)
		{
			Debug.Log("Connection - PASS");
		}

		yield return 0;
	}

	private IEnumerator Submit()
	{
		isSubmitting = true;
		
		yield return StartCoroutine(CheckInternet());

		if(isConnectionGood)
		{
			WWWForm m_form = new WWWForm();
			m_form.AddField("deviceModel", m_deviceList[0].deviceModel);
			m_form.AddField("deviceName", m_deviceList[0].deviceName);
			m_form.AddField("deviceType", m_deviceList[0].deviceType);
			m_form.AddField("deviceUniqueIdentifier", m_deviceList[0].deviceUniqueIdentifier);
			m_form.AddField("graphicsDeviceID", m_deviceList[0].graphicsDeviceID);
			m_form.AddField("graphicsDeviceName", m_deviceList[0].graphicsDeviceName);
			m_form.AddField("graphicsDeviceType", m_deviceList[0].graphicsDeviceType);
			m_form.AddField("graphicsDeviceVendor", m_deviceList[0].graphicsDeviceVendor);
			m_form.AddField("graphicsDeviceVendorID", m_deviceList[0].graphicsDeviceVendorID);
			m_form.AddField("graphicsDeviceVersion", m_deviceList[0].graphicsDeviceVersion);
			m_form.AddField("graphicsMemorySize", m_deviceList[0].graphicsMemorySize);
			m_form.AddField("operatingSystem", m_deviceList[0].operatingSystem);
			m_form.AddField("processorType", m_deviceList[0].processorType);
			m_form.AddField("processorCount", m_deviceList[0].processorCount);
			m_form.AddField("systemMemorySize", m_deviceList[0].systemMemorySize);
			m_form.AddField("runningResolution", m_deviceList[0].runningResolution);
			m_form.AddField("runningUnityVersion", m_deviceList[0].runningUnityVersion);
			m_form.AddField("runningPlatform", m_deviceList[0].runningPlatform);
			m_form.AddField("targetFrameRate", m_deviceList[0].targetFrameRate);
			m_form.AddField("scriptingBackend", m_deviceList[0].scriptingBackend);
			m_form.AddField("renderingPath", m_deviceList[0].renderingPath);
	//		Debug.Log("BINARY SIZE: "+m_deviceList[0].screenshotBinary.Length);
			m_form.AddField("screenshotBinary",m_deviceList[0].screenshotBinaryFull);

			Dictionary<string,string> m_headers = m_form.headers;
			byte[] m_rawData = m_form.data;

			WWW handle_www = new WWW("http://unityqakit.us.lt/performance/www/submit_data.php",m_rawData,m_headers);
			yield return handle_www;

			if (!string.IsNullOrEmpty(handle_www.error))
			{
	            Debug.Log("[WWW] "+handle_www.error);
			}

			Debug.Log(handle_www.text);

			isSubmitted = true;
			isSubmitting = false;
		}
		else
		{
			Debug.Log("FAILED TO CONNECT. CHECK INTERNET CONNECTION!");
		}

		yield return 0;
	}

	private void OnPostRender()
	{
		if(isMakingScreenshot)
		{
			lastScreenshot = new Texture2D(Screen.width,Screen.height);
	        lastScreenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
	        lastScreenshot.Apply();

	        isMakingScreenshot = false;
	    }
	}

	private void OnGUI()
	{
		if(isCollecting)
		{
			GUI.Label(new Rect(10,10,200,200),"COLLECTING DATA...");
			return;
		}

		if(isSubmitting)
		{
			GUI.Label(new Rect(10,10,200,200),"SUBMITTING DATA...");
			return;
		}

		if(isCapturingScreen)
		{
			GUI.Label(new Rect(10,10,200,200),"CAPTURING SCREEN...");
			return;
		}

		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"ALL DONE");

		if(!isSubmitted)
		{
			if(GUI.Button(new Rect(250,250,300,200),"SUBMIT DATA"))
			{
				StartCoroutine(Submit());
			}
		}
		else if(isSubmitted)
		{
			GUI.Label(new Rect(350,350,300,200),"SUBMITTED!");
		}

		if(m_deviceList.Count>0)
		{
			if(m_deviceList[0].screenshot)
			{
				GUI.DrawTexture(new Rect(500,30,Screen.width/4,Screen.height/4),m_deviceList[0].screenshot);
			}
		}

		for(int i=0;i<m_deviceList.Count;i++)
		{
			DeviceInfo d = m_deviceList[i];

			GUI.Label(new Rect(10,10,1000,1000),
				"deviceModel: "+d.deviceModel+"\n"+
				"deviceName: "+d.deviceName+"\n"+
				"deviceType: "+d.deviceType+"\n"+
				"deviceUniqueIdentifier: "+d.deviceUniqueIdentifier+"\n"+
				"graphicsDeviceID: "+d.graphicsDeviceID+"\n"+
				"graphicsDeviceName: "+d.graphicsDeviceName+"\n"+
				"graphicsDeviceType: "+d.graphicsDeviceType+"\n"+
				"graphicsDeviceVendor: "+d.graphicsDeviceVendor+"\n"+
				"graphicsDeviceVendorID: "+d.graphicsDeviceVendorID+"\n"+
				"graphicsDeviceVersion: "+d.graphicsDeviceVersion+"\n"+
				"graphicsMemorySize: "+d.graphicsMemorySize+"\n"+
				"operatingSystem: "+d.operatingSystem+"\n"+
				"processorType: "+d.processorType+"\n"+
				"processorCount: "+d.processorCount+"\n"+
				"systemMemorySize: "+d.systemMemorySize+"\n"+
				"runningResolution: "+d.runningResolution+"\n"+
				"runningUnityVersion: "+d.runningUnityVersion+"\n"+
				"runningPlatform: "+d.runningPlatform+"\n"+
				"targetFrameRate: "+d.targetFrameRate+"\n"+
				"scriptingBackend: "+d.scriptingBackend+"\n"+
				"renderingPath: "+d.renderingPath
			);
		}
	}
}
