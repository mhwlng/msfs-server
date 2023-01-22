var _bankDegrees = 0;
var _pitchDegrees = 0;
var _indicatedAltitude = 0;

var selectedaltitudereached = false;
var apaltlock = 0;

export function InitG5() {

}

export function SetG5Values(bankDegrees, pitchDegrees, indicatedAltitude) {

    console.log(bankDegrees, pitchDegrees, indicatedAltitude);
    var d3obj = d3.select(document.getElementById("garmin").contentDocument).select('svg');

    // credit https://github.com/joeherwig/portable-sim-panels

    if (_bankDegrees != bankDegrees) {

        _bankDegrees = bankDegrees;

        var bank = bankDegrees;

        d3obj.select('#bank-indicator').attr('transform', 'rotate(' + bank + ', 500, 373.5)');
        //d3.select('#horizon-gradient').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
        //d3.select('#pitch-ruler-fade-out-top').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
    }

    if (_pitchDegrees != pitchDegrees) {

        _pitchDegrees = pitchDegrees;

        var pitch = (pitchDegrees * 1 > 180) ? (pitchDegrees - 360) * -1 : pitchDegrees * - 1;

        d3obj.select('#horizon-gradient').attr('gradientTransform',
            'rotate(' + bank + ', 500, 373.5) translate(0, ' + pitch * 22.825 + ')');
        d3obj.select('#pitch-ruler-fade-out-top').attr('gradientTransform',
            'translate(0 ' + pitch + '), rotate(' + bank + ' 500, 373.5)')
        d3obj.select('#pitch-ruler-fade-out-top').attr('gradientTransform',
            'translate(0 ' + pitch + '), rotate(' + bank + ' 500, 373.5)')
        d3obj.select('#pitch-ruler').attr('transform',
            'rotate(' + bank + ', 500, 373.5) translate(0, ' + pitch * 22.825 + ')');
    }

    if (_indicatedAltitude != indicatedAltitude) {

        _indicatedAltitude = indicatedAltitude;

        var currentalt = indicatedAltitude;

        if (currentalt > 1000) {
            d3obj.select('#alt-value-thousands').text(Math.floor(currentalt / 1000));
            d3obj.select('#alt-value-hundreds').text(('00' + Math.round(currentalt) % 1000).slice(-3));
        } else {
            d3obj.select('#alt-value-thousands').text('');
            d3obj.select('#alt-value-hundreds').text((Math.floor(currentalt) % 1000));
        }
        d3obj.select('#alt-ruler-complete').attr('transform', 'translate(0,' + currentalt * 1.8545 + ')')
        d3obj.select('#ap-selected-alt').attr('transform', 'translate(0,' + (currentalt * 1 + apaltlock * 1) * 1.8545 + ')')

        if (Math.abs(currentalt + apaltlock) <= 30) {
            d3obj.select('#ap-alt-value').style('fill', '#0bbbbb')
            selectedaltitudereached = true;
        }
        if (Math.abs(currentalt + apaltlock) > 200 && selectedaltitudereached) {
            d3obj.select('#ap-alt-value').style('fill', 'yellow')   // selected ap alt deviation warning
        }
    }

}