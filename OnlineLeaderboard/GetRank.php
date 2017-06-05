<?php
    error_reporting(E_ALL);
    ini_set('display_errors', 1);
    
    // Configuration
    $hostname = 'localhost';
    $username = 'root';
    $password = 'root';
    $database = 'tdtest';

    try {
        $pdo = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);        
    }
    catch (PDOException $e) {
        echo 'Error: ' . $e->getMessage();
        exit();
    }

    $id = (int)$_GET['id'];

    $rankQuery = "SELECT  uo.*,
        (
        SELECT  COUNT(*)
        FROM    Scores ui
        WHERE   (ui.score, -ui.ts) >= (uo.score, -uo.ts)
        ) AS rank
    FROM    Scores uo
    WHERE   id = '$id';";
 
    $stmt = $pdo->query($rankQuery);
    $stmt->setFetchMode(PDO::FETCH_ASSOC);
 
    $result = $stmt->fetchAll();
 
    if(count($result) == 1) {
        foreach($result as $r) {
            echo $r['rank'];
        }
    }
    else {
        echo 'to many results';
    }

?>