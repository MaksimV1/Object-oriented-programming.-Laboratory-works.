using System;
using System.Windows.Forms;
using LocationLibrary;

namespace Lab16
{
    public partial class ObjectDialog : Form
    {
        public Place ResultObject { get; private set; }

        private ComboBox cmbType;
        private TextBox txtName, txtPopulation, txtDistrict, txtFounder, txtYear, txtDistricts, txtArea, txtStreet, txtBuilding, txtCity;
        private Label lblName, lblPopulation, lblDistrict, lblFounder, lblYear, lblDistricts, lblArea, lblStreet, lblBuilding, lblCity;
        private Button btnOk, btnCancel;

        private Type selectedType;

        public ObjectDialog(Place existingObject = null)
        {
            InitializeComponent();
            if (existingObject != null)
            {
                txtName.Text = existingObject.Name;
                txtPopulation.Text = existingObject.Population.ToString();
                if (existingObject is LocationLibrary.Region r) txtDistrict.Text = r.FederalDistrict;
                if (existingObject is City c)
                {
                    txtFounder.Text = c.FounderName;
                    txtYear.Text = c.FoundedYear.ToString();
                }
                if (existingObject is Metropolis m)
                {
                    txtDistricts.Text = m.NumberOfDistricts.ToString();
                    txtArea.Text = m.AreaKm2.ToString();
                }
                if (existingObject is Address a)
                {
                    txtStreet.Text = a.Street;
                    txtBuilding.Text = a.BuildingNumber.ToString();
                    txtCity.Text = a.City;
                }
                if (existingObject is Metropolis) cmbType.SelectedItem = "Мегаполис";
                else if (existingObject is City) cmbType.SelectedItem = "Город";
                else if (existingObject is LocationLibrary.Region) cmbType.SelectedItem = "Область";
                else if (existingObject is Address) cmbType.SelectedItem = "Адрес";
                else cmbType.SelectedItem = "Место";
            }
            else
            {
                cmbType.SelectedIndex = 0;
            }
            UpdateFieldsVisibility();
        }

        private void InitializeComponent()
        {
            this.cmbType = new ComboBox();
            this.txtName = new TextBox();
            this.txtPopulation = new TextBox();
            this.txtDistrict = new TextBox();
            this.txtFounder = new TextBox();
            this.txtYear = new TextBox();
            this.txtDistricts = new TextBox();
            this.txtArea = new TextBox();
            this.txtStreet = new TextBox();
            this.txtBuilding = new TextBox();
            this.txtCity = new TextBox();
            this.lblName = new Label();
            this.lblPopulation = new Label();
            this.lblDistrict = new Label();
            this.lblFounder = new Label();
            this.lblYear = new Label();
            this.lblDistricts = new Label();
            this.lblArea = new Label();
            this.lblStreet = new Label();
            this.lblBuilding = new Label();
            this.lblCity = new Label();
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

            this.cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbType.Items.AddRange(new object[] { "Место", "Область", "Город", "Мегаполис", "Адрес" });
            this.cmbType.Location = new System.Drawing.Point(120, 12);
            this.cmbType.Size = new System.Drawing.Size(200, 21);
            this.cmbType.SelectedIndexChanged += (s, e) => UpdateFieldsVisibility();

            this.lblName.Text = "Название:"; this.lblName.Location = new System.Drawing.Point(12, 50);
            this.txtName.Location = new System.Drawing.Point(120, 47); this.txtName.Size = new System.Drawing.Size(200, 20);

            this.lblPopulation.Text = "Население:"; this.lblPopulation.Location = new System.Drawing.Point(12, 80);
            this.txtPopulation.Location = new System.Drawing.Point(120, 77); this.txtPopulation.Size = new System.Drawing.Size(200, 20);

            this.lblDistrict.Text = "Округ:"; this.lblDistrict.Location = new System.Drawing.Point(12, 110);
            this.txtDistrict.Location = new System.Drawing.Point(120, 107); this.txtDistrict.Size = new System.Drawing.Size(200, 20);

            this.lblFounder.Text = "Основатель:"; this.lblFounder.Location = new System.Drawing.Point(12, 140);
            this.txtFounder.Location = new System.Drawing.Point(120, 137); this.txtFounder.Size = new System.Drawing.Size(200, 20);

            this.lblYear.Text = "Год основания:"; this.lblYear.Location = new System.Drawing.Point(12, 170);
            this.txtYear.Location = new System.Drawing.Point(120, 167); this.txtYear.Size = new System.Drawing.Size(200, 20);

            this.lblDistricts.Text = "Районов:"; this.lblDistricts.Location = new System.Drawing.Point(12, 200);
            this.txtDistricts.Location = new System.Drawing.Point(120, 197); this.txtDistricts.Size = new System.Drawing.Size(200, 20);

            this.lblArea.Text = "Площадь (км²):"; this.lblArea.Location = new System.Drawing.Point(12, 230);
            this.txtArea.Location = new System.Drawing.Point(120, 227); this.txtArea.Size = new System.Drawing.Size(200, 20);

            this.lblStreet.Text = "Улица:"; this.lblStreet.Location = new System.Drawing.Point(12, 260);
            this.txtStreet.Location = new System.Drawing.Point(120, 257); this.txtStreet.Size = new System.Drawing.Size(200, 20);

            this.lblBuilding.Text = "Номер дома:"; this.lblBuilding.Location = new System.Drawing.Point(12, 290);
            this.txtBuilding.Location = new System.Drawing.Point(120, 287); this.txtBuilding.Size = new System.Drawing.Size(200, 20);

            this.lblCity.Text = "Город:"; this.lblCity.Location = new System.Drawing.Point(12, 320);
            this.txtCity.Location = new System.Drawing.Point(120, 317); this.txtCity.Size = new System.Drawing.Size(200, 20);

            this.btnOk.Text = "OK"; this.btnOk.Location = new System.Drawing.Point(100, 370); this.btnOk.DialogResult = DialogResult.OK; this.btnOk.Click += BtnOk_Click;
            this.btnCancel.Text = "Отмена"; this.btnCancel.Location = new System.Drawing.Point(200, 370); this.btnCancel.DialogResult = DialogResult.Cancel;

            this.ClientSize = new System.Drawing.Size(350, 420);
            this.Controls.AddRange(new Control[] { cmbType, lblName, txtName, lblPopulation, txtPopulation, lblDistrict, txtDistrict,
                lblFounder, txtFounder, lblYear, txtYear, lblDistricts, txtDistricts, lblArea, txtArea,
                lblStreet, txtStreet, lblBuilding, txtBuilding, lblCity, txtCity, btnOk, btnCancel });
            this.Text = "Ввод данных";
            this.ResumeLayout(false);
        }

