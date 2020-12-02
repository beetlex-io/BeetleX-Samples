<div>
    <div class="menu-title" v-if="isCollapse==false">
        <div style="display:inline-block"><h2> WebFamily Admin</h2></div>
        <a href="javascript:void(0)" @click="isCollapse=!isCollapse;onChange()"><i class="el-icon-s-fold"></i></a>
    </div>
    <el-menu default-active="Home" @select="onClick" class="el-menu-vertical-demo" :collapse="isCollapse">
        <el-menu-item index="0" v-if="isCollapse==true">
            <i class="el-icon-s-unfold"></i>
        </el-menu-item>
        <el-menu-item index="Home">
            <i class="el-icon-s-home"></i>
            <span slot="title">主页</span>
        </el-menu-item>
        <el-submenu index="1">
            <template slot="title">
                <i class="el-icon-location"></i>
                <span slot="title">产品</span>
            </template>

            <el-menu-item index="category">分类</el-menu-item>

            <el-menu-item index="product">产品明细</el-menu-item>

        </el-submenu>

        <el-menu-item index="orders">
            <i class="el-icon-shopping-cart-full"></i>
            <span slot="title">订单</span>
        </el-menu-item>
        <el-menu-item index="customers">
            <i class="el-icon-s-custom"></i>
            <span slot="title">客户</span>
        </el-menu-item>
        <el-menu-item index="employees">
            <i class="el-icon-user"></i>
            <span slot="title">雇员</span>
        </el-menu-item>
        <el-menu-item index="about">
            <i class="el-icon-question"></i>
            <span slot="title">关于</span>
        </el-menu-item>
    </el-menu>
</div>
<script>
    {
        data(){
            return { isCollapse: false };
        },
        methods: {
            onClick(e){
                if (e == 0) {
                    this.isCollapse = !this.isCollapse;
                    this.onChange();
                }
                else if (e == 'Home') {
                    this.$open('home', '主页');
                }
                else if (e == "category") {
                    this.$open('category', "产品分类");
                }
                else if (e == 'product') {
                    this.$open('products', "产品信息", {});
                }
                else if (e == 'orders') {
                    this.$open('orders', "订单", {});
                }
                else if (e == 'customers') {
                    this.$open('customers', '客户', {})
                }
                else if (e == 'employees') {
                    this.$open('employees', '雇员', {})
                }
                else if (e == 'about') {
                    this.$open('about', '关于', {});
                }
            },
            onChange(e)
            {
                this.$emit("change", this.isCollapse);
            }
        },
    }
</script>