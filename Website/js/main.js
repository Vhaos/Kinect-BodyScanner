$(document).ready(function()    {
    $("#my-menu").hide();
    $("#menu-icon").click(function()  {
        $("#my-menu").fadeToggle();
        console.log("click");
    });
});
