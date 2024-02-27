using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.WinForms;

namespace SectionCutter
{
    class PlotResults
    {
        public PlotResults () { }

        //listResults is all of the gathered F1,F2, F3, M1... from each load step.
        //SelectedObjects = load steps you want to plot
        //range of values = the coordinates of where the cut is located.
        //shear chart = cartiesian chart for chart of interest


        public static void GraphShearResults(List<SectionResults> listResults, List<string> SelectedObjects, double[] rangeofvalues, LiveCharts.WinForms.CartesianChart shearChart, int givenIndex) 
        {
            // Define your DangerBrush
            shearChart.Series.Clear();
            SolidColorBrush DangerBrush = new SolidColorBrush(Color.FromRgb(255,215, 0));
            
            if (SelectedObjects.Count <= 1)
            {
                List<ChartValues<LiveCharts.Defaults.ObservablePoint>> plottingPoints = new List<ChartValues<LiveCharts.Defaults.ObservablePoint>>();
                for (int i = 0; i < SelectedObjects.Count; i++)
                {
                    ChartValues<LiveCharts.Defaults.ObservablePoint> shearPoints = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();
                    for (int j = 0; j < listResults[0].F1.Length; j++)
                    {
                        shearPoints.Add(new LiveCharts.Defaults.ObservablePoint { X = rangeofvalues[j], Y = listResults[0].F1[j] });
                    }

                    var scatterShearSeries = new LiveCharts.Wpf.LineSeries
                    {
                        Title = listResults[0].LoadDirection, //this will need to be written, map to name of load case selected.
                        Values = shearPoints,
                        Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 140, 105)),
                        Fill = System.Windows.Media.Brushes.Transparent,

                    };
                    var Mapper = Mappers.Xy<ObservableValue>()
                        .X((item, index) => index)
                        .Y(item => item.Value)
                        .Fill((item, index) => index == givenIndex ? DangerBrush : null)
                        .Stroke((item, index) => index == givenIndex ? DangerBrush : null);
                    scatterShearSeries.Configuration = Mapper;

                    //shearChart.Series.Clear();
                    shearChart.Series.Add(scatterShearSeries);
                }
            }
            else
            {
                List<ChartValues<LiveCharts.Defaults.ObservablePoint>> plottingPoints = new List<ChartValues<LiveCharts.Defaults.ObservablePoint>>();
                for (int i = 0; i < SelectedObjects.Count; i++)
                {
                    int mySelectedDirection = 0;
                    mySelectedDirection = listResults.FindIndex(x => x.LoadDirection == SelectedObjects[i]);
                    ChartValues<LiveCharts.Defaults.ObservablePoint> shearPoints = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();
                    for (int j = 0; j < listResults[mySelectedDirection].F1.Length; j++)
                    {
                        shearPoints.Add(new LiveCharts.Defaults.ObservablePoint { X = rangeofvalues[j], Y = listResults[mySelectedDirection].F1[j] });
                    }

                    var scatterShearSeries = new LiveCharts.Wpf.LineSeries
                    {
                        Title = listResults[mySelectedDirection].LoadDirection, //this will need to be written, map to name of load case selected.
                        Values = shearPoints,
                        Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 140, 105)),
                        Fill = System.Windows.Media.Brushes.Transparent,
                    };
                    var Mapper = Mappers.Xy<ObservableValue>()
                        .X((item, index) => index)
                        .Y(item => item.Value)
                        .Fill((item, index) => index == givenIndex ? DangerBrush : null)
                        .Stroke((item, index) => index == givenIndex ? DangerBrush : null);
                    scatterShearSeries.Configuration = Mapper;
                    shearChart.Series.Add(scatterShearSeries);
                    
                }
            }
        }

        public static void GraphMomentResults(List<SectionResults> listResults, List<string> SelectedObjects, double[] rangeofvalues, LiveCharts.WinForms.CartesianChart momentChart)
        {
            momentChart.Series.Clear();
            if (SelectedObjects.Count <= 1)
            {
                List<ChartValues<LiveCharts.Defaults.ObservablePoint>> plottingPoints = new List<ChartValues<LiveCharts.Defaults.ObservablePoint>>();
                for (int i = 0; i < SelectedObjects.Count; i++)
                {
                    ChartValues<LiveCharts.Defaults.ObservablePoint> momentPoints = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();
                    for (int j = 0; j < listResults[0].F1.Length; j++)
                    {
                        momentPoints.Add(new LiveCharts.Defaults.ObservablePoint { X = rangeofvalues[j], Y = listResults[0].M3[j] });
                    }

                    var scatterShearSeries = new LiveCharts.Wpf.LineSeries
                    {
                        Title = listResults[0].LoadDirection, //this will need to be written, map to name of load case selected.
                        Values = momentPoints,
                        Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 140, 105)),
                        Fill = System.Windows.Media.Brushes.Transparent,


                    };
                    momentChart.Series.Add(scatterShearSeries);
                }
            }
            else
            {
                List<ChartValues<LiveCharts.Defaults.ObservablePoint>> plottingPoints = new List<ChartValues<LiveCharts.Defaults.ObservablePoint>>();
                for (int i = 0; i < SelectedObjects.Count; i++)
                {
                    int mySelectedDirection = listResults.FindIndex(x => x.LoadDirection == SelectedObjects[i]);
                    ChartValues<LiveCharts.Defaults.ObservablePoint> momentPoints = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();
                    for (int j = 0; j < listResults[mySelectedDirection].F1.Length; j++)
                    {
                        momentPoints.Add(new LiveCharts.Defaults.ObservablePoint { X = rangeofvalues[j], Y = listResults[mySelectedDirection].M3[j] });
                    }

                    var scatterShearSeries = new LiveCharts.Wpf.LineSeries
                    {
                        Title = listResults[mySelectedDirection].LoadDirection, //this will need to be written, map to name of load case selected.
                        Values = momentPoints,
                        Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 140, 105)),
                        Fill = System.Windows.Media.Brushes.Transparent,


                    };
                    momentChart.Series.Add(scatterShearSeries);
                }
            }
        }
    }
}
