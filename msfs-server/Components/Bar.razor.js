
var barranges = {}

async function loadRanges(
    fileName,
    d3obj,
    rangeMin,
    rangeMax,
    color,
    rulerWidth) {

    var fullrange = rangeMax - rangeMin;
    var stepwidth = 150 / fullrange;
    var zeroposition = rangeMin * stepwidth * -1;

    const response = await fetch(`/config/barranges/${fileName}.json`, { cache: "no-store" });

    barranges = await response.json();

    barranges.ranges.forEach(function (range, i) {
        let rangemin = (range.minValue * stepwidth) + zeroposition;
        let rangemax = (range.maxValue * stepwidth) + zeroposition;
        d3obj.append("rect")
            .attr("class", 'range-' + i)
            .attr("fill", range.color)
            .attr("x", rangemin)
            .attr("y", 48 - range.width)
            .attr("width", rangemax - rangemin)
            .attr("height", range.width);
    })
    
    d3obj.append("rect")
        .attr("class", "ruler")
        .attr("fill", color)
        .attr("x", 0)
        .attr("y", 48)
        .attr("width", 150)
        .attr("height", rulerWidth);
    d3obj.append("rect")
        .attr("class", "rulerBarStart")
        .attr("fill", color)
        .attr("x", 0)
        .attr("y", 33)
        .attr("width", rulerWidth)
        .attr("height", 15);
    d3obj.append("rect")
        .attr("class", "rulerBarEnd")
        .attr("fill", color)
        .attr("x", 150 - rulerWidth)
        .attr("y", 33)
        .attr("width", rulerWidth)
        .attr("height", 15);
    d3obj.append("path")
        .attr("class", "needle")
        .attr("id", "needle")
        .attr("fill", "white")
        .attr("stroke", "black")
        .attr("stroke-width", "2px")
        .attr("d", "M -10,30 0,45 10,30 Z")
        .attr("inkscape:connector-curvature", 33);

    d3obj.select(".ruler").attr("fill", color);
    d3obj.select(".rulerBarStart").attr("fill", color);
    d3obj.select(".rulerBarEnd").attr("fill", color);
}

export function Init(
    id,
    label,
    rangeMin,
    rangeMax,
    rulerWidth,
    color,
    barRangesFileName) {

    var d3obj = d3.select(document.getElementById(id).contentDocument).select('svg');

    d3obj.select('#label').text(label);

    loadRanges(
        barRangesFileName,
        d3obj,
        rangeMin,
        rangeMax,
        color,
        rulerWidth);
    
}

export function SetValues(
    id,
    rangeMin,
    rangeMax,
    color,
    value
) {

    // credit https://github.com/joeherwig/portable-sim-panels

    var d3obj = d3.select(document.getElementById(id).contentDocument).select('svg');

    var rulercolor = color;

    barranges.ranges.forEach(function (range, i) {
        let range2 = d3obj.select(".range-" + i);
        if (value > range.minValue && value <= range.maxValue && range.alerttype) {
            range2.attr("height", range.activewidth);
            range2.attr("y", 48 - range.activewidth);
            rulercolor = range.color;
        } else {
            range2.attr("height", range.width);
            range2.attr("y", 48 - range.width);
        };
        d3obj.select(".ruler").attr("fill", rulercolor);
        d3obj.select(".rulerBarStart").attr("fill", rulercolor);
        d3obj.select(".rulerBarEnd").attr("fill", rulercolor);
    })

    var fullrange = rangeMax - rangeMin;
    var stepwidth = 150 / fullrange;
    var zeroposition = rangeMin * stepwidth * -1;

    var position = (value * stepwidth) + zeroposition;

    d3obj.select('#needle').attr('transform', 'translate(' + position + ', 0)');

    if (stepwidth >= 2.5) {
        d3obj.select('#value').text(value.toFixed(1));
    } else {
        d3obj.select('#value').text(Math.round(value));
    }



  
    
}