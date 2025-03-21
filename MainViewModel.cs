using LiveCharts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using LiveCharts.Wpf;

namespace test_01
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> People { get; set; }
        public ICommand LoadCsvCommand { get; set; }


        // グラフ用のプロパティ
        
        public ObservableCollection<int> PopulationTotalValues { get; set; }
        public ObservableCollection<int> PopulationMaleValues { get; set; }
        public ObservableCollection<int> PopulationFemaleValues { get; set; }
        public ObservableCollection<string> YearLabels { get; set; }

        // LiveCharts用のプロパティ
        public SeriesCollection SeriesCollection { get; set; }
        
        // 1920年 - 2015年のデータ作成
        private static readonly int[] Years = Enumerable.Range(1920, (2015 - 1920) / 5 + 1).Select(x => x * 5).ToArray();


        public MainViewModel()
        {
            try
            {
                // 初期化
                People = new ObservableCollection<Person>();

                PopulationTotalValues = new ObservableCollection<int>();
                PopulationMaleValues = new ObservableCollection<int>();
                PopulationFemaleValues = new ObservableCollection<int>();

                YearLabels = new ObservableCollection<string>();
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "総人口",
                        Values = new ChartValues<int>()  // 初期は空の値
                    },
                    new ColumnSeries
                    {
                        Title = "男性人口",
                        Values = new ChartValues<int>()
                    },
                    new ColumnSeries
                    {
                        Title = "女性人口",
                        Values = new ChartValues<int>()
                    }
                };

                // UI スレッドで初期データを追加
                /*
                Application.Current.Dispatcher.Invoke(() =>
                {
                    YearLabels.Add("2020");
                    YearLabels.Add("2021");
                    YearLabels.Add("2022");
                    YearLabels.Add("2023");
                });
                */

                LoadCsvCommand = new RelayCommand(LoadCsv);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"ViewModel 初期化エラー: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /**
         *  CSV読み込みボタンを押した、処理
         **/
        private void LoadCsv()
        {
            string FilePath = "./c01.csv";

            People.Clear();
            PopulationTotalValues.Clear();
            PopulationMaleValues.Clear();
            PopulationFemaleValues.Clear();
            YearLabels.Clear();

            try
            {
                if (File.Exists(FilePath))
                {
                    var lines = File.ReadAllLines(FilePath, Encoding.GetEncoding("Shift_JIS"));

                    // 最初の行（ヘッダー行）をスキップ
                    var dataLines = lines.Skip(1).ToList();

                    // 前の年を保持する変数
                    string previousYear = string.Empty; 

                    for (int i = 0; i < dataLines.Count; i++)
                    {
                        var values = dataLines[i].Split(',');
                        if (values.Length >= 9)
                        {
                            var totalPopulation = int.TryParse(values[6], out int total) ? total : 0;
                            var malePopulation = int.TryParse(values[7], out int male) ? male : 0;
                            var femalePopulation = int.TryParse(values[8], out int female) ? female : 0;

                            People.Add(new Person
                            {
                                PrefectureCode = values[0],
                                PrefectureName = values[1],
                                Era = values[2],
                                JapaneseYear = values[3],
                                GregorianYear = values[4],
                                Note = values[5],
                                PopulationTotal = totalPopulation,
                                PopulationMale = malePopulation,
                                PopulationFemale = femalePopulation
                            });

                  
                            // **2 行目（i == 0）のデータだけをグラフに反映**
                            if (previousYear != values[4])  // 年が変更された場合
                            {
                                previousYear = values[4];

                                // 年が変更された場合のみ、グラフにデータを追加
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    YearLabels.Add(values[4]);  // 西暦（年）をラベルに追加

                                    PopulationTotalValues.Add(totalPopulation / 10000);
                                    PopulationMaleValues.Add(malePopulation / 10000);
                                    PopulationFemaleValues.Add(femalePopulation / 10000);

                                    SeriesCollection[0].Values.Add(totalPopulation); // 総人口
                                    SeriesCollection[1].Values.Add(malePopulation);  // 男性人口
                                    SeriesCollection[2].Values.Add(femalePopulation); // 女性人口
                                });
                            }
                        }
                    }

                    // 変更をビューに通知
                    OnPropertyChanged(nameof(PopulationTotalValues));
                    OnPropertyChanged(nameof(PopulationMaleValues));
                    OnPropertyChanged(nameof(PopulationFemaleValues));
                    OnPropertyChanged(nameof(YearLabels));

                    OnPropertyChanged(nameof(SeriesCollection)); // グラフの更新を通知
                }
                else
                {
                    MessageBox.Show("---ERR--- CSVファイルが見つかりません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"LoadCsv エラー: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
