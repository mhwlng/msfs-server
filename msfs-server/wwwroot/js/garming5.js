

export function InitG5() {

}

export function SetG5Values(bankdegrees, pitchdegrees) {

    var d3obj = d3.select(document.getElementById("garmin").contentDocument).select('svg');

    // credit https://github.com/joeherwig/portable-sim-panels

    var bank = bankdegrees;

    d3obj.select('#bank-indicator').attr('transform', 'rotate(' + bank + ', 500, 373.5)');
    //d3.select('#horizon-gradient').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
    //d3.select('#pitch-ruler-fade-out-top').attr('gradientTransform','translate(0 '+pitch+'), rotate('+bank+' 500, 373.5)')
    
    var pitch = (pitchdegrees * 1 > 180) ? (pitchdegrees - 360) * -1 : pitchdegrees * - 1;
    d3obj.select('#horizon-gradient').attr('gradientTransform', 'rotate(' + bank + ', 500, 373.5) translate(0, ' + pitch * 22.825 + ')');
    d3obj.select('#pitch-ruler-fade-out-top').attr('gradientTransform', 'translate(0 ' + pitch + '), rotate(' + bank + ' 500, 373.5)')
    d3obj.select('#pitch-ruler-fade-out-top').attr('gradientTransform', 'translate(0 ' + pitch + '), rotate(' + bank + ' 500, 373.5)')
    d3obj.select('#pitch-ruler').attr('transform', 'rotate(' + bank + ', 500, 373.5) translate(0, ' + pitch * 22.825 + ')');

}