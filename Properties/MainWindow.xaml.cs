using System;

namespace test_01

public class MainWindow : INotifyPropertyChanged
{
	public ObservableCollection<Person> People { get; set; }
	public ICommand LoadCsvCommand { get; set; }


	public MainViewModel()
	{
		People = new ObservableCollection<Person>();
		LoadCsvCommand = new RelayCommand(LoadCsv);
    }

	public void LoadCsv()
	{
		string FilePath = "./c01.csv";

		People.Clear();

		// CSV 処理
		if(File.Exists(FilePath))
		{
			var lines = File.ReadAllLines(FilePath, Encoding.GetEncoding("Shift_JIS"));

			foreach(var line in lines)
			{
				var values = line.split(',');
				if(values.Length >= 9)
				{
					People.Add(new Person
					{
						PrefectureCode = values[0],
						PrefectureName = values[1],
						Era = values[2],
						JapaneseYear = values[3],
						GregorianYear = values[4],
						Note = values[5],
						PopulationTotal = int.TryParse(values[6], out int total) ? total : 0,     // グラフにも使用
						PopulationMale = int.TryParse(values[7], out int male) ? male : 0,       // グラフにも使用
                        PopulationFemale = int.TryParse(values[8], out int female) ? female : 0 // グラフにも使用
                    });
				}
			}

		} else
		{
            MessageBox.Show("---ERR--- CSVファイルが見つかりません。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


}
