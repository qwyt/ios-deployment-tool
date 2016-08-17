<head>
<link rel="stylesheet" type="text/css" href="table.css">
</head>

<?php
	error_reporting(0);
	include ("conn.php");

	$id = $_GET["id"];

	$all_deviceinfo = mysqli_query($conn,"SELECT screenshotBinary FROM DeviceInfo where id=$id");

	$deviceinfo = mysqli_fetch_array($all_deviceinfo);

	echo "<img src='data:image/png;charset=utf8;base64,".$deviceinfo["screenshotBinary"]."'/>";
?>