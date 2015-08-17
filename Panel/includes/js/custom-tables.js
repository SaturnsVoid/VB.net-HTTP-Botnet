// SparkLine Line Graphs
$(function () {
  $('#unique-visitors').sparkline('html', {
    type: 'line',
    fillColor: '#f9f9f9',
    lineColor: '#860e1c',
    height: 20,
  });
  $('#monthly-sales').sparkline('html', {
    type: 'line',
    fillColor: '#f9f9f9',
    lineColor: '#3eb157',
    height: 20,
  });
  $('#current-balance').sparkline('html', {
    type: 'line',
    fillColor: '#f9f9f9',
    lineColor: '#9564e2',
    height: 20,
  });
  $('#registrations').sparkline('html', {
    type: 'line',
    fillColor: '#f9f9f9',
    lineColor: '#3660aa',
    height: 20,
  });
  $('#site-visits').sparkline('html', {
    type: 'line',
    fillColor: '#f9f9f9',
    lineColor: '#333333',
    height: 20,
  });
});


// SparkLine Bar Graphs
$(function () {
  $('#ax').sparkline('html', {
    type: 'bar',
    barColor: '#f26645',
    barWidth: 4,
    height: 20,
  });
  $('#cx').sparkline('html', {
    type: 'bar',
    barColor: '#3eb157',
    barWidth: 4,
    height: 20,
  });
  $('#bx').sparkline('html', {
    type: 'bar',
    barColor: '#dba26b',
    barWidth: 4,
    height: 20,
  });
  $('#ex').sparkline('html', {
    type: 'bar',
    barColor: '#3660aa',
    barWidth: 4,
    height: 20,
  });
  $('#dx').sparkline('html', {
    type: 'bar',
    barColor: '#d14836',
    barWidth: 4,
    height: 20,
  });
});

//Data Tables
$(document).ready(function () {
  $('#data-table').dataTable({
    "sPaginationType": "full_numbers"
  });
});

jQuery('.delete-row').click(function () {
  var conf = confirm('Continue delete?');
  if (conf) jQuery(this).parents('tr').fadeOut(function () {
    jQuery(this).remove();
  });
    return false;
});