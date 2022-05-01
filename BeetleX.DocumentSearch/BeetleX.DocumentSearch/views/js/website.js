
var __resizeHandlers = [];
function __addResizeHandler(handler) {
    __resizeHandlers.push(handler);
};
function __resize() {
    setTimeout(() => {
        __resizeHandlers.forEach(v => {
            v();
        });
    }, 200);
}
window.onresize = function () {
    __resizeHandlers.forEach(v => {
        v();
    });
};



//timer
var __app__timers = [];

var __app__timeCount = 0;

__addTimerHandler = function (callback) {
    __app__timers.push(callback);
};

__runTimer = function () {
    try {
        __app__timeCount++;
        __app__timers.forEach((v) => {
            v(__app__timeCount);
        });
    }
    catch (err) {
        console.log("run timer error", err);
    }
};
setInterval(__runTimer, 1000);

//vue
var sys__controlid = 0;
var btn_confirmButtonText = "确定";
var btn_cancelButtonText = "取消";

function sys_getID() {
    sys__controlid = sys__controlid + 1;
    return sys__controlid;
}
Vue.prototype.$getID = function () {
    return sys_getID();
};
Vue.prototype.$nformat = function (value) {
    return new Intl.NumberFormat().format(value);
}
Vue.prototype.$confirmMsg = function (msg, callback, cancel, title) {
    if (!title)
        title = '疑问';
    this.$confirm(msg, title, {
        confirmButtonText: btn_confirmButtonText,
        cancelButtonText: btn_cancelButtonText,
        type: 'warning'
    }).then(() => { callback(); }).catch(() => {
        if (cancel)
            cancel();
    });
};
Vue.prototype.$errorMsg = function (msg) {
    this.$message({
        message: msg,
        type: 'error',
        duration: 5000,
        showClose: true,
    });
};
Vue.prototype.$successMsg = function (msg) {
    this.$message({
        message: msg,
        type: 'success',
        duration: 5000,
        showClose: true,
    });
};
Vue.prototype.$confirmInput = function (msg, title, callback, pattern, errormsg) {
    this.$prompt(msg, title, {
        confirmButtonText: btn_confirmButtonText,
        cancelButtonText: btn_cancelButtonText,
        inputPattern: pattern,
        inputErrorMessage: errormsg
    }).then((value) => {
        callback(value)
    }).catch(() => { });
}
Vue.prototype.$confirmPassword = function (msg, title, callback) {
    this.$prompt(msg, title, {
        confirmButtonText: btn_confirmButtonText,
        cancelButtonText: btn_cancelButtonText,
        inputType: 'password'
    }).then((value) => {
        callback(value)
    }).catch(() => { });
}
Vue.prototype.$post = function (url, data, headers) {
    var action = new beetlexAction(url);
    if (!headers)
        headers = new Object();
    action.headers = headers;
    return action.asyncpost(data);
}
Vue.prototype.$get = function (url, data, headers) {
    var action = new beetlexAction(url);
    if (!headers)
        headers = new Object();
    action.headers = headers;
    return action.asyncget(data);
}
Vue.prototype.$put = function (url, data, headers) {
    var action = new beetlexAction(url);
    if (!headers)
        headers = new Object();
    action.headers = headers;
    return action.asyncput(data);
}

Vue.prototype.$postCatch = function (url, data, headers) {
    var action = new beetlexAction(url);
    if (!headers)
        headers = new Object();
    action.headers = headers;
    return action.asyncpost(data, true);
}
Vue.prototype.$getCatch = function (url, data, headers) {
    var action = new beetlexAction(url);
    if (!headers)
        headers = new Object();
    action.headers = headers;
    return action.asyncget(data, true);
}
Vue.prototype.$putCatch = function (url, data, headers) {
    var action = new beetlexAction(url);
    if (!headers)
        headers = new Object();
    action.headers = headers;
    return action.asyncput(data, true);
}