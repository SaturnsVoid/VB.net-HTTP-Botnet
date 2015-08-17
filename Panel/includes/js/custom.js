//Timer for messages and tasks
var i = 12, j=35;

function incrementI() {
  i++;  
  document.getElementById('newSignup').innerHTML = i;
}
setInterval('incrementI()', 25000);

function incrementJ() {
  j++;
  document.getElementById('messagesCountDown').innerHTML = j;
}
setInterval('incrementJ()', 12000);


//Tooltip
$('a').tooltip('hide');

//Popover
$('.popover-pop').popover('hide');

//Collapse
$('#myCollapsible').collapse({
  toggle: false
})

//Dropdown
$('.dropdown-toggle').dropdown();


// Retina Mode
function retina(){
  retinaMode = (window.devicePixelRatio > 1);
  return retinaMode;
}


