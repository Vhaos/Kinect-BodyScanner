$(document).ready(function()    {
    $("#my-menu").hide();
    $("#design2").hide();
    $("#design3").hide();
    $("#menu-icon").click(function()  {
        $("#my-menu").fadeToggle();
    });
    
    $("#design1Tab").click(function()  {
        $("#design1").show();
        if($("#design2").is(":visible")) {
            $("#design2").hide();
        }
        if($("#design3").is(":visible")) {
            $("#design3").hide();
        }
        $("#design1Tab").addClass("my-active");
        $("#design2Tab").removeClass("my-active");
        $("#design3Tab").removeClass("my-active");
    });
    $("#design2Tab").click(function()  {
        $("#design2").show();
        if($("#design1").is(":visible")) {
            $("#design1").hide();
        }
        if($("#design3").is(":visible")) {
            $("#design3").hide();
        }
        $("#design2Tab").addClass("my-active");
        $("#design1Tab").removeClass("my-active");
        $("#design3Tab").removeClass("my-active");
    });
    $("#design3Tab").click(function()  {
        $("#design3").show();
        if($("#design1").is(":visible")) {
            $("#design1").hide();
        }
        if($("#design2").is(":visible")) {
            $("#design2").hide();
        }
        $("#design3Tab").addClass("my-active");
        $("#design1Tab").removeClass("my-active");
        $("#design2Tab").removeClass("my-active");
    });
});
