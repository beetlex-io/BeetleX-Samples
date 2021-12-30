<div>
    <el-row>
        <el-card class="box-card">
            <div slot="header" class="clearfix">
                <span>产品销售走势</span>

            </div>
            <div id="productStats" style="width:100%;height:300px"></div>
        </el-card>
    </el-row>
    <br />
    <el-row>
        <el-col :span="8" style="padding-right:5px;">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>雇员销售统计</span>

                </div>
                <div id="employeeStats" style="width:100%;height:350px"></div>
            </el-card>
        </el-col>

        <el-col :span="8" style="        padding-left: 5px;
        padding-right: 5px;
">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>客户销售统计</span>

                </div>
                <div id="customerTopStats" style="width:100%;height:350px"></div>
            </el-card>
        </el-col>

        <el-col :span="8" style="padding-left:5px;">
            <el-card class="box-card">
                <div slot="header" class="clearfix">
                    <span>产品销售统计</span>

                </div>
                <div id="productTopStats" style="width:100%;height:350px"></div>
            </el-card>
        </el-col>
    </el-row>
</div>
<script>
    export default{
        data(){
            return {
                productStats: null,
                employeeStats: null,
                customerTopStats: null,
                productTopStats: null
            };
        },
        methods: {
            onResize(){
               
                if (this.productStats)
                    this.productStats.resize();
                if (this.employeeStats)
                    this.employeeStats.resize();
                if (this.customerTopStats)
                    this.customerTopStats.resize();
                if (this.productTopStats)
                    this.productTopStats.resize();
            },
            onOrderStats()
            {
                var action = new beetlexAction('/OrderStats').asyncget().then(r => {
                    option = {
                        tooltip: {
                            trigger: 'axis',
                            axisPointer: {
                                type: 'cross',
                                label: {
                                    backgroundColor: '#6a7985'
                                }
                            }
                        },
                        legend: {
                            data: r.Products
                        },
                        toolbox: {
                            feature: {
                                saveAsImage: {}
                            }
                        },
                        grid: {
                            left: '3%',
                            right: '4%',
                            bottom: '3%',
                            containLabel: true
                        },
                        xAxis: [
                            {
                                type: 'category',
                                boundaryGap: false,
                                data: r.Months
                            }
                        ],
                        yAxis: [
                            {
                                type: 'value'
                            }
                        ],
                        series: []
                    };
                    for (i = 0; i < r.Products.length; i++) {
                        option.series.push({
                            name: r.Products[i],
                            type: 'line',
                            stack: '总量',
                            areaStyle: {},
                            data: r.Items[i]
                        });
                    }
                    this.productStats.setOption(option, true);
                });
            },
            onEmployeeStats(){
                var action = new beetlexAction('/EmployeeStats').asyncget().then(r => {
                    option = {
                        tooltip: {
                            trigger: 'item',
                            formatter: '{a} <br/>{b} : {c} ({d}%)'
                        },
                        legend: {
                            orient: 'horizontal',
                            left: 'top',
                            data: r
                        },
                        series: [
                            {
                                name: 'employee',
                                type: 'pie',
                                radius: '55%',
                                center: ['50%', '60%'],
                                data:r,
                                emphasis: {
                                    itemStyle: {
                                        shadowBlur: 10,
                                        shadowOffsetX: 0,
                                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                                    }
                                }
                            }
                        ]
                    };
                    this.employeeStats.setOption(option, true);
                });
            },
            onCustomerStats(){
                var action = new beetlexAction('/CustomerStats').asyncget().then(r => {
                    option = {
                        tooltip: {
                            trigger: 'item',
                            formatter: '{a} <br/>{b} : {c} ({d}%)'
                        },
                        legend: {
                            orient: 'horizontal',
                            left: 'top',
                            data: r
                        },
                        series: [
                            {
                                name: 'employee',
                                type: 'pie',
                                radius: '55%',
                                center: ['50%', '60%'],
                                data: r,
                                emphasis: {
                                    itemStyle: {
                                        shadowBlur: 10,
                                        shadowOffsetX: 0,
                                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                                    }
                                }
                            }
                        ]
                    };
                    this.customerTopStats.setOption(option, true);
                });
            },
            onProductStats(){
                var action = new beetlexAction('/ProductStats').asyncget().then(r => {
                    option = {
                        tooltip: {
                            trigger: 'item',
                            formatter: '{a} <br/>{b} : {c} ({d}%)'
                        },
                        legend: {
                            orient: 'horizontal',
                            left: 'top',
                            data: r
                        },
                        series: [
                            {
                                name: 'employee',
                                type: 'pie',
                                radius: '55%',
                                center: ['50%', '60%'],
                                data: r,
                                emphasis: {
                                    itemStyle: {
                                        shadowBlur: 10,
                                        shadowOffsetX: 0,
                                        shadowColor: 'rgba(0, 0, 0, 0.5)'
                                    }
                                }
                            }
                        ]
                    };
                    this.productTopStats.setOption(option, true);
                });
            }
        },
        mounted(){
            this.$addResize(this.onResize);
            var dom = document.getElementById("productStats");
            this.productStats = echarts.init(dom);
            dom = document.getElementById("employeeStats");
            this.employeeStats = echarts.init(dom);

            dom = document.getElementById("customerTopStats");
            this.customerTopStats = echarts.init(dom);

            dom = document.getElementById("productTopStats");
            this.productTopStats = echarts.init(dom);

            this.onOrderStats();
            this.onEmployeeStats();
            this.onCustomerStats();
            this.onProductStats();
        }

    }
</script>