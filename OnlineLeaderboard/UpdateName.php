<?php
    include 'dbConfig.php'; // db connection variables

    // connect to db
    try {
        $pdo = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);        
    }
    catch (PDOException $e) {
        echo 'Error: ' . $e->getMessage();
        exit();
    }

    // get values from request link
    $id = (int)$_POST['id'];
    $name = $_POST['name'];
    $hash = $_POST['hash'];
    
    // compute hash to compare with given one
    $realHash = md5($id . $name . $secretKey); 

    if($realHash == $hash) {
        // prepare query for execution
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        $sql = "UPDATE Scores SET name ='$name' WHERE id=$id";
        $stmt = $pdo->prepare($sql);

        // if good send a all good message        
        try {
            $stmt->execute();
            echo $stmt->rowCount() . " records UPDATED successfully";
        } // if error return it
        catch(Exception $e) { //PDOException?
            echo 'Error: ' . $e->getMessage();
            exit();
        }
    }

    // Close connection
    $pdo = null;
?>