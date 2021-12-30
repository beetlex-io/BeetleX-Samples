<div>
    <el-card class="box-card" v-for="item in list.result" style="width:250px;float:left;margin:4px;min-height:200px;">
        <div slot="header" class="clearfix">
            <span><b>{{item.CategoryName}}</b></span>
            <el-button @click="onSelect(item)" style="margin-left:10px;" round size="mini">{{item.Count}}</el-button>
        </div>
        {{item.Description}}
    </el-card>
</div>
<script>
    export default {
        data(){
            return {
                list: new beetlexAction('Categories', null, [])
            };
        },
        methods: {
            onSelect(e){
                this.$openWindow('products', '(分类)'+e.CategoryName + '/产品', e)
            },
        },
        mounted(){
            this.list.get();
        }
    }
</script>