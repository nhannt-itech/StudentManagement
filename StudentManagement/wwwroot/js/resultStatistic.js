// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

// Pie Chart Example
var ctx = document.getElementById("myPieChart");
var myPieChart = new Chart(ctx, {
    type: 'doughnut',
    data: {
        labels: [],
        datasets: [{
            data: [],
            backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', '#e74a3b', '#f6c23e', '#5a5c69'],
            hoverBackgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', '#e74a3b', '#f6c23e', '#5a5c69'],
            hoverBorderColor: "rgba(234, 236, 244, 1,1,1)",
        }],
    },
    options: {
        maintainAspectRatio: false,
        tooltips: {
            backgroundColor: "rgb(255,255,255,255,255,255)",
            bodyFontColor: "#858796",
            borderColor: '#dddfeb',
            borderWidth: 1,
            xPadding: 15,
            yPadding: 15,
            displayColors: false,
            caretPadding: 10,
        },
        legend: {
            display: false
        },
        cutoutPercentage: 80,
    },
});

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
            myPieChart.data.labels = response.labels; // label là hàng ngang, các tiêu đề, ví du : tháng 1,tháng 2,....
            myPieChart.data.datasets[0].data = response.values; // value là giá trị tương ứng cho từng tiêu đề...
            myPieChart.update();
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
