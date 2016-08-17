<head>
<link rel="stylesheet" type="text/css" href="table.css">
</head>

<?php
	error_reporting(0);
	include ("conn.php");

	$all_deviceinfo = mysqli_query($conn,"SELECT * FROM DeviceInfo ORDER BY ID DESC");

	echo "<div class='CSSTableGenerator' >";
	echo "<table border=1><tr>
	<td><b>ID</b></td>
	<td><b>Screenshot</b></td>
	<td><b>Model</b></td>
	<td><b>deviceName</b></td>
	<td><b>Platform</b></td>
	<td><b>Type</b></td>
	<td><b>gfxType</b></td>
	<td><b>scrBackend</b></td>
	<td><b>Render</b></td>
	<td><b>gfxDeviceName</b></td>
	<td><b>Vendor</b></td>
	<td><b>gfxVersion</b></td>
	<td><b>gfxMem</b></td>
	<td><b>OS</b></td>
	<td><b>proccessor</b></td>
	<td><b>sysMem</b></td>
	<td><b>Res</b></td>
	<td><b>Unity</b></td>
	<td><b>tFPS</b></td>
	<td><b>UID</b></td>
	</tr>
	";

	while($d=mysqli_fetch_array($all_deviceinfo))
	{
		echo "<tr>";
		echo "<td>".$d["id"]."</td>";
		echo "<td><a target=_blank href=image_parser.php?id=".$d["id"]."><img width=64px height=64px src='data:image/png;charset=utf8;base64,".$d["screenshotBinary"]."'/></a></td>";
		echo "<td>".checkText($d["deviceModel"])."</td>";
		echo "<td>".checkText($d["deviceName"])."</td>";
		echo "<td>".checkText($d["runningPlatform"])."</td>";
		echo "<td>".checkText($d["deviceType"])."</td>";
		echo "<td>".checkText($d["graphicsDeviceType"])."</td>";
		echo "<td>".checkText($d["scriptingBackend"])."</td>";
		echo "<td>".checkText($d["renderingPath"])."</td>";
		echo "<td>".checkText($d["graphicsDeviceName"])." (".checkText($d["graphicsDeviceID"]).")</td>";
		echo "<td>".checkText($d["graphicsDeviceVendor"])." (".checkText($d["graphicsDeviceVendorID"]).")</td>";
		echo "<td>".checkText($d["graphicsDeviceVersion"])."</td>";
		echo "<td>".checkText($d["graphicsMemorySize"])."</td>";
		echo "<td>".checkText($d["operatingSystem"])."</td>";
		echo "<td>".checkText($d["processorType"])." (".checkText($d["processorCount"])." cores)</td>";
		echo "<td>".checkText($d["systemMemorySize"])."</td>";
		echo "<td>".checkText($d["runningResolution"])."</td>";
		echo "<td>".checkText($d["runningUnityVersion"])."</td>";
		echo "<td>".checkText($d["targetFrameRate"])."</td>";
		echo "<td>".checkText($d["deviceUniqueIdentifier"])."</td>";
		echo "</tr>";
	}

	echo "</table>";
	echo "</div>";
?>

<?

function checkText($data)
{
	$data = str_replace("\n","",$data);
	$data = str_replace("\r","",$data);

	if($data=="<unknown>"||$data=="")
	{
		return "<font color=red>N/A</font>";
	}
	else
	{
		return $data;
	}
}

function data_uri($file, $mime) 
{  
  $contents = file_get_contents($file);
  $base64   = base64_encode($contents); 
  return ('data:' . $mime . ';base64,' . $base64);
}
?>