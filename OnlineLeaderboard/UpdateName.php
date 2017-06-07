<?php
    // error_reporting(E_ALL);
    // ini_set('display_errors', 1);

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

    $id = (int)$_POST['id'];
    $name = $_POST['name'];

    $hash = $_POST['hash'];
    $realHash = md5($id . $name . $secretKey); 

    if($realHash == $hash) {
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        $sql = "UPDATE Scores SET name ='$name' WHERE id=$id";
        $stmt = $pdo->prepare($sql);

        try {
            $stmt->execute();
            echo $stmt->rowCount() . " records UPDATED successfully";
        } catch(Exception $e) { //PDOException?
            echo 'Error: ' . $e->getMessage();
            exit();
        }
    }

    // Close connection
    $pdo = null;
?>