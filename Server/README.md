## Summary
Basic RESTful API server Kinect Bodyscanner written in PHP. Allows you to create a web service where data can be accessed in a simple URL format such as 
`http://localhost/[resource]`.

## Installation
Simply deploy directory files onto your local web server. Controllers and accepted endpoints can be found in the `classes/controllers/` directory. First I would recommend downloading and installing [Xampp](https://www.apachefriends.org) for Windows, an easy to install Apache and PHP distribution; this can be found [here](https://www.apachefriends.org/download.html). Full installation instructions can be found below.
	
To install:
 * Install Xampp to C:\ folder
 * Place the folder "***Server***" into C:\xammp\htdocs
 * Open Xampp Control Panel
 * Click Apache Start
 * Open up your browser and go to `localhost/server/test`
 * Bodyscanner server is now installed :+1:

To Configure the ftp client
 * Open Xampp Control Panel
 * Click Config on the Apache row
 * Start the FileZilla module
 * Click Admin on the FileZilla row
 * Click ok (no administration password is needed)
 * Go to Edit > Users > General > Add
 * Name this user "anonymous", click OK
 * Go to the Shared Folders page
 * Add new Shared directory for anonymous, this will be the "C:\xampp\htdocs\server\scans" folder
 * Give this directory Read, write, delete and append permissions
 * ftp client is now configured 

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
GET http://localhost/server/ProcessPC/5522e70fe6cb4/F
```
Assuming pointcloud.xyz was saved to server, and now want the server to start processing it. Specify F or M for gender in url.

**Response Body** PlainText
```
"OK, started processing pointcloud"
```	
___

**Endpoint definition**  
```
PUT http://localhost/server/ProcessPC/5522e70fe6cb4
```
Upload local pointcloud file to the server. Must specify path to the local file in request body.

**Example Request Body** JSON
```
{"file":"C:\\Users\\Kareem\\Documents\\Test\\pointcloud.xyz"}
```	

**Response Body** PlainText
```
"successfully uploaded C:\Users\Kareem\Documents\Test\pointcloud.xyz"
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
