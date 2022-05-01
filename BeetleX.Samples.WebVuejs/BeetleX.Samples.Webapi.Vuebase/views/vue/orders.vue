<div>
    <el-form :inline="true" size="mini">
        <el-form-item label="Employee">
            <el-select v-model="listOrders.data.employee" placeholder="Employee">
                <el-option label="Null" value=""></el-option>
                <el-option v-for="item in listEmployee.result" :label="item.FirstName+' '+item.LastName" :value="item.EmployeeID"></el-option>
            </el-select>
        </el-form-item>
        <el-form-item label="Customer">
            <el-select v-model="listOrders.data.customer" placeholder="Customer">
                <el-option label="Null" value=""></el-option>
                <el-option v-for="item in listCustomer.result" :label="item.CompanyName" :value="item.CustomerID"></el-option>
            </el-select>
        </el-form-item>
        <el-form-item>
            <el-button type="primary" @click="onSearch">Search</el-button>
        </el-form-item>
    </el-form>
    <el-table :data="listOrders.result.Items"
              style="width: 100%" size="mini">
        <el-table-column prop="OrderID"
                         label="OrderID"
                         width="180">
        </el-table-column>
        <el-table-column prop="CompanyName"
                         label="Customer"
                         width="180">
        </el-table-column>
        <el-table-column label="Employee">
            <template slot-scope="scope">
                {{scope.row.FirstName}} {{scope.row.LastName}}
            </template>
        </el-table-column>
        <el-table-column prop="OrderDate"
                         label="OrderDate">
        </el-table-column>
        <el-table-column prop="RequiredDate"
                         label="RequiredDate">
        </el-table-column>
        <el-table-column prop="ShippedDate"
                         label="ShippedDate">
        </el-table-column>
    </el-table>
    <el-pagination background
                   layout="prev, pager, next"
                   :total="listOrders.result.Count"
                   :page-size="listOrders.result.Size" @current-change="onPageChange">
    </el-pagination>
</div>
<script>
    {
        data(){
            return {
                listCustomer: new beetlexAction("/ListCustomerName", null, []),
                listEmployee: new beetlexAction("ListEmployeeName", null, []),
                listOrders: new beetlexAction("/Orders", { index: 0, customer: '', employee: '' }, { Size: 20, Items: [], Count: 0 })
            };
        },
        methods: {
            onPageChange(e){
                this.listOrders.data.index = e - 1;
                this.listOrders.get();
            },
            onSearch(){
                this.listOrders.data.index =0;
                this.listOrders.get();
            },
        },
        mounted(){
            this.listCustomer.get();
            this.listEmployee.get();
            this.listOrders.get();
        },
    }
</script>