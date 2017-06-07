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
    $id = (int)$_GET['id'];

    // get player rank query
    $sql1 = "SELECT  uo.*,
        (
        SELECT  COUNT(*)
        FROM    Scores ui
        WHERE   (ui.score, -ui.ts) >= (uo.score, -uo.ts)
        ) AS rank
    FROM    Scores uo
    WHERE   id = '$id';";

    // execute player rank query    
    $stmt1 = $pdo->query($sql1); // should this be prepare???
    $stmt1->setFetchMode(PDO::FETCH_ASSOC);
    $result1 = $stmt1->fetchAll();

    // store player rank in a variable
    $playerRank = 0;
    if(count($result1) == 1) {
        foreach($result1 as $r) {
            $playerRank = (int)$r['rank'];
        }
    }
    else {
        Die("Something went wrong");
    }

    // get surrounding scores queries
    $sql2 = "SET @rownum := 0";
    $sql3 = "SELECT rank, name, score, id FROM (
            SELECT @rownum := @rownum + 1 AS rank, name, score, id
            FROM Scores ORDER BY score DESC
            ) as result WHERE rank > $playerRank-5
            limit 10;";

    // execute queries
    $stmt2 = $pdo->query($sql2);
    $stmt2->execute();

    $stmt3 = $pdo->query($sql3);
    $stmt3->setFetchMode(PDO::FETCH_ASSOC);
    $result2 = $stmt3->fetchAll();

    // echo the result line by line
    if(count($result2) > 0) {
        foreach($result2 as $r) {
            echo $r['id'], "\t", $r['rank'], "\t", $r['name'], "\t", $r['score'], "\n";
        }
    }

    // Close connection
    $pdo = null;

?>