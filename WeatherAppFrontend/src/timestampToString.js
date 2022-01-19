export default (timestamp) => {
    return `${timestamp.toDateString()} ${timestamp.toLocaleTimeString().slice(0, -3)}`;
};