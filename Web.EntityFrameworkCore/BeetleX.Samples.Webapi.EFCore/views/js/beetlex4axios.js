function UrlHelper() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0].toLowerCase()] = hash[1];
    }
    this.queryString = vars;
    this.tag = null;
    this.ssl = window.location.protocol == "https:"
    var url = document.location.pathname;
    var tagIndex = document.location.href.indexOf('#');
    if (tagIndex > 0) {
        this.tag = document.location.href.substring(tagIndex + 1);
        this.tag = this.tag.substring(0, (this.tag.indexOf("?") == -1) ? this.tag.length : this.tag.indexOf("?"));
    }
    this.folder = url.substring(url.indexOf('/'), url.lastIndexOf('/'));
    url = url.substring(0, (url.indexOf("#") == -1) ? url.length : url.indexOf("#"));
    url = url.substring(0, (url.indexOf("?") == -1) ? url.length : url.indexOf("?"));
    url = url.substring(url.lastIndexOf("/") + 1, url.length);
    if (url) {
        this.fileName = decodeURIComponent(url);
        this.ext = this.fileName.substring(this.fileName.lastIndexOf(".") + 1, this.fileName.length)
        this.fileNameWithOutExt = this.fileName.substring(0, this.fileName.lastIndexOf(".") == -1 ? this.fileName.length : this.fileName.lastIndexOf("."));
    }

}

var _url = new UrlHelper();

function beetlexWebSocket() {
    this.wsUri = null;
    if (window.location.protocol == "https:") {
        this.wsUri = "wss://" + window.location.host;
    }
    else {
        this.wsUri = "ws://" + window.location.host;
    }
    this.websocket;
    this.status = false;
    this.disconnect = null;
    this.connected = null;
    this.messagHandlers = new Object();
    this.timeout = 2000;
    this.receive = null;
}

beetlexWebSocket.prototype.send = function (url, params, callback) {
    if (this.status == false) {
        if (callback != null) {
            callback({ Url: url, Code: 505, Error: 'disconnect' })
        }
    }
    this.messagHandlers[params._requestid] = callback;
    var data = { url: url, params: params };
    this.websocket.send(JSON.stringify(data));
}

beetlexWebSocket.prototype.onOpen = function (evt) {
    this.status = true;
    if (this.connected)
        this.connected();
}

beetlexWebSocket.prototype.onClose = function (evt) {
    this.status = false;
    var _this = this;
    if (this.disconnect)
        this.disconnect();
    if (evt.code == 1006) {
        setTimeout(function () {
            _this.connect();
        }, _this.timeout);
        if (_this.timeout < 10000)
            _this.timeout += 1000;
    }

}

beetlexWebSocket.prototype.onMessage = function (evt) {
    var msg = JSON.parse(evt.data);
    var callback = this.messagHandlers[msg.ID];
    if (callback)
        callback(msg);
    else
        if (this.receive) {
            if (msg.Data === undefined)
                this.receive(msg);
            else
                this.receive(msg.Data);
        }
}

beetlexWebSocket.prototype.onReceiveMessage = function (callback) {
    this.callback = callback;
};
beetlexWebSocket.prototype.onError = function (evt) {

}

beetlexWebSocket.prototype.connect = function () {
    if (this.status == false) {
        if (!this.websocket)
            this.websocket = new WebSocket(this.wsUri);
        _this = this;
        this.websocket.onopen = function (evt) { _this.onOpen(evt) };
        this.websocket.onclose = function (evt) { _this.onClose(evt) };
        this.websocket.onmessage = function (evt) { _this.onMessage(evt) };
        this.websocket.onerror = function (evt) { _this.onError(evt) };
    }
}

function beetlex4axios() {
    this._requestid = new Date().getTime();
    this._maxrequestid = this._requestid + 5000;
    this.errorHandlers = new Object();
    this.websocket = new beetlexWebSocket();
    this.loading = 0;
    this.error = null;

}

