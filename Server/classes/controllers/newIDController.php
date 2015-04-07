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
    	header('HTTP/1.1 201 Created');
    	header('Location: /news/');
    	header("Access-Control-Allow-Orgin: *");
        header("Access-Control-Allow-Methods: *");
        

        $id = $request->url_elements[1];
        return $id;
    	
    }

    protected function generateUniqueID()
    {
        //file_put_contents($this->articles_file, serialize($articles));
        return uniqid();
    }
}