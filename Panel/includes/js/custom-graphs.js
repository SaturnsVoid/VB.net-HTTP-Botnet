// Flot Charts
var updateInterval = 60;

$("#updateInterval").val(updateInterval).change(function () {
  var v = $(this).val();
  if (v && !isNaN(+v)) {
    updateInterval = +v;
    if (updateInterval < 1)
      updateInterval = 1;
    if (updateInterval > 2000)
      updateInterval = 2000;
    $(this).val("" + updateInterval);
  }
});

var data = [], totalPoints = 200;
function getRandomData() {
  if (data.length > 0)
    data = data.slice(1);

  // do a random walk
  while (data.length < totalPoints) {
    var prev = data.length > 0 ? data[data.length - 1] : 90;
    var y = prev + Math.random() * 10 - 5;
    if (y < 0)
      y = 0;
    if (y > 100)
      y = 100;
    data.push(y);
  }

  // zip the generated y values with the x values
  var res = [];
  for (var i = 0; i < data.length; ++i)
    res.push([i, data[i]])
  return res;
}

if($("#serverLoad").length){
  var options = {
    series: { shadowSize: 1 },
    lines: { show: true, lineWidth: 3, fill: true, fillColor: { colors: [ { opacity: 0.5 }, { opacity: 0.5 } ] }},
    yaxis: { min: 0, max: 200, tickFormatter: function (v) { return v + "%"; }},
    xaxis: { show: false },
    colors: ["#3660aa"],
    grid: { tickColor: "#f2f2f2",
      borderWidth: 0, 
    },
  };
  var plot = $.plot($("#serverLoad"), [ getRandomData() ], options);
  function update() {
    plot.setData([ getRandomData() ]);
    // since the axes don't change, we don't need to call plot.setupGrid()
    plot.draw();
    setTimeout(update, updateInterval);
  }
  update();
}

if($("#realtimechart").length){
  var options = {
    series: { shadowSize: 1 },
    lines: { lineWidth: 1, fill: true, fillColor: { colors: [ { opacity: 1 }, { opacity: 0.1 } ] }},
    yaxis: { min: 0, max: 200 },
    xaxis: { show: false },
    colors: ["#3660aa"],
    grid: { 
      tickColor: "#eeeeee",
      borderWidth: 0 
    }
  };
  var plot = $.plot($("#realtimechart"), [ getRandomData() ], options);
  function update() {
    plot.setData([ getRandomData() ]);
    // since the axes don't change, we don't need to call plot.setupGrid()
    plot.draw();
    setTimeout(update, updateInterval);
  }
  update();
}

//Section Charts
$(function() {

  var data = [{
    label: "Facebook",
    data: [[1998, 19], [1999, 92], [2000, 335], [2001, 63], [2002, 226], [2003, 26], [2004, 296], [2005, 46], [2006, 310], [2007, 52], [2008, 344], [2009, 76], [2010, 68], [2011, 28], [2012, 50], [2013, 72]]
  },{
    label: "Twitter",
    data: [[1998, 20], [1999, 59], [2000, 217], [2001, 45], [2002, 168], [2003, 38], [2004, 348], [2005, 78], [2006, 272], [2007, 84], [2008, 236], [2009, 38], [2010, 60], [2011, 20], [2012, 42], [2013, 64]]
  }];

  var options = {
    series: {
      lines: { show: true,
        lineWidth: 2,
        fill: true, fillColor: { colors: [ { opacity: 0.5 }, { opacity: 0.2 } ] }
        },
      points: { show: true, 
        lineWidth: 2 
        },
      shadowSize: 0
    },
    grid: { hoverable: true, 
      clickable: true, 
      tickColor: "#eeeeee",
      borderWidth: 0
    },
    legend: {
      noColumns: 2
    },
    colors: ["#3660aa", "#3eb157"],
     xaxis: {ticks:12, tickDecimals: 0},
     yaxis: {ticks:3, tickDecimals: 0},
    selection: {
      mode: "x"
    }
  };

  var placeholder = $("#section-chart");

  placeholder.bind("plotselected", function (event, ranges) {

    $("#selection").text(ranges.xaxis.from.toFixed(1) + " to " + ranges.xaxis.to.toFixed(1));

    var zoom = $("#zoom").attr("checked");

    if (zoom) {
      plot = $.plot(placeholder, data, $.extend(true, {}, options, {
        xaxis: {
          min: ranges.xaxis.from,
          max: ranges.xaxis.to
        }
      }));
    }
  });

  placeholder.bind("plotunselected", function (event) {
    $("#selection").text("");
  });

  var plot = $.plot(placeholder, data, options);

  $("#clearSelection").click(function () {
    plot.clearSelection();
  });

  $("#setSelection").click(function () {
    plot.setSelection({
      xaxis: {
        from: 1994,
        to: 1995
      }
    });
  });

  // Add the Flot version string to the footer

  $("#footer").prepend("Flot " + $.plot.version + " &ndash; ");
});

//Google Visualization 
google.load("visualization", "1", {
  packages: ["corechart"]
});

