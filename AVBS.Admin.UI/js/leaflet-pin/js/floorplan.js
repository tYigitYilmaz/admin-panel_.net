var isFormConfigured = false;
var isPlanUploaded = false;

var leafletCustom;
var map;

function initFloorPlan(divId, url) {

    leafletCustom = new LeafletCustom();

    $("#File").change(function () {
        var formData = new FormData();
        formData.append('file', $('#File')[0].files[0]);

        $.ajax({
            url: '/Floor/UploadFloor',
            type: 'POST',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (data) {
                if (data.error === 0) {
                    $(".leaflet-image-layer").remove();

                    if (url == '') { // Create
                        map = leafletCustom.createMap(divId, data.url);
                    } else { // Edit
                        leafletCustom.addImageOverlay(data.url);
                    }
                    //L.imageOverlay(data.url, leafletCustom._bounds).addTo(map);
                    $("#FileName").val(data.filename);
                    $("#TempFileName").val(data.tempfilename);
                    isPlanUploaded = true;
                }
            }
        });
    });

    if (url == '') return;

    map = leafletCustom.createMap(divId, url);

    //Add marker
    //var newMarkerGroup = new L.LayerGroup();
    map.on('click', addMarker);

    isPlanUploaded = true;
    populatePins();

}

function addMarker(e) {

    // Add marker to map at click location; add popup window
    if (!isPlanUploaded) return;

    $('#exampleModal').modal();

    $("#addPin").unbind("click");

    $("#addPin").on("click", function () { 
        $('#SelectGateway').css('border', '1px solid #e4e7ea;');
        var depColor = $("#SelectDepartment option:selected").data("color");
        var dep = $("#SelectDepartment option:selected").val();
        var gat = $("#SelectGateway option:selected").val();
        if (gat === "-1") {
            $('#SelectGateway').css('border', '1px solid red ');
            return;
        } else {
            $('#SelectGateway').css('border', '1px solid #e4e7ea;');
        }

        var resolution = $(".leaflet-wrapper").width();
        var midPoint = leafletCustom._map.getBounds()._northEast.lng / 2;


        var room = $("#RoomName").val();
        var roomStatus = $("#SelectRoomStatus option:selected").val();

 
        var customPin = L.divIcon({
            className: 'location-pin',
            html: '<div><img src="/Images/gateway.svg" class="pin" onclick="info(' + e.latlng.lat + ', ' + e.latlng.lng + ', ' + dep + ', ' + gat + ', \'' + room + '\', ' + roomStatus + ')" data-room="' + room + '" data-roomstatus="' + roomStatus + '" data-gat="' + gat + '" data-lng="' + e.latlng.lng + '" data-lat="' + e.latlng.lat + '" data-dep="' + dep + '" data-resolution="' + resolution + '" data-midpoint="' + midPoint + '"style="float:left;"><div style="width: 10px;height: 10px;color: red;border-radius: 50%;background:' + depColor + ';position:absolute;float:left; margin-left: 11.6px;margin-top: 20px;"></div></div>',
            iconSize: [32.39, 28.63],
            iconAnchor: [16.195, 28.63]
        });

        var newMarker = new L.marker(e.latlng, {
            icon: customPin
        }).addTo(map);
        newMarker.bindPopup("");

        arrangeGatewayIdList();
        $("#exampleModal").modal('hide');
    });

}

function info(lat, lng, dep, gat, room, roomstatus) {
    $(".leaflet-popup-pane").hide();
    $("#removePin").attr("data-lat", lat);
    $("#removePin").attr("data-lng", lng);

    $("#updatePin").attr("data-lat", lat);
    $("#updatePin").attr("data-lng", lng);

    $("#SelectDepartmentEdit").val(dep);
    $("#SelectGatewayEdit").val(gat);
    $("#RoomNameEdit").val(room);
    $("#SelectRoomStatusEdit").val(roomstatus);

    $('#exampleModalEdit').modal();
}

// Remove Pin
$("#removePin").on("click", function () {
    var lat = $(this).attr("data-lat");
    var lng = $(this).attr("data-lng");
    $.each($('.pin'), function (i, e) {
        if ($(e).attr("data-lat") == lat && $(e).attr("data-lng") == lng) {
            $(e).parent().remove(); // $(e).parent().remove(); !!! indexcreate de böyle TODO incele
        }
    });
    arrangeGatewayIdList();
    $("#exampleModalEdit").modal('hide');
});

