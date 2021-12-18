<div>
    <auto-form style="position:absolute;top:5px" ref="search" size="mini" @command="onFormCommand" :style="{display:data.CategoryId?'none':''}" v-model="list.data">

    </auto-form>
    <auto-grid height="100%" style="position:absolute;top:40px;bottom:35px;left:10px;right:10px;" ref="dataList" :data="list.result.Items" size="mini" delete="true" selection="true" :edit="true"
               @itemdelete="onDelete" @itemchange="onChange" @command="onCommand">

    </auto-grid>
    <el-pagination style="position:absolute;bottom:0px;" background
                   layout="prev, pager, next"
                   :total="list.result.Count"
                   :page-size="list.result.Size" @current-change="onPageChange">
    </el-pagination>
</div>
<script>
    export default{
        props: ['token'],
            data(){
            return {
                data: this.token,
                list: new beetlexAction('/Products', { name: '', category: '', index: 0 }, { Count: 0, Index: 0, Pages: 0, Size: 0, Items: [] }),
                items: [],
            };
        },
        methods: {
            setFilter(){
                console.log("products filter", this.data);

                this.list.data.category = this.data.CategoryId;
                this.list.data.index = 0;
                this.onList();
            },
            onPageChange(e){
                this.list.data.index = e - 1;
                this.onList();
            },
            onFormCommand(e){
                this.list.data.index = 0;
                this.onList();
            },
            onDelete(e){
                this.$confirmMsg('是否要删除' + e.data.ProductName + "?", () => {
                    e.success();
                });
            },
            onCommand(e){
                console.log("products cmd", e);
                this.$openWindow('(产品)' + e.data.ProductName + '/订单', '(产品)' + e.data.ProductName + '/订单', 'orders', { product: e.data.ProductID },'fas fa-shopping-cart');
            },
            onChange(e){
                this.$confirmMsg('是否要保存' + e.data.ProductName + "?", () => {
                    e.success();
                });
            },
            onList(){
                this.list.get()
            },
        },
        watch: {
            tag(val){
                this.data = val;

                this.setFilter();

            }
        },
        mounted(){
            var grid = new autoData();
            var f;
            f = grid.addButton('viewOrder', 'null');
            f.icon = "el-icon-document-copy";
            f.title = "查看订单";
            f.width = '40';
            grid.addText("ProductName", "Name");
            f = grid.addSelect("CategoryName", "Category");
            f.dataurl = '/CategoriesSelectOptions';
            grid.addText("QuantityPerUnit");
            grid.addNumber("UnitPrice");
            grid.addNumber("UnitsInStock");
            grid.bindGrid(this.$refs.dataList);
            this.setFilter();
            var search = new autoData();
            search.addText('name', "名称");
            f = search.addSelect('category', "分类");
            f.dataurl = '/CategoriesSelectOptions';
            f.nulloption = true;
            f = search.addButton('search', '查询');
            f.icon = "el-icon-search";
            search.bindForm(this.$refs.search);


        }
    }
</script>