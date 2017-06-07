<?php
    
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
    
    // TODO 
    // add line number as rank (+1) to have the same receiving funcitno for building the UI
    $stmt = $pdo->query('SELECT * FROM scores ORDER BY score DESC LIMIT 10');
    $stmt->setFetchMode(PDO::FETCH_ASSOC);
 
    $result = $stmt->fetchAll();
 
    if(count($result) > 0) {
        foreach($result as $r) {
            echo $r['id'], "\t", $r['name'], "\t", $r['score'], "\n";
        }
    }
    
?>