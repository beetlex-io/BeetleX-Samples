<div id="menus">
    <el-menu default-active="1" size="mini" @select="onSelect">

        <el-menu-item index="1">
            <i class="el-icon-menu"></i>
            <span slot="title">用户</span>
        </el-menu-item>
        <el-menu-item index="2">
            <i class="el-icon-document"></i>
            <span slot="title">雇员</span>
        </el-menu-item>
        <el-menu-item index="3">
            <i class="el-icon-setting"></i>
            <span slot="title">订单</span>
        </el-menu-item>
    </el-menu>
</div>
<script>
    {
        data(){
            return {}
        },
        methods: {
            onSelect(i){
                console.log(i);
                var module = 'employees';
                if (i == 2) {
                    module = 'employees';
                }
                else if (i == 3) {
                    module = 'orders';
                }
                else {
                    module = 'customers';
                }
                this.$emit('select', module);
            }
        }
    }
</script>