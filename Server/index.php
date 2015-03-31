<?php

class Server {
	private $curl = null;
	private $config = null;
	private $routes = null;
	public function init(){

		$this->getConfig();
		$this->getRoutes();
		$this->setProxy();
		$routes = $this->getURLRoutes();
		$this->checkRoute($routes);
	}
	private function getConfig(){
		$configString = file_get_contents('config.json');
		$this->config = json_decode(stripslashes($configString),true);

	}
	private function getRoutes(){
		$routesString = file_get_contents('routes.json');
		$this->routes = json_decode(stripslashes($routesString),true);
	}
	private function setProxy(){
		$this->curl = curl_init();
		// $userAgent = 'Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.1.4322)';
		// curl_setopt($this->curl, CURLOPT_USERAGENT, $userAgent);
		// curl_setopt($this->curl, CURLOPT_PROXY,  $this->config['proxy']['url']);
		// curl_setopt($this->curl, CURLOPT_PROXYPORT,   $this->config['proxy']['port']);
	}
	private function getURLRoutes(){
		$requestURI = explode('phprouter/', $_SERVER['REQUEST_URI']);
		//$requestURI = explode('/',$requestURI[1] );
		//echo implode("/",$requestURI);
		return $requestURI[1];
	}
	private function checkRoute($routes){
		switch($routes){
			case strpos($routes,'service/post/'):
			$this->post(explode("service/post/",$routes)[1]);
			break;
			case strpos($routes,'service/put/'):
			$this->post(explode("service/put/",$routes)[1],'PUT');
			break;
			case strpos($routes,'service/delete/'):
			$this->post(explode("service/delete/",$routes)[1],'DELETE');
			break;
			case strpos($routes,'service/get/'):
			$this->get(explode("service/get/",$routes)[1]);
			break;
		}
	}
	private function getRouteName($url){
		$name = explode('/', $url);
		return $name[0];
	}

	private function post($url,$method=null){
		$serviceUrl = $url;
		$routeName = $this->getRouteName($url);
		if($this->routes[$routeName])
		{
			$arr = explode('_|_'.$routeName.'/', '_|_'.$url);
			$serviceUrl = $this->routes[$routeName]['url'].(sizeof($arr)>1?$arr[1]:"");

		}else{

		}

		$timeout = 5;
		curl_setopt($this->curl, CURLOPT_URL, $serviceUrl);
		if($method)curl_setopt($this->curl, CURLOPT_CUSTOMREQUEST, $method);
		curl_setopt($this->curl, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($this->curl, CURLOPT_CONNECTTIMEOUT, $timeout);
		curl_setopt($this->curl, CURLOPT_FOLLOWLOCATION, true);
		curl_setopt($this->curl, CURLOPT_HEADER, 1);
		curl_setopt($this->curl, CURLOPT_POST, true);
		 curl_setopt($this->curl, CURLOPT_POSTFIELDS, $_POST);
		$data = curl_exec($this->curl);
		curl_close($this->curl);
		echo $data;
	}
	private function get($url){
		$serviceUrl = $url;
		$routeName = $this->getRouteName($url);
		if($this->routes[$routeName])
		{
			$arr = explode('_|_'.$routeName.'/', '_|_'.$url);
			$serviceUrl = $this->routes[$routeName]['url'].(sizeof($arr)>1?$arr[1]:"");

		}else{

		}

		$timeout = 5;
		curl_setopt($this->curl, CURLOPT_URL, $serviceUrl);
		curl_setopt($this->curl, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($this->curl, CURLOPT_CONNECTTIMEOUT, $timeout);
		curl_setopt($this->curl, CURLOPT_FOLLOWLOCATION, true);
		$data = curl_exec($this->curl);
		curl_close($this->curl);
		echo $data;
	}
}

$server = new Server();
$server->init();

?>