beetlex4axios.prototype.useWebsocket = function (host) {
    if (host)
        this.websocket.wsUri = host;
    this.websocket.connect();
}

beetlex4axios.prototype.getErrorHandler = function (code) {
    return this.errorHandlers[code];
}

beetlex4axios.prototype.SetErrorHandler = function (code, callback) {
    this.errorHandlers[code] = callback;
}

beetlex4axios.prototype.getRequestID = function () {

    this._requestid++;
    if (this._requestid > this._maxrequestid) {
        this._requestid = new Date().getTime();
        this._maxrequestid = this._requestid + 5000;
    }

    return this._requestid;
}
beetlex4axios.prototype.completed = function () {
    var that = this;
    setTimeout(function () {
        if (that.loading > 0)
            that.loading--;
    }, 400);
}
beetlex4axios.prototype.get = function (handler, url, params, callback) {
    handler.loading = true;
    var httpurl = url;
    if (!params)
        params = new Object();
    var _this = this;
    this.loading++;
    params['_requestid'] = this.getRequestID();
    if (this.websocket.status == true && handler.http == false) {
        var wscallback = function (r) {
            _this.completed();
            handler.loading = false;
            var data = r;
            if (data.Code && data.Code != 200) {
                _this.onError(data.Code, data.Error);
                if (handler.error) {
                    handler.error({ code: data.Code, error: data.Error });
                    handler.error = null;
                }
            }
            else {
                if (callback) {
                    if (data.Data === undefined)
                        callback(data);
                    else
                        callback(data.Data);
                }
            }
        };
        this.websocket.send(url, params, wscallback);
    }
    else {
        var headers = handler.headers;
        headers['Content-Type'] = 'application/json;charset=UTF-8'
        axios.get(httpurl, { params: params, headers: headers })
            .then(function (response) {
                console.log("axios get success", response);
                _this.completed();
                handler.loading = false;
                var data = response.data;
                if (data.Code && data.Code != 200) {
                    _this.onError(data.Code, data.Error);
                    if (handler.error) {
                        handler.error({ code: data.Code, error: data.Error });
                        handler.error = null;
                    }
                }
                else {
                    if (callback) {
                        if (data.Data === undefined)
                            callback(data);
                        else
                            callback(data.Data);
                    }
                }
            })
            .catch(function (error) {
                console.log("axios get error", error, error.response);
                _this.completed();
                handler.loading = false;
                var code = error.response ? error.response.status : 500;
                var message = error;
                if (error.response.data)
                    message = error.response.data;
                else
                    message = error.response.statusText;
                _this.onError(code, message);
                if (handler.error) {
                    handler.error({ code: code, error: message });
                    handler.error = null;
                }
            });
    }
};

