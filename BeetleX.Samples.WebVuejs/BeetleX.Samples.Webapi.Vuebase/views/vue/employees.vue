<div>
    <el-table :data="getEmployees.result"
              style="width: 100%" size="mini">
        <el-table-column prop="EmployeeID"
                         label="EmployeeID"
                         width="180">
        </el-table-column>
        <el-table-column prop="LastName"
                         label="LastName"
                         width="180">
        </el-table-column>
        <el-table-column prop="FirstName"
                         label="FirstName">
        </el-table-column>
        <el-table-column prop="Title"
                         label="Title">
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
                getEmployees: new beetlexAction('/Employees', null, []),
            }
        },
        methods: {

        },
        mounted(){
            this.getEmployees.get();
        },
    }
</script>