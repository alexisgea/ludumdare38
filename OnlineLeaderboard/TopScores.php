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
    
    $stmt = $pdo->query('SELECT * FROM scores ORDER BY score DESC LIMIT 10');
    $stmt->setFetchMode(PDO::FETCH_ASSOC);
    $result = $stmt->fetchAll();

    // TODO(Done to be tested)
    // add line number as rank (+1) to have the same receiving funcitno for building the UI
    $rank = 1;
    if(count($result) > 0) {
        foreach($result as $r) {
            //echo $r['id'], "\t", $r['name'], "\t", $r['score'], "\n";
            echo $r['id'], "\t", $rank, "\t", $r['name'], "\t", $r['score'], "\n";
            $rank += 1;
        }
    }

    // Close connection
    $pdo = null;
?>