beetlex4axios.prototype.onError = function (code, message) {
    var handler = this.getErrorHandler(code);
    if (handler)
        handler(message);
    else {
        if (this.error)
            this.error(message);
        else
            alert(message);
    }
}
beetlex4axios.prototype.put = function (handler, url, data, callback) {
    handler.loading = true;
    var _this = this;
    var headers = handler.headers;
    console.log("axios put", url, data);
    axios.put(url, data, { headers: headers })
        .then(function (response) {
            console.log("axios put success", response);
            _this.completed();
            handler.loading = false;
            var data = response.data;
            if (data.Code && data.Code != 200) {
                _this.onError(data.Code, data.Error);
                if (handler.error) {
                    handler.error({ code: data.Code, error: data.Error });
                    handler.error = null;
                }
            }
            else {
                if (callback) {
                    if (data.Data === undefined)
                        callback(data);
                    else
                        callback(data.Data);
                }
            }
        })
        .catch(function (error) {
            console.log("axios post error", error);
            _this.completed();
            handler.loading = false;
            var code = error.response ? error.response.status : 500;
            var message = error.message;
            if (error.response.data)
                message = error.response.data;
            else
                message = error.response.statusText;
            _this.onError(code, message);
            if (handler.error) {
                handler.error({ code: code, error: message });
                handler.error = null;
            }
        });
}
beetlex4axios.prototype.post = function (handler, url, params, callback) {
    handler.loading = true;
    var httpurl = url;
    if (!params)
        params = new Object();
    var id = this.getRequestID();
    var _this = this;
    params['_requestid'] = id;
    this.loading++;
    if (this.websocket.status == true && handler.http == false) {
        var wscallback = function (r) {
            _this.completed();
            handler.loading = false;
            var data = r;
            if (data.Code && data.Code != 200) {
                _this.onError(data.Code, data.Error);
                if (handler.error) {
                    handler.error({ code: data.Code, error: data.Error });
                    handler.error = null;
                }
            }
            else {
                if (callback) {
                    if (data.Data === undefined)
                        callback(data);
                    else
                        callback(data.Data);
                }
            }
        };
        this.websocket.send(url, params, wscallback);
    }
    else {
        var headers = handler.headers;
        headers['Content-Type'] = 'application/json;charset=UTF-8'
        axios.post(httpurl, JSON.stringify(params), { headers: headers })
            .then(function (response) {
                console.log("axios post success", response);
                _this.completed();
                handler.loading = false;
                var data = response.data;
                if (data.Code && data.Code != 200) {
                    _this.onError(data.Code, data.Error);
                    if (handler.error) {
                        handler.error({ code: data.Code, error: data.Error });
                        handler.error = null;
                    }
                }
                else {
                    if (callback) {
                        if (data.Data === undefined)
                            callback(data);
                        else
                            callback(data.Data);
                    }
                }
            })
            .catch(function (error) {
                console.log("axios post error", error);
                _this.completed();
                handler.loading = false;
                var code = error.response ? error.response.status : 500;
                var message = error.message;
                if (error.response.data)
                    message = error.response.data;
                else
                    message = error.response.statusText;
                _this.onError(code, message);
                if (handler.error) {
                    handler.error({ code: code, error: message });
                    handler.error = null;
                }
            });
    }
};

var beetlex = new beetlex4axios();

function beetlexAction(actionUrl, actionData, defaultResult) {
    this.url = actionUrl;
    this.data = actionData;
    this.result = defaultResult;
    this.requesting = null;
    this.requested = null;
    this.loading = false;
    this.http = false;
    this.token = null;
    this.error = null;
    this.headers = {};
}

beetlexAction.prototype.userHttp = function () {
    this.http = true;
    return this;
}

beetlexAction.prototype.onCallback = function (data) {
    if (this.requested)
        this.requested(data);
}

beetlexAction.prototype.onValidate = function (data) {
    if (this.requesting)
        return this.requesting(data);
    return true;
}

beetlexAction.prototype.asyncget = function (data) {
    var result = new Promise((resolve, error) => {
        this.requested = resolve;
        this.error = error;
        this.get(data);
    });
    return result;
}

beetlexAction.prototype.get = function (data) {
    var _this = this;
    var _postData = this.data;
    if (data)
        _postData = data;
    if (!this.onValidate(_postData))
        return;
    beetlex.get(this, this.url, _postData, function (r) {
        _this.result = r;
        _this.onCallback(r);
    });
};
beetlexAction.prototype.asyncput = function (data) {
    var result = new Promise((resolve, error) => {
        this.requested = resolve;
        this.error = error;
        this.put(data);
    });
    return result;
}
beetlexAction.prototype.put = function (data) {
    var _this = this;
    beetlex.put(this, this.url, data, function (r) {
        _this.result = r;
        _this.onCallback(r);
    });
};

beetlexAction.prototype.asyncpost = function (data) {
    var result = new Promise((resolve, error) => {
        this.requested = resolve;
        this.error = error;
        this.post(data);
    });
    return result;
}

beetlexAction.prototype.post = function (data) {
    var _this = this;
    var _postData = this.data;
    if (data)
        _postData = data;
    if (!this.onValidate(_postData))
        return;
    beetlex.post(this, this.url, _postData, function (r) {
        _this.result = r;
        _this.onCallback(r);
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
