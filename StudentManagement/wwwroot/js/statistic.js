




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

    PieChart('/manager/statistic/StatisticGender', data = null);

})


function PieChart(url, data = null) {//data này có thể có có thể ko
    if (data == null) {
        if (url == '/manager/statistic/StatisticGender') {
            $('#titlePie').text('Thống kê tỉ lệ nam nữ toàn trường');
        }
        else if (url == '/manager/statistic/StatisticGender10') {
            $('#titlePie').text('Thống kê tỉ lệ nam nữ khối 10');
        }
        else if (url == '/manager/statistic/StatisticGender11') {
            $('#titlePie').text('Thống kê tỉ lệ nam nữ khối 11');
        }
        else {
            $('#titlePie').text('Thống kê tỉ lệ nam nữ khối 12');
        }

        $.getJSON(url).done(function (response) {
            myPieChart.data.labels = response.labels; // label là hàng ngang, các tiêu đề, ví du : tháng 1,tháng 2,....
            myPieChart.data.datasets[0].data = response.values; // value là giá trị tương ứng cho từng tiêu đề...
            myPieChart.update();
        })


    }

}