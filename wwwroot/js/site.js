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
function readURL(input) {
	if (input.files && input.files[0]) {
		var reader = new FileReader();
		reader.onload = function (e) {
			$('#profile-picture').attr('src', e.target.result);
			$('#spinner').removeClass('spinner');
		}
		reader.readAsDataURL(input.files[0]);
	}
}

// upload profile picture
$("#Picture").change(function () {
	$('#spinner').addClass('spinner');
	readURL(this);
});

// Make whole social media post clickable
$(".social-media-post").click(function () {
	window.open($(this).data("href"));
	return false;
});