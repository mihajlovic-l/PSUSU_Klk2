using Animation_Studios.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Animation_Studios
{
    public partial class ShowWindow : Window
    {
        public Show Show { get; private set; }
        private System.Collections.Generic.IEnumerable<string> _existingNames;

        public ShowWindow(System.Collections.Generic.IEnumerable<string> existingNames = null)
        {
            InitializeComponent();
            OkButton.Click += OkButton_Click;
            Loaded += ShowWindow_Loaded;
            _existingNames = existingNames ?? new string[0];
        }

        public ShowWindow(Show s, System.Collections.Generic.IEnumerable<string> existingNames = null) : this(existingNames)
        {
            Show = s;
        }

        private void ShowWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate genres
            GenreList.Items.Clear();
            foreach (Genre g in Enum.GetValues(typeof(Genre)))
            {
                if (g == Genre.None) continue;
                var cb = new CheckBox { Content = g.ToString(), Tag = g };
                GenreList.Items.Add(cb);
            }

            // Populate rating radio buttons
            RatingPanel.Children.Clear();
            for (int i = 1; i <= 10; i++)
            {
                var rb = new RadioButton { Content = i.ToString() + " ", Tag = i, GroupName = "Rating" };
                RatingPanel.Children.Add(rb);
            }

            StatusCombo.ItemsSource = Enum.GetValues(typeof(ShowStatus)).Cast<ShowStatus>().Select(s => s.ToString());

            if (Show != null)
            {
                NameBox.Text = Show.Name;
                StartedPicker.SelectedDate = Show.Started == DateTime.MinValue ? (DateTime?)null : Show.Started;
                EndedPicker.SelectedDate = Show.Ended == DateTime.MinValue ? (DateTime?)null : Show.Ended;
                DirectorBox.Text = Show.Director;
                EpisodesBox.Text = Show.NumberOfEpisodes.ToString();
                MovieCheck.IsChecked = Show.Movie;
                StatusCombo.SelectedItem = Show.Status.ToString();

                foreach (CheckBox cb in GenreList.Items)
                {
                    var g = (Genre)cb.Tag;
                    cb.IsChecked = Show.Genre.HasFlag(g);
                }

                foreach (RadioButton rb in RatingPanel.Children)
                {
                    if ((int)rb.Tag == Show.Rating) rb.IsChecked = true;
                }
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            var errors = new System.Collections.Generic.List<string>();

            var name = NameBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                errors.Add("Name must not be empty.");
            else if (_existingNames != null && System.Linq.Enumerable.Any(_existingNames, n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase)))
                errors.Add("Show name already exists.");

            if (!StartedPicker.SelectedDate.HasValue)
                errors.Add("Started date must be selected.");

            var director = DirectorBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(director))
                errors.Add("Director must not be empty.");

            var epsText = EpisodesBox.Text?.Trim();
            int eps = 0;
            if (string.IsNullOrWhiteSpace(epsText))
                errors.Add("Number of episodes must not be empty.");
            else if (!int.TryParse(epsText, out eps))
                errors.Add("Number of episodes must be a valid integer.");

            if (StatusCombo.SelectedItem == null)
                errors.Add("Status must be selected.");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Show == null) Show = new Show();

            Show.Name = name;
            Show.Started = StartedPicker.SelectedDate ?? DateTime.MinValue;
            Show.Ended = EndedPicker.SelectedDate;
            Show.Director = director;
            Show.NumberOfEpisodes = eps;
            Show.Movie = MovieCheck.IsChecked == true;
            if (StatusCombo.SelectedItem != null)
                Show.Status = (ShowStatus)Enum.Parse(typeof(ShowStatus), StatusCombo.SelectedItem.ToString());

            // genres
            Genre g = Genre.None;
            foreach (CheckBox cb in GenreList.Items)
            {
                if (cb.IsChecked == true)
                {
                    g |= (Genre)cb.Tag;
                }
            }
            Show.Genre = g;

            // rating
            int rating = 0;
            foreach (RadioButton rb in RatingPanel.Children)
            {
                if (rb.IsChecked == true)
                {
                    rating = (int)rb.Tag;
                    break;
                }
            }
            Show.Rating = rating;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
