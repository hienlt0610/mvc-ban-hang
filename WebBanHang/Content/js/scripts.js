function addThousandsSeparator(input) {
    var output = input
    if (parseFloat(input)) {
        input = new String(input); // so you can perform string operations
        var parts = input.split("."); // remove the decimal part
        parts[0] = parts[0].split("").reverse().join("").replace(/(\d{3})(?!$)/g, "$1,").split("").reverse().join("");
        output = parts.join(".");
    }

    return output;
}
/*	OWL CAROUSEL
/*===================================================================================*/
$(document).ready(function () {
    var dragging = true;
    var owlElementID = "#owl-main";

    function fadeInReset() {
        if (!dragging) {
            $(owlElementID + " .caption .fadeIn-1, " + owlElementID + " .caption .fadeIn-2, " + owlElementID + " .caption .fadeIn-3").stop().delay(800).animate({ opacity: 0 }, { duration: 400, easing: "easeInCubic" });
        }
        else {
            $(owlElementID + " .caption .fadeIn-1, " + owlElementID + " .caption .fadeIn-2, " + owlElementID + " .caption .fadeIn-3").css({ opacity: 0 });
        }
    }

    function fadeInDownReset() {
        if (!dragging) {
            $(owlElementID + " .caption .fadeInDown-1, " + owlElementID + " .caption .fadeInDown-2, " + owlElementID + " .caption .fadeInDown-3").stop().delay(800).animate({ opacity: 0, top: "-15px" }, { duration: 400, easing: "easeInCubic" });
        }
        else {
            $(owlElementID + " .caption .fadeInDown-1, " + owlElementID + " .caption .fadeInDown-2, " + owlElementID + " .caption .fadeInDown-3").css({ opacity: 0, top: "-15px" });
        }
    }

    function fadeInUpReset() {
        if (!dragging) {
            $(owlElementID + " .caption .fadeInUp-1, " + owlElementID + " .caption .fadeInUp-2, " + owlElementID + " .caption .fadeInUp-3").stop().delay(800).animate({ opacity: 0, top: "15px" }, { duration: 400, easing: "easeInCubic" });
        }
        else {
            $(owlElementID + " .caption .fadeInUp-1, " + owlElementID + " .caption .fadeInUp-2, " + owlElementID + " .caption .fadeInUp-3").css({ opacity: 0, top: "15px" });
        }
    }

    function fadeInLeftReset() {
        if (!dragging) {
            $(owlElementID + " .caption .fadeInLeft-1, " + owlElementID + " .caption .fadeInLeft-2, " + owlElementID + " .caption .fadeInLeft-3").stop().delay(800).animate({ opacity: 0, left: "15px" }, { duration: 400, easing: "easeInCubic" });
        }
        else {
            $(owlElementID + " .caption .fadeInLeft-1, " + owlElementID + " .caption .fadeInLeft-2, " + owlElementID + " .caption .fadeInLeft-3").css({ opacity: 0, left: "15px" });
        }
    }

    function fadeInRightReset() {
        if (!dragging) {
            $(owlElementID + " .caption .fadeInRight-1, " + owlElementID + " .caption .fadeInRight-2, " + owlElementID + " .caption .fadeInRight-3").stop().delay(800).animate({ opacity: 0, left: "-15px" }, { duration: 400, easing: "easeInCubic" });
        }
        else {
            $(owlElementID + " .caption .fadeInRight-1, " + owlElementID + " .caption .fadeInRight-2, " + owlElementID + " .caption .fadeInRight-3").css({ opacity: 0, left: "-15px" });
        }
    }

    function fadeIn() {
        $(owlElementID + " .active .caption .fadeIn-1").stop().delay(500).animate({ opacity: 1 }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeIn-2").stop().delay(700).animate({ opacity: 1 }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeIn-3").stop().delay(1000).animate({ opacity: 1 }, { duration: 800, easing: "easeOutCubic" });
    }

    function fadeInDown() {
        $(owlElementID + " .active .caption .fadeInDown-1").stop().delay(500).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeInDown-2").stop().delay(700).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeInDown-3").stop().delay(1000).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
    }

    function fadeInUp() {
        $(owlElementID + " .active .caption .fadeInUp-1").stop().delay(500).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeInUp-2").stop().delay(700).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeInUp-3").stop().delay(1000).animate({ opacity: 1, top: "0" }, { duration: 800, easing: "easeOutCubic" });
    }

    function fadeInLeft() {
        $(owlElementID + " .active .caption .fadeInLeft-1").stop().delay(500).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeInLeft-2").stop().delay(700).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeInLeft-3").stop().delay(1000).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
    }

    function fadeInRight() {
        $(owlElementID + " .active .caption .fadeInRight-1").stop().delay(500).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeInRight-2").stop().delay(700).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
        $(owlElementID + " .active .caption .fadeInRight-3").stop().delay(1000).animate({ opacity: 1, left: "0" }, { duration: 800, easing: "easeOutCubic" });
    }

    $(owlElementID).owlCarousel({

        autoPlay: 5000,
        stopOnHover: true,
        navigation: true,
        pagination: true,
        singleItem: true,
        addClassActive: true,
        transitionStyle: "fade",
        navigationText: ["<i class='icon fa fa-angle-left'></i>", "<i class='icon fa fa-angle-right'></i>"],

        afterInit: function() {
            fadeIn();
            fadeInDown();
            fadeInUp();
            fadeInLeft();
            fadeInRight();
        },

        afterMove: function() {
            fadeIn();
            fadeInDown();
            fadeInUp();
            fadeInLeft();
            fadeInRight();
        },

        afterUpdate: function() {
            fadeIn();
            fadeInDown();
            fadeInUp();
            fadeInLeft();
            fadeInRight();
        },

        startDragging: function() {
            dragging = true;
        },

        afterAction: function() {
            fadeInReset();
            fadeInDownReset();
            fadeInUpReset();
            fadeInLeftReset();
            fadeInRightReset();
            dragging = false;
        }

    });

