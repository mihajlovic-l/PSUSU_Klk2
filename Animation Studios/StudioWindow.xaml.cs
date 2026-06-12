using Animation_Studios.Models;
using System;
using System.Windows;

namespace Animation_Studios
{
    public partial class StudioWindow : Window
    {
        public Studio Studio { get; private set; }

        public StudioWindow()
        {
            InitializeComponent();
            OkButton.Click += OkButton_Click;
        }

        public StudioWindow(Studio s) : this()
        {
            Studio = s;
            NameBox.Text = s.Name;
            CountryBox.Text = s.Country;
            HQBox.Text = s.Headquarters;
            FoundedPicker.SelectedDate = s.Founded == DateTime.MinValue ? (DateTime?)null : s.Founded;
            EmployeesBox.Text = s.NumberOfEmployees.ToString();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Studio == null) Studio = new Studio();

            Studio.Name = NameBox.Text.Trim();
            Studio.Country = CountryBox.Text.Trim();
            Studio.Headquarters = HQBox.Text.Trim();
            Studio.Founded = FoundedPicker.SelectedDate ?? DateTime.MinValue;
            int.TryParse(EmployeesBox.Text, out int employees);
            Studio.NumberOfEmployees = employees;

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
