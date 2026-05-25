#pragma warning disable SYSLIB0011
#nullable disable

using LocationLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Lab16
{
    public partial class MainForm : Form
    {
        private MyNewCollection<Place> collection;
        private Journal journal;
        private string lastQueryReport = "";

        public MainForm()
        {
            InitializeComponent();
            collection = new MyNewCollection<Place>("Основная коллекция");
            journal = new Journal();
            collection.CollectionCountChanged += journal.HandleCountChanged;
            collection.CollectionReferenceChanged += journal.HandleReferenceChanged;
            CreateMenu();
            RefreshDisplay();
        }

        private void CreateMenu()
        {
            var menu = new MenuStrip();

            var fileMenu = new ToolStripMenuItem("Файл");
            fileMenu.DropDownItems.Add("Загрузить коллекцию", null, LoadCollection_Click);
            fileMenu.DropDownItems.Add("Сохранить коллекцию", null, SaveCollection_Click);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add("Выход", null, (s, e) => Application.Exit());
            menu.Items.Add(fileMenu);

            var collMenu = new ToolStripMenuItem("Коллекция");
            collMenu.DropDownItems.Add("Создать случайную коллекцию", null, GenerateRandom_Click);
            collMenu.DropDownItems.Add("Добавить элемент", null, AddItem_Click);
            collMenu.DropDownItems.Add("Удалить по ключу", null, DeleteByKey_Click);
            collMenu.DropDownItems.Add("Редактировать по ключу", null, EditByKey_Click);
            collMenu.DropDownItems.Add("Найти по ключу", null, FindByKey_Click);
            menu.Items.Add(collMenu);

            var viewMenu = new ToolStripMenuItem("Просмотр");
            viewMenu.DropDownItems.Add("Визуализировать дерево", null, VisualizeTree_Click);
            menu.Items.Add(viewMenu);

            var sortMenu = new ToolStripMenuItem("Сортировка/Фильтр");
            sortMenu.DropDownItems.Add("Сортировать по населению", null, SortByPopulation_Click);
            sortMenu.DropDownItems.Add("Фильтр по типу", null, Filter_Click);
            menu.Items.Add(sortMenu);

            var queryMenu = new ToolStripMenuItem("Запросы");
            queryMenu.DropDownItems.Add("Выполнить запросы", null, ExecuteQueries_Click);
            menu.Items.Add(queryMenu);

            var journalMenu = new ToolStripMenuItem("Журнал");
            journalMenu.DropDownItems.Add("Показать журнал", null, ShowJournal_Click);
            journalMenu.DropDownItems.Add("Сохранить журнал", null, SaveJournalToFile_Click);
            journalMenu.DropDownItems.Add("Загрузить журнал", null, LoadJournalFromFile_Click);
            menu.Items.Add(journalMenu);

            var reportMenu = new ToolStripMenuItem("Отчёты");
            reportMenu.DropDownItems.Add("Сохранить результат запроса", null, SaveQueryResult_Click);
            reportMenu.DropDownItems.Add("Просмотреть отчёт", null, ViewReport_Click);
            menu.Items.Add(reportMenu);

            this.MainMenuStrip = menu;
            this.Controls.Add(menu);
        }

        private void RefreshDisplay()
        {
            if (collection.Count == 0)
            {
                dgv.DataSource = null;
                statusLabel.Text = "Коллекция пуста.";
                return;
            }
            var list = collection.ToList();
            var displayList = list.Select(p => new
            {
                Тип = p.GetType().Name,
                Название = p.Name,
                Население = p.Population,
                Дополнительно = GetAdditionalInfo(p)
            }).ToList();
            dgv.DataSource = displayList;
            statusLabel.Text = $"Всего элементов: {collection.Count}";
        }

        private string GetAdditionalInfo(Place p)
        {
            switch (p)
            {
                case Metropolis m:
                    return $"Округ: {m.FederalDistrict}, Районов: {m.NumberOfDistricts}, Площадь: {m.AreaKm2:F1} км²";
                case City c:
                    return $"Округ: {c.FederalDistrict}, Основатель: {c.FounderName}, Год: {c.FoundedYear}";
                case LocationLibrary.Region r:
                    return $"Округ: {r.FederalDistrict}";
                case Address a:
                    return $"Улица: {a.Street}, {a.BuildingNumber}, {a.City}";
                default:
                    return "";
            }
        }

        private void GenerateRandom_Click(object sender, EventArgs e)
        {
            var dialog = new Form { Text = "Количество элементов", Size = new Size(200, 100), StartPosition = FormStartPosition.CenterParent };
            var numUpDown = new NumericUpDown { Minimum = 1, Maximum = 1000, Value = 10, Location = new Point(10, 10) };
            var btnOk = new Button { Text = "OK", Location = new Point(10, 40), DialogResult = DialogResult.OK };
            dialog.Controls.Add(numUpDown);
            dialog.Controls.Add(btnOk);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int count = (int)numUpDown.Value;
                collection.Clear();
                Random rnd = new Random();
                Type[] types = { typeof(Place), typeof(LocationLibrary.Region), typeof(City), typeof(Metropolis), typeof(Address) };
                for (int i = 0; i < count; i++)
                {
                    Type t = types[rnd.Next(types.Length)];
                    Place obj = (Place)Activator.CreateInstance(t);
                    obj.RandomInit();
                    collection.Add(obj);
                    journal.HandleCountChanged(collection, new CollectionHandlerEventArgs(collection.Name, "AddRandom", obj));
                }
                RefreshDisplay();
                MessageBox.Show($"Создано {count} случайных объектов.");
            }
        }

        private void DeleteByKey_Click(object sender, EventArgs e)
        {
            if (collection.Count == 0) { MessageBox.Show("Коллекция пуста."); return; }
            string key = Microsoft.VisualBasic.Interaction.InputBox("Введите название для удаления:", "Удаление", "");
            if (string.IsNullOrEmpty(key)) return;
            var list = collection.ToList();
            var found = list.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (found == null) { MessageBox.Show("Не найдено."); return; }
            int index = list.IndexOf(found);
            if (collection.Remove(index)) RefreshDisplay();
            else MessageBox.Show("Ошибка удаления.");
        }

        private void EditByKey_Click(object sender, EventArgs e)
        {
            if (collection.Count == 0) { MessageBox.Show("Коллекция пуста."); return; }
            string key = Microsoft.VisualBasic.Interaction.InputBox("Введите название для редактирования:", "Редактирование", "");
            if (string.IsNullOrEmpty(key)) return;
            var list = collection.ToList();
            var found = list.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (found == null) { MessageBox.Show("Не найдено."); return; }
            int index = list.IndexOf(found);
            Place edited = EditObjectForm.EditObject(found);
            if (edited != null) { collection[index] = edited; RefreshDisplay(); MessageBox.Show("Обновлено."); }
        }

        private void AddItem_Click(object sender, EventArgs e)
        {
            Place newObj = CreateObjectForm.CreateObject();
            if (newObj != null)
            {
                collection.Add(newObj);
                journal.HandleCountChanged(collection, new CollectionHandlerEventArgs(collection.Name, "AddManual", newObj));
                RefreshDisplay();
                MessageBox.Show("Добавлено.");
            }
        }

        private void FindByKey_Click(object sender, EventArgs e)
        {
            if (collection.Count == 0) { MessageBox.Show("Коллекция пуста."); return; }
            string key = Microsoft.VisualBasic.Interaction.InputBox("Введите название для поиска:", "Поиск", "");
            if (string.IsNullOrEmpty(key)) return;
            var found = collection.ToList().FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (found == null) MessageBox.Show("Не найдено.");
            else MessageBox.Show(found.ToString(), "Найден объект");
        }

        private void SortByPopulation_Click(object sender, EventArgs e)
        {
            if (collection.Count == 0) return;
            collection.TransformToSearchTree();
            RefreshDisplay();
            MessageBox.Show("Коллекция отсортирована по населению.");
        }

        private void Filter_Click(object sender, EventArgs e)
        {
            if (collection.Count == 0) return;
            var dlg = new Form { Text = "Фильтр по типу", Size = new Size(300, 150) };
            var cmb = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(10, 10), Width = 200 };
            cmb.Items.AddRange(new[] { "Место", "Область", "Город", "Мегаполис", "Адрес" });
            cmb.SelectedIndex = 0;
            var btn = new Button { Text = "OK", Location = new Point(10, 50), DialogResult = DialogResult.OK };
            dlg.Controls.Add(cmb);
            dlg.Controls.Add(btn);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string typeName = cmb.SelectedItem.ToString();
                Type filterType = typeName switch
                {
                    "Область" => typeof(LocationLibrary.Region),
                    "Город" => typeof(City),
                    "Мегаполис" => typeof(Metropolis),
                    "Адрес" => typeof(Address),
                    _ => typeof(Place)
                };
                var filtered = collection.Where(p => p.GetType() == filterType || (filterType == typeof(Place) && p.GetType().BaseType == typeof(Place))).ToList();
                dgv.DataSource = filtered.Select(p => new { Тип = p.GetType().Name, Название = p.Name, Население = p.Population }).ToList();
                statusLabel.Text = $"Отфильтровано: {filtered.Count}";
            }
        }

        private void ExecuteQueries_Click(object sender, EventArgs e)
        {
            if (collection.Count == 0) { MessageBox.Show("Коллекция пуста."); return; }
            var results = new List<string>();
            double avgPopulation = collection.AveragePopulation(p => p.Population);
            results.Add($"Среднее население: {avgPopulation:F2}");
            var highPopCities = collection.Filter(p => p is City && p.Population > 500000).ToList();
            results.Add($"Городов с населением > 500000: {highPopCities.Count}");
            var sortedByName = collection.OrderByAlphabet(p => p.Name);
            results.Add("Первые 5 по алфавиту: " + string.Join(", ", sortedByName.Take(5).Select(p => p.Name)));
            var minPop = collection.Min(p => p.Population);
            var maxPop = collection.Max(p => p.Population);
            results.Add($"Население: от {minPop} до {maxPop}");
            var groupByType = collection.GroupBy(p => p.GetType().Name).Select(g => $"{g.Key}: {g.Count()} шт.");
            results.AddRange(groupByType);
            lastQueryReport = string.Join(Environment.NewLine, results);
            MessageBox.Show(lastQueryReport, "Результаты запросов");
        }

        private void SaveCollection_Click(object sender, EventArgs e)
        {
            if (collection.Count == 0) { MessageBox.Show("Нечего сохранять."); return; }
            var dialog = new SaveFileDialog { Filter = "Бинарный файл (*.bin)|*.bin|JSON файл (*.json)|*.json|XML файл (*.xml)|*.xml", Title = "Сохранить коллекцию" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var list = collection.ToList();
                    string ext = System.IO.Path.GetExtension(dialog.FileName).ToLower();
                    if (ext == ".bin")
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        using (FileStream fs = new FileStream(dialog.FileName, FileMode.Create))
                            formatter.Serialize(fs, list);
                    }
                    else if (ext == ".json")
                    {
                        var options = new JsonSerializerOptions { WriteIndented = true };
                        string json = JsonSerializer.Serialize(list, options);
                        File.WriteAllText(dialog.FileName, json);
                    }
                    else if (ext == ".xml")
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Place>));
                        using (FileStream fs = new FileStream(dialog.FileName, FileMode.Create))
                            serializer.Serialize(fs, list);
                    }
                    else
                    {
                        MessageBox.Show("Неподдерживаемый формат.");
                        return;
                    }
                    MessageBox.Show("Коллекция сохранена.");
                }
                catch (Exception ex) { MessageBox.Show("Ошибка сохранения: " + ex.Message); }
            }
        }

        private void LoadCollection_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Бинарный файл (*.bin)|*.bin|JSON файл (*.json)|*.json|XML файл (*.xml)|*.xml", Title = "Загрузить коллекцию" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<Place> loaded = null;
                    string ext = System.IO.Path.GetExtension(dialog.FileName).ToLower();

                    if (ext == ".bin")
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open))
                            loaded = (List<Place>)formatter.Deserialize(fs);
                    }
                    else if (ext == ".json")
                    {
                        string json = File.ReadAllText(dialog.FileName);
                        loaded = JsonSerializer.Deserialize<List<Place>>(json);
                    }
                    else if (ext == ".xml")
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Place>));
                        using (FileStream fs = new FileStream(dialog.FileName, FileMode.Open))
                            loaded = (List<Place>)serializer.Deserialize(fs);
                    }
                    else
                    {
                        MessageBox.Show("Неподдерживаемый формат.");
                        return;
                    }
                    if (loaded != null)
                    {
                        collection.Clear();
                        foreach (var item in loaded) collection.Add(item);
                        RefreshDisplay();
                        MessageBox.Show($"Загружено {loaded.Count} объектов.");
                    }
                }
                catch (Exception ex) { MessageBox.Show("Ошибка загрузки: " + ex.Message); }
            }
        }

        private void ShowJournal_Click(object sender, EventArgs e)
        {
            var entries = journal.GetEntries();
            if (entries.Count == 0) { MessageBox.Show("Журнал пуст."); return; }
            MessageBox.Show(string.Join(Environment.NewLine, entries), "Журнал операций");
        }

        private void SaveJournalToFile_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "Текстовый файл (*.txt)|*.txt" };
            if (dlg.ShowDialog() == DialogResult.OK)
                File.WriteAllLines(dlg.FileName, journal.GetEntries().Select(entry => entry.ToString()));
        }

        private void LoadJournalFromFile_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Текстовый файл (*.txt)|*.txt" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                journal.Clear();
                foreach (var line in File.ReadAllLines(dlg.FileName))
                    journal.AddRawEntry(line);
                MessageBox.Show("Журнал загружен.");
            }
        }

        private void SaveQueryResult_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lastQueryReport)) { MessageBox.Show("Сначала выполните запросы."); return; }
            var dlg = new SaveFileDialog { Filter = "Текстовый файл (*.txt)|*.txt" };
            if (dlg.ShowDialog() == DialogResult.OK)
                File.WriteAllText(dlg.FileName, lastQueryReport);
        }

        private void ViewReport_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Текстовый файл (*.txt)|*.txt" };
            if (dlg.ShowDialog() == DialogResult.OK)
                MessageBox.Show(File.ReadAllText(dlg.FileName), "Отчёт");
        }

        private void VisualizeTree_Click(object sender, EventArgs e)
        {
            if (collection.Root == null) { MessageBox.Show("Дерево пустое."); return; }
            var vizForm = new TreeVisualizerForm<Place>(collection.Root);
            vizForm.ShowDialog();
        }
    }
}