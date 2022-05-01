var ajaxBaseUrl = null;
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

var UUID = (function () {
    var self = {};
    var lut = []; for (var i = 0; i < 256; i++) { lut[i] = (i < 16 ? '0' : '') + (i).toString(16); }
    self.generate = function () {
        var d0 = Math.random() * 0xffffffff | 0;
        var d1 = Math.random() * 0xffffffff | 0;
        var d2 = Math.random() * 0xffffffff | 0;
        var d3 = Math.random() * 0xffffffff | 0;
        return lut[d0 & 0xff] + lut[d0 >> 8 & 0xff] + lut[d0 >> 16 & 0xff] + lut[d0 >> 24 & 0xff] + '-' +
            lut[d1 & 0xff] + lut[d1 >> 8 & 0xff] + '-' + lut[d1 >> 16 & 0x0f | 0x40] + lut[d1 >> 24 & 0xff] + '-' +
            lut[d2 & 0x3f | 0x80] + lut[d2 >> 8 & 0xff] + '-' + lut[d2 >> 16 & 0xff] + lut[d2 >> 24 & 0xff] +
            lut[d3 & 0xff] + lut[d3 >> 8 & 0xff] + lut[d3 >> 16 & 0xff] + lut[d3 >> 24 & 0xff];
    }
    return self;
})();

var _url = new UrlHelper();

function changeAjaxPath() {
    ajaxBaseUrl = _url.folder;
    console.log("ajaxBaseUrl", ajaxBaseUrl);
}
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
beetlexWebSocket.prototype.reconnect = function () {
    this.status = false;
    if (this.websocket)
        this.websocket.close();
},
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
    this.headers = {};
    this.baseUrl = "";


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
                if (handler.error) {
                    handler.onError(data.Code, data.Error);
                }
                else {
                    _this.onError(data.Code, data.Error);
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
        Object.getOwnPropertyNames(this.headers).forEach(v => {
            headers[v] = this.headers[v];
        });
        if (!headers['Content-Type'])
            headers['Content-Type'] = 'application/json;charset=UTF-8'
        if (ajaxBaseUrl) {
            httpurl = ajaxBaseUrl + httpurl;
        }
        axios.get(httpurl, { params: params, headers: headers })
            .then(function (response) {
                _this.completed();
                handler.loading = false;
                var data = response.data;
                if (data.Code && data.Code != 200) {
                    if (handler.error) {
                        handler.onError(data.Code, data.Error);
                    }
                    else {
                        _this.onError(data.Code, data.Error);
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
                _this.completed();
                handler.loading = false;
                var message = error;

                if (error.response) {
                    var code = error.response ? error.response.status : 500;
                    message = error.message;
                    if (error.response.data)
                        message = error.response.data;
                    else
                        message = error.response.statusText;
                }
                if (handler.error) {
                    handler.onError(code, message);
                }
                else {

                    _this.onError(code, message);
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
beetlex4axios.prototype.put = function (handler, httpurl, data, callback) {
    handler.loading = true;
    var _this = this;
    var headers = handler.headers;
    Object.getOwnPropertyNames(this.headers).forEach(v => {
        headers[v] = this.headers[v];
    });
    var type = headers['Content-Type'];
    if (type && type.indexOf('application/json') >= 0)
        data = JSON.stringify(data);
    if (ajaxBaseUrl) {
        httpurl = ajaxBaseUrl + httpurl;
    }
    axios.put(httpurl, data, { headers: headers })
        .then(function (response) {
            _this.completed();
            handler.loading = false;
            var data = response.data;
            if (data.Code && data.Code != 200) {
                if (handler.error) {
                    handler.onError(data.Code, data.Error);
                }
                else {
                    _this.onError(data.Code, data.Error);
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

            _this.completed();
            handler.loading = false;
            var message = error;
            if (error.response) {
                var code = error.response ? error.response.status : 500;
                message = error.message;
                if (error.response.data)
                    message = error.response.data;
                else
                    message = error.response.statusText;
            }
            if (handler.error) {
                handler.onError(code, message);
            }
            else {
                _this.onError(code, message);
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

                if (handler.error) {
                    handler.onError(data.Code, data.Error);

                }
                else {
                    _this.onError(data.Code, data.Error);
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
        Object.getOwnPropertyNames(this.headers).forEach(v => {
            headers[v] = this.headers[v];
        });
        if (!headers['Content-Type'])
            headers['Content-Type'] = 'application/json;charset=UTF-8'
        var type = headers['Content-Type'];
        if (type && type.indexOf('application/json') >= 0)
            params = JSON.stringify(params);
        if (ajaxBaseUrl) {
            httpurl = ajaxBaseUrl + httpurl;
        }
        axios.post(httpurl, params, { headers: headers })
            .then(function (response) {
                _this.completed();
                handler.loading = false;
                var data = response.data;
                if (data.Code && data.Code != 200) {
                    if (handler.error) {
                        handler.onError(code, message);
                    }
                    else {
                        _this.onError(data.Code, data.Error);
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
                _this.completed();
                handler.loading = false;
                var message = error;
                if (error.response) {
                    var code = error.response ? error.response.status : 500;
                    message = error.message;
                    if (error.response.data)
                        message = error.response.data;
                    else
                        message = error.response.statusText;
                }

                if (handler.error) {
                    handler.onError(code, message);
                }
                else {
                    _this.onError(code, message);
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
beetlexAction.prototype.onError = function (code, msg) {
    try {
        this.error({ code: code, error: msg });
    }
    catch {
        beetlex.onError(code, msg);
    }
    finally {
        this.error = null;
    }
};
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

beetlexAction.prototype.asyncget = function (data, catchError) {
    var result = new Promise((resolve, error) => {
        this.requested = resolve;
        if (catchError)
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
beetlexAction.prototype.asyncput = function (data, catchError) {
    var result = new Promise((resolve, error) => {
        this.requested = resolve;
        if (catchError)
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

beetlexAction.prototype.asyncpost = function (data, catchError) {
    var result = new Promise((resolve, error) => {
        this.requested = resolve;
        if (catchError)
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
