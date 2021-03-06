<?php
    include 'dbConfig.php'; // db connection variables

    // get values from request link
    $name = $_POST['name'];
    $score = $_POST['score'];
    $hash = $_POST['hash'];

    // compute hash to compare with given one
    $realHash = md5($name . $score . $secretKey); 

    if($realHash == $hash) {
        // connect to db
        try {
            $pdo = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);        
        }
        catch (PDOException $e) {
            echo 'Error: ' . $e->getMessage();
            exit();
        }

        $ts = CURRENT_TIMESTAMP;
        
        // prepare query for execution
        $sql = "INSERT INTO Scores SET name = '$name', score = '$score', ts = CURRENT_TIMESTAMP;";
        $stmt = $pdo->prepare($sql);
        // if good return player id
        try {
            $stmt->execute();
            $id = $pdo->lastInsertId();
            echo $id;
        } // if error return it
        catch(Exception $e) { // PDOException?
            echo 'Error: ' . $e->getMessage();
        }

        // Close connection
        $pdo = null;
    }

?>