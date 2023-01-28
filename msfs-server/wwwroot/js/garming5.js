
var _bankDegrees = 0;
var _pitchDegrees = 0;
var _indicatedAltitude = 0;
var _verticalSpeed = 0;
var _airspeedIndicated = 0;

var _autopilotMaster = 0;
var _autoPilotAltitudeLockVar = -1;
var _autopilotAltitudeLock = 0;
var _gpsGroundSpeed = 0;
var _kohlsmanSetting = 0;
var _planeHeadingMagnetic = 0;
var _autoPilotHeadingLockDir = -1;
var _autopilotHeadingLock = 0;
var _turnCoordinatorBall = 0;
var _navCDI = 0;
var _navGSI = 0;

var selectedaltitudereached = false;

var closeHdgLockDirBox;

var apaltlock = -999;

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

export function SetG5Values(
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
    navCDI,
    navGSI
    
    ) {

    var d3obj = d3.select(document.getElementById("garmin").contentDocument).select('svg');
    
    console.log("autopilotMaster",autopilotMaster);
    console.log("autoPilotAltitudeLockVar",autoPilotAltitudeLockVar);
    console.log("autopilotAltitudeLock", autopilotAltitudeLock);
    console.log("autopilotHeadingLock", autopilotHeadingLock);
    console.log("autoPilotHeadingLockDir", autoPilotHeadingLockDir);

    console.log("turnCoordinatorBall", turnCoordinatorBall);

    console.log("navCDI", navCDI);
    console.log("navGSI", navGSI);

    //console.log(bankDegrees, pitchDegrees, indicatedAltitude, verticalSpeed, airspeedIndicated);

    // credit https://github.com/joeherwig/portable-sim-panels

    var aphdglockdir = Math.floor(autoPilotHeadingLockDir);
    
    if (_pitchDegrees != pitchDegrees || _bankDegrees != bankDegrees) { // PLANE_PITCH_DEGREES

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

    if (_bankDegrees != bankDegrees) { // PLANE_BANK_DEGREES

        d3obj.select('#bank-indicator').attr('transform', 'rotate(' + bankDegrees + ', 500, 373.5)');
        //d3.select('#horizon-gradient').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
        //d3.select('#pitch-ruler-fade-out-top').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
    }

   
    if (_indicatedAltitude != indicatedAltitude) { // INDICATED_ALTITUDE

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
    
    if (_airspeedIndicated != airspeedIndicated) { // AIRSPEED_INDICATED

        d3obj.select('#cas-value tspan').text(Math.round(airspeedIndicated));
        d3obj.select('#cas-ruler').attr('transform', 'translate(0,' + airspeedIndicated * 12.364 + ')')
    }

    if (_gpsGroundSpeed != gpsGroundSpeed) { // GPS_GROUND_SPEED
        d3obj.select('#gs-value').text(Math.round(gpsGroundSpeed));
    }

    if (_kohlsmanSetting != kohlsmanSetting) { // KOHLSMAN_SETTING_MB
        d3obj.select('#baro-value').text(kohlsmanSetting.toFixed(0));
    }
    
    if (_planeHeadingMagnetic != planeHeadingMagnetic) { // PLANE_HEADING_DEGREES_MAGNETIC

        var HdgValue = planeHeadingMagnetic === 0 ? 360 : planeHeadingMagnetic;

        varhdgtext = Math.abs(Math.floor(HdgValue % 360));
        if (HdgValue < 0) {
            hdgtext = 360 - Math.abs(Math.floor(hdgtext));
        }
        d3obj.select('#hdg-ruler').attr('transform', 'translate(' + Math.round(0 - HdgValue * 15.4) + ', 300)');
        d3obj.select('#ap-selected-hdg-bug').attr('transform', 'translate(' + ((HdgValue - aphdglockdir) * -15.4) + ', 0)');
        d3obj.select('#hdg-value').text(hdgtext);
    }

    
    // what does this do ???
    if (_verticalSpeed != verticalSpeed) { // VERTICAL_SPEED

        d3obj.select('#vsi').attr('height', Math.abs(verticalSpeed * 0.0752));

        var vs = verticalSpeed > 0 ? verticalSpeed * -0.0752 + 746 : 746;

        d3obj.select('#vsi').attr('transform', 'translate(0, ' + vs + ')');
    }

    if (_turnCoordinatorBall != turnCoordinatorBall) { // TURN_COORDINATOR_BALL

        d3obj.select('#slip-indicator-ball').attr('transform', 'translate(' + Math.round(turnCoordinatorBall * 1.2) + ', 0)');
    }


    if (_autopilotMaster != autopilotMaster) { // AUTOPILOT_MASTER

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

    if (_autoPilotAltitudeLockVar != autoPilotAltitudeLockVar) { //AUTOPILOT_ALTITUDE_LOCK_VAR

        if (apaltlock !== autoPilotAltitudeLockVar * -1) {
            selectedaltitudereached = false;
            d3obj.select('#ap-alt-value').style('fill', '#0bbbbb')
        }

        apaltlock = autoPilotAltitudeLockVar * -1;

        d3obj.select('#ap-alt-value').text(Math.round(autoPilotAltitudeLockVar));
    }

    if (_autopilotAltitudeLock != autopilotAltitudeLock) { // AUTOPILOT_ALTITUDE_LOCK

        if (autopilotAltitudeLock && apaltlock && autopilotMaster) {
            d3obj.select('#ap-selected-alt').style("display", "block");
            d3obj.select('#ap-alt-active-sign').style("display", "block");
        } else {
            d3obj.select('#ap-selected-alt').style("display", "none");
            d3obj.select('#ap-alt-active-sign').style("display", "none");
        }

    }

    if (_autoPilotHeadingLockDir != autoPilotHeadingLockDir) { // AUTOPILOT_HEADING_LOCK_DIR

        clearTimeout(closeHdgLockDirBox);
        d3obj.select('#ap-hdg-selected-value').text(aphdglockdir + '°');
        d3obj.select('#ap-hdg-selected-box').style('display', 'block');

        closeHdgLockDirBox = setTimeout(function () {
            d3obj.select('#ap-hdg-selected-box').style('display', 'none');
        }, 3000);

    }

    if (_autopilotHeadingLock != autopilotHeadingLock) { // AUTOPILOT_HEADING_LOCK

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


    // doesn't work ???
    if (_navCDI != navCDI) { // NAV_1_CDI

        d3obj.select('#cdi').attr('transform', 'translate(' + navCDI * 1.07 + ', 0)');
    }


    // doesn't work ???
    if (_navGSI != navGSI) { // NAV_1_GSI

        d3obj.select('#gsi').attr('transform', 'translate(0, ' + navGSI * 0.96 + ')');
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
    _navCDI = navCDI;
    _navGSI = navGSI;

}