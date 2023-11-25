﻿using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using test3.Models;

namespace test3.Controllers
{
    public class ExportController : Controller
    {
        // GET: Export
        public ActionResult ExportSinhVien(string tenLop, string tenKhoa)
        {
            List<Student> students;
            bool isSearchByClass = !string.IsNullOrEmpty(tenLop);
            bool isSearchByDepartment = !string.IsNullOrEmpty(tenKhoa);

            if (isSearchByClass)
            {
                students = GetStudentListByClassFromDatabase(tenLop);
            }
            else if (isSearchByDepartment)
            {
                students = GetStudentListByDepartmentFromDatabase(tenKhoa);
            }
            else
            {
                students = GetStudentListFromDatabase();
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachSinhVien");

                worksheet.Cell(1, 1).Value = "StudentID";
                worksheet.Cell(1, 2).Value = "FullName";
                worksheet.Cell(1, 3).Value = "DateOfBirth";
                worksheet.Cell(1, 4).Value = "Gender";
                worksheet.Cell(1, 5).Value = "Address";
                worksheet.Cell(1, 6).Value = "ContactNumber";
                worksheet.Cell(1, 7).Value = "Email";
                worksheet.Cell(1, 8).Value = "ClassID";
                worksheet.Cell(1, 9).Value = "DepartmentID";

                for (int i = 0; i < students.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = students[i].StudentID;
                    worksheet.Cell(row, 2).Value = students[i].FullName;
                    worksheet.Cell(row, 3).Value = students[i].DateOfBirth;
                    worksheet.Cell(row, 4).Value = students[i].Gender ? "Nam" : "Nữ";
                    worksheet.Cell(row, 5).Value = students[i].Address;
                    worksheet.Cell(row, 6).Value = students[i].ContactNumber;
                    worksheet.Cell(row, 7).Value = students[i].Email;
                    worksheet.Cell(row, 8).Value = students[i].ClassID;
                    worksheet.Cell(row, 9).Value = students[i].DepartmentID;
                }

                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachSinhVien.xlsx");
            }
        }
        private List<Student> GetStudentListFromDatabase()
        {
            var db = new QuanliSVEntities();
            return db.Students.ToList();
        }
        private List<Student> GetStudentListByClassFromDatabase(string tenLop)
        {
            var db = new QuanliSVEntities();
            var students = db.Students
                     .Where(s => s.Class.ClassName.ToLower() == tenLop.ToLower())
                     .ToList();
            return db.Students.ToList();
        }
        private List<Student> GetStudentListByDepartmentFromDatabase(string tenKhoa)
        {
            var students = db.Students
                             .Where(s => s.Class.Department.DepartmentName.ToLower() == tenKhoa.ToLower())
                             .ToList();

            return students;
        }
        private QuanliSVEntities db = new QuanliSVEntities();
        public ActionResult ExportMonHoc()
        {
            // Lấy danh sách sinh viên từ cơ sở dữ liệu hoặc từ nơi bạn lưu trữ dữ liệu
            List<Cours> subjects = GetSubjectListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachMonHoc");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "CourseID";
                worksheet.Cell(1, 2).Value = "CourseName";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "Credits";
                worksheet.Cell(1, 5).Value = "DepartmentName";
                worksheet.Cell(1, 6).Value = "SemesterName";

                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < subjects.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = subjects[i].CourseID;
                    worksheet.Cell(row, 2).Value = subjects[i].CourseName;
                    worksheet.Cell(row, 3).Value = subjects[i].Description;
                    worksheet.Cell(row, 4).Value = subjects[i].Credits;
                    var departmentName = GetDepartmentName(subjects[i].DepartmentID);
                    var semesterName = GetSemesterName(subjects[i].SemesterID);

                    worksheet.Cell(row, 5).Value = departmentName;
                    worksheet.Cell(row, 6).Value = semesterName;

                }

                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachMonHoc.xlsx");
            }
        }

        public ActionResult ExportLop()
        {
            // Lấy danh sách sinh viên từ cơ sở dữ liệu hoặc từ nơi bạn lưu trữ dữ liệu
            List<Class> classes = GetClassListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachMonHoc");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "ClassID";
                worksheet.Cell(1, 2).Value = "ClassName";
                worksheet.Cell(1, 3).Value = "StartDate";
                worksheet.Cell(1, 4).Value = "EndDate";
                worksheet.Cell(1, 5).Value = "HeadTeacher";
                worksheet.Cell(1, 6).Value = "MaxStudents";

                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < classes.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = classes[i].ClassID;
                    worksheet.Cell(row, 2).Value = classes[i].ClassName;
                    worksheet.Cell(row, 3).Value = classes[i].StartDate;
                    worksheet.Cell(row, 4).Value = classes[i].EndDate;
                    worksheet.Cell(row, 5).Value = classes[i].HeadTeacher;
                    worksheet.Cell(row, 6).Value = classes[i].MaxStudents;

                }
                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachLop.xlsx");
            }
        }

        private string GetDepartmentName(int departmentID)
        {
            var department = db.Departments.Find(departmentID);
            return department != null ? department.DepartmentName : string.Empty;
        }

        // Hàm để lấy tên học kỳ từ SemesterID
        private string GetSemesterName(int semesterID)
        {
            var semester = db.Semesters.Find(semesterID);
            return semester != null ? semester.SemesterName : string.Empty;
        }


        //danh sach sinh vien dat hoc bong
        public ActionResult ExportSVDHB(string MaKi)
        {
            List<sp_DanhSachSinhVienDatHocBong_Result> dssvdhb = db.sp_DanhSachSinhVienDatHocBong(MaKi).ToList();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("DanhSachSinhVienDatHocBong");


            var headers = new List<string> { "Mã Sinh Viên", "Tên Sinh Viên", "..." };
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
            }


            for (int i = 0; i < dssvdhb.Count; i++)
            {
                var sv = dssvdhb[i];
                worksheet.Cell(i + 2, 1).Value = sv.SemesterID;
                worksheet.Cell(i + 2, 2).Value = sv.FullName;

            }

            var memoryStream = new System.IO.MemoryStream();
            workbook.SaveAs(memoryStream);

            return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachSinhVienDatHocBong.xlsx");
        }

        //export hoc ki
        public ActionResult ExportHocKi()
        {
            
            List<Semester> semesters = GetSemesterListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachHocKi");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "SemesterID";
                worksheet.Cell(1, 2).Value = "SemesterName";
                worksheet.Cell(1, 3).Value = "StartDate";
                worksheet.Cell(1, 4).Value = "EndDate";
                

                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < semesters.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = semesters[i].SemesterID;
                    worksheet.Cell(row, 2).Value = semesters[i].SemesterName;
                    worksheet.Cell(row, 3).Value = semesters[i].StartDate;
                    worksheet.Cell(row, 4).Value = semesters[i].EndDate;
                    

                }
                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachHocKi.xlsx");
            }
        }

        //hocbong
        public ActionResult ExportHocBong()
        {

            List<Scholarship> scholarships = GetScholarshipListFromDatabase();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("DanhSachHocBong");

                // Định dạng tiêu đề
                worksheet.Cell(1, 1).Value = "ScholarshipID";
                worksheet.Cell(1, 2).Value = "ScholarshipName";
                worksheet.Cell(1, 3).Value = "Description";
                worksheet.Cell(1, 4).Value = "StartDate";
                worksheet.Cell(1, 5).Value = "EndDate";
                worksheet.Cell(1, 6).Value = "Conditions";


                // Ghi danh sách sinh viên vào file Excel
                for (int i = 0; i < scholarships.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = scholarships[i].ScholarshipID;
                    worksheet.Cell(row, 2).Value = scholarships[i].ScholarshipID;
                    worksheet.Cell(row, 3).Value = scholarships[i].Description;
                    worksheet.Cell(row, 4).Value = scholarships[i].StartDate;
                    worksheet.Cell(row, 5).Value = scholarships[i].EndDate;
                    worksheet.Cell(row, 6).Value = scholarships[i].Conditions;


                }
                // Tạo tệp Excel và trả về nó cho người dùng
                var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);

                return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachHocBong.xlsx");
            }
        }



        private List<Cours> GetSubjectListFromDatabase()
        {
            var db = new QuanliSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.Courses.ToList();
        }
        private List<Class> GetClassListFromDatabase()
        {
            var db = new QuanliSVEntities();
            // Viết mã lấy danh sách sinh viên từ cơ sở dữ liệu của bạn ở đây
            return db.Classes.ToList();
        }
        private List<Semester> GetSemesterListFromDatabase()
        {
            var db = new QuanliSVEntities();
            return db.Semesters.ToList();
        }
        private List<Scholarship> GetScholarshipListFromDatabase()
        {
            var db = new QuanliSVEntities();
            return db.Scholarships.ToList();
        }
        
    }
}