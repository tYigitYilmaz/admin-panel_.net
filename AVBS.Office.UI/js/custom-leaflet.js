function LeafletCustom() {
    var _map;
    var _bounds; 
    var w, h;
    var overlay;


    // create map
    this.createMap = function (mapDivId, svgUrl) {

        if (this._map != undefined || this._map != null || this._map) {
            this.destroyMap();
        }

        this._map = L.map(mapDivId, {
            minZoom: 1,
            maxZoom: 6,
            center: [0,0],
            zoom: 3,
            attributionControl: false,
            crs: L.CRS.Simple
        });

        // dimensions of the image 
        w = $("#" + mapDivId).width();
        h = $("#" + mapDivId).height();


        // calculate the edges of the image, in coordinate space
        var southWest = this._map.unproject([0, h], this._map.getMaxZoom() - 3);
        var northEast = this._map.unproject([w, 0], this._map.getMaxZoom() - 3);
        _bounds = new L.LatLngBounds(southWest, northEast);
 
        // tell leaflet that the map is exactly as big as the image
        this._map.setMaxBounds(_bounds);

        if (svgUrl == null || svgUrl == '') return this._map;

        // add the image overlay, 
        // so that it covers the entire map
        this.addImageOverlay(svgUrl);

        // TODO içini boyamak için
        //d3.xml(svgUrl, "image/svg+xml", function (data) {
        //    $.each($(data).find("g"), function (index, value) {
        //        countries.push(value);
        //    });
        //    window.asdasd = data;
        //var countries = [];
        //var countriesOverlay;
        //function projectPoint(x, y) {
        //    var point = map.latLngToLayerPoint(new L.LatLng(y, x));
        //    this.stream.point(point.x, point.y);
        //}
        //    countriesOverlay = L.d3SvgOverlay(function (sel, proj) {
        //        var svg = d3.select("#" + mapDivId).select("svg").attr("pointer-events", "auto");
        //        var g = svg.select("g");
        //        var transform = d3.geoTransform({ point: projectPoint });
        //        var path = d3.geoPath().projection(transform);
        //        console.log(countries);
        //        var upd = sel.data(countries);
        //        upd.enter()
        //          .append('path')
        //          .attr('d', upd.attr("d"))
        //          .attr('stroke', 'black')
        //          .attr('fill', function () { return d3.hsl(Math.random() * 360, 0.9, 0.5) })
        //          .attr('fill-opacity', '0.5');
        //        upd.attr('stroke-width', 1 / proj.scale);
        //    });
        //    L.control.layers({ "Geo Tiles": overlay }, { "Countries": countriesOverlay }).addTo(this._map);
        //    countriesOverlay.addTo(this._map);
        //});

        return this._map;
    }

    this.addImageOverlay = function (url, width, height) {
        overlay = L.imageOverlay(url, _bounds, { interactive: true });
        this._map.addLayer(overlay);
        //overlay.addTo(this._map);

        return overlay;
    }

    // destroy all existing maps
    this.destroyMap = function () {
        this._map.remove();
    }

    this.centerLeafletMapOnMarker = function (lat, lng) {
        var latLngs = [L.latLng(lat, lng)];
        var markerBounds = L.latLngBounds(latLngs);
        this._map.fitBounds(markerBounds);
    }

}