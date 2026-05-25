using System;
using System.Collections.Generic;

namespace Lab16
{
    public class JournalEntry
    {
        public string CollectionName { get; set; }
        public string ChangeType { get; set; }
        public string ObjectData { get; set; }
        public JournalEntry(string collectionName, string changeType, string objectData)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ObjectData = objectData;
        }
        public override string ToString() => $"Коллекция: {CollectionName}, Изменение: {ChangeType}, Данные: {ObjectData}";
    }

    public class Journal
    {
        private List<JournalEntry> entries = new List<JournalEntry>();

        public void HandleCountChanged(object source, CollectionHandlerEventArgs args)
        {
            entries.Add(new JournalEntry(args.CollectionName, args.ChangeType, args.ChangedObject?.ToString() ?? "null"));
        }
        public void HandleReferenceChanged(object source, CollectionHandlerEventArgs args)
        {
            entries.Add(new JournalEntry(args.CollectionName, args.ChangeType, args.ChangedObject?.ToString() ?? "null"));
        }
        public List<JournalEntry> GetEntries() => entries;
        public void Clear() => entries.Clear();
        public void AddRawEntry(string rawLine)
        {
            var parts = rawLine.Split(',');
            if (parts.Length >= 3)
            {
                string coll = parts[0].Replace("Коллекция:", "").Trim();
                string type = parts[1].Replace("Изменение:", "").Trim();
                string data = parts[2].Replace("Данные:", "").Trim();
                entries.Add(new JournalEntry(coll, type, data));
            }
        }
        public void Print() { foreach (var e in entries) Console.WriteLine(e); }
    }
}