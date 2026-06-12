using Animation_Studios.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Animation_Studios.Data
{
    [Serializable]
    public class DataStore
    {
        public ObservableCollection<Studio> Studios { get; set; } = new ObservableCollection<Studio>();
    }

    public static class Repository
    {
        private static string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.xml");
        private static DataStore store = new DataStore();

        public static ObservableCollection<Studio> Studios => store.Studios;

        static Repository()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    store = new DataStore();
                    Save();
                    return;
                }

                var ser = new XmlSerializer(typeof(DataStore));
                using (var stream = File.OpenRead(FilePath))
                {
                    store = (DataStore)ser.Deserialize(stream);
                }
            }
            catch
            {
                store = new DataStore();
            }
        }

        public static void Save()
        {
            var ser = new XmlSerializer(typeof(DataStore));
            using (var stream = File.Create(FilePath))
            {
                ser.Serialize(stream, store);
            }
        }

        public static Studio GetStudio(Guid id) => Studios.FirstOrDefault(s => s.Id == id);
        public static void AddStudio(Studio s)
        {
            Studios.Add(s);
            Save();
        }
        public static void UpdateStudio(Studio s)
        {
            var existing = GetStudio(s.Id);
            if (existing == null) return;
            var idx = Studios.IndexOf(existing);
            Studios[idx] = s;
            Save();
        }
        public static void DeleteStudio(Guid id)
        {
            var existing = GetStudio(id);
            if (existing == null) return;
            Studios.Remove(existing);
            Save();
        }
    }
}
