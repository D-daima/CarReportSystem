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
            dgvCarReportData.DataSource = _CarReports;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initButton();
        }

        private CarReport.CarMaker CheckRbottom()
        {
            if (rbToyota.Checked == true) {
                return CarReport.CarMaker.トヨタ;
            } else if (rbNissan.Checked == true) {
                return CarReport.CarMaker.日産;
            } else if (rbHonda.Checked == true) {
                return CarReport.CarMaker.ホンダ;
            } else if (rbSubaru.Checked == true) {
                return CarReport.CarMaker.スバル;
            } else if (rbOutCar.Checked == true) {
                return CarReport.CarMaker.外車;
            } else if(rbOther.Checked == true){
                return CarReport.CarMaker.その他;
            }
            return CarReport.CarMaker.DEFAULT;
        }


        private void btAdd_Click(object sender, EventArgs e)
        {
            if (cbAuthor.Text == "" && cbName.Text == "" && CheckRbottom() == CarReport.CarMaker.DEFAULT) {
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
                Maker = CheckRbottom(),
                CarName = cbName.Text,
                Report = tbReport.Text,
                imgPicture = pbImage.Image

            };
            setComboBoxAuthor(cbAuthor.Text);
            setComboBoxCarName(cbName.Text);
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
            pbImage.Image = null;
        }

        private void btChange_Click(object sender, EventArgs e)
        {
            CarReport carChange = _CarReports[dgvCarReportData.CurrentRow.Index];
            carChange.CreatedDate = dtpCreatedDate.Value;
            carChange.Author = cbAuthor.Text;
            carChange.Maker = CheckRbottom();
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
            _CarReports.RemoveAt(dgvCarReportData.CurrentRow.Index);
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
            if (ofdDateOpen.ShowDialog() == DialogResult.OK) {
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
            }
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
            CarReport selectedCar = _CarReports[dgvCarReportData.CurrentRow.Index];
            dtpCreatedDate.Value = selectedCar.CreatedDate;
            cbAuthor.Text = selectedCar.Author;
            Check(selectedCar.Maker);
            cbName.Text = selectedCar.CarName;
            tbReport.Text = selectedCar.Report;
            pbImage.Image = selectedCar.imgPicture;
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
    }
}
