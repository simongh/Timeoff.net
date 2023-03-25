$(document).ready(function () {
	$('button.pubicholiday-remove-btn').on('click', function (e) {
		e.stopPropagation();

		$('#deleteid').attr('value', $(this).attr('value'));

		var delete_form = $('#delete_publicholiday_form');
		delete_form.submit();

		return false;
	});

	$('#publicholiday-import-btn').on('click', function (e) {
		e.stopPropagation();

		var import_form = $('#import_publicholiday_form');

		import_form.submit();

		return false;
	});
});