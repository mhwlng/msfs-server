

var map;
var marker;
var flightPathButton;
var followButton;

var following = true;

var trackline;
var gpsWaypoints;

var layerControl;

export function Init() {

    const cartoDBlayer = L.tileLayer(`https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png`, {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors &copy; <a href="https://carto.com/attributions">CARTO</a>'
    });

    const openStreetMap = L.tileLayer(`https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png`, {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    });

    const topoMap = L.tileLayer(`https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png`, {
        attribution: 'Map data: &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, <a href="http://viewfinderpanoramas.org">SRTM</a> | Map style: &copy; <a href="https://opentopomap.org">OpenTopoMap</a> (<a href="https://creativecommons.org/licenses/by-sa/3.0/">CC-BY-SA</a>)'
    });

    const StadiaDark = L.tileLayer(`https://tiles.stadiamaps.com/tiles/alidade_smooth_dark/{z}/{x}/{y}{r}.png`, {
        attribution: '&copy; <a href="https://stadiamaps.com/">Stadia Maps</a>, &copy; <a href="https://openmaptiles.org/">OpenMapTiles</a> &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors'
    });

    const stadiaOutdoors = L.tileLayer(`https://tiles.stadiamaps.com/tiles/outdoors/{z}/{x}/{y}{r}.png`, {
        attribution: '&copy; <a href="https://stadiamaps.com/">Stadia Maps</a>, &copy; <a href="https://openmaptiles.org/">OpenMapTiles</a> &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors'
    });

    // openaip key: https://www.openaip.net/users/clients#tab-clients

    var airspaces = new L.tileLayer(`https://api.tiles.openaip.net/api/data/airspaces/{z}/{x}/{y}.png?apiKey=${config.OPENAIP_KEY}`, {
        attribution: "<a href=\"https://www.openaip.net\" target=\"_blank\" style=\"\">openAIP</a>"
    });

    var reporting_points = new L.tileLayer(`https://api.tiles.openaip.net/api/data/reporting-points/{z}/{x}/{y}.png?apiKey=${config.OPENAIP_KEY}`, {
        attribution: "<a href=\"https://www.openaip.net\" target=\"_blank\" style=\"\">openAIP</a>"
    });

    var airports = new L.tileLayer(`https://api.tiles.openaip.net/api/data/airports/{z}/{x}/{y}.png?apiKey=${config.OPENAIP_KEY}`, {
        attribution: "<a href=\"https://www.openaip.net\" target=\"_blank\" style=\"\">openAIP</a>"
    });

    var navaids = new L.tileLayer(`https://api.tiles.openaip.net/api/data/navaids/{z}/{x}/{y}.png?apiKey=${config.OPENAIP_KEY}`, {
        attribution: "<a href=\"https://www.openaip.net\" target=\"_blank\" style=\"\">openAIP</a>"
    });

    /*
    var obstacles = new L.tileLayer(`https://api.tiles.openaip.net/api/data/obstacles/{z}/{x}/{y}.png?apiKey=${config.OPENAIP_KEY}`, {
        attribution: "<a href=\"https://www.openaip.net\" target=\"_blank\" style=\"\">openAIP</a>"
    });
    var hangglidings = new L.tileLayer(`https://api.tiles.openaip.net/api/data/hang-glidings/{z}/{x}/{y}.png?apiKey=${config.OPENAIP_KEY}`, {
        attribution: "<a href=\"https://www.openaip.net\" target=\"_blank\" style=\"\">openAIP</a>"
    });
    var hotspots = new L.tileLayer(`https://api.tiles.openaip.net/api/data/hotspots/{z}/{x}/{y}.png?apiKey=${config.OPENAIP_KEY}`, {
        attribution: "<a href=\"https://www.openaip.net\" target=\"_blank\" style=\"\">openAIP</a>"
    });
    */

    // AIRAC 2312 = europe, see https://www.openflightmaps.org/ed-germany/ changes monthly !!!

    var ofm = new L.tileLayer(`https://nwy-tiles-api.prod.newaydata.com/tiles/{z}/{x}/{y}.png?path=${config.AIRAC}/aero/latest`, {
        attribution: '<a target="_blank" href="http://openflightmaps.org">&copy; open flightmaps association</a>'
    })

    const baseMaps = {
        "OpenStreetMap": openStreetMap,
        "CartoDB Voyager": cartoDBlayer,
        "Topography": topoMap,
        "Stadia Outdoors": stadiaOutdoors,
        "Stadia Dark": StadiaDark
    }

    const overlayMaps = {
        "Airspaces": airspaces,
        "Reporting Points": reporting_points,
        "Airports": airports,
        "Navaids": navaids,

        "OFM Europe": ofm
    }

    map = L.map('map', {
        center: [51.505, -0.09],
        zoom: 13,
        layers: [cartoDBlayer],
        preferCanvas: true
    });

    const icon = L.icon({
        iconUrl: "/icons/airplane.svg",
        iconSize: [40, 40]
    });

    marker = L.marker([51.505, -0.09], {
        icon: icon,
        rotationAngle: 0,
        rotationOrigin: 'center center'
    }).addTo(map);

    trackline = L.polyline([], { color: 'red', smoothFactor: 3, opacity: 1.0 }).addTo(map);

    gpsWaypoints = L.polyline([], { color: '#c842f5', smoothFactor: 3, opacity: 1.0, dashArray: '20, 10' }).addTo(map);

    flightPathButton = L.easyButton({
        states: [{
            stateName: 'display-flight-path',
            icon: '<span style="padding-top:4px;" class="material-icons">timeline</span>',
            title: 'flight path shown',
            onClick: function (btn, map) {

                trackline.setStyle({ opacity: 0 });

                gpsWaypoints.setStyle({ opacity: 0 });

                btn.state('hide-flight-path');
            }
        }, {
            stateName: 'hide-flight-path',
            icon: '<span style="padding-top:4px;color:grey;" class="material-icons">timeline</span>',
            title: 'flight path hidden',
            onClick: function (btn, map) {

                trackline.setStyle({ opacity: 1.0 });

                gpsWaypoints.setStyle({ opacity: 1.0 });

                btn.state('display-flight-path');
            }
        }]
    }).addTo(map);

    followButton = L.easyButton({
        states: [{
            stateName: 'follow',
            icon: '<span style="padding-top:4px;" class="material-icons">flight</span>',
            title: 'aircraft followed',
            onClick: function (btn, map) {

                following = false;
                btn.state('dont-follow');
            }
        }, {
            stateName: 'dont-follow',
            icon: '<span style="padding-top:4px;color:grey;" class="material-icons">flight</span>',
            title: 'aircraft not followed',
            onClick: function (btn, map) {

                following = true;
                btn.state('follow');
            }
        }]
    }).addTo(map);

    map.on("dragstart", () => {
        following = false;

        followButton.state('dont-follow');
    });

    layerControl = L.control.layers(baseMaps, overlayMaps).addTo(map);

}

