# Summary
Basic RESTful API server Kinect Bodyscanner written in PHP. Allows you to create a web service where data can be accessed in a simple URL format such as 
**http://localhost/server/[resource]/**.

# Installation
Simply deploy this directory file "server" onto your web server. Controllers (endpoints) can be found in the **contro** 
generates a new tracking ID number based on server microtime and sends it back.llers/** directory.

# API

**endpoint** : http://localhost/phpresttest

**GET /newID** 
generates a new tracking ID number based on server microtime and sends it back. 

**POST /newID/"youridhere"** 
Send the server your ID number, creates a folder locally on the computer for you to save your pointcloud to and begins computing the pointcloud in ScanMeasure   