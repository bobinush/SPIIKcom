// Table Row Clickable - första länken i raden.
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
	setTimeout(() => $("#success").fadeOut(500), 1000);
}

// FadeOut alertboxes
setTimeout(() => $(".alert").fadeOut(500), 1000);

// upload profile picture
function readURL(input, previewId, spinner) {
	if (input.files) {
		var count = 0;
		var selectorSrc = ".upload-picture-preview-" + previewId;
		$(selectorSrc).empty();
		for (var i = 0; i < input.files.length; i++) {
			var reader = new FileReader();
			reader.onload = function (e) {
				// This event is triggered each time the reading operation is successfully completed.
				$(selectorSrc).append('<img src="' + e.target.result + '" />');
				count++;
				if (count === input.files.length) {
					spinner.removeClass('spinner');
				}
			}
			reader.readAsDataURL(input.files[i]);
		}
	}
}

// upload profile picture
$(".upload-picture").change(function () {
	var previewId = $(this).attr("data-preview");
	var spinner = $('#spinner');
	spinner.addClass('spinner');
	readURL(this, previewId, spinner);
});