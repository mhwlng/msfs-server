

var _autopilotMaster;

var _autopilotAltitudeLock;

var _autopilotHeadingLock;

var _autopilotNav1Lock;

var _autopilotFlightDirectorActive;

var _autopilotBackcourseHold;

var _autopilotVerticalHold;

var _autopilotYawDamper;

var _autopilotApproachHold;

var ap;

function setValue(id, state) {

    var d3obj = d3.select(document.getElementById("garminapbar").contentDocument).select('svg');

    let fillColor = (state * 1 ? '#00ff00' : '#333333')
    d3obj.select('#' + id + '').style('fill', fillColor)
    if (id === 'ap' && !ap) {
        setValue('alt', 0)
        setValue('hdg', 0)
        setValue('vs', 0)
        setValue('fd', 0)
        setValue('nav', 0)
        setValue('yd', 0)
        setValue('apr', 0)
        setValue('bc', 0)
    }
}

export function Init() {

}

export function SetValues(
    autopilotMaster,
    autopilotAltitudeLock,
    autopilotHeadingLock,
    autopilotNav1Lock,
    autopilotFlightDirectorActive,
    autopilotBackcourseHold,
    autopilotVerticalHold,
    autopilotYawDamper,
    autopilotApproachHold
) {

    // credit https://github.com/joeherwig/portable-sim-panels

    var d3obj = d3.select(document.getElementById("garminapbar").contentDocument).select('svg');

    ap = autopilotMaster * 1;

    if (_autopilotMaster != autopilotMaster) {

        setValue('ap', ap);
    }

    if (_autopilotHeadingLock != autopilotHeadingLock || _autopilotMaster != autopilotMaster) {

        var hdg = autopilotHeadingLock && ap ? autopilotHeadingLock * 1 : 0
        ap && hdg ? setValue('hdg', hdg) : setValue('hdg', 0);
    }

    if (_autopilotAltitudeLock != autopilotAltitudeLock || _autopilotMaster != autopilotMaster) {

        var alt = autopilotAltitudeLock && ap ? autopilotAltitudeLock * 1 : 0
        ap && alt ? setValue('alt', alt) : setValue('alt', 0);
    }

    if (_autopilotNav1Lock != autopilotNav1Lock || _autopilotMaster != autopilotMaster) {

        var nav = autopilotNav1Lock && ap ? autopilotNav1Lock * 1 : 0
        ap && nav ? setValue('nav', nav) : setValue('nav', 0);
    }

    if (_autopilotFlightDirectorActive != autopilotFlightDirectorActive || _autopilotMaster != autopilotMaster) {

        var fd = autopilotFlightDirectorActive && ap ? autopilotFlightDirectorActive * 1 : 0
        ap && fd ? setValue('fd', fd) : setValue('fd', 0);

    }
    if (_autopilotBackcourseHold != autopilotBackcourseHold || _autopilotMaster != autopilotMaster) {

        var bc = autopilotBackcourseHold && ap ? autopilotBackcourseHold * 1 : 0
        ap && bc ? setValue('bc', bc) : setValue('bc', 0);
    }

    if (_autopilotVerticalHold != autopilotVerticalHold || _autopilotMaster != autopilotMaster) {

        var vs = autopilotVerticalHold && ap ? autopilotVerticalHold * 1 : 0
        ap && vs ? setValue('vs', vs) : setValue('vs', 0);
    }

    if (_autopilotYawDamper != autopilotYawDamper || _autopilotMaster != autopilotMaster) {

        var yd = autopilotYawDamper && ap ? autopilotYawDamper * 1 : 0
        ap && yd ? setValue('yd', yd) : setValue('yd', 0);
    }
    if (_autopilotApproachHold != autopilotApproachHold || _autopilotMaster != autopilotMaster) {

        var apr = autopilotApproachHold && ap ? autopilotApproachHold * 1 : 0
        ap && apr ? setValue('apr', apr) : setValue('apr', 0);
    }

    _autopilotMaster = autopilotMaster;

    _autopilotAltitudeLock = autopilotAltitudeLock;

    _autopilotHeadingLock = autopilotHeadingLock;

    _autopilotNav1Lock = autopilotNav1Lock;

    _autopilotFlightDirectorActive = autopilotFlightDirectorActive;

    _autopilotBackcourseHold = autopilotBackcourseHold;

    _autopilotVerticalHold = autopilotVerticalHold;

    _autopilotYawDamper = autopilotYawDamper;

    _autopilotApproachHold = autopilotApproachHold;


}