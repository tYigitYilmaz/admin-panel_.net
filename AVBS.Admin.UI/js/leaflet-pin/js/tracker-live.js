var liveData = [];
var floorGatewayIdList = gatewayIdList;
var map;
var isPersonnelInfo = false;

// create popup contents
var gatewayInfoPopup = '<h3 class="box-title m-b-0 t-a-c room-name">Room</h3>' +
                        '<h6>Number of Personnels: <span class="text-muted personnel-count">0</small></h6>' +
                        '<div class="table-responsive">' + 
                            '<table id="live-popup-room-personnel-table" class="display nowrap m-t-0" cellspacing="0" width="100%">' +
                               ' <tbody></tbody> ' +
                           ' </table> ' + 
                        '</div>';

var personnelInfoPopup;

function info(lat, lng, dep, gat, room) {
    floorGatewayIdList = [gat];
    leafletCustom.centerLeafletMapOnMarker(lat, lng);
    //getLiveData();
}

function populatePins() {
    $.ajax({
        url: '/Floor/GetPins/' +  window.currentFloorId,
        type: 'POST',
        success: function (data) {
            if (data.error === 0) {
                $.each(data.pins, function (e, i) {
                    var customPin = L.divIcon({
                        className: 'location-pin',
                        html: '<div id="pin_' + i.Id + '"> </div>' +
                            '<span id="gat_' + i.GatewayId + '" onclick="info(' + i.Lat + ', ' + i.Lng + ', ' + i.DepartmentId + ', ' + i.GatewayId + ', \'' + i.RoomName + '\')"  data-room="' + i.RoomName + '"  data-gat="' + i.GatewayId + '" data-lng="' + i.Lng + '" data-lat="' + i.Lat + '" data-dep="' + i.DepartmentId + '" class="gatewayPersonnelCount" style=" position: absolute; background: white; border:4px solid ' + i.Color + ';border-radius: 50%; width: 25px; height: 25px;text-align: center;" class="">0</span>',
                        iconSize: [32.39, 28.63],
                        iconAnchor: [16.195, 28.63]
                    });

                    var ratio = $('#image-map').width() / i.Resolution;

                    var lngDivideTwo = i.MidPoint;
                    var diffFactor = lngDivideTwo - lngDivideTwo * ratio;

                    var newMarker = new L.marker([i.Lat, i.Lng - diffFactor], {
                        icon: customPin
                    }).addTo(map);

                    newMarker.bindPopup(gatewayInfoPopup);

                    newMarker.on('click', function (e) { 
                        var popup = e.target.getPopup();
                        newMarker.openPopup();
                        $('#image-map').focus();

                        if (isPersonnelInfo) {
                            popup.setContent(personnelInfoPopup);
                            isPersonnelInfo = false;
                            $(".leaflet-popup").css('top', '-190px');
                            $(".leaflet-popup-content").css('height', 'auto');

                        } else {
                            $(".leaflet-popup").css('top', '');
                            popup.setContent(gatewayInfoPopup);
                            initRoomPersonnelTable();
                            $(".leaflet-popup .personnel-count").html(liveData.PersonnelData.length);
                        }

                        $('.leaflet-popup .room-name').html(i.RoomName);
                    });

                });
            }
        }
    });
}

function liveInit( ) {

    map = leafletCustom.createMap('image-map', window.planUrl);
    populatePins();
    floorGatewayIdList = window.gatewayIdList;
    getLiveData();
    setInterval(function () {
        getLiveData();

    }, 1000); 
}

function getLiveData() {

    $.ajax({
        url: '/Tracker/TrackLiveFloor/',
        type: 'POST',
        data: "{ 'gatewayIdList': [" + floorGatewayIdList + "] }",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.error === 0) {
            } else {
                liveData = data;
                //reinitializeTable();
                window.tableInit();
                updateGatewayCount();
                initRoomPersonnelTable();
            }
        }
    });

}

function updateGatewayCount() {

    $('.gatewayPersonnelCount').html(0);
    if (liveData.PinData) {
        $.each(liveData.PinData, function (e, i) {
            $('#gat_' + i.GatewayId).html(i.PersonnelCount);
        });
    }
}

