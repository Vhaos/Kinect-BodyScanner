<?php

/*
 * Point cloud processor Controller.
 * 
*/ 

class processPCController extends AbstractController
{
    /*
     * GET method.
     */ 

    public function get($request)
    {	
    	//return "invalid request";
    	$output = 'null';
        $disabled = explode(', ', ini_get('disable_functions'));
    	
    	return exec('"C:\Program Files (x86)\Tony Ruto\Home Scanner Tools\ScanMeasureCmd.exe" "C:\xampp\htdocs\phpresttest\scans\5522e70fe6cb4\pointcloud.wrl" "MKF1"');
    	//exec('C:\Program Files (x86)\Tony Ruto\Home Scanner Tools\ScanMeasure.exe');
    	//return !in_array('exec', $disabled);
    	//return $output;
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

        echo exec('whoami');

        return "running process";
    	
    }

    public function exec_enabled() {
    	$disabled = explode(', ', ini_get('disable_functions'));
    	return !in_array('exec', $disabled);
    }
}