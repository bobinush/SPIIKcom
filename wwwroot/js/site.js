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

// Kontakform
function success() {
	// töm inputs
	$("form input").each(function () {
		$(this).val("");
	});
	$("form textarea").val("");

	// Checkkrysset bredvid knappen
	$("#success").show();
	setTimeout(() => $("#success").fadeOut(1000), 1000);
}

// FadeOut alertboxes
setTimeout(() => $(".alert").fadeOut(500), 2000);