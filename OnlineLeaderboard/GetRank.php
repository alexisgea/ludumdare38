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

    // get value from link
    $id = (int)$_GET['id'];

    // querry player rank
    $rankQuery = "SELECT  uo.*,
        (
        SELECT  COUNT(*)
        FROM    Scores ui
        WHERE   (ui.score, -ui.ts) >= (uo.score, -uo.ts)
        ) AS rank
    FROM    Scores uo
    WHERE   id = '$id';";
 
    // execute querry
    $stmt = $pdo->query($rankQuery);
    $stmt->setFetchMode(PDO::FETCH_ASSOC);
 
    $result = $stmt->fetchAll();
 
    // return values
    if(count($result) == 1) {
        foreach($result as $r) {
            echo $r['rank'];
        }
    }
    else {
        echo 'to many results';
    }

?>