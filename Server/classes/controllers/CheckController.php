<?php

/*
 * Point cloud processor Controller.
 * 
*/ 

class CheckController extends AbstractController
{
    protected $dir = "scans";

	protected $read_dir = "C:\\xampp\htdocs\server\scans";

    /*
     * GET method.
     */ 

    public function get($request)
    {	
    	$id = $request->url_elements[1];
        
        if ($this->is_scan_complete($id))
        {
            $array = $this->xml2array($id);

            /*$result = null;

            while ('measure' != current($array)) {
                
                $result = $result.key($array); 
                next($array);
            }
            
            return $array->measure[0];
            return $this->parse_array($array);*/
            return $array;

        } else {
            return $this->_requestStatus(200).", Scan not yet finished";
        }
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

        return $this -> _requestStatus(405);
    }

    private function parse_array($array)
    {
        $measurements = array( 
            'Height' => $array->measure[0], 
            'Hip' => $array->measure[1],
            'Chest' => $array->measure[2], 
            'Waist' => $array->measure[3],
            'Inside Leg' => $array->measure[4]   
        );
    }

    private function xml2array($id)
    {
        $xml = simplexml_load_file( $this->dir. '/' . $id . '/' . 'pointcloud_fixed.xml');
        $json = json_encode($xml);
        $array = json_decode($json,TRUE);
        return $array;
    }

    private function is_scan_complete($id)
    {
        $filename = $this->dir. '/' . $id . '/' . 'pointcloud_fixed.xml';

        if (file_exists($filename)) 
        {
            return TRUE;
        } else {
            return FALSE;
        }
    }

    private function _requestStatus($code) 
    {
    	$status = array(  
    		200 => 'OK',
    		404 => 'Not Found',   
    		405 => 'Method Not Allowed',
    		500 => 'Internal Server Error',
    		); 
    	return ($status[$code])?$status[$code]:$status[500];
    } 
}
