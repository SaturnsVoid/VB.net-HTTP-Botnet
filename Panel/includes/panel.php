<?php
//require_once("../config.php");
?>
<!doctype html>
<html>
<head>
<meta charset="utf-8">
<title>Master of Puppets</title>
    <script src="../js/html5-trunk.js"></script>
    <link href="../icomoon/style.css" rel="stylesheet">
    <link href="../css/main.css" rel="stylesheet">
    <link href="../css/fullcalendar.css" rel="stylesheet">
</head>
<body>
<!-- if you need user information, just put them into the $_SESSION variable and output them here -->
Hey, <?php echo $_SESSION['user_name']; ?>. You are logged in.
Try to close this browser tab and open it again. Still logged in! ;)

<!-- because people were asking: "index.php?logout" is just my simplified form of "index.php?logout=true" -->
<a href="../index.php?logout">Logout</a>
</body>
</html>
