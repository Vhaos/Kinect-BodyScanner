<?php

class ResponseJson
{
    protected $data;

    public function __construct($data)
    {
        $this->data = $data;
        return $this;
    }

    public function render()
    {
        header('Content-Type: application/json');
        return json_encode($this->data);
    }
}