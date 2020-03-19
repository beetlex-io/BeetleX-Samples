/*
* Generate component javascript with vuejs Copyright © beetlex.io 2019-2020 email:admin@beetlex.io 
*/
var _22bed4352da043bbbc4faaacb105a7ca='<div><auto-form ref="login" url="/register" v-model="register.data" size="mini" @completed="onCompleted"></auto-form><el-button size="mini" @click="if($refs.login.success())register.post()">注册</el-button>';
Vue.component('models-register',
    {
        data(){
            return {
                register: new beetlexAction('/register', {}),
                checkConfirmPassword: (rule, value, callback) => {
                    var password = this.$refs.login.getField('Password');
                    var cpassword = this.$refs.login.getField('ConfirmPassword');
                    if (password.value != cpassword.value)
                        callback(new Error('确认密码不正确!'));
                    else
                        callback();
                },
            }
        },
        methods: {
            onCompleted(){
                this.$refs.login.getField('ConfirmPassword').rules.push({ validator: this.checkConfirmPassword, trigger: 'blur' });
            },
        },
        mounted() {
            this.register.requested = (r) => {
                alert(JSON.stringify(r));
            };
        }
,template:_22bed4352da043bbbc4faaacb105a7ca
});
