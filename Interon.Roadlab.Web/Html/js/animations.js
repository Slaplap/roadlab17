// slide right 
$('.from-right').click(function() {
	$(this).addClass("slideRight");
});
$(window).scroll(function() {
	$('.from-right').each(function() {
		var imagePos = $(this).offset().top;
		var topOfWindow = $(window).scrollTop();
		if (imagePos < topOfWindow + 400) {
			$(this).addClass("slideRight");
		}
	});
});
// slide right end
// BENEFITS 
$('.benefits-blocks__img').click(function() {
	$(this).addClass("fadeInUp");
});
$(window).scroll(function() {
	$('.benefits-blocks__img').each(function() {
		var imagePos = $(this).offset().top;
		var topOfWindow = $(window).scrollTop();
		if (imagePos < topOfWindow + 700) {
			$(this).addClass("fadeInUp");
		}
	});
});
// FEATURES
$('.features__img').click(function() { // add the class of the element
	$(this).addClass("fadeInUp"); // add the animation effect
});
$(window).scroll(function() {
	$('.features__img').each(function() { // add the class of the element
		var imagePos = $(this).offset().top;
		var topOfWindow = $(window).scrollTop();
		if (imagePos < topOfWindow + 500) { // add the when position on screen
			$(this).addClass("fadeInUp"); // add the animation effect
		}
	});
});
// PRODUCTS
//HR
$('.products__hr').click(function() { // add the class of the element
	$(this).addClass("slideRight"); // add the animation effect
});
$(window).scroll(function() {
	$('.products__hr').each(function() { // add the class of the element
		var imagePos = $(this).offset().top;
		var topOfWindow = $(window).scrollTop();
		if (imagePos < topOfWindow + 700) { // add the when position on screen
			$(this).addClass("slideRight"); // add the animation effect
		}
	});
});
//--------------------------------------------------------------------------------------------
// gallery
var slideIndex = 1;
showSlides(slideIndex);

function plusSlides(n) {
	showSlides(slideIndex += n);
}

function currentSlide(n) {
	showSlides(slideIndex = n);
}

function showSlides(n) {
	var i;
	var slides = document.getElementsByClassName("mySlides");
	var dots = document.getElementsByClassName("demo");
	var captionText = document.getElementById("caption");
	if (n > slides.length) {
		slideIndex = 1
	}
	if (n < 1) {
		slideIndex = slides.length
	}
	for (i = 0; i < slides.length; i++) {
		slides[i].style.display = "none";
	}
	for (i = 0; i < dots.length; i++) {
		dots[i].className = dots[i].className.replace(" active", "");
	}
	slides[slideIndex - 1].style.display = "block";
	dots[slideIndex - 1].className += " active";
	captionText.innerHTML = dots[slideIndex - 1].alt;
}
// gallery end		
// parallax 
(function() {
	// Add event listener
	document.addEventListener("mousemove", parallax);
	const elem = document.querySelector("#parallax");
	// Magic happens here
	function parallax(e) {
		let _w = window.innerWidth / 2;
		let _h = window.innerHeight / 2;
		let _mouseX = e.clientX;
		let _mouseY = e.clientY;
		let _depth1 = `${50 - (_mouseX - _w) * 0.01}% ${50 - (_mouseY - _h) * 0.01}%`;
		let _depth2 = `${50 - (_mouseX - _w) * 0.02}% ${50 - (_mouseY - _h) * 0.02}%`;
		let _depth3 = `${50 - (_mouseX - _w) * 0.06}% ${50 - (_mouseY - _h) * 0.06}%`;
		let x = `${_depth3}, ${_depth2}, ${_depth1}`;
		console.log(x);
		elem.style.backgroundPosition = x;
	}
})();

