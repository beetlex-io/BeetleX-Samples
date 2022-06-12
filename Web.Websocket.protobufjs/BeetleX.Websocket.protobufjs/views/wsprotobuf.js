protobuf.load("/wsprotobuf.proto", function(err, root) {
    if (err)
        throw err;
   var Register=root.lookupType("wsprotobuf.Register");
   var Registermapper = new protobufMapper(2,Register);
   protobufDefault.register(Registermapper);
   var User=root.lookupType("wsprotobuf.User");
   var Usermapper = new protobufMapper(1,User);
   protobufDefault.register(Usermapper);
});
function Register(){
    this.Password=null;
    this.EMail=null;
};
Register.prototype.getData=function(){
    return protobufDefault.encode(2,this);
};
function User(){
    this.Name=null;
    this.Email=null;
    this.ResponseTime=null;
};
User.prototype.getData=function(){
    return protobufDefault.encode(1,this);
};
