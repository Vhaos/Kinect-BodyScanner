<?php

/*
 * Point cloud processor Controller.
 * 
*/ 

class ProcessPCController extends AbstractController
{
	protected $read_dir = "C:\\xampp\htdocs\phpresttest\scans";

	protected $write_dir = "scans\\";
	
	protected $scan_measure_path = "C:\Program Files (x86)\Tony Ruto\Home Scanner Tools\ScanMeasureCmd.exe";

    /*
     * GET method.
     */ 

    public function get($request)
    {	
    	return $this -> _requestStatus(405);
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

    	$id = $request->url_elements[1];
    	$gender = $this->get_gender($request->url_elements[2]);

    	if($this -> exec_enabled())
    	{
    		$cmd = $this->generate_cmd($id,$gender);
    	}else{
    		return "exec function is disabled.";
    	}

    	$output = $this->execInBackground($cmd);

    	return $this->_requestStatus(200).", started processing pointcloud";
    }

    private function generate_cmd($id, $gender) 
    {
    	if($this -> exec_enabled())
    	{
    		return 'cd C:\Program Files (x86)\Tony Ruto\Home Scanner Tools & start /B ScanMeasureCmd.exe ' .$this->read_dir .'\\'. $id .'\pointcloud.xyz '.$gender;
    	}else{
    		return "exec function not enabled.";
    	} 
    }

    private function execInBackground($cmd) 
    { 
    	if (substr(php_uname(), 0, 7) == "Windows")
    	{ 
    		pclose(popen($cmd, 'r'));
    	} else { 
    		exec($cmd . " > /dev/null &");
    	} 
    } 

    private function exec_enabled() 
    {
    	$disabled = explode(', ', ini_get('disable_functions'));
    	return !in_array('exec', $disabled);
    }

    private function get_gender($arg)
    {
    	if($arg == 'M' | $arg == 'm')
    	{
    		return "MKF2";
    	}elseif ($arg == 'F' | $arg == 'f') {
    		return "MKF1";
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