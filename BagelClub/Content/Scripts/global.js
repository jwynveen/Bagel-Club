//TODO: Check to make sure jQuery has been loaded

(function ($) {
	$('#shopping-list ul').hide();
	$('.btn-print').bind("click", function (e) {
		window.print();
		e.preventDefault();
	});
})(jQuery);