<div>
    <el-form :inline="true" class="demo-form-inline" size="mini">
        <el-form-item label="">
            <el-input v-model="searchText" placeholder="查询内容"></el-input>
        </el-form-item>
        <el-form-item>
            <el-button type="primary" @click="page=0;onList();">查询</el-button>
        </el-form-item>
    </el-form>
    <el-table size="mini" :data="items">
        <el-table-column type="selection" width="45"></el-table-column>
        <el-table-column label="Title">
            <template slot-scope="item">
                <el-link @click="$emit('select',item.row)">{{item.row.Title}}</el-link>
            </template>
        </el-table-column>
        <el-table-column label="Category">
            <template slot-scope="item">
                <label>{{item.row.Category}}</label>
            </template>
        </el-table-column>
        <el-table-column label="Tag">
            <template slot-scope="item">
                <label>{{item.row.Tag}}</label>
            </template>
        </el-table-column>
        <el-table-column label="CreateTime">
            <template slot-scope="item">
                <label>{{item.row.CreateTime}}</label>
            </template>
        </el-table-column>
        <div slot="append" style="text-align:right;padding:5px;">
            <el-pagination background layout="prev, pager, next" :page-size="10" :total="count" @current-change="onPageChange">
            </el-pagination>
        </div>
    </el-table>
    <el-row>
        <el-col :span="6" style="padding:5px">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>分类汇总</span>


                </div>
                <el-tag v-for="item in categories" size="mini" style="margin:2px;"
                        type="info"
                        effect="dark">
                    {{ item.Name }}({{ item.Value }})
                </el-tag>
            </el-card>
        </el-col>
        <el-col :span="6" style="padding:5px">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>标签汇总</span>

                </div>
                <el-tag v-for="item in tags" size="mini" style="margin:2px;"
                        effect="dark">
                    {{ item.Name }}({{ item.Value }})
                </el-tag>
            </el-card>
        </el-col>
        <el-col :span="6" style="padding:5px">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>年汇总</span>

                </div>
                <el-tag v-for="item in years" size="mini" style="margin:2px;"
                        type="success"
                        effect="dark">
                    {{ item.Name }}({{ item.Value }})
                </el-tag>
            </el-card>
        </el-col>
        <el-col :span="6" style="padding:5px">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>年月汇总</span>

                </div>
                <el-tag v-for="item in months" size="mini" style="margin:2px;"
                        type="warning"
                        effect="dark">
                    {{ item.Name }}({{ item.Value }})
                </el-tag>
            </el-card>
        </el-col>
    </el-row>
</div>
<script>
    export default {
        props: [],
        data() {
            return {
                searchText: '',
                count: 0,
                items: [],
                page: 0,
                categories: [],
                tags: [],
                years: [],
                months: [],
            };
        },
        methods: {
            onPageChange(e) {
                this.page = e - 1;
                this.onList();
            },
            onAggs() {
                this.$get("/AggsCategories").then(r => {
                    this.categories = r;
                });
                this.$get("/AggsTags").then(r => {
                    this.tags = r;
                });
                this.$get("/AggsYear").then(r => {
                    this.years = r;
                });
                this.$get("/AggsYearMonth").then(r => {
                    this.months = r;
                });
            },
            onList() {
                this.onAggs();
                this.$get("/Search", { searchText: this.searchText, page: this.page }).then(r => {
                    this.items = r.Item1;
                    this.count = r.Item2;
                    console.log(r);
                });
            }
        },
        mounted() {
            this.onList();
        }
    }
</script>
