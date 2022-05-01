<div>
    <el-form :inline="true" size="mini">
        <el-form-item label="Country">
            <el-select v-model="getCustomers.data.country" placeholder="Country">
                <el-option label="Null" value=""></el-option>
                <el-option v-for="item in listCountry.result" :label="item" :value="item"></el-option>
            </el-select>
        </el-form-item>
        <el-form-item>
            <el-button type="primary" @click="onSearch">Search</el-button>
        </el-form-item>
    </el-form>
    <el-table :data="getCustomers.result.Items"
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
    <el-pagination background
                   layout="prev, pager, next"
                   :total="getCustomers.result.Count"
                   :page-size="getCustomers.result.Size" @current-change="onPageChange">
    </el-pagination>
</div>
<script>
    {
        data(){
            return {
                getCustomers: new beetlexAction("/Customers", { country: '', index: 0 }, { Items: [], Count: 0, Size: 20 }),
                listCountry: new beetlexAction("/CustomerCountry", null, []),
            };
        },
        methods: {
            onSearch(){
                this.getCustomers.data.index = 0;
                this.getCustomers.get();
            },
            onPageChange(e){
                this.getCustomers.data.index = e - 1;
                this.getCustomers.get();
                console.log(e);
            },
        },
        mounted(){
            this.getCustomers.get();
            this.listCountry.get();
        }
    }
</script>