<div>
    <el-form size="mini" :model="record" label-width="120px" ref="dataform">
        <el-row>
            <el-col :span="24">
                <el-form-item label="CompanyName" prop="CompanyName"><el-input size="mini" v-model="record.CompanyName"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="24">
                <el-form-item label="Address" prop="Address"><el-input size="mini" v-model="record.Address"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="ContactTitle" prop="ContactTitle"><el-input size="mini" v-model="record.ContactTitle"></el-input></el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="ContactName" prop="ContactName"><el-input size="mini" v-model="record.ContactName"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="Region" prop="Region"><el-input size="mini" v-model="record.Region"></el-input></el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="Country" prop="Country"><el-input size="mini" v-model="record.Country"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="City" prop="City"><el-input size="mini" v-model="record.City"></el-input></el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="PostalCode" prop="PostalCode"><el-input size="mini" v-model="record.PostalCode"></el-input></el-form-item>
            </el-col>
        </el-row>
        <el-row>
            <el-col :span="12">
                <el-form-item label="Phone" prop="Phone"><el-input size="mini" v-model="record.Phone"></el-input></el-form-item>
            </el-col>
            <el-col :span="12">
                <el-form-item label="Fax" prop="Fax"><el-input size="mini" v-model="record.Fax"></el-input></el-form-item>
            </el-col>
        </el-row>
    </el-form>
</div>
<script>
    export default {
        props: ['token'],
        data() {
            return {
                record: this.token
            };
        },
        methods: {
        },
        mounted() {
        }
    }
</script>
