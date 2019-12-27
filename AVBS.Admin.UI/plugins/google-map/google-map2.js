var popupMap;

var markerBounds2;
var markers2 = [];

function clearMarkers2() {
    for (var i = 0; i < this.markers2.length; i++) {
        this.markers2[i].setMap(null);
    }
    markers2 = [];
    markerBounds2 = new google.maps.LatLngBounds();
}

function initPopupMap() {
    var latlng = new google.maps.LatLng(39.9333635, 32.8597419);

    var mapoptions = {
        zoom: 12,
        mapTypeControl: false,
        center: latlng,
        panControl: false,
        rotateControl: false,
        streetViewControl: false,
        fullscreenControl: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        styles: [
    {
        "elementType": "geometry",
        "stylers": [
      {
          "color": "#3f5b88 "
      }
        ]
    },
    {
        "elementType": "labels.icon",
        "stylers": [
      {
          "visibility": "off"
      }
        ]
    },
    {
        "elementType": "labels.text.fill",
        "stylers": [
      {
          "color": "#616161"
      }
        ]
    },
    {
        "elementType": "labels.text.stroke",
        "stylers": [
      {
          "color": "#f5f5f5"
      }
        ]
    },
    {
        "featureType": "administrative.land_parcel",
        "elementType": "labels.text.fill",
        "stylers": [
      {
          "color": "#bdbdbd"
      }
        ]
    },
    {
        "featureType": "poi",
        "elementType": "geometry",
        "stylers": [
      {
          "color": "#2d4060"
      }
        ]
    },
    {
        "featureType": "poi",
        "elementType": "labels.text.fill",
        "stylers": [
      {
          "color": "#757575"
      }
        ]
    },
    {
        "featureType": "poi.park",
        "elementType": "geometry",
        "stylers": [
      {
          "color": "#364e74"
      }
        ]
    },
    {
        "featureType": "poi.park",
        "elementType": "labels.text.fill",
        "stylers": [
      {
          "color": "#9e9e9e"
      }
        ]
    },
    {
        "featureType": "road",
        "elementType": "geometry",
        "stylers": [
      {
          "color": "#ffffff"
      }
        ]
    },
    {
        "featureType": "road.arterial",
        "elementType": "labels.text.fill",
        "stylers": [
      {
          "color": "#757575"
      }
        ]
    },
    {
        "featureType": "road.highway",
        "elementType": "geometry",
        "stylers": [
      {
          "color": "#dadada"
      }
        ]
    },
    {
        "featureType": "road.highway",
        "elementType": "labels.text.fill",
        "stylers": [
      {
          "color": "#616161"
      }
        ]
    },
    {
        "featureType": "road.local",
        "elementType": "labels.text.fill",
        "stylers": [
      {
          "color": "#9e9e9e"
      }
        ]
    },
    {
        "featureType": "transit.line",
        "elementType": "geometry",
        "stylers": [
      {
          "color": "#e5e5e5"
      }
        ]
    },
    {
        "featureType": "transit.station",
        "elementType": "geometry",
        "stylers": [
      {
          "color": "#2d4060"
      }
        ]
    },
    {
        "featureType": "water",
        "elementType": "geometry",
        "stylers": [
      {
          "color": "#c9c9c9"
      }
        ]
    },
    {
        "featureType": "water",
        "elementType": "labels.text.fill",
        "stylers": [
      {
          "color": "#9e9e9e"
      }
        ]
    }
        ]
    };

    popupMap = new google.maps.Map(document.getElementById('google-popupMap'), mapoptions);

    clearMarkers2();

    $('#exampleModal').on('shown.bs.modal', function () {
        google.maps.event.trigger(popupMap, "resize");
        popupMap.setCenter(latlng);
    });

    return popupMap;
}

function focusPin2(lat, lng) {
    var position = new google.maps.LatLng(lat, lng);
    popupMap.setZoom(12);
    popupMap.panTo(position);
}

function mapFitBounds2() {
    //markers2.forEach(function (marker) {
    //    markerBounds2.extend(marker.position);
    //});
    popupMap.fitBounds(markerBounds2);
}

function initAutocomplete2() {
    popupMap = initPopupMap();
 
 
     

    icon = {
        url: "http://maps.google.com/mapfiles/ms/icons/orange-dot.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };

    // Create the search box and link it to the UI element.
    var input = document.getElementById('Address');
    // if no searchbox return
    if (input == null) return;

    var searchBox = new google.maps.places.SearchBox(input);
    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    // Bias the SearchBox results towards current map's viewport.
    popupMap.addListener('bounds_changed', function () {
        searchBox.setBounds(popupMap.getBounds());
    });

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    searchBox.addListener('places_changed', function () {

        clearMarkers2();

        // Update Address input 
        //$("#Address").val($("#pac-input").val());

        var places = searchBox.getPlaces();

        if (places.length == 0) {
            return;
        }

        console.log(places.length);

        // For each place, get the icon2, name and location.
        places.forEach(function (place) {

            markers2 = [];

            if (!place.geometry) {
                console.log("Returned place contains no geometry");
                return;
            }

            // Create a marker for each place.
            markers2.push(new google.maps.Marker({
                map: popupMap,
                icon: icon,
                title: place.name,
                position: place.geometry.location
            }));

            var lat = place.geometry.location.lat();
            var long = place.geometry.location.lng();
            focusPin2(lat,long);

            $("#lat").val(lat);
            $("#long").val(long);

        });

    });
}

