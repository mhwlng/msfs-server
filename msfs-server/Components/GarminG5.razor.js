
var _bankDegrees;
var _pitchDegrees;
var _indicatedAltitude;
var _verticalSpeed;
var _airspeedIndicated;

var _autopilotMaster;
var _autoPilotAltitudeLockVar;
var _autopilotAltitudeLock;
var _gpsGroundSpeed;
var _kohlsmanSetting;
var _planeHeadingMagnetic;
var _autoPilotHeadingLockDir;
var _autopilotHeadingLock;
var _turnCoordinatorBall;
var _nav1CDI;
var _nav1GSI;

var selectedaltitudereached = false;

var _selectedaltitudereached;

var closeHdgLockDirBox;

var g5config = {
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

export function Init() {


    if (g5config.AIRSPEED_INDICATED?.speedBands?.length > 0) {
        g5config.AIRSPEED_INDICATED.speedBands.forEach(addSpeedband)
    }

    if (g5config.AIRSPEED_INDICATED?.markers?.length > 0) {
        g5config.AIRSPEED_INDICATED.markers.forEach(addMarker)
    }


}

export function SetValues(
    bankDegrees,
    pitchDegrees,
    indicatedAltitude,
    verticalSpeed,
    airspeedIndicated,

    autopilotMaster,
    autoPilotAltitudeLockVar,
    autopilotAltitudeLock,
    gpsGroundSpeed,
    kohlsmanSetting,
    planeHeadingMagnetic,
    autoPilotHeadingLockDir,
    autopilotHeadingLock,
    turnCoordinatorBall,
    nav1CDI,
    nav1GSI

) {

    var d3obj = d3.select(document.getElementById("garmin").contentDocument).select('svg');

    //console.log(bankDegrees, pitchDegrees, indicatedAltitude, verticalSpeed, airspeedIndicated);

    // credit https://github.com/joeherwig/portable-sim-panels

    var apaltlock = autoPilotAltitudeLockVar * -1;

    var aphdglockdir = Math.floor(autoPilotHeadingLockDir);

    var hdgValue = planeHeadingMagnetic === 0 ? 360 : planeHeadingMagnetic;

    var hdgtext = Math.abs(Math.floor(hdgValue % 360));
    if (hdgValue < 0) {
        hdgtext = 360 - Math.abs(Math.floor(hdgtext));
    }

    var vs = verticalSpeed > 0 ? verticalSpeed * -0.0752 + 746 : 746;

    var pitch = (pitchDegrees * 1 > 180) ? (pitchDegrees - 360) * -1 : pitchDegrees * - 1;

    if (_pitchDegrees != pitchDegrees || _bankDegrees != bankDegrees) { // PLANE_PITCH_DEGREES  "x * (360 / 65536 / 65536)"

        d3obj.select('#horizon-gradient').attr('gradientTransform',
            'rotate(' + bankDegrees + ', 500, 373.5) translate(0, ' + pitch * 22.825 + ')');
        d3obj.select('#pitch-ruler-fade-out-top').attr('gradientTransform',
            'translate(0 ' + pitch + '), rotate(' + bankDegrees + ' 500, 373.5)')
        d3obj.select('#pitch-ruler-fade-out-top').attr('gradientTransform',
            'translate(0 ' + pitch + '), rotate(' + bankDegrees + ' 500, 373.5)')
        d3obj.select('#pitch-ruler').attr('transform',
            'rotate(' + bankDegrees + ', 500, 373.5) translate(0, ' + pitch * 22.825 + ')');

    }

    if (_bankDegrees != bankDegrees) { // PLANE_BANK_DEGREES "x * (360 / 65536 / 65536) - 360"

        d3obj.select('#bank-indicator').attr('transform', 'rotate(' + bankDegrees + ', 500, 373.5)');
        //d3.select('#horizon-gradient').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
        //d3.select('#pitch-ruler-fade-out-top').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
    }


    if (_indicatedAltitude != indicatedAltitude || _autoPilotAltitudeLockVar != autoPilotAltitudeLockVar || _selectedaltitudereached != selectedaltitudereached) { // INDICATED_ALTITUDE

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

    if (_airspeedIndicated != airspeedIndicated) { // AIRSPEED_INDICATED "x / 128"

        d3obj.select('#cas-value tspan').text(Math.round(airspeedIndicated));
        d3obj.select('#cas-ruler').attr('transform', 'translate(0,' + airspeedIndicated * 12.364 + ')')
    }

    if (_gpsGroundSpeed != gpsGroundSpeed) { // GPS_GROUND_SPEED
        d3obj.select('#gs-value').text(Math.round(gpsGroundSpeed));
    }

    if (_kohlsmanSetting != kohlsmanSetting) { // KOHLSMAN_SETTING_MB
        d3obj.select('#baro-value').text(kohlsmanSetting.toFixed(0));
    }

    if (_planeHeadingMagnetic != planeHeadingMagnetic || _autoPilotHeadingLockDir != autoPilotHeadingLockDir) { // PLANE_HEADING_DEGREES_MAGNETIC  "x * (360 / 65536 / 65536)"

        d3obj.select('#hdg-ruler').attr('transform', 'translate(' + Math.round(0 - hdgValue * 15.4) + ', 300)');
        d3obj.select('#ap-selected-hdg-bug').attr('transform', 'translate(' + ((hdgValue - aphdglockdir) * -15.4) + ', 0)');
        d3obj.select('#hdg-value').text(hdgtext);
    }


    // what does this do ???
    if (_verticalSpeed != verticalSpeed) { // VERTICAL_SPEED

        d3obj.select('#vsi').attr('height', Math.abs(verticalSpeed * 0.0752));

        d3obj.select('#vsi').attr('transform', 'translate(0, ' + vs + ')');
    }

    if (_turnCoordinatorBall != turnCoordinatorBall) { // TURN_COORDINATOR_BALL

        d3obj.select('#slip-indicator-ball').attr('transform', 'translate(' + Math.round(turnCoordinatorBall * 1.2) + ', 0)');
    }


    if (_autopilotMaster != autopilotMaster || _autoPilotAltitudeLockVar != autoPilotAltitudeLockVar || _autopilotHeadingLock != autopilotHeadingLock) { // AUTOPILOT_MASTER

        if (autopilotHeadingLock && autopilotMaster) {
            d3obj.select('#ap-selected-hdg-bug').style('display', 'block');
            d3obj.select('#ap-hdg-active-sign').style('display', 'block');
            d3obj.select('#hdg-lock-indicator').style('display', 'block');
        } else {
            d3obj.select('#ap-selected-hdg-bug').style('display', 'none');
            d3obj.select('#ap-hdg-active-sign').style('display', 'none');
            d3obj.select('#hdg-lock-indicator').style('display', 'none');
        }
        if (apaltlock && autopilotMaster) {
            d3obj.select('#ap-selected-alt').style("display", "block");
            d3obj.select('#ap-alt-active-sign').style("display", "block");
        } else {
            d3obj.select('#ap-selected-alt').style("display", "none");
            d3obj.select('#ap-alt-active-sign').style("display", "none");
        }
    }

    if (_autoPilotAltitudeLockVar != autoPilotAltitudeLockVar) { //AUTOPILOT_ALTITUDE_LOCK_VAR "x * 3.28084 / 65536"

        selectedaltitudereached = false;

        d3obj.select('#ap-alt-value').style('fill', '#0bbbbb')

        d3obj.select('#ap-alt-value').text(Math.round(autoPilotAltitudeLockVar));
    }

    if (_autopilotAltitudeLock != autopilotAltitudeLock || _autoPilotAltitudeLockVar != autoPilotAltitudeLockVar || _autopilotMaster != autopilotMaster) { // AUTOPILOT_ALTITUDE_LOCK

        if (autopilotAltitudeLock && apaltlock && autopilotMaster) {
            d3obj.select('#ap-selected-alt').style("display", "block");
            d3obj.select('#ap-alt-active-sign').style("display", "block");
        } else {
            d3obj.select('#ap-selected-alt').style("display", "none");
            d3obj.select('#ap-alt-active-sign').style("display", "none");
        }

    }

    if (_autoPilotHeadingLockDir != autoPilotHeadingLockDir) { // AUTOPILOT_HEADING_LOCK_DIR  "x / 65536 * 360"

        clearTimeout(closeHdgLockDirBox);

        d3obj.select('#ap-hdg-selected-value').text(aphdglockdir + '°');
        d3obj.select('#ap-hdg-selected-box').style('display', 'block');

        closeHdgLockDirBox = setTimeout(function () {
            d3obj.select('#ap-hdg-selected-box').style('display', 'none');
        }, 3000);

    }

    if (_autopilotHeadingLock != autopilotHeadingLock || _autoPilotHeadingLockDir != autoPilotHeadingLockDir || _autopilotMaster != autopilotMaster) { // AUTOPILOT_HEADING_LOCK

        if (autopilotHeadingLock && aphdglockdir && autopilotMaster) {
            d3obj.select('#ap-selected-hdg-bug').style('display', 'block');
            d3obj.select('#ap-hdg-active-sign').style('display', 'block');
            d3obj.select('#hdg-lock-indicator').style('display', 'block');
        } else {
            d3obj.select('#ap-selected-hdg-bug').style('display', 'none');
            d3obj.select('#ap-hdg-active-sign').style('display', 'none');
            d3obj.select('#hdg-lock-indicator').style('display', 'none');
        }
    }


    if (_nav1CDI != nav1CDI) { // NAV_1_CDI

        d3obj.select('#cdi').attr('transform', 'translate(' + nav1CDI * 1.07 + ', 0)');
    }


    if (_nav1GSI != nav1GSI) { // NAV_1_GSI

        d3obj.select('#gsi').attr('transform', 'translate(0, ' + nav1GSI * 0.96 + ')');
    }

    _bankDegrees = bankDegrees;
    _pitchDegrees = pitchDegrees;
    _indicatedAltitude = indicatedAltitude;
    _verticalSpeed = verticalSpeed;
    _airspeedIndicated = airspeedIndicated;

    _gpsGroundSpeed = gpsGroundSpeed;
    _planeHeadingMagnetic = planeHeadingMagnetic;

    _autopilotMaster = autopilotMaster;
    _autoPilotAltitudeLockVar = autoPilotAltitudeLockVar;
    _autopilotAltitudeLock = autopilotAltitudeLock;
    _kohlsmanSetting = kohlsmanSetting;
    _autoPilotHeadingLockDir = autoPilotHeadingLockDir;
    _autopilotHeadingLock = autopilotHeadingLock;
    _turnCoordinatorBall = turnCoordinatorBall;
    _nav1CDI = nav1CDI;
    _nav1GSI = nav1GSI;

    _selectedaltitudereached = selectedaltitudereached;

}