using System;
using System.Windows.Forms;
using DataBase;

namespace lab5
{
    public partial class Form1 : Form
    {
        DataStorage data = new DataStorage();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDlg.InitialDirectory = Application.StartupPath;
            if (openFileDlg.ShowDialog() == DialogResult.OK)
            {
                ShowData(openFileDlg.FileName);
            }
        }

        private void ShowData(String datapath)
        {
            try
            {
                data = DataStorage.DataCreator(datapath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке данных что-то сломалось");
            }

            dgvRaw.DataSource = data.GetRawData();
            dgvRaw.ReadOnly = true;
            dgvSummary.DataSource = data.GetSummaryData();
            dgvSummary.ReadOnly = true;
        }
    }
}
