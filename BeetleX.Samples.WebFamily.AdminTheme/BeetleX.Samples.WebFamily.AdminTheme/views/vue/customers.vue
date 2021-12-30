<div>
    <auto-form style="position:absolute;top:5px" ref="search" size="mini" @command="onFormCommand" v-model="list.data">

    </auto-form>
    <auto-grid height="100%" style="position:absolute;top:40px;bottom:35px;left:10px;right:10px;" ref="dataList" :data="list.result.Items" size="mini" delete="true"
               @itemdelete="onDelete" @command="onCommand">

    </auto-grid>
    <el-pagination style="position:absolute;bottom:0px;" background
                   layout="prev, pager, next"
                   :total="list.result.Count"
                   :page-size="list.result.Size" @current-change="onPageChange">
    </el-pagination>
</div>
<script>
    export default{
        data(){
            return {
                list: new beetlexAction('/Customers', { name: '', country: '', index: 0 }, { Count: 0, Index: 0, Pages: 0, Size: 0, Items: [] }),
            };
        },
        methods: {
            onPageChange(e){
                this.list.data.index = e - 1;
                this.onList();
            },
            onFormCommand(e){
                this.list.data.index = 0;
                this.onList();
            },
            onList(){
                this.list.get()
            },
            onDelete(e){
                this.$confirmMsg('是否要删除' + e.data.CompanyName + "?", () => {
                    e.success();
                });
            },
            onCommand(e){
                if (e.field == 'CompanyName') {
                    this.$openWindow('(客户)' + e.data.CompanyName, '(客户)' + e.data.CompanyName, 'customerinfo', e.data,'fas fa-hospital-user');
                }
                else {
                    this.$openWindow(e.data.CompanyName + '/订单', '(客户)' + e.data.CompanyName + '/订单', 'orders', { customer: e.data.CustomerID },'fas fa-shopping-cart');
                }
            },
        },
        mounted(){

            var grid = new autoData();
            var f;
            f = grid.addButton('viewOrder', 'null');
            f.icon = "el-icon-document-copy";
            f.title = "查看订单";
            f.width = '40';
            grid.addLink("CompanyName", "CompanyName");
            grid.addText("ContactName", "ContactName");
            grid.addText("ContactTitle", "ContactTitle");
            grid.addText("Country", "Country");
            grid.addText("City", "City");
            grid.addText("Address", "Address");
            grid.bindGrid(this.$refs.dataList);

            var search = new autoData();
            search.addText('name', "客户名");
            f = search.addSelect('country', "国家");
            f.dataurl = '/CustomerCountrySelectOptions';
            f.nulloption = true;
            f = search.addButton('search', '查询');
            f.icon = "el-icon-search";
            search.bindForm(this.$refs.search);
            this.onList();
        }
    }
</script>