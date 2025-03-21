using System.ComponentModel;

namespace test_01
{
    public class Person : INotifyPropertyChanged
    {
        /**
         * プロパティ
         * */
        private string prefectureCode;
        private string prefectureName;
        private string era;
        private string japaneseYear;
        private string gregorianYear;
        private string note;
        private int populationTotal; // グラフにも使用
        private int populationMale;  // グラフにも使用
        private int populationFemale;// グラフにも使用


        /**
         * ゲッター、セッター
         * */
        public string PrefectureCode
        {
            get => prefectureCode;
            set { prefectureCode = value; OnPropertyChanged(nameof(PrefectureCode)); }
        }

        public string PrefectureName
        {
            get => prefectureName;
            set { prefectureName = value; OnPropertyChanged(nameof(PrefectureName)); }
        }

        public string Era
        {
            get => era;
            set { era = value; OnPropertyChanged(nameof(Era)); }
        }

        public string JapaneseYear
        {
            get => japaneseYear;
            set { japaneseYear = value; OnPropertyChanged(nameof(JapaneseYear)); }
        }

        public string GregorianYear
        {
            get => gregorianYear;
            set { gregorianYear = value; OnPropertyChanged(nameof(GregorianYear)); }
        }

        public string Note
        {
            get => note;
            set { note = value; OnPropertyChanged(nameof(Note)); }
        }

        public int PopulationTotal
        {
            get => populationTotal;
            set { populationTotal = value; OnPropertyChanged(nameof(PopulationTotal)); }
        }

        public int PopulationMale
        {
            get => populationMale;
            set { populationMale = value; OnPropertyChanged(nameof(PopulationMale)); }
        }

        public int PopulationFemale
        {
            get => populationFemale;
            set { populationFemale = value; OnPropertyChanged(nameof(PopulationFemale)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}