<div>
    <div id="bar-simple" style="height:500px;">

    </div>
</div>
<script>
    export default {
        data() {
            return {};
        },
        mounted() {
            var chartDom = document.getElementById('bar-simple');
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
                    data: [120, 200, 150, 80, 70, 110, 130],
                    type: 'bar'
                }]
            };

            option && myChart.setOption(option);


            this.$addResize(function () {
                myChart.resize();
            });
        }
    }
</script>