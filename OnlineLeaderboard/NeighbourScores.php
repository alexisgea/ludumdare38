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


    // FIRST IDEA
    // order by score
    // lookup id value
    // take 5 raw up
    // take 5 raw down

    // SECOND IDEA
    // compute rank
    // order by rank
    // look up id
    // select within rank range

    $id = (int)$_GET['id'];

    // get player rank
    $sql1 = "SELECT  uo.*,
        (
        SELECT  COUNT(*)
        FROM    Scores ui
        WHERE   (ui.score, -ui.ts) >= (uo.score, -uo.ts)
        ) AS rank
    FROM    Scores uo
    WHERE   id = '$id';";

    // potential replacement query
    /*
    $sql1 = "SET @rownum := 0";
    $sql2 = "SELECT rank, name, score, id FROM (
            SELECT @rownum := @rownum + 1 AS rank, name, score, id
            FROM Scores ORDER BY score ASC
            ) as result WHERE id=$id";
            // DESC
    */
    
    $stmt1 = $pdo->query($sql1); // should this be prepare???
    $stmt1->setFetchMode(PDO::FETCH_ASSOC);
    $result1 = $stmt1->fetchAll();

    $playerRank = 0;
    if(count($result1) == 1) {
        foreach($result1 as $r) {
            $playerRank = (int)$r['rank'];
        }
    }
    else {
        Die("Something went wrong");
    }

    // get array of values needed
    $sql2 = "SET @rownum := 0";
    $sql3 = "SELECT rank, name, score, id FROM (
            SELECT @rownum := @rownum + 1 AS rank, name, score, id
            FROM Scores ORDER BY score DESC
            ) as result WHERE rank > $playerRank-5
            limit 10;";
            // DESC

    $stmt2 = $pdo->query($sql2);
    $stmt2->execute();

    $stmt3 = $pdo->query($sql3);
    $stmt3->setFetchMode(PDO::FETCH_ASSOC);
    $result2 = $stmt3->fetchAll();

    if(count($result2) > 0) {
        foreach($result2 as $r) {
            echo $r['id'], "\t", $r['rank'], "\t", $r['name'], "\t", $r['score'], "\n";
        }
    }

    // Close connection
    $pdo = null;

?>