// Update Pin
$("#updatePin").on("click", function () {
    var lat = $(this).attr("data-lat");
    var lng = $(this).attr("data-lng");

    // removes pin
    $.each($('.pin'), function (i, e) {
        if ($(e).attr("data-lat") == lat && $(e).attr("data-lng") == lng) {
            $(e).parent().remove();
        }
    });

    // adds pin
    var depColor = $("#SelectDepartmentEdit option:selected").data("color");
    var dep = $("#SelectDepartmentEdit option:selected").val();
    var gat = $("#SelectGatewayEdit option:selected").val();
    var room = $("#RoomNameEdit").val();
    var roomStatus = $("#SelectRoomStatusEdit option:selected").val();

    var resolution = $(".leaflet-wrapper").width();
    var midPoint = leafletCustom._map.getBounds()._northEast.lng / 2;

    var customPin = L.divIcon({
        className: 'location-pin',
        html: '<div><img src="/Images/gateway.svg" class="pin" onclick="info(' + lat + ', ' + lng + ', ' + dep + ', ' + gat + ', \'' + room + '\', ' + roomStatus + ')" data-room="' + room + '" data-roomstatus="' + roomStatus + '"  data-gat="' + gat  + '" data-resolution="' + resolution + '" data-midpoint="' + midPoint + '" data-lng="' + lng + '" data-lat="' + lat + '" data-dep="' + dep + '" style="float:left;"><div style="width: 10px;height: 10px;color: red;border-radius: 50%;background:' + depColor + ';position:absolute;float:left; margin-left: 11.6px;margin-top: 20px;"></div></div>',
        iconSize: [32.39, 28.63],
        iconAnchor: [16.195, 28.63]
    });
    var newMarker = new L.marker([lat, lng], {
        icon: customPin
    }).addTo(map);
    newMarker.bindPopup("");
    arrangeGatewayIdList();
    $("#exampleModalEdit").modal('hide');
});

$("form").submit(function (e) {
    if (!isFormConfigured) {
        e.preventDefault();
        var pushData = [];
        var pinInfo = [];
        $.each($('.pin'), function (i, e) {
            pushData.push($(e).attr("data-lat"));
            pushData.push($(e).attr("data-lng"));
            pushData.push($(e).attr("data-dep"));
            pushData.push($(e).attr("data-gat"));
            pushData.push($(e).attr("data-room"));
            pushData.push($(e).attr("data-midpoint"));
            pushData.push($(e).attr("data-resolution"));
            pushData.push($(e).attr("data-roomstatus") + "*");
            pinInfo.push(pushData);
            pushData = [];
        });
        $("#PinInfo").val(pinInfo);
        isFormConfigured = true;
        $(this).submit();
    }

});

function arrangeGatewayIdList() {
    $('#SelectGateway option').removeAttr('disabled');
    $('#SelectGatewayEdit option').removeAttr('disabled');
    $.each($('.pin'), function (i, e) {
        var gat = $(e).attr("data-gat");
        $.each($('#SelectGateway option'), function (index, el) {
            if ($(el).val() == gat) {
                $(el).attr('disabled', 'disabled');
            }
        });
        $.each($('#SelectGatewayEdit option'), function (index, el) {
            if ($(el).val() == gat) {
                $(el).attr('disabled', 'disabled');
            }
        });
    });
    $("#SelectGateway").val(-1);

}

function populatePins() {
    $.ajax({
        url: '/Floor/GetPins/' + $("#FloorId").val(),
        type: 'POST',
        success: function (data) {
            if (data.error === 0) {
                $.each(data.pins, function (e, i) {
                    var customPin = L.divIcon({
                        className: 'location-pin',
                        html: '<div ><img src="/Images/gateway.svg" class="pin" onclick="info(' + i.Lat + ', ' + i.Lng + ', ' + i.DepartmentId + ', ' + i.GatewayId + ', \'' + i.RoomName + '\', ' + i.RoomStatusId + ')" data-room="' + i.RoomName + '" data-roomstatus="' + i.RoomStatusId + '"  data-gat="' + i.GatewayId + '" data-lng="' + i.Lng + '" data-lat="' + i.Lat + '" data-dep="' + i.DepartmentId + '" data-resolution="' + i.Resolution + '" data-midpoint="' + i.MidPoint + '" style="float:left;"><div style="width: 10px;height: 10px;color: red;border-radius: 50%;background:' + i.Color + ';position:absolute;float:left; margin-left: 11.6px;margin-top: 20px;"></div></div>',
                        iconSize: [32.39, 28.63],
                        iconAnchor: [16.195, 28.63]
                    });

                    var ratio = $('#image-map').width() / i.Resolution;

                    var lngDivideTwo = i.MidPoint;
                    var diffFactor = lngDivideTwo - lngDivideTwo * ratio;
 

                    var newMarker = new L.marker([i.Lat, i.Lng - diffFactor], {
                        icon: customPin
                    }).addTo(map);
                    newMarker.bindPopup("");
                });

                arrangeGatewayIdList();
            }
        }
    });
}


