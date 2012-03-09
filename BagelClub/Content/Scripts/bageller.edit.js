$(function () {
	$("form").validate({
		submitHandler: function (form) {
			alert("submit");
			form.submit();
		}
	});
});
var Bageller = {
	NextIndex: 0,
	ListSelector: '#choice-list',
	Add: function (templateSelector) {
		var controls = $(templateSelector).html().replace(/{index}/g, '[' + this.NextIndex + ']');
		var element = $('<li class="ui-state-default">' + controls + '</li>').appendTo(this.ListSelector);
		jQuery.validator.unobtrusive.parse('form');
		this.NextIndex++;
	},
	Remove: function (obj) {
		$(obj).parent().remove();
		this.RefreshIndices();
	},
	RefreshIndices: function () {
		var i = 0;
		$(this.ListSelector + ' input').each(function () {
			$(this).attr('name', 'Item.Choices[' + i + '].Bagel');
			i++;
		});
		i = 0;
		$(this.ListSelector + ' select').each(function () {
			$(this).attr('name', 'Item.Choices[' + i + '].Location.LocationId');
			i++;
		});
		this.NextIndex = i;
	}
}