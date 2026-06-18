using Animation_Studios.Models;
using Animation_Studios.Data.EF;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Animation_Studios.Data
{
    public static class Repository
    {
        private static ObservableCollection<Studio> _studios = new ObservableCollection<Studio>();

        public static ObservableCollection<Studio> Studios => _studios;

        static Repository()
        {
            // do not block UI on startup; call StartBackgroundLoad from UI on load
        }

        private static System.Collections.Generic.List<Studio> LoadFromDb()
        {
            using (var ctx = new AnimationContext())
            {
                return ctx.Studios.Include(s => s.Shows).ToList();
            }
        }

        public static void StartBackgroundLoad()
        {
            Task.Run(() =>
            {
                try
                {
                    var list = LoadFromDb();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _studios.Clear();
                        foreach (var s in list)
                            _studios.Add(s);
                    });
                }
                catch
                {
                    // ignore load errors on background thread
                }
            });
        }

        public static void Refresh()
        {
            StartBackgroundLoad();
        }

        public static void AddStudio(Studio s)
        {
            Task.Run(() =>
            {
                try
                {
                    using (var ctx = new AnimationContext())
                    {
                        ctx.Studios.Add(s);
                        ctx.SaveChanges();
                    }
                    Application.Current.Dispatcher.Invoke(() => StartBackgroundLoad());
                }
                catch (Exception ex)
                {
                    var msg = "Database error:\n" + ex.ToString();
                    try { Application.Current.Dispatcher.Invoke(() => MessageBox.Show(msg, "Database error", MessageBoxButton.OK, MessageBoxImage.Error)); } catch { }
                }
            });
        }

        public static void UpdateStudio(Studio s)
        {
            Task.Run(() =>
            {
                try
                {
                    using (var ctx = new AnimationContext())
                    {
                        var existing = ctx.Studios.Include(x => x.Shows).FirstOrDefault(x => x.Id == s.Id);
                        if (existing == null) return;
                        existing.Name = s.Name;
                        existing.Country = s.Country;
                        existing.Headquarters = s.Headquarters;
                        existing.Founded = s.Founded;
                        existing.NumberOfEmployees = s.NumberOfEmployees;
                        ctx.SaveChanges();
                    }
                    Application.Current.Dispatcher.Invoke(() => StartBackgroundLoad());
                }
                catch (Exception ex)
                {
                    var msg = "Database error:\n" + ex.ToString();
                    try { Application.Current.Dispatcher.Invoke(() => MessageBox.Show(msg, "Database error", MessageBoxButton.OK, MessageBoxImage.Error)); } catch { }
                }
            });
        }

        public static void DeleteStudio(Guid id)
        {
            Task.Run(() =>
            {
                try
                {
                    using (var ctx = new AnimationContext())
                    {
                        var existing = ctx.Studios.Include(x => x.Shows).FirstOrDefault(x => x.Id == id);
                        if (existing == null) return;
                        // remove related shows
                        ctx.Shows.RemoveRange(existing.Shows);
                        ctx.Studios.Remove(existing);
                        ctx.SaveChanges();
                    }
                    Application.Current.Dispatcher.Invoke(() => StartBackgroundLoad());
                }
                catch (Exception ex)
                {
                    var msg = "Database error:\n" + ex.ToString();
                    try { Application.Current.Dispatcher.Invoke(() => MessageBox.Show(msg, "Database error", MessageBoxButton.OK, MessageBoxImage.Error)); } catch { }
                }
            });
        }

        // Show operations
        public static void AddShow(Guid studioId, Show show)
        {
            Task.Run(() =>
            {
                try
                {
                    using (var ctx = new AnimationContext())
                    {
                        show.Id = Guid.NewGuid();
                        show.StudioId = studioId;
                        ctx.Shows.Add(show);
                        ctx.SaveChanges();
                    }
                    Application.Current.Dispatcher.Invoke(() => StartBackgroundLoad());
                }
                catch (Exception ex)
                {
                    var msg = "Database error:\n" + ex.ToString();
                    try { Application.Current.Dispatcher.Invoke(() => MessageBox.Show(msg, "Database error", MessageBoxButton.OK, MessageBoxImage.Error)); } catch { }
                }
            });
        }

        public static void UpdateShow(Show show)
        {
            Task.Run(() =>
            {
                try
                {
                    using (var ctx = new AnimationContext())
                    {
                        var existing = ctx.Shows.FirstOrDefault(x => x.Id == show.Id);
                        if (existing == null) return;
                        existing.Name = show.Name;
                        existing.Started = show.Started;
                        existing.Ended = show.Ended;
                        existing.Director = show.Director;
                        existing.NumberOfEpisodes = show.NumberOfEpisodes;
                        existing.Genre = show.Genre;
                        existing.Rating = show.Rating;
                        existing.Status = show.Status;
                        existing.Movie = show.Movie;
                        ctx.SaveChanges();
                    }
                    Application.Current.Dispatcher.Invoke(() => StartBackgroundLoad());
                }
                catch (Exception ex)
                {
                    var msg = "Database error:\n" + ex.ToString();
                    try { Application.Current.Dispatcher.Invoke(() => MessageBox.Show(msg, "Database error", MessageBoxButton.OK, MessageBoxImage.Error)); } catch { }
                }
            });
        }

        public static void DeleteShow(Guid showId)
        {
            Task.Run(() =>
            {
                try
                {
                    using (var ctx = new AnimationContext())
                    {
                        var existing = ctx.Shows.FirstOrDefault(x => x.Id == showId);
                        if (existing == null) return;
                        ctx.Shows.Remove(existing);
                        ctx.SaveChanges();
                    }
                    Application.Current.Dispatcher.Invoke(() => StartBackgroundLoad());
                }
                catch (Exception ex)
                {
                    var msg = "Database error:\n" + ex.ToString();
                    try { Application.Current.Dispatcher.Invoke(() => MessageBox.Show(msg, "Database error", MessageBoxButton.OK, MessageBoxImage.Error)); } catch { }
                }
            });
        }
    }
}
