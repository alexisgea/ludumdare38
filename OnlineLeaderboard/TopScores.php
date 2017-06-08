<?php
    include 'dbConfig.php'; // db connection variables

    $hash = $_POST['hash'];
    $t = $_POST['t'];

    // compute hash to compare with given one
    $realHash = md5($t . $secretKey); 

    if($realHash == $hash) {

        // connect to db
        try {
            $pdo = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);        
        }
        catch (PDOException $e) {
            echo 'Error: ' . $e->getMessage();
            exit();
        }
        
        $stmt = $pdo->query('SELECT * FROM Scores ORDER BY score DESC LIMIT 10');
        $stmt->setFetchMode(PDO::FETCH_ASSOC);
        $result = $stmt->fetchAll();

        // echo the result as a json
        $rank = 1;
        if(count($result) > 0) {
            $resultArray = array(); 
            foreach($result as $r) {
                $resultArray[] = array("Id"=>$r['id'], "Rank"=>$rank, "Name"=>$r['name'], "Score"=>$r['score'], );
                $rank += 1;
            }

            $resultJSON = json_encode(array("ScoreLines" => $resultArray));
            echo $resultJSON;
        }

        // Close connection
        $pdo = null;
    }
?>