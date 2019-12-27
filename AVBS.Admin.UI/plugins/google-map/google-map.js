var icon;
var map;

var markerBounds;
var markers = [];

function clearMarkers() {
    markers = [];
    markerBounds = new google.maps.LatLngBounds();
}

function initMap() {
    icon = {
        url: "http://maps.google.com/mapfiles/ms/icons/orange-dot.png",
        size: new google.maps.Size(71, 71),
        origin: new google.maps.Point(0, 0),
        anchor: new google.maps.Point(17, 34),
        scaledSize: new google.maps.Size(25, 25)
    };


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

    map = new google.maps.Map(document.getElementById('google-map'), mapoptions);
    clearMarkers();

    return map;
}

function focusPin(lat, lng) {
    var position = new google.maps.LatLng(lat, lng);
    map.setZoom(8);
    map.panTo(position);

}

function mapFitBounds() {
    markers.forEach(function (marker) {
        markerBounds.extend(marker.position);
    });
    map.fitBounds(markerBounds);
}

//function initAutocomplete() {
//    map = initMap();
//    icon = {
//        url: "http://maps.google.com/mapfiles/ms/icons/orange-dot.png",,
//        size: new google.maps.Size(71, 71),
//        origin: new google.maps.Point(0, 0),
//        anchor: new google.maps.Point(17, 34),
//        scaledSize: new google.maps.Size(25, 25)
//    };

//    // Create the search box and link it to the UI element.
//    var input = document.getElementById('Address');
//    // if no searchbox return
//    if (input == null) return;

//    var searchBox = new google.maps.places.SearchBox(input);
//    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

//    // Bias the SearchBox results towards current map's viewport.
//    map.addListener('bounds_changed', function () {
//        searchBox.setBounds(map.getBounds());
//    });

//    // Listen for the event fired when the user selects a prediction and retrieve
//    // more details for that place.
//    searchBox.addListener('places_changed', function () {

//        clearMarkers();

//        // Update Address input 
//        //$("#Address").val($("#pac-input").val());

//        var places = searchBox.getPlaces();

//        if (places.length == 0) {
//            return;
//        }

//        // For each place, get the icon, name and location.
//        places.forEach(function (place) {

//            if (!place.geometry) {
//                console.log("Returned place contains no geometry");
//                return;
//            }

//            // Create a marker for each place.
//            markers.push(new google.maps.Marker({
//                map: map,
//                icon: icon,
//                title: place.name,
//                position: place.geometry.location
//            }));

//            $("#lat").val(place.geometry.location.lat());
//            $("#long").val(place.geometry.location.lng());

//        });

//        mapFitBounds();
//    });
//}

