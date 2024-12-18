using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

namespace PresentationTier.ViewModels
{
    public class DashboardViewModel : ReactiveObject
    {
        public ObservableCollection<SensorReadingViewModel> SensorReadings { get; set; }

        private PlotModel _sensorChartModel;
        public PlotModel SensorChartModel
        {
            get => _sensorChartModel;
            set => this.RaiseAndSetIfChanged(ref _sensorChartModel, value);
        }

        public DashboardViewModel()
        {
            SensorReadings = new ObservableCollection<SensorReadingViewModel>();

            // Load some sample data
            LoadSampleData();

            // Initialize the chart
            InitializeChart();
        }

        private void LoadSampleData()
        {
            SensorReadings.Add(new SensorReadingViewModel { SensorType = "Temperature", Value = 23.5, Timestamp = DateTime.Now });
            SensorReadings.Add(new SensorReadingViewModel { SensorType = "pH", Value = 6.8, Timestamp = DateTime.Now.AddMinutes(1) });
            SensorReadings.Add(new SensorReadingViewModel { SensorType = "EC", Value = 1.2, Timestamp = DateTime.Now.AddMinutes(2) });
            SensorReadings.Add(new SensorReadingViewModel { SensorType = "Temperature", Value = 24.0, Timestamp = DateTime.Now.AddMinutes(3) });
            SensorReadings.Add(new SensorReadingViewModel { SensorType = "pH", Value = 7.0, Timestamp = DateTime.Now.AddMinutes(4) });
        }

        private void InitializeChart()
        {
            var plotModel = new PlotModel { Title = "Sensor Readings Over Time" };

            // Add a LineSeries for each Sensor Type
            var tempSeries = new LineSeries { Title = "Temperature", Color = OxyColors.Red, MarkerType = MarkerType.Circle };
            var phSeries = new LineSeries { Title = "pH", Color = OxyColors.Green, MarkerType = MarkerType.Circle };
            var ecSeries = new LineSeries { Title = "EC", Color = OxyColors.Blue, MarkerType = MarkerType.Circle };

            foreach (var reading in SensorReadings)
            {
                if (reading.SensorType == "Temperature")
                    tempSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(reading.Timestamp), reading.Value));
                else if (reading.SensorType == "pH")
                    phSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(reading.Timestamp), reading.Value));
                else if (reading.SensorType == "EC")
                    ecSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(reading.Timestamp), reading.Value));
            }

            plotModel.Series.Add(tempSeries);
            plotModel.Series.Add(phSeries);
            plotModel.Series.Add(ecSeries);

            // Add a time-based X-axis
            plotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "HH:mm",
                Title = "Time",
                IntervalType = DateTimeIntervalType.Auto,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            // Add a Y-axis
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Sensor Values",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            SensorChartModel = plotModel;
        }
    }

    public class SensorReadingViewModel
    {
        public string SensorType { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
