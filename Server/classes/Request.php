<?php

class Request
{
    /*
     * URL elements.
     */

    public $url_elements = array();
    
    /*
     *  The HTTP method used.
     */

    public $method;
    
    /*
     *  Any parameters sent with the request.
     */

    public $parameters;
}
