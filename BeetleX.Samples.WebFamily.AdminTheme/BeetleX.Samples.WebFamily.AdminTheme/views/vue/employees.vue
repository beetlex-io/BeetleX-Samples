<div>
    <auto-grid height="100%" style="position:absolute;top:0px;bottom:0px;left:10px;right:10px;" ref="dataList" :data="list.result" size="mini" @command="onCommand">

    </auto-grid>
</div>
<script>
    export default{
        data(){
            return {
                list: new beetlexAction('/Employees', null, []),
            };
        },
        methods: {
            onCommand(e){
                console.log('employees',e)
                if (e.field == 'Name') {
                    this.$openWindow('(雇员)' + e.data.Name, '(雇员)' + e.data.Name, 'employeeinfo', e.data,'fas fa-users');
                }
                else {
                    this.$openWindow('(雇员)' + e.data.FirstName + ' ' + e.data.LastName + '/订单', '(雇员)' + e.data.FirstName + ' ' + e.data.LastName + '/订单', 'orders', { employee: e.data.EmployeeID }, 'fas fa-shopping-cart');
                }
            },
            onList(){
                this.list.get()
            },
        },
        mounted(){
            var grid = new autoData();
            var f;
            f = grid.addButton('viewOrder', 'null');
            f.icon = "el-icon-document-copy";
            f.title = "查看订单";
            f.width = '40';
            grid.addLink("Name", "Name");
            grid.addText("Title", "Title");
            grid.addText("BirthDate", "BirthDate");
            grid.addText("City", "City");
            grid.addText("Country", "Country");
            grid.addText("Region", "Region");
            grid.addText("Address", "Address");
            grid.bindGrid(this.$refs.dataList);
            this.onList();
        }
    }
</script>