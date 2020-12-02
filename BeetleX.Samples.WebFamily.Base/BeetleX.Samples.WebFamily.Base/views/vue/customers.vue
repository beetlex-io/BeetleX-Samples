<div>
    <el-table :data="getCustomers.result"
              style="width: 100%" size="mini">
        <el-table-column prop="CustomerID"
                         label="CustomerID"
                         width="180">
        </el-table-column>
        <el-table-column prop="CompanyName"
                         label="CompanyName"
                         width="180">
        </el-table-column>
        <el-table-column prop="ContactName"
                         label="ContactName">
        </el-table-column>
        <el-table-column prop="ContactTitle"
                         label="ContactTitle">
        </el-table-column>
        <el-table-column prop="Country"
                         label="Country">
        </el-table-column>
        <el-table-column prop="Address"
                         label="Address">
        </el-table-column>
    </el-table>
</div>
<script>
    {
        data(){
            return {
                getCustomers: new beetlexAction("/Customers", null, []),
            };
        },
        methods: {

        },
        mounted(){
            this.getCustomers.get();
        }
    }
</script>