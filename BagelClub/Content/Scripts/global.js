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
			});

			$('#shopping-list select[name="list-type"]').bind("change", function () {
				var $this = $(this);
				$this.parent('section').children('fieldset').slideUp("fast", function () {
					$(this).removeClass('hide');
				}).siblings('fieldset.' + $this.val()).slideDown("fast");
			});

			$('#shopping-list select[name="bagel-shop-type"]').bind("change", function () {
				var $this = $(this);
				$('#shopping-list ul.item-list:visible li.shop').slideUp("fast", function () {
					//callback
				});
				$('#shopping-list ul.item-list:hidden li.shop').hide();

				$('#shopping-list ul.item-list:visible li.shop' + $this.val()).slideDown("fast");
				$('#shopping-list ul.item-list:hidden li.shop' + $this.val()).show();

				if ($this.val() != '')
					$('#shopping-list li.shop' + $this.val() + ' h3').click();
			});

			// btn utilities
			$('.btn-print').bind("click", function (e) {
				window.print();
				e.preventDefault();
			});

			// bagels are here
			$('#bagel-here-form .btn-here').bind("click", function () {
				$(this).hide().next('fieldset').show().children('#bagel-location').focus();
			});

			$('#bagel-here-form').validate();

			$('button[type="reset"]').bind("click", function () {
				$(this).parent('fieldset').hide().siblings('button').show();
			});
		})(jQuery);
	} catch (err) {
		alert(err);
	}
}