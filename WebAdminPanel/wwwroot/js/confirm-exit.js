window.confirmExit = function (message) {
    window.onbeforeunload = function () {
        return message;
    };
};
window.clearConfirmExit = function () {
    window.onbeforeunload = null;
};