if ($(owlElementID).hasClass("owl-one-item")) {
    $(owlElementID + ".owl-one-item").data('owlCarousel').destroy();
}

$(owlElementID + ".owl-one-item").owlCarousel({
    singleItem: true,
    navigation: false,
    pagination: false
});

$('#transitionType li a').click(function () {

    $('#transitionType li a').removeClass('active');
    $(this).addClass('active');

    var newValue = $(this).attr('data-transition-type');

    $(owlElementID).data("owlCarousel").transitionTypes(newValue);
    $(owlElementID).trigger("owl.next");

    return false;

});


$('.home-owl-carousel').each(function(){

    var owl = $(this);
    var  itemPerLine = owl.data('item');
    if(!itemPerLine){
        itemPerLine = 4;
    }
    owl.owlCarousel({
        items : itemPerLine,
        itemsTablet:[768,2],
        navigation : true,
        pagination : false,

        navigationText: ["", ""]
    });
});

$('.homepage-owl-carousel').each(function(){

    var owl = $(this);
    var  itemPerLine = owl.data('item');
    if(!itemPerLine){
        itemPerLine = 4;
    }
    owl.owlCarousel({
        items : itemPerLine,
        itemsTablet:[768,2],
        itemsDesktop : [1199,2],
        navigation : true,
        pagination : false,

        navigationText: ["", ""]
    });
});

$(".blog-slider").owlCarousel({
    items : 3,
    itemsDesktopSmall :[979,2],
    itemsDesktop : [1199,2],
    navigation : true,
    slideSpeed : 300,
    pagination: false,
    navigationText: ["", ""]
});

$(".best-seller").owlCarousel({
    items : 3,
    navigation : true,
    itemsDesktopSmall :[979,2],
    itemsDesktop : [1199,2],
    slideSpeed : 300,
    pagination: false,
    paginationSpeed : 400,
    navigationText: ["", ""]
});

$(".sidebar-carousel").owlCarousel({
    items : 1,
    itemsTablet:[768,2],
    itemsDesktopSmall :[979,2],
    itemsDesktop : [1199,1],
    navigation : true,
    slideSpeed : 300,
    pagination: false,
    paginationSpeed : 400,
    navigationText: ["", ""]
});

$(".brand-slider").owlCarousel({
    items : 6,
    navigation : true,
    slideSpeed : 300,
    pagination: false,
    paginationSpeed : 400,
    navigationText: ["", ""]
});    
$("#advertisement").owlCarousel({
    items : 1,
    itemsDesktopSmall :[979,2],
    itemsDesktop : [1199,1],
    navigation : true,
    slideSpeed : 300,
    pagination: true,
    paginationSpeed : 400,
    navigationText: ["", ""]
});    

var $owl_controls_custom = $('.owl-controls-custom');
$('.owl-next' , $owl_controls_custom).click(function(event){
    var selector = $(this).data('owl-selector');
    var owl = $(selector).data('owlCarousel');
    owl.next();
    return false;
});
$('.owl-prev' , $owl_controls_custom).click(function(event){
    var selector = $(this).data('owl-selector');
    var owl = $(selector).data('owlCarousel');
    owl.prev();
    return false;
});

$(".owl-next").click(function(){
    $($(this).data('target')).trigger('owl.next');
    return false;
});

$(".owl-prev").click(function(){
    $($(this).data('target')).trigger('owl.prev');
    return false;
});

});

/*===================================================================================*/
/*  LAZY LOAD IMAGES USING ECHO
/*===================================================================================*/
$(document).ready(function(){
    echo.init({
        offset: 100,
        throttle: 250,
        unload: false
    });
});

/*===================================================================================*/
/*  RATING
/*===================================================================================*/

