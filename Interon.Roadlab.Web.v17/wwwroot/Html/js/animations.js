$(document).ready(function() {
    handleSlideRight();
    handleBenefits();
    handleFeatures();
    handleProducts();
    handleScrollEvents();
    handleGallery();
    handleParallax();
	handleOwlCarousel();
});
function handleOwlCarousel() {
 
    var owl = $('.owl-carousel');
    owl.owlCarousel({
        items: 4,
        loop: true,
        margin: 10,
        autoplay: true,
        autoplayTimeout: 1000,
        autoplayHoverPause: true
    });
	
}
function handleSlideRight() {
    $('.from-right').click(function() {
        $(this).addClass("slideRight");
    });
}
function showSlides(n) {
    var i;
    var slides = document.getElementsByClassName("mySlides");
    if (!slides || slides.length === 0) {
        return;
    }
    var dots = document.getElementsByClassName("demo");
    var captionText = document.getElementById("caption");
    if (n > slides.length) {n = 1}
    if (n < 1) {n = slides.length}
    for (i = 0; i < slides.length; i++) {
        slides[i].style.display = "none";  
    }
    for (i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(" active", "");
    }
    slides[n-1].style.display = "block";  
    dots[n-1].className += " active";
    captionText.innerHTML = dots[n-1].alt;
}
function handleBenefits() {
    $('.benefits-blocks__img').click(function() {
        $(this).addClass("fadeInUp");
    });
}

function handleFeatures() {
    $('.features__img').click(function() {
        $(this).addClass("fadeInUp");
    });
}

function handleProducts() {
    $('.products__hr').click(function() {
        $(this).addClass("slideRight");
    });
}

function handleScrollEvents() {
    $(window).scroll(function() {
        addClassOnScroll('.from-right', 'slideRight', 400);
        addClassOnScroll('.benefits-blocks__img', 'fadeInUp', 700);
        addClassOnScroll('.features__img', 'fadeInUp', 500);
        addClassOnScroll('.products__hr', 'slideRight', 700);
    });
}

function handleGallery() {
    var slideIndex = 1;
    showSlides(slideIndex);
}

function handleParallax() {
    const elem = document.querySelector("#parallax");
    if (elem) {
        document.addEventListener("mousemove", function(e) {
            parallax(e, elem);
        });
    }
}