export function SetValues(
    latitude,
    longitude,
    heading,
    flightPlanActive,
    nextWPLatitude,
    nextWPLongitude,
    prevWPLatitude,
    prevWPLongitude) {

    //console.log(`${latitude} ${longitude} `);

    const newPos = new L.LatLng(latitude, longitude);

    marker.setLatLng(newPos);
    marker.setRotationAngle(heading);

    if (following) {
        map.setView(newPos);
    }

    // Trackline Update
    trackline.addLatLng([latitude, longitude]);

    // Trackline clear when distance between points > 2000m (MSFS places the plane in menu to 0,0)
    var tracklinelen = trackline.getLatLngs().length;
    if (tracklinelen > 1) {
        if (trackline.getLatLngs()[tracklinelen - 1].distanceTo(trackline.getLatLngs()[tracklinelen - 2]) > 2000) {
            trackline.setLatLngs([]);
            // Force Frequecy Sync
            //syncRadio();
        }
    };

    if (flightPlanActive && nextWPLatitude != 0 && nextWPLongitude != 0/* && prevWPLatitude != 0 && prevWPLongitude != 0*/) {

        gpsWaypoints.setLatLngs([[latitude, longitude], [nextWPLatitude, nextWPLongitude]]);

    } else {
        gpsWaypoints.setLatLngs([]);
    }
}