$(document).ready(function () {
  drawChart1();
  drawChart2();
  drawChart3();
  drawChart4();
  drawRegionsMap();
  drawTable();
  candlestick();
  bubbleChart();
})

function drawChart1() {
  var data = google.visualization.arrayToDataTable([
    ['Year', 'Google+', 'Facebook'],
    ['2005', 90, 30],
    ['2006', 180, 260],
    ['2007', 1050, 320],
    ['2008', 1390, 650],
    ['2009', 2120, 970],
    ['2010', 3970, 1560],
    ['2011', 2650, 2390],
    ['2012', 1390, 2940]
    ]);

  var options = {
    width: 'auto',
    pointSize: 7,
    lineWidth: 1,
    height: '200',
    backgroundColor: 'transparent',
    colors: ['#3eb157', '#3660aa', '#d14836', '#dba26b', '#666666', '#f26645'],
    tooltip: {
      textStyle: {
        color: '#666666',
        fontSize: 11
      },
      showColorCode: true
    },
    legend: {
      textStyle: {
        color: 'black',
        fontSize: 12
      }
    },
    chartArea: {
      left: 40,
      top: 10,
      height: "80%"
    }
  };

  var chart = new google.visualization.AreaChart(document.getElementById('area_chart'));
  chart.draw(data, options);
}


function drawChart2() {
  var data = google.visualization.arrayToDataTable([
    ['Month', 'Visitors', 'Sales'],
    ['Jan', 99, 27],
    ['Feb', 1200, 731],
    ['Mar', 20, 197],
    ['Apr', 1967, 1591],
    ['May', 39, 212],
    ['June', 210, 967],
    ['July', 61, 109],
    ['Aug', 1830, 2967],
    ['Sep', 120, 38],
    ['Oct', 2280, 1967],
    ['Nov', 10, 79],
    ['Dec', 1290, 1967],
    ]);

  var options = {
    width: 'auto',
    height: '160',
    backgroundColor: 'transparent',
    colors: ['#3eb157', '#3660aa', '#d14836', '#dba26b', '#666666', '#f26645'],
    tooltip: {
      textStyle: {
        color: '#666666',
        fontSize: 11
      },
      showColorCode: true
    },
    legend: {
      textStyle: {
        color: 'black',
        fontSize: 12
      }
    },
    chartArea: {
      left: 100,
      top: 10
    },
    focusTarget: 'category',
    hAxis: {
      textStyle: {
        color: 'black',
        fontSize: 12
      }
    },
    vAxis: {
      textStyle: {
        color: 'black',
        fontSize: 12
      }
    },
    pointSize: 8,
    chartArea: {
      left: 60,
      top: 10,
      height: '80%'
    },
    lineWidth: 2,
  };

  var chart = new google.visualization.LineChart(document.getElementById('line_chart'));
  chart.draw(data, options);
}


function drawChart3() {
  var data = google.visualization.arrayToDataTable([
    ['Year', 'Visits', 'Orders', 'Income', 'Expenses'],
    ['2007', 300, 800, 900, 300],
    ['2008', 1170, 860, 1220, 564],
    ['2009', 260, 1120, 2870, 2340],
    ['2010', 1030, 540, 3430, 1200],
    ['2011', 200, 700, 1700, 770],
    ['2012', 1170, 2160, 3920, 800],
    ['2013', 2170, 1160, 2820, 500] ]);

  var options = {
    width: 'auto',
    height: '160',
    backgroundColor: 'transparent',
    colors: ['#3eb157', '#3660aa', '#d14836', '#dba26b', '#666666', '#f26645'],
    tooltip: {
      textStyle: {
        color: '#666666',
        fontSize: 11
      },
      showColorCode: true
    },
    legend: {
      textStyle: {
        color: 'black',
        fontSize: 12
      }
    },
    chartArea: {
      left: 60,
      top: 10,
      height: '80%'
    },
  };

  var chart = new google.visualization.ColumnChart(document.getElementById('column_chart'));
  chart.draw(data, options);
}

function drawChart4() {
  var data = google.visualization.arrayToDataTable([
    ['Task', 'Hours per Day'],
    ['Eat', 2],
    ['Work', 9],
    ['Commute', 2],
    ['Read', 2],
    ['Sleep', 7],
    ['Play', 2],
    ]);

  var options = {
    width: 'auto',
    height: '265',
    backgroundColor: 'transparent',
    colors: ['#3eb157', '#3660aa', '#d14836', '#dba26b', '#666666', '#f26645'],
    tooltip: {
      textStyle: {
        color: '#666666',
        fontSize: 11
      },
      showColorCode: true
    },
    legend: {
      position: 'left',
      textStyle: {
        color: 'black',
        fontSize: 12
      }
    },
    chartArea: {
      left: 0,
      top: 10,
      width: "100%",
      height: "100%"
    }
  };

  var chart = new google.visualization.PieChart(document.getElementById('pie_chart'));
  chart.draw(data, options);
}

