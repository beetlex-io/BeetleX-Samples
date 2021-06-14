<div>
    <div id="line-simple" style="height:600px">

    </div>
</div>
<script>
    export default {
        data() {
            return {};
        },
        mounted() {
            var chartDom = document.getElementById('line-simple');
            var myChart = echarts.init(chartDom);
            var option;
            option = {
                xAxis: {
                    type: 'category',
                    data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
                },
                yAxis: {
                    type: 'value'
                },
                series: [{
                    data: [150, 230, 224, 218, 135, 147, 260],
                    type: 'line'
                }]
            };
            option && myChart.setOption(option);
            this.$addResize(function () {
                myChart.resize();
            });
        }
    }
</script>