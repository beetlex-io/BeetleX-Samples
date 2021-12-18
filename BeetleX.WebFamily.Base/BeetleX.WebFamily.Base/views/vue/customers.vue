<div>
    <div>
        <el-form :inline="true" size="mini" :model="search" label-width="120px">
            <el-form-item label="Country" prop="country">
                <el-select size="mini" v-model="search.country">
                    <el-option label="all..." value="" key=""></el-option>
                    <el-option v-for="(item,index) in country_options" :label="item.label" :value="item.value" :key="item.value"></el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="Name" prop="name"><el-input size="mini" v-model="search.name"></el-input></el-form-item>
            <el-button size="mini" style="padding-left:10px; padding-right:10px;" @click="search.index=0;onList()">search</el-button>
        </el-form>
    </div>
    <el-table size="mini" :data="items">
        <el-table-column type="selection" width="45"></el-table-column>
        <el-table-column label="CompanyName">
            <template slot-scope="item">
                <el-link @click="onViewDetail(item.row)">{{item.row.CompanyName}}</el-link>
            </template>
        </el-table-column>
        <el-table-column label="Address">
            <template slot-scope="item">
                <label>{{item.row.Address}}</label>
            </template>
        </el-table-column>
        <el-table-column label="Region">
            <template slot-scope="item">
                <label>{{item.row.Region}}</label>
            </template>
        </el-table-column>
        <el-table-column label="Country">
            <template slot-scope="item">
                <label>{{item.row.Country}}</label>
            </template>
        </el-table-column>
        <el-table-column label="City">
            <template slot-scope="item">
                <label>{{item.row.City}}</label>
            </template>
        </el-table-column>
        <el-table-column label="Phone">
            <template slot-scope="item">
                <label>{{item.row.Phone}}</label>
            </template>
        </el-table-column>
        <div slot="append" style="text-align:right;padding:5px;">
            <el-pagination background layout="prev, pager, next" :current-page="search.index+1" :page-size="10" :total="count" @current-change="onPageChange">
            </el-pagination>
        </div>
    </el-table>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                items: [],
                count: 0,
                search: {
                    country: '',
                    name: '',
                    index: 0,
                },
                country_options: [],
            };
        },
        methods: {
            onPageChange(e) {
                this.search.index = e - 1;
                this.onList();
            },
            onViewDetail(e) {
                this.$openWindow('(客户)' + e.CompanyName, '(客户)' + e.CompanyName, 'customerinfo', e, 'fas fa-hospital-user');
            },
            onListCountry() {
                this.$get("/CustomerCountrySelectOptions").then(r => {
                    this.country_options = r;
                });
            },
            onList() {
                this.$get('/Customers', this.search).then(r => {
                    this.items = r.Items;
                    this.count = r.Count;
                });
            }
        },
        mounted() {
            this.onListCountry();
            this.onList();
        }
    }
</script>
