using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Animation_Studios
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StudioGrid.ItemsSource = Data.Repository.Studios;
            // start loading data from the database in background
            Data.Repository.StartBackgroundLoad();
            StudioGrid.SelectionChanged += StudioGrid_SelectionChanged;

            StudioAdd.Click += StudioAdd_Click;
            StudioInfo.Click += StudioInfo_Click;
            StudioEdit.Click += StudioEdit_Click;
            StudioDelete.Click += StudioDelete_Click;

            ShowAdd.Click += ShowAdd_Click;
            ShowInfo.Click += ShowInfo_Click;
            ShowEdit.Click += ShowEdit_Click;
            ShowDelete.Click += ShowDelete_Click;
        }

        private void StudioGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var s = StudioGrid.SelectedItem as Models.Studio;
            if (s != null)
            {
                ShowGrid.ItemsSource = s.Shows;
            }
            else
            {
                ShowGrid.ItemsSource = null;
            }
        }

        private void StudioAdd_Click(object sender, RoutedEventArgs e)
        {
            var existing = Data.Repository.Studios.Select(x => x.Name);
            var w = new StudioWindow(existing);
            if (w.ShowDialog() == true)
            {
                Data.Repository.AddStudio(w.Studio);
            }
        }

        private void StudioInfo_Click(object sender, RoutedEventArgs e)
        {
            var s = StudioGrid.SelectedItem as Models.Studio;
            if (s != null) new DetailsWindow(s).ShowDialog();
        }

        private void StudioEdit_Click(object sender, RoutedEventArgs e)
        {
            var s = StudioGrid.SelectedItem as Models.Studio;
            if (s == null) return;
            var copy = new Models.Studio
            {
                Id = s.Id,
                Name = s.Name,
                Country = s.Country,
                Headquarters = s.Headquarters,
                Founded = s.Founded,
                NumberOfEmployees = s.NumberOfEmployees,
                Shows = s.Shows
            };
            var existing = Data.Repository.Studios.Where(x => x.Id != s.Id).Select(x => x.Name);
            var w = new StudioWindow(copy, existing);
            if (w.ShowDialog() == true)
            {
                Data.Repository.UpdateStudio(w.Studio);
            }
        }

        private void StudioDelete_Click(object sender, RoutedEventArgs e)
        {
            var s = StudioGrid.SelectedItem as Models.Studio;
            if (s == null) return;
            var res = MessageBox.Show($"Delete studio {s.Name}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                Data.Repository.DeleteStudio(s.Id);
            }
        }

        private void ShowAdd_Click(object sender, RoutedEventArgs e)
        {
            var s = StudioGrid.SelectedItem as Models.Studio;
            if (s == null) { MessageBox.Show("Select a studio first."); return; }
            var existing = s.Shows.Select(x => x.Name);
            var w = new ShowWindow(existing);
            if (w.ShowDialog() == true)
            {
                Data.Repository.AddShow(s.Id, w.Show);
                // refresh: find the studio in repository and rebind its shows
                var updated = Data.Repository.Studios.FirstOrDefault(x => x.Id == s.Id);
                ShowGrid.ItemsSource = null;
                ShowGrid.ItemsSource = updated?.Shows;
            }
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            var sh = ShowGrid.SelectedItem as Models.Show;
            if (sh != null) new DetailsWindow(sh).ShowDialog();
        }

        private void ShowEdit_Click(object sender, RoutedEventArgs e)
        {
            var s = StudioGrid.SelectedItem as Models.Studio;
            var sh = ShowGrid.SelectedItem as Models.Show;
            if (s == null || sh == null) return;
            var existing = s.Shows.Where(x => x.Id != sh.Id).Select(x => x.Name);
            var w = new ShowWindow(sh, existing);
            if (w.ShowDialog() == true)
            {
                Data.Repository.UpdateShow(w.Show);
                var updated = Data.Repository.Studios.FirstOrDefault(x => x.Id == s.Id);
                ShowGrid.ItemsSource = null;
                ShowGrid.ItemsSource = updated?.Shows;
            }
        }

        private void ShowDelete_Click(object sender, RoutedEventArgs e)
        {
            var s = StudioGrid.SelectedItem as Models.Studio;
            var sh = ShowGrid.SelectedItem as Models.Show;
            if (s == null || sh == null) return;
            var res = MessageBox.Show($"Delete show {sh.Name}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (res == MessageBoxResult.Yes)
            {
                Data.Repository.DeleteShow(sh.Id);
                var updated = Data.Repository.Studios.FirstOrDefault(x => x.Id == s.Id);
                ShowGrid.ItemsSource = null;
                ShowGrid.ItemsSource = updated?.Shows;
            }
        }
    }
}
