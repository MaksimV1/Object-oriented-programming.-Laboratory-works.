using System;
using System.Collections.Generic;


namespace Lab14
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

        public override string ToString()
        {
            return $"Коллекция: {CollectionName}, Изменение: {ChangeType}, Данные объекта: {ObjectData}";
        }
    }

    public class Journal
    {
        private List<JournalEntry> entries = new List<JournalEntry>();

        public void HandleCountChanged(object source, CollectionHandlerEventArgs args)
        {
            JournalEntry entry = new JournalEntry(
                args.CollectionName,
                args.ChangeType,
                args.ChangedObject.ToString()
            );
            entries.Add(entry);
        }

        public void HandleReferenceChanged(object source, CollectionHandlerEventArgs args)
        {
            JournalEntry entry = new JournalEntry(
                args.CollectionName,
                args.ChangeType,
                args.ChangedObject.ToString()
            );
            entries.Add(entry);
        }

        public void Print()
        {
            Console.WriteLine("Журнал записей:");
            foreach (var entry in entries)
            {
                Console.WriteLine(entry);
            }
        }
    }
}