$(document).ready(function(){
    $('.rating').rateit({max: 5, step: 1, value : 4, resetable : false , readonly : true});
});


/*===================================================================================*/
/* SINGLE PRODUCT GALLERY
/*===================================================================================*/
$(document).ready(function(){
    $('#owl-single-product').owlCarousel({
        items:1,
        itemsTablet:[768,2],
        itemsDesktop : [1199,1]

    });

    $('#owl-single-product-thumbnails').owlCarousel({
        items: 4,
        pagination: true,
        rewindNav: true,
        itemsTablet : [768, 4],
        itemsDesktop : [1199,3]
    });

    $('#owl-single-product2-thumbnails').owlCarousel({
        items: 6,
        pagination: true,
        rewindNav: true,
        itemsTablet : [768, 4],
        itemsDesktop : [1199,3]
    });

    $('.single-product-slider').owlCarousel({
        stopOnHover: true,
        rewindNav: true,
        singleItem: true,
        pagination: true
    });

    $(".slider-next").click(function () {
        var owl = $($(this).data('target'));
        owl.trigger('owl.next');
        return false;
    });

    $(".slider-prev").click(function () {
        var owl = $($(this).data('target'));
        owl.trigger('owl.prev');
        return false;
    });

    $('.single-product-gallery .horizontal-thumb').click(function(){
        var $this = $(this), owl = $($this.data('target')), slideTo = $this.data('slide');
        owl.trigger('owl.goTo', slideTo);
        $this.addClass('active').parent().siblings().find('.active').removeClass('active');
        return false;
    });
});


/*===================================================================================*/
/*  QUANTITY
/*===================================================================================*/

$('.quant-input .plus').click(function() {
    var val = $(this).parent().next().val();
    val = parseInt(val) + 1;
    $(this).parent().next().val(val);
});
$('.quant-input .minus').click(function() {
    var val = $(this).parent().next().val();
    if (val > 0) {
        val = parseInt(val) - 1;
        $(this).parent().next().val(val);
    }
});


/*===================================================================================*/
/*  WOW 
/*===================================================================================*/

$(document).ready(function () {
    new WOW().init();
});


/*===================================================================================*/
/*  TOOLTIP 
/*===================================================================================*/
$("[data-toggle='tooltip']").tooltip(); 

$('#transitionType li a').click(function () {

    $('#transitionType li a').removeClass('active');
    $(this).addClass('active');

    var newValue = $(this).attr('data-transition-type');

    $(owlElementID).data("owlCarousel").transitionTypes(newValue);
    $(owlElementID).trigger("owl.next");

    return false;

});

var lastId;
var lastBtn;
$("a.add-cart").on('click', function (event) {
    lastBtn = $(this);
    var id = $(this).data("id");
    lastId = id;
    $.getJSON("/Cart/GetListColor/" + id, function (data) {
        var items = [];
        if (data.status == "OK") {
            if (parseInt(data.count) > 0) {
                $.each(data.colors, function (key, val) {
                    items.push("<option value='" + val.color_id + "' id='color" + key + "'>" + val.color_name + "</option>");
                });
                $("#list_color").html(items.join(""));
                $("#myModal").modal("show");
            } else {
                add_to_cart(id, null);
            }
        }
    });
    return false;
});

$('#btn_add').on('click', function (e) {
    e.stopPropagation();
    add_to_cart(lastId, $("#list_color").val());
    $("#myModal").modal("hide");
});

function add_to_cart(productid, colorid) {
    if (colorid == null) {
        $.post("/Cart/AddCart", { id: productid, quantity: 1 }, function (data) {
            Msg.success("Thêm giỏ hàng thành công", 1500);
            $("#cart_shop").html(data);
        });
    } else {
        $.post("/Cart/AddCart", { id: productid, color: colorid,quantity:1 }, function (data) {
            Msg.success("Thêm giỏ hàng thành công", 1500);
            $("#cart_shop").html(data);
        });
    }
}

    //Xóa giỏ hàng
$('body').on('click', '.remove', function () {
    var productId = $(this).data('id');
    var colorId = $(this).data('color');
    if (colorId == 0) {
        $.post("/Cart/RemoveCart", { id: productId }, function (data) {
            $("#cart_shop").html(data);
        });
    } else {
        $.post("/Cart/RemoveCart", { id: productId, color: colorId }, function (data) {
            $("#cart_shop").html(data);
        });
    }
});

    //chỉ cho phép nhập số
$('.number').keypress(function (event) {
    if (event.which < 46 || event.which >= 58 || event.which == 47) {
        event.preventDefault();
    }

    if (event.which == 46 && $(this).val().indexOf('.') != -1) {
        this.value = '';
    }
});

    //Tắt focus vào button khi click
$(".btn").click(function (event) {
    // Removes focus of the button.
    $(this).blur();
});
