$.getJSON('/Admin/Dashboard/GetChartData', function (data) {
	new Chart(document.getElementById('revenueChart'), {
		type: 'line',
		data: {
			labels: data.monthlyRevenue.map(r => r.label),
			datasets: [{
				label: 'Revenue',
				data: data.monthlyRevenue.map(r => r.revenue),
				borderWidth: 1
			}]
		},
		options: {
			scales: {
				y: {
					beginAtZero: true
				}
			}
		}
	});
})