/*
* Generate component javascript with vuejs Copyright © beetlex.io 2019-2020 email:admin@beetlex.io 
*/
var _996be6fc25484dcdb006dbbf4c5b7124='<div><auto-form ref="login" url="/login" v-model="login.data" size="mini"></auto-form><el-button size="mini" @click="if($refs.login.success())login.post()">登陆</el-button>';
Vue.component('models-login',
    {
        data(){
            return {
                login: new beetlexAction('/login', {})
            }
        },
        mounted() {
            this.login.requested = (r) => {
                alert(JSON.stringify(r));
            };
        }
,template:_996be6fc25484dcdb006dbbf4c5b7124
});
