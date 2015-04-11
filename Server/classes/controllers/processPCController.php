<?php

/*
 * Unique ID Controller.
 * eg. 5522e70fe6cb4
*/ 

class newIDController extends AbstractController
{
    /*
     * GET method.
     */ 

    public function get($request)
    {	
    	$uniqueId = $this -> generateUniqueID();
        return $uniqueId;
    }

    /*
     * POST method.
     */ 

    public function post($request)
    {	
        //setting up header files
    	header('HTTP/1.1 201 Created');
    	header('Location: /news/');
    	header("Access-Control-Allow-Orgin: *");
        header("Access-Control-Allow-Methods: *");

        // name of the root folder for our scans
        $dir = "scans";
        $file_to_write = 'status.txt';

        $id = $request->url_elements[1];

        if( is_dir($dir. '/' .$id) === false )
        {
            mkdir($dir. '/' .$id, 0700);
        }
        
        $file = fopen($dir. '/' . $id . '/' . $file_to_write,"w");
        fwrite($file,"not yet processed");
        fclose($file);

        return "just created ".$id." folder";
    	
    }

    protected function generateUniqueID()
    {
        //file_put_contents($this->articles_file, serialize($articles));
        return uniqid();
    }
}