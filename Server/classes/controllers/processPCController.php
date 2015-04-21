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
        //setting up header files
        header('HTTP/1.1 201 Created');
        header('Location: /news/');
        header("Access-Control-Allow-Orgin: *");
        header("Access-Control-Allow-Methods: *");

        $id = $request->url_elements[1];
        $gender = $this->get_gender($request->url_elements[2]);

        if($this -> exec_enabled())
        {
            $cmd = $this->process_pointcloud_cmd($id,$gender);
        }else{
            return "exec function is disabled.";
        }

        $output = $this->execInBackground($cmd);

        return $this->_requestStatus(200).", started processing pointcloud";

        //return $this -> _requestStatus(405);
    }

    /*
     * PUT method.
     * 
     */ 

    public function put($request)
    {	
        header('HTTP/1.1 201 Created');
        header('Location: /news/');
        header("Access-Control-Allow-Orgin: *");
        header("Access-Control-Allow-Methods: *");

        $id = $request->url_elements[1];

        $ftp_server="localhost"; 
        $ftp_user_name="newuser"; 
        $ftp_user_pass=""; 
        $file = $request->parameters['file'];  //tobe uploaded 
        $remote_file = $id . '/pointcloud.xyz'; 

        // set up basic connection 
        $conn_id = ftp_connect($ftp_server); 

        // login with username and password 
        $login_result = ftp_login($conn_id, $ftp_user_name, $ftp_user_pass); 

        // upload a file 
        if (ftp_put($conn_id, $remote_file, $file, FTP_BINARY)) { 
            echo "successfully uploaded $file\n"; 
            exit;
            ftp_close($conn_id);  
        } else { 
            echo "There was a problem while uploading $file\n"; 
            exit; 
            ftp_close($conn_id); 
        }

        // close the connection 
        ftp_close($conn_id); 

        return "request completed";

    }

    private function process_pointcloud_cmd($id, $gender) 
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

    private function does_file_exist($filename){
        // Check if file already exists
        if (file_exists($target_file)) {
            return TRUE;
            $uploadOk = 0;
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