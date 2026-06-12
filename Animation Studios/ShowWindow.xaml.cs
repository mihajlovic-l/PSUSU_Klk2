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

        public ShowWindow()
        {
            InitializeComponent();
            OkButton.Click += OkButton_Click;
            Loaded += ShowWindow_Loaded;
        }

        public ShowWindow(Show s) : this()
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
                var rb = new RadioButton { Content = i.ToString(), Tag = i, GroupName = "Rating" };
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
            if (Show == null) Show = new Show();

            Show.Name = NameBox.Text.Trim();
            Show.Started = StartedPicker.SelectedDate ?? DateTime.MinValue;
            Show.Ended = EndedPicker.SelectedDate ?? DateTime.MinValue;
            Show.Director = DirectorBox.Text.Trim();
            int.TryParse(EpisodesBox.Text, out int ep);
            Show.NumberOfEpisodes = ep;
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
            foreach (RadioButton rb in RatingPanel.Children)
            {
                if (rb.IsChecked == true)
                {
                    Show.Rating = (int)rb.Tag;
                    break;
                }
            }

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
