using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarReportSystem {
    public partial class Form1 : Form {

        BindingList<CarReport> _CarReports = new BindingList<CarReport>();

        public Form1()
        {
            InitializeComponent();
            //dgvCarReportData.DataSource = _CarReports;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: このコード行はデータを 'infosys202002DataSet.CarReport' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            dgvCarReportData.Columns[0].Visible = false; //idを非表示にする
            initButton();
        }

        private void CheckRbottom()
        {
            var maker = dgvCarReportData.CurrentRow.Cells[3].Value;
            switch (maker) {
                case  "トヨタ":
                    rbToyota.Checked = true;
                    break;
                case "日産":
                    rbNissan.Checked = true;
                    break;
                case "ホンダ":
                    rbHonda.Checked = true;
                    break;
                case "スバル":
                    rbSubaru.Checked = true;
                    break;
                case "外車":
                    rbOutCar.Checked = true;
                    break;
                case "その他":
                    rbOther.Checked = true;
                    break;
            }
        }


        private void btAdd_Click(object sender, EventArgs e)
        {
            if (cbAuthor.Text == "" || cbName.Text == "") {
                MessageBox.Show("記録者と車名を入力、メーカーを選択してください。",
                "エラー",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }
            CarReport carReport = new CarReport()
            {
                CreatedDate = dtpCreatedDate.Value,
                Author = cbAuthor.Text,
                CarName = cbName.Text,
                Report = tbReport.Text,
                imgPicture = pbImage.Image

            };
            setComboBoxAuthor(cbAuthor.Text);
            setComboBoxCarName(cbName.Text);
             dgvCarReportData.SelectedCells.ToString();
            _CarReports.Insert(0, carReport);
            dgvCarReportData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvCarReportData.ClearSelection();
            initButton();
            inputItemClear();
        }

        private void inputItemClear()
        {
            
            cbAuthor.Text = "";
            cbName.Text = "";
            Deleted();
            tbReport.Text = "";
            pbImage.Image = null;
        }

        private void btEnd_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btOpenImage_Click(object sender, EventArgs e)
        {
            if(ofdImageOpen.ShowDialog() == DialogResult.OK) {
                pbImage.Image = Image.FromFile(ofdImageOpen.FileName);
                pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void btDeleteImage_Click(object sender, EventArgs e)
        {
            if(pbImage.Image != null) {
                DialogResult dialog = MessageBox.Show("本当によろしいですか？", "確認", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes) {
                    pbImage.Image = null;
                } else if (dialog == DialogResult.No) {

                }
            }
        }

        private void btChange_Click(object sender, EventArgs e)
        {
            CarReport carChange = _CarReports[dgvCarReportData.CurrentRow.Index];
            carChange.CreatedDate = dtpCreatedDate.Value;
            carChange.Author = cbAuthor.Text;
            CheckRbottom();
            carChange.CarName = cbName.Text;
            carChange.Report = tbReport.Text;
            carChange.imgPicture = pbImage.Image;
            setComboBoxAuthor(cbAuthor.Text);
            setComboBoxCarName(cbName.Text);
            inputItemClear();
            dgvCarReportData.Refresh();

        }

        private void initButton()
        {
            if (_CarReports.Count <= 0) {
                btChange.Enabled = false;
                btDelete.Enabled = false;
            } else {
                btChange.Enabled = true;
                btDelete.Enabled = true;
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            
            dgvCarReportData.Refresh();
            inputItemClear();
            initButton();
            dgvCarReportData.ClearSelection();
        }


        private void setComboBoxAuthor(string Author)
        {
            if (!cbAuthor.Items.Contains(Author)) {
                //コンボボックスの候補に追加
                cbAuthor.Items.Add(Author);
            }
        }

        private void setComboBoxCarName(string Carname)
        {
            if (!cbName.Items.Contains(Carname)) {
                //コンボボックスの候補に追加
                cbName.Items.Add(Carname);
            }
        }

        private void btOpenFile_Click(object sender, EventArgs e)
        {
            /*if (ofdDateOpen.ShowDialog() == DialogResult.OK) {
                using (FileStream fs = new FileStream(ofdDateOpen.FileName, FileMode.Open))
                {
                    try 
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        //逆シリアル化して読み込む
                        _CarReports = (BindingList<CarReport>)formatter.Deserialize(fs);
                        //データグリッドビューに再設定
                        dgvCarReportData.DataSource = _CarReports;
                        initButton();
                        //選択されている箇所をコントロールへ表示
                        dgvCarReportData_Click(sender, e);//イベントハンドラを呼び出す
                    }
                    catch (SerializationException se)
                    {
                        Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
                        throw;
                    }
                }
                //メーカー、車名を読み込み
                for (int i = 0; i < dgvCarReportData.Rows.Count; i++) {
                    setComboBoxAuthor(_CarReports[i].Author);
                    setComboBoxCarName(_CarReports[i].CarName);
                }
            }*/
            this.carReportTableAdapter.Fill(this.infosys202002DataSet.CarReport);
            dgvCarReportData_Click(sender, e);
            
        }

        private void Check(CarReport.CarMaker maker)
        {
            if(CarReport.CarMaker.トヨタ == maker) {
                 rbToyota.Checked = true;
            } else if (CarReport.CarMaker.日産 == maker) {
                rbNissan.Checked = true;
            } else if (CarReport.CarMaker.ホンダ == maker) {
                rbHonda.Checked = true;
            } else if (CarReport.CarMaker.スバル == maker) {
                rbSubaru.Checked = true;
            } else if (CarReport.CarMaker.外車 == maker) {
                rbOutCar.Checked = true;
            } else if (CarReport.CarMaker.その他 == maker) {
                rbOther.Checked = true;
            }
        }


        private void Deleted()
        {
            if (rbToyota.Checked == true) {
                rbToyota.Checked = false;
            } else if (rbNissan.Checked == true) {
                rbNissan.Checked = false;
            } else if (rbHonda.Checked == true) {
                rbHonda.Checked = false;
            } else if (rbSubaru.Checked == true) {
                rbSubaru.Checked = false;
            } else if (rbOutCar.Checked == true) {
                rbOutCar.Checked = false;
            } else if (rbOther.Checked == true) {
                rbOther.Checked = false;
            }
        }


        private void dgvCarReportData_Click(object sender, EventArgs e)
        {
            if (dgvCarReportData.CurrentRow == null) {
                return;
            }
            //var test =  dgvCarReportData.CurrentRow.Cells[2].Value;
            CheckRbottom();
            
            /*CarReport selectedCar = _CarReports[dgvCarReportData.CurrentRow.Index];
            dtpCreatedDate.Value = selectedCar.CreatedDate;
            cbAuthor.Text = selectedCar.Author;
            Check(selectedCar.Maker);
            cbName.Text = selectedCar.CarName;
            tbReport.Text = selectedCar.Report;
            pbImage.Image = selectedCar.imgPicture;*/
        }

        private void btSaveFile_Click(object sender, EventArgs e)
        {
            if (sfdSaveData.ShowDialog() == DialogResult.OK) {

                BinaryFormatter formatter = new BinaryFormatter();

                //ファイルストリームを作成
                using (FileStream fs = new FileStream(sfdSaveData.FileName, FileMode.Create)) {
                    try {
                        formatter.Serialize(fs, _CarReports);
                    }
                    catch (SerializationException se) {
                        Console.WriteLine("Failed to serialize. Reason: " + se.Message);
                        throw;
                    }
                }
            }
        }

        private void 新規作成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputItemClear();
        }

        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdSaveData.ShowDialog() == DialogResult.OK) {
                BinaryFormatter formatter = new BinaryFormatter();
                //ファイルストリームを作成
                using (FileStream fs = new FileStream(sfdSaveData.FileName, FileMode.Create)) {
                    try {
                        formatter.Serialize(fs, _CarReports);
                    }
                    catch (SerializationException se) {
                        Console.WriteLine("Failed to serialize. Reason: " + se.Message);
                        throw;
                    }
                }
            }
        }

        private void 開くOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdDateOpen.ShowDialog() == DialogResult.OK) {
                using (FileStream fs = new FileStream(ofdDateOpen.FileName, FileMode.Open)) {
                    try {
                        BinaryFormatter formatter = new BinaryFormatter();
                        //逆シリアル化して読み込む
                        _CarReports = (BindingList<CarReport>)formatter.Deserialize(fs);
                        //データグリッドビューに再設定
                        dgvCarReportData.DataSource = _CarReports;
                        initButton();
                        //選択されている箇所をコントロールへ表示
                        dgvCarReportData_Click(sender, e);//イベントハンドラを呼び出す
                    }
                    catch (SerializationException se) {
                        Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
                        throw;
                    }
                }
            }
        }
    }
}
