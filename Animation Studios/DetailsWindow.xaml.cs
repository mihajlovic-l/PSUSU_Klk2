using Animation_Studios.Models;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Animation_Studios
{
    public partial class DetailsWindow : Window
    {
        public DetailsWindow(object obj)
        {
            InitializeComponent();

            if (obj == null) return;

            var type = obj.GetType();
            TitleBlock.Text = type.Name + " details";

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var val = pi.GetValue(obj);
                var tb = new TextBlock { Text = pi.Name + ": " + (val?.ToString() ?? "") };
                PropsList.Items.Add(tb);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
