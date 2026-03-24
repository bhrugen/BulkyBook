
// Chart colors - Chart.js default palette in hex format
const chartColors = ['#FF6384', '#FF9F40', '#FFCD56', '#4BC0C0', '#36A2EB', '#9966FF', '#C9CBCF'];

// Status-specific colors for order status chart
const statusColorMap = {
	'Pending': '#FFCD56',      // Yellow
	'Approved': '#4BC0C0',     // Teal
	'Processing': '#36A2EB',   // Blue
	'Shipped': '#9966FF',      // Purple
	'Cancelled': '#FF6384',    // Pink/Red
	'Refunded': '#FF9F40',     // Orange
	'Unknown': '#C9CBCF'       // Gray
};


$.getJSON('/Admin/Dashboard/GetChartData', function (data) {
	new Chart(document.getElementById('revenueChart'), {
		type: 'line',
		data: {
			labels: data.monthlyRevenue.map(r => r.label),
			datasets: [{
				label: 'Revenue',
				data: data.monthlyRevenue.map(r => r.revenue),
				borderWidth: 1,
				borderColor: chartColors[3],              // Line color - teal
				backgroundColor: chartColors[3] + '20',   // Fill under line - transparent teal
				fill: true,                               // Fill area under the line - yes
				tension: 0.4,                             // Curve smoothness - smooth curves
				pointRadius: 3,                           // Normal dot size - 3 pixels
				pointHoverRadius: 6,                      // Dot size on hover - 6 pixels
				pointBackgroundColor: chartColors[3]      // Dot fill color - teal
			}]
		},
		options: {
			responsive: true,                             // Chart resizes with window - yes
			maintainAspectRatio: false,                   // Fill container height - yes
			plugins: {
				legend: { display: false },               // Show legend - no (single dataset)
				tooltip: {                                // Popup on hover
					backgroundColor: 'rgba(0, 0, 0, 0.8)', // Dark background
					padding: 12,                           // Inner spacing - 12px
					cornerRadius: 8,                       // Rounded corners - 8px
					displayColors: false,                  // Hide color box
					callbacks: {
						label: ctx => '$' + ctx.parsed.y.toFixed(2) // Format as "$123.45"
					}
				}
			},


			scales: {
				y: {
					beginAtZero: true,
					grid: {
						color: 'rgba(0, 0, 0, 0.05)',     // Grid lines - light gray
						drawBorder: false                  // Hide axis border
					},
					ticks: {
						callback: v => '$' + v,           // Add dollar sign
						padding: 10                        // Space from chart - 10px
					}
				},
				x: {                                      // X-axis (horizontal - months)
					grid: { display: false },             // Hide grid lines
					ticks: { padding: 10 }                // Space from chart - 10px
				}
			}
		}
	});

	new Chart(document.getElementById('ordersChart'), {
		type: 'bar',
		data: {
			labels: data.monthlyOrders.map(r => r.label),
			datasets: [{
				label: 'Orders by Month',
				data: data.monthlyOrders.map(r => r.count),
				borderWidth: 1,
				backgroundColor: chartColors[0] + '80',   // Bar fill - 50% transparent red
				borderColor: chartColors[0],              // Bar border - solid red
				borderWidth: 1,                           // Border thickness - 1px
				borderRadius: 8,                          // Rounded corners - 8px
				hoverBackgroundColor: chartColors[0]      // Hover color - solid red
			}]
		},
		options: {
			responsive: true,                             // Resize with window - yes
			maintainAspectRatio: false,                   // Fill container height - yes
			plugins: {
				legend: { display: false },               // Show legend - no
				tooltip: {
					backgroundColor: 'rgba(0, 0, 0, 0.8)', // Dark background
					padding: 12,                           // Inner spacing - 12px
					cornerRadius: 8,                       // Rounded corners - 8px
					displayColors: false                   // Hide color box
				}
			},
			scales: {
				y: {                                      // Y-axis (vertical - counts)
					beginAtZero: true,                    // Start at 0 - yes
					grid: {
						color: 'rgba(0, 0, 0, 0.05)',     // Grid lines - light gray
						drawBorder: false                  // Hide axis border
					},
					ticks: {
						stepSize: 1,                      // Whole numbers only
						padding: 10                        // Space from chart - 10px
					}
				},
				x: {                                      // X-axis (horizontal - months)
					grid: { display: false },             // Hide grid lines
					ticks: { padding: 10 }                // Space from chart - 10px
				}
			}
		}
	});



	new Chart(document.getElementById('statusChart'), {
		type: 'doughnut',
		data: {
			labels: data.statusBreakdown.map(r => r.status),
			datasets: [{
				label: 'Order Status',
				data: data.statusBreakdown.map(r => r.count),
				backgroundColor: data.statusBreakdown.map(s => statusColorMap[s.status] || '#999'), // Color by status
				borderWidth: 2,                           // Space between segments - 2px
				borderColor: '#fff',                      // Segment separator - white
				hoverBorderWidth: 3,                      // Thicker border on hover - 3px
				hoverOffset: 10                           // Segment moves out - 10px
			}]
		},
		options: {
			responsive: true,                             // Resize with window - yes
			maintainAspectRatio: false,                   // Fill container - yes
			cutout: '70%',                                // Center hole size - 70% of radius
			plugins: {
				legend: {                                 // Legend configuration
					position: 'bottom',                   // Position - bottom of chart
					labels: {
						padding: 15,                      // Space between items - 15px
						usePointStyle: true,              // Use circles not rectangles
						pointStyle: 'circle',             // Legend marker shape - circle
						font: { size: 12 }                // Text size - 12px
					}
				},
				tooltip: {
					backgroundColor: 'rgba(0, 0, 0, 0.8)', // Dark background
					padding: 12,                           // Inner spacing - 12px
					cornerRadius: 8                        // Rounded corners - 8px
				}
			}
		}
	});



	new Chart(document.getElementById('categoryChart'), {
		type: 'bar',
		data: {
			labels: data.productsPerCategory.map(r => r.category),
			datasets: [{
				label: 'Products by Category',
				data: data.productsPerCategory.map(r => r.count),
				backgroundColor: chartColors.map(c => c + '80'), // Bar fills - 50% transparent
				borderColor: chartColors,                 // Bar borders - solid colors
				borderWidth: 1,                           // Border thickness - 1px
				borderRadius: 8,                          // Rounded corners - 8px
				hoverBackgroundColor: chartColors         // Hover - solid colors
			}]
		},
		options: {
			indexAxis: 'y',
			responsive: true,
			maintainAspectRatio: false,
			plugins: {
				legend: { display: false },               // Show legend - no
				tooltip: {
					backgroundColor: 'rgba(0, 0, 0, 0.8)', // Dark background
					padding: 12,                           // Inner spacing - 12px
					cornerRadius: 8,                       // Rounded corners - 8px
					displayColors: false                   // Hide color box
				}
			},
			scales: {
				x: {                                      // X-axis (horizontal - product counts)
					beginAtZero: true,                    // Start at 0 - yes
					grid: {
						color: 'rgba(0, 0, 0, 0.05)',     // Grid lines - light gray
						drawBorder: false                  // Hide axis border
					},
					ticks: {
						stepSize: 1,                      // Whole numbers only
						padding: 10                        // Space from chart - 10px
					}
				},
				y: {                                      // Y-axis (vertical - category names)
					grid: { display: false },             // Hide grid lines
					ticks: { padding: 10 }                // Space from chart - 10px
				}
			}
		}
	});
})