        private void UpdateFieldsVisibility()
        {
            string type = cmbType.SelectedItem?.ToString();
            bool isPlace = type == "Место";
            bool isRegion = type == "Область";
            bool isCity = type == "Город";
            bool isMetropolis = type == "Мегаполис";
            bool isAddress = type == "Адрес";

            lblDistrict.Visible = txtDistrict.Visible = isRegion || isCity || isMetropolis;
            lblFounder.Visible = txtFounder.Visible = isCity || isMetropolis;
            lblYear.Visible = txtYear.Visible = isCity || isMetropolis;
            lblDistricts.Visible = txtDistricts.Visible = isMetropolis;
            lblArea.Visible = txtArea.Visible = isMetropolis;
            lblStreet.Visible = txtStreet.Visible = isAddress;
            lblBuilding.Visible = txtBuilding.Visible = isAddress;
            lblCity.Visible = txtCity.Visible = isAddress;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text.Trim();
                if (string.IsNullOrEmpty(name)) throw new Exception("Название не может быть пустым.");
                int population = int.Parse(txtPopulation.Text);
                string type = cmbType.SelectedItem.ToString();

                switch (type)
                {
                    case "Место":
                        ResultObject = new Place(name, population);
                        break;
                    case "Область":
                        string district = txtDistrict.Text.Trim();
                        ResultObject = new LocationLibrary.Region(name, population, district);
                        break;
                    case "Город":
                        string district2 = txtDistrict.Text.Trim();
                        string founder = txtFounder.Text.Trim();
                        int year = int.Parse(txtYear.Text);
                        ResultObject = new City(name, population, district2, founder, year);
                        break;
                    case "Мегаполис":
                        string district3 = txtDistrict.Text.Trim();
                        string founder2 = txtFounder.Text.Trim();
                        int year2 = int.Parse(txtYear.Text);
                        int districts = int.Parse(txtDistricts.Text);
                        double area = double.Parse(txtArea.Text);
                        ResultObject = new Metropolis(name, population, district3, founder2, year2, districts, area);
                        break;
                    case "Адрес":
                        string street = txtStreet.Text.Trim();
                        int building = int.Parse(txtBuilding.Text);
                        string city = txtCity.Text.Trim();
                        ResultObject = new Address(name, population, street, building, city);
                        break;
                }
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка ввода: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
        }
    }
}