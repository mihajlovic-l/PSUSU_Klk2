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
            if (type.Namespace == "System.Data.Entity.DynamicProxies")
                type = type.BaseType;

            TitleBlock.Text = type.Name + " details";

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var val = pi.GetValue(obj);
                var tb = new TextBlock();

                if (pi.Name == "Id" || pi.Name == "Shows" || pi.Name == "GenreDisplay" || pi.Name == "RatingDisplay" || pi.Name == "StudioId")
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

                    tb = new TextBlock { Text = pi.Name + ": " + display };
                }
                else if (pi.Name == "NumberOfEmployees")
                {
                    tb = new TextBlock { Text = "Number of employees: " + (val?.ToString() ?? "") };
                }
                else if (pi.Name == "NumberOfEpisodes")
                {
                    tb = new TextBlock { Text = "Number of episodes: " + (val?.ToString() ?? "") };
                }
                else if (pi.Name == "Rating")
                {
                    if (val is int r && r == 0)
                        tb = new TextBlock { Text = "Rating: No rating" };
                    else
                        tb = new TextBlock { Text = "Rating: " + (val?.ToString() ?? "") };
                }
                else if (val?.ToString() == "PlanToWatch")
                {
                    tb = new TextBlock { Text = pi.Name + ": Plan to watch" };
                }
                else if (val?.ToString() == "OnHold")
                {
                    tb = new TextBlock { Text = pi.Name + ": On hold" };
                }
                else if (pi.Name == "Studio")
                {
                    var studio = val as Studio;
                    tb = new TextBlock { Text = "Studio: " + (studio?.Name ?? "") };
                }
                else if (pi.Name == "Headquarters")
                {
                    if (val?.ToString() == "")
                        tb = new TextBlock { Text = "Headquarters: No headquarters" };
                    else
                        tb = new TextBlock { Text = "Headquarters: " + (val?.ToString() ?? "") };
                }
                else
                {
                    tb = new TextBlock { Text = pi.Name + ": " + (val?.ToString() ?? "") };
                }

                PropsList.Items.Add(tb);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
