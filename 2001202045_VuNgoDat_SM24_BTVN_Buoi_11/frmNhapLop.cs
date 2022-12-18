using _2001202045_VuNgoDat_SM24_BTVN_Buoi_11.Utilites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2001202045_VuNgoDat_SM24_BTVN_Buoi_11
{
    public partial class frmNhapLop : Form
    {
        Core core;
        DataSet ds_QLSV;
        DataColumn[] keyKhoa = new DataColumn[1];
        DataColumn[] keyLop = new DataColumn[1];
        DataColumn[] keySV = new DataColumn[1];
        public frmNhapLop()
        {
            ds_QLSV = new DataSet();
            core = new Core();
            InitializeComponent();
        }

        private void frmNhapLop_Load(object sender, EventArgs e)
        {
            dgv.MultiSelect = false;
            //Load data
            core.LoadDataIntoCbo(cboKhoa, ds_QLSV, "Select * from khoa", "khoa" ,"tenkhoa", "makhoa");
            core.LoadDataIntoDgv(dgv, ds_QLSV, "select * from lop", "lop");
            //Disable all textbox + cbo
            pnl.Controls.OfType<Control>().ToList().ForEach(x =>
            {
                if (x.GetType() != typeof(Label))
                {
                    x.Enabled = false;
                }
            });
            //Disable btn
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = false;
            //Fill data into SinhVien table
            core.FillData(ds_QLSV, "select * from sinhvien", "sinhvien");
            //Add primary keys for tables
            keyKhoa[0] = ds_QLSV.Tables["KHOA"].Columns[0];
            ds_QLSV.Tables["KHOA"].PrimaryKey = keyKhoa;

            keyLop[0] = ds_QLSV.Tables["LOP"].Columns[0];
            ds_QLSV.Tables["LOP"].PrimaryKey = keyLop;

            keySV[0] = ds_QLSV.Tables["SINHVIEN"].Columns[0];
            ds_QLSV.Tables["SINHVIEN"].PrimaryKey = keySV;

        }
        private void DataBindingToDgv(DataTable dt)
        {
            pnl.Controls.OfType<Control>().ToList().ForEach(x => {
                x.DataBindings.Clear();
            });
            txtMaLop.DataBindings.Add("Text",dt, "MALOP");
            txtTenLop.DataBindings.Add("Text", dt, "TENLOP");
        }

        #region Controls
        private void btnDong_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát","Hệ thống", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string selectCmd = "Select * from Lop";
            DataRow newRow = ds_QLSV.Tables["LOP"].NewRow();
            if (newRow != null)
            {
                if (core.Update(ds_QLSV, selectCmd, "Lop") == 0)
                {
                    MessageBox.Show("Cập nhật thất bại!", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Cập nhật thành công!", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại!", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnLuu.Enabled = false;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            txtMaLop.DataBindings.Clear();
            btnLuu.Enabled = true;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = false;
            dgv.Columns[0].ReadOnly = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa?", "Hệ thống", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                foreach (DataRow rowS in ds_QLSV.Tables["SinhVien"].Rows)
                {
                    if (rowS["malop"].ToString().Equals(txtMaLop.Text.Trim()))
                    {
                        MessageBox.Show("Khóa đang được sử dụng ở bảng SinhVien!", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                }
                DataRow row = ds_QLSV.Tables["Lop"].Rows.Find(txtMaLop.Text.Trim());
                if (row != null)
                {
                    row.Delete();
                }
                core.Update(ds_QLSV, "select * from lop", "lop");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaLop.DataBindings.Clear();
            btnLuu.Enabled = true;
            dgv.AllowUserToAddRows = true;
            dgv.ReadOnly = false;
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                dgv.Rows[i].ReadOnly = true;
            }
            dgv.FirstDisplayedScrollingColumnIndex = dgv.Rows.Count - 2;
        }
        #endregion

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            DataBindingToDgv(ds_QLSV.Tables["LOP"]);
            DataGridViewRow row = dgv.CurrentRow;
            cboKhoa.SelectedValue = row.Cells[2].Value.ToString();
        }

        private void dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Đã tồn tại khóa!", "Hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }
}
