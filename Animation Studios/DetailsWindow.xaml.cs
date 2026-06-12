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

                if (pi.Name == "Id" || pi.Name == "Shows" || pi.Name == "GenreDisplay")
                {
                    continue;
                }
                else if (pi.Name == "Started" || pi.Name == "Ended" || pi.Name == "Founded")
                {
                    string display = string.Empty;
                    if (val is DateTime dt && dt != DateTime.MinValue)
                    {
                        display = dt.ToShortDateString();
                    }
                    else
                    {
                        if (pi.Name == "Ended")
                        {
                            display = "Currently airing";
                        }
                        else
                        {
                            display = string.Empty;
                        }
                    }

                    var tb = new TextBlock { Text = pi.Name + ": " + display };
                    PropsList.Items.Add(tb);
                }
                else if (pi.Name == "NumberOfEmployees")
                {
                    var tb = new TextBlock { Text = "Number of employees: " + (val?.ToString() ?? "") };
                    PropsList.Items.Add(tb);
                }
                else if (pi.Name == "NumberOfEpisodes")
                {
                    var tb = new TextBlock { Text = "Number of episodes: " + (val?.ToString() ?? "") };
                    PropsList.Items.Add(tb);
                }
                else if (val?.ToString() == "PlanToWatch")
                {
                    var tb = new TextBlock { Text = pi.Name + ": Plan to watch" };
                    PropsList.Items.Add(tb);
                }
                else if (val?.ToString() == "OnHold")
                {
                    var tb = new TextBlock { Text = pi.Name + ": On hold" };
                    PropsList.Items.Add(tb);
                }
                else
                {
                    var tb = new TextBlock { Text = pi.Name + ": " + (val?.ToString() ?? "") };
                    PropsList.Items.Add(tb);
                }


            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
