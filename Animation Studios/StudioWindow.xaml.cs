using Animation_Studios.Models;
using System;
using System.Windows;

namespace Animation_Studios
{
    public partial class StudioWindow : Window
    {
        public Studio Studio { get; private set; }
        private System.Collections.Generic.IEnumerable<string> _existingNames;

        public StudioWindow(System.Collections.Generic.IEnumerable<string> existingNames = null)
        {
            InitializeComponent();
            OkButton.Click += OkButton_Click;
            _existingNames = existingNames ?? new string[0];
        }

        public StudioWindow(Studio s, System.Collections.Generic.IEnumerable<string> existingNames = null) : this(existingNames)
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
            var errors = new System.Collections.Generic.List<string>();

            var name = NameBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                errors.Add("Name must not be empty.");
            else if (_existingNames != null && System.Linq.Enumerable.Any(_existingNames, n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase)))
                errors.Add("Studio name already exists.");

            var country = CountryBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(country))
                errors.Add("Country must not be empty.");

            if (!FoundedPicker.SelectedDate.HasValue)
                errors.Add("Founded date must be selected.");

            var empText = EmployeesBox.Text?.Trim();
            int employees = 0;
            if (string.IsNullOrWhiteSpace(empText))
                errors.Add("Number of employees must not be empty.");
            else if (!int.TryParse(empText, out employees))
                errors.Add("Number of employees must be a valid integer.");

            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join("\n", errors), "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Studio == null) Studio = new Studio();

            Studio.Name = name;
            Studio.Country = country;
            Studio.Headquarters = HQBox.Text?.Trim();
            Studio.Founded = FoundedPicker.SelectedDate ?? DateTime.MinValue;
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
