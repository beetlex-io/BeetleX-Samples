function protobufFactory() {
    this.typeMapper = new Object();
}
protobufFactory.prototype.register = function (mapper) {
    this.typeMapper[mapper.id] = mapper;
}

protobufFactory.prototype.encode = function (typeid, msg) {
    var mapper = this.typeMapper[typeid];
    if (!mapper)
        throw typeid + ' protobuf mapper not found!'
    var data = mapper.encode(msg);
    var buffer = new ArrayBuffer(data.length + 4);
    var view = new DataView(buffer);
    view.setInt32(0, typeid, true);
    var uintBuffer = new Uint8Array(buffer);
    uintBuffer.set(data, 4);
    return buffer;
}

protobufFactory.prototype.decode = function (buffers) {
    var view = new DataView(buffers);
    var typeid = view.getInt32(0, true);
    var data = buffers.slice(4);
    var uintbuffer = new Uint8Array(data);
    var mapper = this.typeMapper[typeid];
    if (!mapper)
        throw typeid + ' protobuf mapper not found!'
    return mapper.decode(uintbuffer);
}

function protobufMapper(id, protoMate) {
    this.protobuf = protoMate;
    this.id = id;
}
protobufMapper.prototype.encode = function (msg) {
    return this.protobuf.encode(msg).finish();
}
protobufMapper.prototype.decode = function (buffer) {
    return this.protobuf.decode(buffer);
}

var protobufDefault = new protobufFactory();

function ProtobufClient(debug) {
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
    this.timeout = 2000;
    this.receive = null;
    this.debug = debug;
    this.resolve = null;
    this.connectResolve = null;
}

ProtobufClient.prototype.receiveFrom = function (data) {
    var result = new Promise(resolve => {
        this.resolve = resolve;
    });
    this.send(data);
    return result;
}

ProtobufClient.prototype.send = function (data) {
    var sender = (msg) => {
        var buffer = msg.getData();
        if (this.debug == true)
            console.log("websocket send:", buffer);
        this.websocket.send(buffer);
    }
    if (this.status == false) {
        var result = new Promise(resolve => {
            this.connectResolve = { callback: resolve, data: data };
        });
        this.connect();
        result.then(r => {
            sender(r);
        });
    }
    else {
        sender(data);
    }
}

ProtobufClient.prototype.onOpen = function (evt) {
    this.status = true;
    if (this.debug == true)
        console.log("websocket connected:", this.wsUri);
    if (this.connected) {
        this.connected();
    }
    if (this.connectResolve) {
        var token = this.connectResolve;
        this.connectResolve = null;
        token.callback(token.data);
    }
}

ProtobufClient.prototype.onClose = function (evt) {
    this.status = false;
    if (this.disconnect)
        this.disconnect();
    if (this.debug == true)
        console.log("websocket close:", evt);
}

ProtobufClient.prototype.onMessage = function (evt) {
    if (evt.data instanceof ArrayBuffer) {
        if (this.debug == true)
            console.log("websocket receive:", evt.data);
        var msg = protobufDefault.decode(evt.data);
        if (this.resolve) {
            var callback = this.resolve;
            this.resolve = null;
            callback(msg);
        }
        if (this.receive)
            this.receive(msg);
    }
}

ProtobufClient.prototype.onError = function (evt) {
    console.log("websocket error:", evt);
};
ProtobufClient.prototype.reconnect = function () {
    this.status = false;
    if (this.websocket)
        this.websocket.close();
};
ProtobufClient.prototype.connect = function () {
    if (this.status == false) {
        if (!this.websocket)
            this.websocket = new WebSocket(this.wsUri);
        if (this.debug == true)
            console.log("websocket connect:", this.wsUri);
        this.websocket.binaryType = 'arraybuffer';
        this.websocket.onopen = (evt) => { this.onOpen(evt) };
        this.websocket.onclose = (evt) => { this.onClose(evt) };
        this.websocket.onmessage = (evt) => { this.onMessage(evt) };
        this.websocket.onerror = (evt) => { this.onError(evt) };
    }
}
var wsprotobuf = new ProtobufClient(true);

Vue.prototype.$send = function (data) {
    wsprotobuf.send(data);
}
Vue.prototype.$receiveFrom = function (data) {
    return wsprotobuf.receiveFrom(data);
}
Vue.prototype.$receive = function (callback) {
    wsprotobuf.receive = callback;
}
