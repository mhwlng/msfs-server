

var _gpsGroundSpeed;
var _planeHeadingMagnetic;
var _nav1CDI;
var _nav1GSI;
var _nav1OBS
var _autoPilotHeadingLockDir;
var _autopilotHeadingLock;
var _autopilotMaster;

var closeHdgLockDirBox;

function setSelectedValueBoxContent(label, value) {

    var d3obj = d3.select(document.getElementById("garminhsi").contentDocument).select('svg');

    d3obj.select('#selected-value-box').text(label);
    d3obj.select('#ap-hdg-selected-value').text(value);
    d3obj.select('#ap-hdg-selected-box').style('display', 'block');

    closeHdgLockDirBox = setTimeout(function () {
        d3obj.select('#ap-hdg-selected-box').style('display', 'none');
    }, 3000);
}


export function Init() {

}


export function SetValues(
    gpsGroundSpeed,
    planeHeadingMagnetic,
    nav1OBS,
    nav1CDI,
    nav1GSI,
    autoPilotHeadingLockDir,
    autopilotHeadingLock,
    autopilotMaster

) {

    // credit https://github.com/joeherwig/portable-sim-panels
    
    var d3obj = d3.select(document.getElementById("garminhsi").contentDocument).select('svg');

    var hdgValue = planeHeadingMagnetic === 0 ? 360 : planeHeadingMagnetic;

    var hdgValue2 = hdgValue % 360;

    var hdgtext = Math.abs(Math.floor(hdgValue % 360));
    if (hdgValue < 0) {
        hdgtext = 360 - Math.abs(Math.floor(hdgtext));
    }

    var obsValue = Math.floor(nav1OBS);

    var aphdglockdir = Math.floor(autoPilotHeadingLockDir);

    if (_gpsGroundSpeed != gpsGroundSpeed) { // GPS_GROUND_SPEED
        d3obj.select('#gs-value').text(Math.round(gpsGroundSpeed));
    }

    if (_planeHeadingMagnetic != planeHeadingMagnetic || _nav1OBS != nav1OBS) { // PLANE_HEADING_DEGREES_MAGNETIC  "x * (360 / 65536 / 65536)"

        d3obj.select('#compass-rose').attr('transform', 'rotate(' + hdgValue * -1 + ', 516.98242, 422)');
        d3obj.select('#cdi-set').attr('transform', 'rotate(' + (hdgValue2 * 1 - obsValue * 1) * -1 + ', 516.98242, 422)');
        d3obj.select('#hdg-value').text(hdgtext);
    }

    if (_nav1OBS != nav1OBS) { // NAV_1_OBS

        clearTimeout(closeHdgLockDirBox);


        setSelectedValueBoxContent('Selected CRS', obsValue + '°');
    }

    if (_nav1CDI != nav1CDI) { // NAV_1_CDI

        d3obj.select('#cdi').attr('transform', 'translate(' + nav1CDI * 1.07 + ', 0)');
    }

    if (_nav1GSI != nav1GSI) { // NAV_1_GSI

        d3obj.select('#gsi').attr('transform', 'translate(0, ' + nav1GSI * 0.96 + ')');
    }

    if (_autoPilotHeadingLockDir != autoPilotHeadingLockDir) {

        setSelectedValueBoxContent('Selected Heading', aphdglockdir + '°');
    }

    if (_autopilotHeadingLock != autopilotHeadingLock || _autoPilotHeadingLockDir != autoPilotHeadingLockDir || _autopilotMaster != autopilotMaster) {

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

    _gpsGroundSpeed = gpsGroundSpeed;
    _planeHeadingMagnetic = planeHeadingMagnetic;
    _nav1CDI = nav1CDI;
    _nav1GSI = nav1GSI;
    _nav1OBS = nav1OBS;
    _autoPilotHeadingLockDir = autoPilotHeadingLockDir;
    _autopilotHeadingLock = autopilotHeadingLock;
    _autopilotMaster = autopilotMaster;

}