//function reinitializeTable() {
//    $('#trackerPersonnelTable').DataTable({
//        destroy: true,
//        data: liveData.PersonnelData,
//        processing: true,
//        serverSide: false,
//        dom: 'Bfitlp',
//        buttons: [
//            'excel', 'switchCardView', 'wholeFloor'
//        ],
//        columnDefs: [
//                {
//                    targets: 0,
//                    className: 'all',
//                    render: function (Id, type, full) {
//                        return '<img src="' + full.ImageUrl + '" alt="user-img" width="50" class="img-circle">';
//                    }
//                },
//                {
//                    targets: 1,
//                    className: 'dt-center',
//                    render: function (Id, type, full) {
//                        return full.Name;
//                    }
//                },
//                {
//                    targets: 2,
//                    className: 'dt-center',
//                    render: function (Id, type, full) {
//                        return full.Surname;
//                    }
//                },

//                {
//                    targets: 3,
//                    className: 'dt-center',
//                    render: function (Id, type, full) {
//                        return full.BeaconId;
//                    }
//                },

//                {
//                    targets: 4,
//                    className: 'dt-center',
//                    render: function (Id, type, full) {
//                        return full.DepartmentName;
//                    }
//                },

//                {
//                    targets: 5,
//                    className: 'dt-center',
//                    render: function (Id, type, full) {
//                        return full.Title;
//                    }
//                },
//                {
//                    targets: 6,
//                    className: 'dt-center',
//                    render: function (Id, type, full) {
//                        return full.Email;
//                    }
//                },
//                {
//                    targets: 7,
//                    className: 'dt-center',
//                    render: function (Id, type, full) {
//                        return '<a href="" onclick="showPersonnelInfo(' + full.PinId + ', \'' + full.Name + '\', \'' + full.Surname + '\', \'' + full.ImageUrl + '\', \'' + full.DepartmentColor + '\', \'' + full.Title + '\', \'' + full.Email + '\', event)" data-toggle="tooltip" data-original-title="Track"> <i class="fa fa-location-arrow text-success m-r-10"></i> </a>';
//                    }
//                }

//        ]
//    });
//}

function showWholeFloor() {
    floorGatewayIdList = gatewayIdList;
    //getLiveData();
}

function showPersonnelInfo(id, name, surname, imageUrl, departmentColor, title, email, e) {
    e.preventDefault();

    var tempCopyDiv = $('.personnelInfoWrapper');
    // if no profile image write first letters of name and surname
    if (imageUrl == '' || imageUrl == null) {
        var intials = name.charAt(0).toUpperCase() + surname.charAt(0).toUpperCase();
        $($(tempCopyDiv).find(".no-photo-div")).text(intials).css("background-color", departmentColor).toggleClass("hide");
        $($(tempCopyDiv).find(".img-circle")).toggleClass("hide"); // hide img tag

    } else {
        $($(tempCopyDiv).find(".img-circle")).attr('src', imageUrl);
    }

    $($(tempCopyDiv).find(".personnel-name")).html(name + " " + surname);
    $($(tempCopyDiv).find(".personnel-name")).attr('title', name + " " + surname);
    $($(tempCopyDiv).find(".personnel-title")).html(title);
    $($(tempCopyDiv).find(".personnel-title")).attr('title', title);
    $($(tempCopyDiv).find(".personnel-email")).html(email);
    $($(tempCopyDiv).find(".personnel-email")).attr('title', email);

    personnelInfoPopup = $('.personnelInfoWrapper').html();

    isPersonnelInfo = true;
    $('#pin_' + id).trigger('click');
}

function initRoomPersonnelTable() {
    $('#live-popup-room-personnel-table').DataTable({
        destroy: true,
        data: liveData.PersonnelData,
        processing: true,
        serverSide: false,
        dom: '',
        columnDefs: [
            {
                targets: 0,
                className: 'all',
                render: function (Id, type, full) {
                    return '<img src="' + full.ImageUrl + '" alt="user-img" width="40" class="img-circle">';
                }
            },
             {
                 targets: 1,
                 className: 'dt-center',
                 render: function (Id, type, full) {
                     return full.Name;
                 }
             },
            {
                targets: 2,
                className: 'dt-center',
                render: function (Id, type, full) {
                    return full.Surname;
                }
            }
        ]
    });
   
}