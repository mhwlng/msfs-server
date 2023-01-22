var _bankDegrees = 0;
var _pitchDegrees = 0;
var _indicatedAltitude = 0;
var _verticalSpeed = 0;
var _airspeedIndicated = 0;

var selectedaltitudereached = false;
var apaltlock = 0;


config = {
    "AIRSPEED_INDICATED": {
        "ranges": [
            {
                "color": "white",
                "min": 40,
                "max": 70,
                "width": "3"
            },
            {
                "color": "green",
                "min": 41,
                "max": 117,
                "width": "3"
            }
        ],
        "whitebarstart": 40,
        "whitebarend": 70,
        "greenbarstart": 41,
        "greenbarend": 117,
        "yellowbarstart": 118,
        "yellowbarend": 144,
        "markers": [
            { "type": "fe", "value": 70.387 },
            { "type": "vne", "value": 145 },
            { "type": "x", "value": 51.2695 },
            { "type": "y", "value": 41 }
        ]
    }
}

function addSpeedband(item, index) {

    var d3obj = d3.select(document.getElementById("garmin").contentDocument).select('svg');

    d3obj.select('#cas-ruler')
        .append("svg:rect")
        .attr("id", "cas-speedband-" + item.color)
        .attr('height', (item.max - item.min) * 12.364)
        .attr("width", item.width * 4)
        .attr('style', 'fill: ' + item.color)
        .attr('x', (205))
        .attr('y', (373.74 - (item.max) * 12.364))
}

function addMarker(item, index) {

    var d3obj = d3.select(document.getElementById("garmin").contentDocument).select('svg');

    var marker = d3obj.select('#cas-ruler')
        .append("svg:g").attr("transform", "translate(5," + item.value * -12.364 + ")")
        .attr("id", "casMarker-" + item.type)

    marker
        .append("svg:path")
        .attr("d", "m 218.6751,358.68181 h 37.3004 v 29.8004 h -37.3004 l -13.6972,-14.9002 z")

    marker.append("svg:text").attr("x", "235").attr("y", "382").text(item.type)

}

export function InitG5() {


    if (config.AIRSPEED_INDICATED?.speedBands?.length > 0) {
        config.AIRSPEED_INDICATED.speedBands.forEach(addSpeedband)
    }

    if (config.AIRSPEED_INDICATED?.markers?.length > 0) {
        config.AIRSPEED_INDICATED.markers.forEach(addMarker)
    }
   

}

export function SetG5Values(bankDegrees, pitchDegrees, indicatedAltitude, verticalSpeed, airspeedIndicated) {

    var d3obj = d3.select(document.getElementById("garmin").contentDocument).select('svg');


    //console.log(bankDegrees, pitchDegrees, indicatedAltitude, verticalSpeed, airspeedIndicated);
    
    // credit https://github.com/joeherwig/portable-sim-panels

    if (_pitchDegrees != pitchDegrees || _bankDegrees != bankDegrees) {

        _pitchDegrees = pitchDegrees;

        var pitch = (pitchDegrees * 1 > 180) ? (pitchDegrees - 360) * -1 : pitchDegrees * - 1;

        d3obj.select('#horizon-gradient').attr('gradientTransform',
            'rotate(' + bankDegrees + ', 500, 373.5) translate(0, ' + pitch * 22.825 + ')');
        d3obj.select('#pitch-ruler-fade-out-top').attr('gradientTransform',
            'translate(0 ' + pitch + '), rotate(' + bankDegrees + ' 500, 373.5)')
        d3obj.select('#pitch-ruler-fade-out-top').attr('gradientTransform',
            'translate(0 ' + pitch + '), rotate(' + bankDegrees + ' 500, 373.5)')
        d3obj.select('#pitch-ruler').attr('transform',
            'rotate(' + bankDegrees + ', 500, 373.5) translate(0, ' + pitch * 22.825 + ')');

    }

    if (_bankDegrees != bankDegrees) {

        _bankDegrees = bankDegrees;

        d3obj.select('#bank-indicator').attr('transform', 'rotate(' + bankDegrees + ', 500, 373.5)');
        //d3.select('#horizon-gradient').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
        //d3.select('#pitch-ruler-fade-out-top').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
    }

   
    if (_indicatedAltitude != indicatedAltitude) {

        _indicatedAltitude = indicatedAltitude;

        if (indicatedAltitude > 1000) {
            d3obj.select('#alt-value-thousands').text(Math.floor(indicatedAltitude / 1000));
            d3obj.select('#alt-value-hundreds').text(('00' + Math.round(indicatedAltitude) % 1000).slice(-3));
        } else {
            d3obj.select('#alt-value-thousands').text('');
            d3obj.select('#alt-value-hundreds').text((Math.floor(indicatedAltitude) % 1000));
        }
        d3obj.select('#alt-ruler-complete').attr('transform', 'translate(0,' + indicatedAltitude * 1.8545 + ')')
        d3obj.select('#ap-selected-alt').attr('transform', 'translate(0,' + (indicatedAltitude * 1 + apaltlock * 1) * 1.8545 + ')')

        if (Math.abs(indicatedAltitude + apaltlock) <= 30) {
            d3obj.select('#ap-alt-value').style('fill', '#0bbbbb')
            selectedaltitudereached = true;
        }
        if (Math.abs(indicatedAltitude + apaltlock) > 200 && selectedaltitudereached) {
            d3obj.select('#ap-alt-value').style('fill', 'yellow')   // selected ap alt deviation warning
        }
    }
    
    if (_airspeedIndicated != airspeedIndicated) {

        _airspeedIndicated = airspeedIndicated;

        d3obj.select('#cas-value tspan').text(Math.round(airspeedIndicated));
        d3obj.select('#cas-ruler').attr('transform', 'translate(0,' + airspeedIndicated * 12.364 + ')')
    }

    // what does this do ???
    if (_verticalSpeed != verticalSpeed) {

        _verticalSpeed = verticalSpeed;

        d3obj.select('#vsi').attr('height', Math.abs(verticalSpeed * 0.0752));

        var vs = verticalSpeed > 0 ? verticalSpeed * -0.0752 + 746 : 746;

        d3obj.select('#vsi').attr('transform', 'translate(0, ' + vs + ')');

    }


}