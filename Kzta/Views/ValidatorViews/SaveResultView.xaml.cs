using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Kzta.Views.ValidatorViews
{
    /// <summary>
    /// Логика взаимодействия для SaveResultView.xaml
    /// </summary>
    public partial class SaveResultView : Window
    {
        public SaveResultView()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
