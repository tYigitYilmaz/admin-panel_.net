var mapH;

window.onload = function () {

    // customized heatmap configuration
    var heatmapInstance = h337.create({
        // required container
        container: document.getElementById('heatmap'),
        // backgroundColor to cover transparent areas
        backgroundColor: 'rgba(255,255,255,0)',
        // custom gradient colors
        gradient: {
            // enter n keys between 0 and 1 here
            // for gradient color customization
            '.25': 'blue',
            '.45': 'cyan',
            //'.55': 'green',
            '.55': 'rgb(25,255,103)',
            '.70': 'yellow',
            '.75': 'orange',
            '.90': 'rgb(234,56,7)',
            '.99': 'red'
        },
        // the maximum opacity (the value with the highest intensity will have it)
        maxOpacity: .7,
        // minimum opacity. any value > 0 will produce 
        // no transparent gradient transition
        minOpacity: 0//.3
    });

    var alternativeForm = true;

    // now generate some random data
    var requestRes = [
	  { PersonnelId: 50, DepartmentId: 1, GatewayId: 17, Percentage: 20.5 },
	  { PersonnelId: 49, DepartmentId: 1, GatewayId: 16, Percentage: 41.5 },
	  { PersonnelId: 48, DepartmentId: 2, GatewayId: 17, Percentage: 42.3 },
	  { PersonnelId: 47, DepartmentId: 2, GatewayId: 18, Percentage: 63.1 }
    ];

    var requestResAlternative = [
		{ GatewayId: 16, Percentage: 20.5 },
		{ GatewayId: 17, Percentage: 62.8 },
		{ GatewayId: 18, Percentage: 63.1 }
    ];

    var points = [];

    var width = $("#image-map").width();
    var height = $("#image-map").height();
 

    if (alternativeForm) {
        Math.seedrandom(JSON.stringify(requestResAlternative));

        for (var i = 0; i < requestResAlternative.length; i++) {
            points.push({
                x: Math.floor(Math.random() * width),
                y: Math.floor(Math.random() * height),
                value: Math.floor(requestResAlternative[i].Percentage * 10),
                radius: 100
            });
        }
    } else {
        var personnelFilter = null;
        var departmentFilter = null;

        var filteredRes;
        if (personnelFilter) {
            filteredRes = [];
            for (var i = 0; i < requestRes.length; i++) {
                if (requestRes[i].PersonnelId === personnelFilter) {
                    filteredRes.push(requestRes[i]);
                }
            }
        } else if (departmentFilter) {
            filteredRes = [];
            for (var i = 0; i < requestRes.length; i++) {
                if (requestRes[i].DepartmentId === departmentFilter) {
                    filteredRes.push(requestRes[i]);
                }
            }
        } else {
            filteredRes = requestRes;
        }

        Math.seedrandom(JSON.stringify(requestRes));

        var processedRes = {};
        for (var i = 0; i < filteredRes.length; i++) {
            if (filteredRes[i].GatewayId.toString() in processedRes) {
                processedRes[filteredRes[i].GatewayId.toString()].value += Math.floor(filteredRes[i].Percentage * 10);
            } else {
                processedRes[filteredRes[i].GatewayId.toString()] = {
                    x: Math.floor(Math.random() * width),
                    y: Math.floor(Math.random() * height),
                    value: Math.floor(filteredRes[i].Percentage * 10),
                    radius: 100
                };
            }
        }

        for (key in processedRes) {
            points.push(processedRes[key]);
        }
    }

    /*var points = [
	  {x:42,y:100,value:5,radius:100},
	  {x:123,y:102,value:8,radius:100},
	  {x:206,y:101,value:5,radius:100},
	  {x:289,y:108,value:7,radius:100},
	  {x:401,y:104,value:6,radius:100},
	  {x:42,y:201,value:8,radius:100},
	  {x:123,y:200,value:7,radius:100},
	  {x:204,y:206,value:6,radius:100},
	  {x:285,y:205,value:10,radius:100},
	  {x:362,y:206,value:8,radius:100},
	  {x:40,y:300,value:7,radius:100},
	  {x:120,y:309,value:6,radius:100},
	  {x:201,y:305,value:5,radius:100},
	  {x:286,y:301,value:9,radius:100},
	  {x:367,y:302,value:6,radius:100},
	  {x:448,y:257,value:8,radius:100},
	  {x:588,y:256,value:10,radius:100},
	  {x:725,y:272,value:9,radius:100}
	];*/

    var max = 0;
    //var len = 300;

    // Visual "Correction"
    var minDistance = 25;
    var maxDistance = 100;
    var minNumber = 2;
    var maxNumber = 4;
    var maxValueLax = 1;
    var maxRadiusLax = 15;
    var valueMultiplier = 0.75;
    var radiusMultiplier = 0.8;

    var tempPoint = {};
    var tempNumber;
    var tempDistanceSquare;
    var tempPointCount;
    var initialPointsLength = points.length;

    for (var i = 0; i < initialPointsLength; i++) {
        tempPointCount = 0;
        max = Math.max(max, points[i].value);
        tempNumber = minNumber + Math.floor((maxNumber - minNumber + 1) * Math.random());
        while (tempPointCount < tempNumber) {
            tempPoint.x = points[i].x - maxDistance + Math.floor((2 * maxDistance + 1) * Math.random());
            tempPoint.y = points[i].y - maxDistance + Math.floor((2 * maxDistance + 1) * Math.random());
            tempDistanceSquare = ((tempPoint.x - points[i].x) * (tempPoint.x - points[i].x)) + ((tempPoint.y - points[i].y) * (tempPoint.y - points[i].y))
            if (tempDistanceSquare < (minDistance * minDistance) || tempDistanceSquare > (maxDistance * maxDistance)) {
                continue;
            }
            tempPoint.value = Math.floor(valueMultiplier * (points[i].value - maxValueLax + Math.floor((2 * maxValueLax + 1) * Math.random())));
            max = Math.max(max, tempPoint.value);
            tempPoint.radius = Math.floor(radiusMultiplier * (points[i].radius - maxRadiusLax + Math.floor((2 * maxRadiusLax + 1) * Math.random())));
            points.push(tempPoint);
            tempPoint = {};
            tempPointCount++;
        }
    }

    /*while (len--) {
	  var val = Math.floor(Math.random()*100);
	  var radius = Math.floor(Math.random()*70);

	  max = Math.max(max, val);
	  var point = {
		x: Math.floor(Math.random()*width),
		y: Math.floor(Math.random()*height),
		value: val,
		radius: radius
	  };
	  points.push(point);
	}*/
    // heatmap data format
    var data = {
        max: max,
        data: points
    };
    // if you have a set of datapoints always use setData instead of addData
    // for data initialization
    heatmapInstance.setData(data);

    // Cleanup
    Math.seedrandom();

    // Graphs
    var data = {
        labels: ["Gateway 16", "Gateway 17", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18"],
        series: [
		    [41.5, 62.8, 63.1, 63.1, 63.1, 63.1, 63.1, 631, 63.1, 63.1, 63.1, 63.1, 63.1, 63.1, 63.1, 63.1]
        ],
    };

    var options = {
        width: 100 * data.labels.length,
        height: 400,
        axisY: {
            onlyInteger: true
        }
    };

    var dwell_graph = new Chartist.Bar('#dwell_time', data, options);

    var temp_text;
    var temp_rect;
    var temp_triangle;

    dwell_graph.on('draw', function (ctx) {
        if (ctx.type === 'bar') {
            ctx.element.attr({
                x1: ctx.x2 + 0.001
            });

            temp_text = new Chartist.Svg("text");
            temp_text.text(ctx.value.y + " min");
            temp_text.attr({
                x: ctx.x2,
                y: ctx.y2 - 10,
                "text-anchor": "middle",
                style: "fill: white;"
            });

            temp_rect = new Chartist.Svg("rect"); 00
            temp_rect.attr({
                x: ctx.x2 - 50,
                y: ctx.y2 - 25,
                rx: 3,
                ry: 3,
                width: "100px",
                height: "20px",
                style: "fill: rgba(72,84,101,100)"
            });

            temp_triangle = new Chartist.Svg("polygon");
            temp_triangle.attr({
                points: (ctx.x2 - 10) + "," + (ctx.y2 - 6) + " " + (ctx.x2 + 10) + "," + (ctx.y2 - 6) + " " + (ctx.x2) + "," + (ctx.y2 - 1),
                style: "fill: rgba(72,84,101,100)"
            });

            ctx.group.append(temp_rect);
            ctx.group.append(temp_triangle);
            ctx.group.append(temp_text);
        }
    });

    dwell_graph.on('created', function (ctx) {
        var defs = ctx.svg.elem('defs');
        defs.elem('linearGradient', {
            id: 'gradient',
            x1: 0,
            y1: 1,
            x2: 0,
            y2: 0
        }).elem('stop', {
            offset: 0,
            'stop-color': 'rgb(0,75,255)'
        }).parent().elem('stop', {
            offset: 1,
            'stop-color': 'rgb(0,180,255)'
        });
    });

    var data1 = {
        labels: ["Gateway 16", "Gateway 17", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18", "Gateway 18"],
        series: [
		    [1, 2, 1, 1, 2, 24, 2, 1]
        ],
    };

    var options1 = {
        width: window.innerWidth / 3,
        height: 100 * data1.labels.length,
        reverseData: true,
        horizontalBars: true,
        axisX: {
            onlyInteger: true,
            position: 'start'
        },
        axisY: {
            offset: 70
        }
    };

    var visit_graph = new Chartist.Bar('#visit_freq', data1, options1);

    visit_graph.on('draw', function (ctx) {
        if (ctx.type === 'bar') {
            ctx.element.attr({
                y1: ctx.y2 + 0.001
            });

            temp_text = new Chartist.Svg("text");
            temp_text.text(ctx.value.x + (ctx.value.x <= 1 ? " personnel" : " personnels"));
            temp_text.attr({
                x: ctx.x2,
                y: ctx.y2 - 30,
                "text-anchor": "middle",
                style: "fill: white;"
            });

            temp_rect = new Chartist.Svg("rect"); 00
            temp_rect.attr({
                x: ctx.x2 - 50,
                y: ctx.y2 - 45,
                rx: 3,
                ry: 3,
                width: "100px",
                height: "20px",
                style: "fill: rgba(72,84,101,100)"
            });

            temp_triangle = new Chartist.Svg("polygon");
            temp_triangle.attr({
                points: (ctx.x2 - 10) + "," + (ctx.y2 - 26) + " " + (ctx.x2 + 10) + "," + (ctx.y2 - 26) + " " + (ctx.x2) + "," + (ctx.y2 - 21),
                style: "fill: rgba(72,84,101,100)"
            });

            ctx.group.append(temp_rect);
            ctx.group.append(temp_triangle);
            ctx.group.append(temp_text);
        }
    });

    visit_graph.on("created", function (ctx) {
        var defs = ctx.svg.elem('defs');
        defs.elem('linearGradient', {
            id: 'gradient1',
            x1: 0,
            y1: 0,
            x2: 1,
            y2: 0
        }).elem('stop', {
            offset: 0,
            'stop-color': 'rgb(0,75,255)'
        }).parent().elem('stop', {
            offset: 1,
            'stop-color': 'rgb(0,180,255)'
        });
    });
  
    

    


};
