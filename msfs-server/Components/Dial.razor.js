

var dialranges = {}

async function loadRanges(
    fileName,
    svg,
    rangeMin,
    rangeMax,
    minAngle,
    maxAngle,
    innerRadius,
    outerRadius) {

    var pi = Math.PI;

    var stepangle = (maxAngle - minAngle) / (rangeMax - rangeMin);
    var zeroangle = rangeMin * stepangle * -1 + minAngle;

    const response = await fetch(`/config/dialranges/${fileName}.json`, { cache: "no-store" });

    dialranges = await response.json();

    dialranges.ranges.forEach(function (range, i) {
        let rMin = zeroangle + range.minValue * stepangle;
        let rMax = zeroangle + range.maxValue * stepangle;
        let arc2 = d3.arc()
            .innerRadius(innerRadius - range.width)
            .outerRadius(innerRadius)
            .startAngle(rMin * (pi / 180))
            .endAngle(rMax * (pi / 180));
        svg.append("path")
            .attr("class", "arc-" + i)
            .attr("fill", range.color)
            .attr("d", arc2);

    })
    
}

export function Init(
    id,
    label,
    rangeMin,
    rangeMax,
    minAngle,
    maxAngle,
    innerRadius,
    outerRadius,
    color,
    dialRangesFileName) {

    var d3obj = d3.select(document.getElementById(id).contentDocument).select('svg');

    var pi = Math.PI;

    let svg = d3obj
        .append("svg")
        .attr("viewBox", "0 0 150 150")
        .attr("width", 150)
        .attr("height", 150)
        .append("g")
        .attr("transform", "translate(75,75)");

    let arc = d3.arc()
        .innerRadius(68)
        .outerRadius(70)
        .startAngle(minAngle * (pi / 180))
        .endAngle(maxAngle * (pi / 180));

    svg.append("path")
        .attr("class", "arc")
        .attr("fill", color)
        .attr("d", arc);
   
    d3obj.select('#label').text(label);

    svg.append("path")
        .style("stroke-width", 1)
        .style("stroke", "#000")
        .style("fill", "white")
        .attr("d", "M 0,0 L 2,0 L 4,-50 L 0,-69 L -4,-50 L -2,0 L 0,0")
        .attr("id", "needle")
        .attr('x', -75).attr('y', -75);

    loadRanges(
        dialRangesFileName,
        svg,
        rangeMin,
        rangeMax,
        minAngle,
        maxAngle,
        innerRadius,
        outerRadius);
}

export function SetValues(
    id,
    rangeMin,
    rangeMax,
    minAngle,
    maxAngle,
    innerRadius,
    outerRadius,
    color,
    value
) {

    // credit https://github.com/joeherwig/portable-sim-panels

    var d3obj = d3.select(document.getElementById(id).contentDocument).select('svg');

    var pi = Math.PI;

    var stepangle = (maxAngle - minAngle) / (rangeMax - rangeMin);
    var zeroangle = rangeMin * stepangle * -1 + minAngle;
    
    var degrees = zeroangle + value * stepangle;

    d3obj.select('#needle').attr('transform', 'rotate(' + degrees + ', 0, 0)');
    if (rangeMax - rangeMin <= 99) {
        d3obj.select('#value').text((value * 1).toFixed(1));
    } else {
        d3obj.select('#value').text(Math.round(value));
    }

    let arccolor = color
    dialranges.ranges.forEach(function (range, i) {
        let rMin = zeroangle + range.minValue * stepangle;
        let rMax = zeroangle + range.maxValue * stepangle;
        let arc = d3obj.select(".arc-" + i);
        let arc2;
        if (value > range.minValue && value <= range.maxValue && range.alerttype) {
            arc2 = d3.arc()
                .innerRadius(innerRadius - range.activewidth)
                .outerRadius(innerRadius)
                .startAngle(rMin * (pi / 180))
                .endAngle(rMax * (pi / 180));
            arccolor = range.color;
        } else {
            arc2 = d3.arc()
                .innerRadius(innerRadius - range.width)
                .outerRadius(innerRadius)
                .startAngle(rMin * (pi / 180))
                .endAngle(rMax * (pi / 180));
        }
        arc.attr("d", arc2);
        d3obj.select(".arc").attr("fill", arccolor)
    })
    
}