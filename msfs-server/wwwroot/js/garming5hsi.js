

var _g5hsigpsGroundSpeed;
var _g5hsiplaneHeadingMagnetic;
var _g5hsinavCDI;
var _g5hsinavGSI;
var _g5hsinavOBS
var _g5hsiautoPilotHeadingLockDir;
var _g5hsiautopilotHeadingLock;
var _g5hsiautopilotMaster;

var g5hsicloseHdgLockDirBox;

function setG5hsiSelectedValueBoxContent(label, value) {

    var d3obj = d3.select(document.getElementById("garminhsi").contentDocument).select('svg');
    
    d3obj.select('#selected-value-box').text(label);
    d3obj.select('#ap-hdg-selected-value').text(value);
    d3obj.select('#ap-hdg-selected-box').style('display', 'block');

    g5hsicloseHdgLockDirBox = setTimeout(function () {
        d3obj.select('#ap-hdg-selected-box').style('display', 'none');
    }, 3000);
}


export function InitG5HSI() {

}


export function SetG5HSIValues(
    gpsGroundSpeed,
    planeHeadingMagnetic,
    navOBS,
    navCDI,
    navGSI,
    autoPilotHeadingLockDir,
    autopilotHeadingLock,
    autopilotMaster
    
    ) {

    var d3obj = d3.select(document.getElementById("garminhsi").contentDocument).select('svg');

    var hdgValue = planeHeadingMagnetic === 0 ? 360 : planeHeadingMagnetic;

    var hdgValue2 = hdgValue % 360;

    var hdgtext = Math.abs(Math.floor(hdgValue % 360));
    if (hdgValue < 0) {
        hdgtext = 360 - Math.abs(Math.floor(hdgtext));
    }

    var obsValue = Math.floor(navOBS);

    var aphdglockdir = Math.floor(autoPilotHeadingLockDir);
    
    if (_g5hsigpsGroundSpeed != gpsGroundSpeed) { // GPS_GROUND_SPEED
        d3obj.select('#gs-value').text(Math.round(gpsGroundSpeed));
    }
    
    if (_g5hsiplaneHeadingMagnetic != planeHeadingMagnetic || _g5hsinavOBS != navOBS) { // PLANE_HEADING_DEGREES_MAGNETIC  "x * (360 / 65536 / 65536)"

        d3obj.select('#compass-rose').attr('transform', 'rotate(' + hdgValue * -1 + ', 516.98242, 422)');
        d3obj.select('#cdi-set').attr('transform', 'rotate(' + (hdgValue2 * 1 - obsValue * 1) * -1 + ', 516.98242, 422)');
        d3obj.select('#hdg-value').text(hdgtext);
    }

    if (_g5hsinavOBS != navOBS) { // NAV_1_OBS

        clearTimeout(g5hsicloseHdgLockDirBox);

        d3obj.select('#cdi-set').attr('transform', 'rotate(' + (hdgValue2 * 1 - obsValue * 1) * -1 + ', 516.98242, 422)');

        setG5hsiSelectedValueBoxContent('Selected CRS', obsValue + '°');
    }

    if (_g5hsinavCDI != navCDI) { // NAV_1_CDI

        d3obj.select('#cdi').attr('transform', 'translate(' + navCDI * 1.07 + ', 0)');
    }
    
    if (_g5hsinavGSI != navGSI) { // NAV_1_GSI

        d3obj.select('#gsi').attr('transform', 'translate(0, ' + navGSI * 0.96 + ')');
    }
    
    if (_g5hsiautoPilotHeadingLockDir != autoPilotHeadingLockDir) {

       setG5hsiSelectedValueBoxContent('Selected Heading', aphdglockdir + '°');
    }

    if (_g5hsiautopilotHeadingLock != autopilotHeadingLock || _g5hsiautoPilotHeadingLockDir != autoPilotHeadingLockDir || _g5hsiautopilotMaster != autopilotMaster) {

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
   
    _g5hsigpsGroundSpeed = gpsGroundSpeed;
    _g5hsiplaneHeadingMagnetic = planeHeadingMagnetic;
    _g5hsinavCDI = navCDI;
    _g5hsinavGSI = navGSI;
    _g5hsinavOBS = navOBS;
    _g5hsiautoPilotHeadingLockDir = autoPilotHeadingLockDir;
    _g5hsiautopilotHeadingLock = autopilotHeadingLock;
    _g5hsiautopilotMaster = autopilotMaster;

}