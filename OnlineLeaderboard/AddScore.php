<?php
    
    // error_reporting(E_ALL);
    // ini_set('display_errors', 1);

    // I need to have the config in a specific file
    // Configuration
    $hostname = 'localhost';
    $username = 'root';
    $password = 'root';
    $database = 'tdtest';

    $secretKey = "SuperSecretKey";

    try {
        $pdo = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);        
    }
    catch (PDOException $e) {
        echo 'Error: ' . $e->getMessage();
        exit();
    }

    $name = $_POST['name'];
    $score = $_POST['score'];
    $ts = CURRENT_TIMESTAMP;

    $hash = $_POST['hash'];
    $realHash = md5($name . $score . $secretKey); 
    //$realHash = md5($_GET['name'] . $_GET['score'] . $secretKey); 
    if($realHash == $hash) {
        //$stmt = $pdo->prepare('INSERT INTO scores VALUES (null, :name, :score, CURRENT_TIMESTAMP)');
        $sql = "INSERT INTO Scores SET name = '$name', score = '$score', ts = CURRENT_TIMESTAMP;"; // ts = CURRENT_TIMESTAMP
        $stmt = $pdo->prepare($sql);
        try {
            //$stmt->execute($_GET);
            $stmt->execute();
        } catch(Exception $e) { // PDOException?
            echo 'Error: ' . $e->getMessage();
            exit();
        }
    }

    $id = $pdo->lastInsertId();
    echo $id;

    // Close connection
    $pdo = null;

    // echo 'all good to end of AddScore.php script'
?>