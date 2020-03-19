/*
* Generate component javascript with vuejs Copyright © beetlex.io 2019-2020 email:admin@beetlex.io 
*/
var _8e550c739c9b456c9ed1652b684e2257='<auto-grid url="/employees" @completed="employees.get()"@itemchange="onItemChange"@itemdelete="onItemDelete"@command="onCommand":data="employees.result"size="mini" height="100%"edit="true" delete="true">';
Vue.component('models-employees',
    {
        data(){
            return {
                employees: new beetlexAction('/employees', {}, [])
            }
        },
        methods: {
            onCommand(e){
                this.$open('models-employee', e.data);
            },
            onItemChange(item){
                if (confirm('是否要修改' + item.data.FirstName + '?')) {
                    item.success();
                }
            },
            onItemDelete(item){
                if (confirm('是否要删除' + item.data.FirstName + '?')) {
                    item.success();
                }
            },
        },
        mounted() {
        }
,template:_8e550c739c9b456c9ed1652b684e2257
});
