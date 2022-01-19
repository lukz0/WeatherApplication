export default (item) => {
    let degrees = item?.windFromDirection;
    if (typeof degrees === 'undefined' || degrees === null) return 'Unknown Direction';
    if (degrees < 22.5 || degrees > 337.5) return 'North';
    if (degrees < 67.5) return 'North-East';
    if (degrees < 112.5) return 'East';
    if (degrees < 157.5) return 'South-East';
    if (degrees < 202.5) return 'South';
    if (degrees < 247.5) return 'South-West';
    if (degrees < 292.5) return 'West';
    return 'North-West';
};