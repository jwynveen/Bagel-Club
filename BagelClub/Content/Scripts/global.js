//TODO: Check to make sure jQuery has been loaded
if (typeof jQuery != undefined) {
	try {
		(function ($) {
			// add icon elements .icon-*
			$('[class^="icon-"]').each(function (i, v) {
				$('<span>', {
					"class": 'icon',
					text: "icon"
				}).prependTo(v);
			});

			// shopping list accordion
			$('#shopping-list .item-list ul').hide();
			$('#shopping-list h3').bind("click", function () {
				var $this = $(this);
				// TODO: prevent event duplication
				// TODO: check to see if the LI has a UL to slide
				if (!$this.parent('li').hasClass('open')) {
					$this.next('ul').slideDown("fast", function () {
						//callback
					}).parent('li').addClass('open').siblings('.open').removeClass('open').children('ul').slideUp("fast");
				} else {
					$this.next('ul').slideUp("fast", function () {
						//callback
					}).parent('li').removeClass('open');
				}
			})

			// btn utilities
			$('.btn-print').bind("click", function (e) {
				window.print();
				e.preventDefault();
			});

			// bagels are here
			$('#bagel-here-form .btn-here').bind("click", function () {
				$(this).hide().next('fieldset').show().children('#bagel-location').focus();
			});
			$('button[type="reset"]').bind("click", function () {
				$(this).parent('fieldset').hide().siblings('button').show();
			});
		})(jQuery);
	} catch (err) {
		alert(err);
	}
}