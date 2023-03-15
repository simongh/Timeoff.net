$(document).ready(function () {
  $('button.bankholiday-remove-btn').on('click', function(e){

    e.stopPropagation();

    $('#deleteid').attr('value', $(this).attr('value'));

    var delete_form = $('#delete_bankholiday_form');
    delete_form.submit();

    return false;
  });

  $('#bankholiday-import-btn').on('click', function(e){

    e.stopPropagation();

    var import_form = $('#import_bankholiday_form');

    import_form.submit();

    return false;
  });
});
