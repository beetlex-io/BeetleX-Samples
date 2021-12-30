<div>

    <auto-form style="position:absolute;top:5px" ref="search" size="mini" @command="onFormCommand" 
               :style="{display:(token.product || token.customer || token.employee) ?'none':''}"
               v-model="list.data">

    </auto-form>

    <div style="position:absolute;top:40px;bottom:35px;left:10px;right:10px;overflow-y:auto;padding:5px;">
        <el-card class="box-card order-item" v-for="item in list.result.Items" style="margin:10px 0px;">

            <el-row>
                <el-col :span="12">
                    <div class="grid-content bg-purple" style="background-color:rgba(246, 246, 246, 0.65);border-radius:6px;padding:10px;margin-right:10px;">
                        <el-form :inline="true" size="mini">
                            <el-form-item label="OrderID">
                                {{item.OrderID}}
                            </el-form-item>
                            <el-form-item label="Employee">
                                {{item.Employee}}
                            </el-form-item>
                            <el-form-item label="Customer">
                                {{item.Customer}}
                            </el-form-item>
                            <el-form-item label="OrderDate">
                                {{item.OrderDate}}
                            </el-form-item>
                            <el-form-item label="RequiredDate">
                                {{item.RequiredDate}}
                            </el-form-item>


                            <el-form-item label="ShipAddress">
                                {{item.ShipAddress}}
                            </el-form-item>
                            <el-form-item label="ShipCity">
                                {{item.ShipCity}}
                            </el-form-item>
                            <el-form-item label="ShipCountry">
                                {{item.ShipCountry}}
                            </el-form-item>
                            <el-form-item label="ShipName">
                                {{item.ShipName}}
                            </el-form-item>
                            <el-form-item label="ShipPostalCode">
                                {{item.ShipPostalCode}}
                            </el-form-item>
                            <el-form-item label="ShipRegion">
                                {{item.ShipRegion}}
                            </el-form-item>
                            <el-form-item label="ShippedDate">
                                {{item.ShippedDate}}
                            </el-form-item>
                        </el-form>
                    </div>
                </el-col>
                <el-col :span="12">
                    <div class="grid-content bg-purple-light">
                        <el-table :data="item.Details"
                                  style="width: 100%" size="mini">
                            <el-table-column prop="ProductName"
                                             label="ProductName" width="250">
                                <template slot-scope="scope">
                                    <span v-if="scope.row.ProductID==list.data.product" style="color:#ff6a00;font-weight:bold;">{{scope.row.ProductName}}</span>
                                    <span v-else>{{scope.row.ProductName}}</span>
                                </template>
                            </el-table-column>
                            <el-table-column prop="UnitPrice"
                                             label="UnitPrice">
                            </el-table-column>
                            <el-table-column prop="Quantity"
                                             label="Quantity">
                            </el-table-column>
                            <el-table-column prop="Discount"
                                             label="Discount">
                            </el-table-column>
                        </el-table>
                    </div>
                </el-col>
            </el-row>


        </el-card>
    </div>
    <el-pagination style="position:absolute;bottom:0px;" background
                   layout="prev, pager, next"
                   :total="list.result.Count"
                   :page-size="list.result.Size" @current-change="onPageChange">
    </el-pagination>
</div>
<script>
    export default{
        props: ["token"],
            data(){
            return {
                list: new beetlexAction('/Orders',
                    { product: this.token.product, employee: this.token.employee, customer: this.token.customer, index: 0 },
                    { Count: 0, Index: 0, Pages: 0, Size: 0, Items: [] }),
            };

        },
        methods: {
            onFormCommand(e){
                this.list.data.index = 0;
                this.onList();
            },
            onPageChange(e){
                this.list.data.index = e - 1;
                this.onList();
            },
            onList(){
                this.list.get();
            },
        },
        watch: {
            token(val){
                this.list.data.product = val.product;
                this.list.data.employee = val.employee;
                this.list.data.customer = val.customer;
                this.list.data.index = 0;
                this.list.get();
            }
        },
        mounted(){
            var search = new autoData();
            f = search.addSelect('customer', "客户");
            f.dataurl = '/CustomersSelectOptions';
            f.nulloption = true;

            f = search.addSelect('employee', "雇员");
            f.dataurl = '/EmployeesSelectOptions';
            f.nulloption = true;

            f = search.addButton('search', '查询');
            f.icon = "el-icon-search";
            search.bindForm(this.$refs.search);
            this.onList();
        }
    }
</script>