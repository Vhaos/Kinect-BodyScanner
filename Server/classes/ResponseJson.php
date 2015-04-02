<?php

class ResponseJson
{
    /*
     * Response data.
     */

    protected $data;

    /*
     * Constructor.
     */

    public function __construct($data)
    {
        $this->data = $data;
        return $this;
    }

    /*
     * Render the response as JSON.
     */

    public function render()
    {
        header('Content-Type: application/json');
        return json_encode($this->data);
    }
}