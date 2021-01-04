
var ctx = document.getElementById("myChart").getContext('2d');
var myChart = new Chart(ctx, {
    type: 'pie',
    data: {
        labels: [],
        datasets: [{
            backgroundColor: [
                "#2ecc71",
                "#3498db",
                "#95a5a6",
                "#9b59b6",
                "#f1c40f",
                "#e74c3c",
                "#34495e"
            ],
            data: []
        }]
    }
});


$(document).ready(function () {

    PieChart('/manager/statistic/StatisticGender', data = null);

})





$(document).ready(function () {

    PieChart_Call2('/manager/resultStatistic/StatisticRate', data = null);
    LineChart_Call('/manager/resultStatistic/AreaRateGioi', data = null);
    BarChart_Call('/manager/resultStatistic/AverageScore', data = null);

})


function PieChart_Call2(url, data = null) {//data này có thể có có thể ko
    if (data == null) {
        if (url == '/manager/resultStatistic/StatisticRate') {
            $('#titlePie').text('Thống kê xếp loại học sinh toàn trường');
        }
        else if (url == '/manager/resultStatistic/StatisticRate10') {
            $('#titlePie').text('Thống kê xếp loại học sinh khối 10');
        }
        else if (url == '/manager/resultStatistic/StatisticRate11') {
            $('#titlePie').text('Thống kê xếp loại học sinh khối 11');
        }
        else {
            $('#titlePie').text('Thống kê xếp loại học sinh khối 12');
        }

        $.getJSON(url).done(function (response) {
            myChart.data.labels = response.labels; // label là hàng ngang, các tiêu đề, ví du : tháng 1,tháng 2,....
            myChart.data.datasets[0].data = response.values; // value là giá trị tương ứng cho từng tiêu đề...
            myChart.update();
        })


    }

}



function LineChart_Call(url, data = null) {//data này có thể có có thể ko
    if (data == null) {
        if (url == '/manager/resultStatistic/AreaRateGioi') {
            $('#titleLine').text('Tỉ lệ học sinh giỏi các khối');
        }
        else if (url == '/manager/resultStatistic/AreaRateKha') {
            $('#titleLine').text('Tỉ lệ học sinh khá các khối');
        }
        else if (url == '/manager/resultStatistic/AreaRateTB') {
            $('#titleLine').text('Tỉ lệ học sinh trung bình các khối');
        }
        else {
            $('#titleLine').text('Tỉ lệ học sinh yếu các khối');
        }

        $.getJSON(url).done(function (response) {
            myLineChart.data.labels = response.labels; // label là hàng ngang, các tiêu đề, ví du : tháng 1,tháng 2,....
            myLineChart.data.datasets[0].data = response.values; // value là giá trị tương ứng cho từng tiêu đề...
            myLineChart.update();
        })


    }

}
function BarChart_Call(url, data = null) {//data này có thể có có thể ko
    if (data == null) {

        if (url == '/manager/resultStatistic/AverageScore')
            $('#titleBar').text('Điểm trung bình các môn học toàn trường');
        else if (url == '/manager/resultStatistic/AverageScore10')
            $('#titleBar').text('Điểm trung bình các môn học khối 10');
        else if (url == '/manager/resultStatistic/AverageScore11')
            $('#titleBar').text('Điểm trung bình các môn học khối 11');
        else $('#titleBar').text('Điểm trung bình các môn học khối 12');

        $.getJSON(url).done(function (response) {
            myBarChart.data.labels = response.labels; // label là hàng ngang, các tiêu đề, ví du : tháng 1,tháng 2,....
            myBarChart.data.datasets[0].data = response.values; // value là giá trị tương ứng cho từng tiêu đề...
            myBarChart.update();
        })


    }

}
