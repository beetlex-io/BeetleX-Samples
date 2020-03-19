/*
* Generate component javascript with vuejs Copyright © beetlex.io 2019-2020 email:admin@beetlex.io 
*/
var _46d5c57054854b46bc67a57bfe6c018f='<div><auto-form url="/SaveEmploye" ref="form" v-model="employee" size="mini" @completed="onCompleted"></auto-form><el-button @click="onSave" size="mini">保存</el-button>';
Vue.component('models-employee',
    {
        props: ['token'],
            data(){
            return {
                employee: this.token,
                saveEmployee: new beetlexAction('/SaveEmploye'),
            }
        },
        methods: {
            onCompleted(){
                this.$refs.form.getField('FirstName').enabled = false;
                this.$refs.form.getField('LastName').enabled = false;
            },
            onSave(){
                this.employee.id = this.token.EmployeeID;
                this.saveEmployee.post(this.employee);
            }
        },
        mounted(){
            this.saveEmployee.requested = (r) => {
                this.$message({
                    message: JSON.stringify(r, 0, 4),
                    type: 'success'
                });
            };
        },
        watch: {
            token(val){
                this.employee = val;
            }
        }
,template:_46d5c57054854b46bc67a57bfe6c018f
});
