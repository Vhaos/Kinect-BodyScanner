<?php

/*
 * Unique ID Controller.
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

    protected function generateUniqueID()
    {
        //file_put_contents($this->articles_file, serialize($articles));
        return uniqid();
    }
}