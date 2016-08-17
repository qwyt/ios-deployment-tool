<?php
	error_reporting(0);
	include ("../conn.php");

	$deviceModel = $_POST["deviceModel"];
	$deviceName = $_POST["deviceName"];
	$deviceType = $_POST["deviceType"];
	$deviceUniqueIdentifier = $_POST["deviceUniqueIdentifier"];
	$graphicsDeviceID = $_POST["graphicsDeviceID"];
	$graphicsDeviceName = $_POST["graphicsDeviceName"];
	$graphicsDeviceType = $_POST["graphicsDeviceType"];
	$graphicsDeviceVendor = $_POST["graphicsDeviceVendor"];
	$graphicsDeviceVendorID = $_POST["graphicsDeviceVendorID"];
	$graphicsDeviceVersion = $_POST["graphicsDeviceVersion"];
	$graphicsMemorySize = $_POST["graphicsMemorySize"];
	$operatingSystem = $_POST["operatingSystem"];
	$processorType = $_POST["processorType"];
	$processorCount = $_POST["processorCount"];
	$systemMemorySize = $_POST["systemMemorySize"];
	$runningResolution = $_POST["runningResolution"];
	$runningUnityVersion = $_POST["runningUnityVersion"];
	$runningPlatform = $_POST["runningPlatform"];
	$targetFrameRate = $_POST["targetFrameRate"];
	$scriptingBackend = $_POST["scriptingBackend"];
	$renderingPath = $_POST["renderingPath"];
	$screenshotBinary = $_POST["screenshotBinary"];

	/*if(isset($_FILES['screenshotBinary']))
	{
		move_uploaded_file($_FILES['screenshotBinary']['tmp_name'], "uploads/" . $screenshotBinary.".png");
	}*/

	mysqli_query($conn,"INSERT INTO DeviceInfo (deviceModel,deviceName,deviceType,deviceUniqueIdentifier,graphicsDeviceID,graphicsDeviceName,graphicsDeviceType,graphicsDeviceVendor,graphicsDeviceVendorID,graphicsDeviceVersion,graphicsMemorySize,operatingSystem,processorType,processorCount,systemMemorySize,runningResolution,runningUnityVersion,runningPlatform,targetFrameRate,scriptingBackend,renderingPath,screenshotBinary) VALUES('".$deviceModel."','".$deviceName."','".$deviceType."','".$deviceUniqueIdentifier."','".$graphicsDeviceID."','".$graphicsDeviceName."','".$graphicsDeviceType."','".$graphicsDeviceVendor."','".$graphicsDeviceVendorID."','".$graphicsDeviceVersion."','".$graphicsMemorySize."','".$operatingSystem."','".$processorType."','".$processorCount."','".$systemMemorySize."','".$runningResolution."','".$runningUnityVersion."','".$runningPlatform."','".$targetFrameRate."','".$scriptingBackend."','".$renderingPath."','".$screenshotBinary."')");
?>