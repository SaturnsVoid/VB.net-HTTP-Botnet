<?php
// show potential errors / feedback (from login object)
if (isset($login)) {
    if ($login->errors) {
        foreach ($login->errors as $error) {
            echo $error;
        }
    }
    if ($login->messages) {
        foreach ($login->messages as $message) {
            echo $message;
        }
    }
}
?>
<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <title>Admin Login</title>
    <meta name="author" content="Srinu Basava">
    <meta content="width=device-width, initial-scale=1.0, user-scalable=no" name="viewport">
    <meta name="description" content="White Label Admin Admin UI">
    <meta name="keywords" content="White Label Admin, Admin UI, Admin Dashboard, Srinu Basava">
    <script src="js/html5-trunk.js"></script>
    <link href="icomoon/style.css" rel="stylesheet">
    <link href="css/main.css" rel="stylesheet">
  </head>
  <body>
    <div class="container-fluid">
      <div class="row-fluid">
        <div class="span4 offset4">
          <div class="signin">
            <h1 class="center-align-text">Admin Login</h1>
            <form action="index.php" class="signin-wrapper" method="post" name="loginform">
              <div class="content">
                <input class="input input-block-level" placeholder="Username" type="text" id="login_input_username" name="user_name" value="">
                <input class="input input-block-level" placeholder="Password" type="password"  id="login_input_password" name="user_password">
              </div>
              <div class="actions">
                <input class="btn btn-info pull-right" name="login" type="submit" value="Login">
                <div class="clearfix"></div>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    
    <script src="js/jquery.min.js"></script>
    <script src="js/bootstrap.js"></script>
    
  </body>
</html>