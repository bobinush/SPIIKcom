// Table Row Clickable
// Ändrar muspekare och går till den första länken i raden.
$(".table-row-click tbody td").hover(function () {
	this.style.cursor = "pointer";
});
$('.table-row-click tbody tr').click(function () {
	var href = $('a:first', this).attr('href');
	if (href) {
		window.location = href;
	}
});