//Geo Charts
google.load('visualization', '1', {'packages': ['geochart']});
google.setOnLoadCallback(drawRegionsMap);

function drawRegionsMap() {
  var data = google.visualization.arrayToDataTable([
    ['Country', 'Popularity'],
    ['Germany', 200],
    ['IN', 900],
    ['United States', 300],
    ['Brazil', 400],
    ['Canada', 500],
    ['France', 600],
    ['RU', 700]
    ]);

  var options = {
    width: 'auto',
    height: '280',
    backgroundColor: 'transparent',
    colors: ['#3eb157', '#3660aa', '#d14836', '#dba26b', '#666666', '#f26645'],
  };

  var chart = new google.visualization.GeoChart(document.getElementById('geo_chart'));
  chart.draw(data, options);
};

//Table Charts
google.load('visualization', '1', {packages:['table']});
google.setOnLoadCallback(drawTable);
function drawTable() {
  var data = new google.visualization.DataTable();
  data.addColumn('string', 'Name');
  data.addColumn('number', 'Salary');
  data.addColumn('boolean', 'Full Time Employee');
  data.addRows([
    ['Mike',  {v: 10000, f: '$10,000'}, true],
    ['Carry',   {v: 18000, f: '$18,000'},  false],
    ['Arjun', {v: 12500, f: '$12,500'}, false],
    ['Basava',   {v: 28000, f: '$17,000'}, true],
    ['Sandy',  {v: 10000, f: '$11,000'}, true]
    ]);

  var table = new google.visualization.Table(document.getElementById('table_chart'));
  table.draw(data, {showRowNumber: true});
}

//Candlestick Chart
function candlestick() {
  var data = google.visualization.arrayToDataTable([
    ['Mon', 20, 28, 38, 45],
    ['Tue', 31, 38, 55, 66],
    ['Wed', 50, 55, 77, 80],
    ['Thu', 77, 77, 66, 50],
    ['Fri', 68, 66, 22, 15]
    // Treat first row as data as well.
    ], true);

  var options = {
    legend: 'none',
    width: 'auto',
    height: '280',
    backgroundColor: 'transparent',
    colors: ['#3eb157', '#3660aa', '#d14836', '#dba26b', '#666666', '#f26645'],
  };

  var chart = new google.visualization.CandlestickChart(document.getElementById('candlestick_chart'));
  chart.draw(data, options);
}

// google.setOnLoadCallback(drawVisualization);

//Bubble Chart

google.load("visualization", "1", {packages:["corechart"]});
google.setOnLoadCallback(bubbleChart);
function bubbleChart() {
  var data = google.visualization.arrayToDataTable([
    ['ID', 'Life Expectancy', 'Fertility Rate', 'Region',     'Population'],
    ['CAN',    80.66,              1.67,      'North America',  33739900],
    ['DEU',    79.84,              1.36,      'Europe',         81902307],
    ['DNK',    78.6,               1.84,      'Europe',         5523095],
    ['SL',     72.73,              2.78,      'South Asia',    109716203],
    ['GBR',    80.05,              2,         'Europe',         61801570],
    ['IRN',    72.49,              1.7,       'Middle East',    73137148],
    ['IRQ',    68.09,              4.77,      'Middle East',    31090763],
    ['ISR',    81.55,              2.96,      'Middle East',    7485600],
    ['RUS',    68.6,               1.54,      'Europe',         141850000],
    ['USA',    78.09,              2.05,      'North America',  307007000]
    ]);

  var options = {
    title: 'Correlation between life expectancy, fertility rate and population of some world countries (2012)',
    hAxis: {title: 'Life Expectancy'},
    vAxis: {title: 'Fertility Rate'},
    colors: ['#3eb157', '#3660aa', '#d14836', '#dba26b', '#666666', '#f26645'],
    fontSize: 11,
    bubble: {textStyle: {fontSize: 11}}
  };

  var chart = new google.visualization.BubbleChart(document.getElementById('bubble_chart'));
  chart.draw(data, options);
}

//Resize charts and graphs on window resize
$(document).ready(function () {
  $(window).resize(function(){
    drawChart1();
    drawChart2();
    drawChart3();
    drawChart4();
    drawTable();
    bubbleChart();
    drawRegionsMap();
    candlestick()
  });
});



//NVD3 Charts

//lineWithFocusChart
nv.addGraph(function() {
  var chart = nv.models.lineWithFocusChart();

  chart.xAxis
      .tickFormat(d3.format(',f'));
  chart.x2Axis
      .tickFormat(d3.format(',f'));

  chart.yAxis
      .tickFormat(d3.format(',.2f'));
  chart.y2Axis
      .tickFormat(d3.format(',.2f'));

  d3.select('#lineWithChart svg')
      .datum(testData())
    .transition().duration(500)
      .call(chart);

  nv.utils.windowResize(chart.update);

  return chart;
});



function testData() {
  return stream_layers(3, 128, .1).map(function(data, i) {
    return { 
      key: 'Data - '+ i,
      values: data
    };
  });
}