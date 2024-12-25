using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private dgvSinhVien context;
        private Student selectedStudent;

        public Form1()
        {
            InitializeComponent();

        }

        private void FillFacultyComboBox(List<Faculty> listFaculties)
        {
            this.comboKhoa.DataSource = listFaculties;
            this.comboKhoa.DisplayMember = "FacultyName";
            this.comboKhoa.ValueMember = "FacultyID";
        }
        private void BindGrid(List<Student> listStudents)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in listStudents)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = item.StudentID;
                dgvSinhVien.Rows[index].Cells[1].Value = item.FullName;
                dgvSinhVien.Rows[index].Cells[2].Value = item.Faculty.Facultyame;
                dgvSinhVien.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }
        private void ClearForm()
        {
            txt_MSSV.Clear();
            txtHoten.Clear();
            comboKhoa.SelectedIndex = -1;
            txtDiemTB.Clear();
        }
        private void LoadData()
        {
            try
            {
                context = new dgvSinhVien();
                List<Faculty> listFalcutys = context.Faculties.ToList();
                List<Student> listStudents = context.Students.ToList();
                FillFacultyComboBox(listFalcutys);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                context = new dgvSinhVien();

                // Kiểm tra và thêm dữ liệu mẫu nếu Faculties trống
                if (!context.Faculties.Any())
                {
                    context.Faculties.Add(new Faculty { FacultyID = 1, Facultyame = "Khoa CNTT" });
                    context.Faculties.Add(new Faculty { FacultyID = 2, Facultyame = "Khoa Kinh tế" });
                    context.SaveChanges();
                }

                List<Faculty> listFaculties = context.Faculties.ToList();
                List<Student> listStudents = context.Students.ToList();

                FillFacultyComboBox(listFaculties);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Kiểm tra nếu chọn dòng hợp lệ
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];

                
                string studentIDString = row.Cells[0].Value.ToString();
                string studentFullName = row.Cells[1].Value.ToString();
                string averageScore = row.Cells[3].Value.ToString();
                int facultyID = (int)row.Cells[2].Value;

               
                txt_MSSV.Text = studentIDString;
                txtHoten.Text = studentFullName;
                txtDiemTB.Text = averageScore;
                comboKhoa.SelectedValue = facultyID; // Lấy giá trị của khóa học từ ComboBox
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_MSSV.Text) ||
                    string.IsNullOrWhiteSpace(txtHoten.Text) ||
                    string.IsNullOrWhiteSpace(txtDiemTB.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                if (txt_MSSV.Text.Length != 10)
                {
                    MessageBox.Show("Mã số sinh viên phải có 10 ký tự!");
                    return;
                }

                int studentID;
                if (int.TryParse(txt_MSSV.Text, out studentID))
                {
                    Student existingStudent = context.Students.FirstOrDefault(s => s.StudentID == studentID);
                    if (existingStudent != null)
                    {
                        // Xử lý khi tìm thấy sinh viên
                    }
                    else
                    {
                        MessageBox.Show("Sinh viên không tồn tại.");
                    }
                }
                else
                {
                    MessageBox.Show("Mã số sinh viên phải là số!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
     
     
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra input
                if (string.IsNullOrWhiteSpace(txt_MSSV.Text) ||
                    string.IsNullOrWhiteSpace(txtHoten.Text) ||
                    string.IsNullOrWhiteSpace(txtDiemTB.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                if (txt_MSSV.Text.Length != 10)
                {
                    MessageBox.Show("Mã số sinh viên phải có 10 ký tự!");
                    return;
                }

                // Chuyển đổi MSSV từ string sang int
                int studentID;
                if (int.TryParse(txt_MSSV.Text, out studentID))
                {
                    // Tìm sinh viên theo MSSV
                    Student existingStudent = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                    if (existingStudent == null)
                    {
                        MessageBox.Show("Không tìm thấy MSSV cần sửa!");
                        return;
                    }

                    // Cập nhật thông tin
                    existingStudent.FullName = txtHoten.Text;
                    existingStudent.FacultyID = Convert.ToInt32(comboKhoa.SelectedValue);
                    existingStudent.AverageScore = float.Parse(txtDiemTB.Text);

                    // Lưu thay đổi
                    context.SaveChanges();

                    MessageBox.Show("Cập nhật dữ liệu thành công!");
                    ClearForm();
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Mã số sinh viên phải là số!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (selectedStudent != null)
            {
                try
                {
                    Student existingStudent = context.Students.FirstOrDefault(s => s.StudentID == selectedStudent.StudentID);
                    if (existingStudent == null)
                    {
                        MessageBox.Show("Không tìm thấy MSSV cần xóa!");
                        return;
                    }

                    DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên có mã số {selectedStudent.StudentID}?", "Thông báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        context.Students.Remove(existingStudent);
                        context.SaveChanges();

                        MessageBox.Show("Xóa sinh viên thành công!");
                        ClearForm();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để xóa.");
            }
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn thoát không?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {


        }

        private void comboKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvSinhVien_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
