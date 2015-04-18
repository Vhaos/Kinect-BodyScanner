## Summary
Basic RESTful API server Kinect Bodyscanner written in PHP. Allows you to create a web service where data can be accessed in a simple URL format such as 
`http://localhost/[resource]`.

## Installation
Simply deploy directory files onto your local web server. Controllers and accepted endpoints can be found in the `classes/controllers/` directory. First I would recommend downloading and installing [Xampp](https://www.apachefriends.org) for Windows, an easy to install Apache and PHP distribution; this can be found [here](https://www.apachefriends.org/download.html). Full installation instructions can be found below.
	
To install:
 * Install Xampp to C:\ folder
 * Place the folder "***Server***" into C:\xammp\htdocs
 * Start up Xampp then start up Apache on it's control panel
 * Open up your browser and go to `localhost/server/test`
 * Bodyscanner server is now installed :+1:


For the app to work, you must:
 * Open Xampp Control Panel
 * Click Config on the Apache row
 * Click Apache (httpd-xampp.conf)
 * Comment out "Require Local" at the end of the file.

## Development

Assuming you are using Google Chrome, you may also wish to get the REST Console Chrome App, available from the Chrome web store [here](http://bit.ly/1k9zsGu). Full description of the available API calls are displayed below.

**Endpoint definition**
```
GET http://localhost/server/test
```
Used for testing purposes, and see if the server is responding at all  

**Response Body** PlainText
```
"Test Passed, dis API be sick"
```			
___	
  
**Endpoint definition**  
```
GET http://localhost/server/newID
```
Generate a new unique ID

**Response Body** PlainText
```
"5522e70fe6cb4"
```	

___

**Endpoint definition**  
```
POST http://localhost/server/newID/5522e70fe6cb4
```
Assuming you've gotten your id, creates a folder for your id on server

**Response Body** PlainText
```
"just created 5522e70fe6cb4 folder"
```	
___

**Endpoint definition**  
```
POST http://localhost/server/ProcessPC/5522e70fe6cb4/F
```
Assuming pointcloud.xyz was saved to server, and now want the server to start processing it. Specify F or M for gender in url.

**Response Body** PlainText
```
"OK, started processing pointcloud"
```	
___

**Endpoint definition**  
```
GET http://localhost/server/check/5522e70fe6cb4
```
Check whether point cloud has finished processing, and sends back results if it has:

**Response Body** JSON
```json
{
    "file": {
        "@attributes": {
            "name": "5522e70fe6cb4\/\/pointcloud"
        }
    },
    "measure": ["147.529", "105.469", "94.8168", "81.6679", "75.0965"]
}
```
