using LiveCharts;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace test_01
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            // コンポーネント初期化
            InitializeComponent();

            // データバインド適用
            DataContext = new MainViewModel();
        }

    }

}
