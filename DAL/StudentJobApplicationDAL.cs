using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using StudentJobApplication.Models;
using System.Configuration;

namespace StudentJobApplication.DAL
{
    public class StudentJobApplicationDAL
    {
        string conString = ConfigurationManager.ConnectionStrings["StudentConnection"].ToString();
        public List<Students> GetAllJobApplications()
        {
            List<Students> jobApplicationList = new List<Students>();
            using (SqlConnection conn = new SqlConnection(conString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_GetAllStudentJobApplications";
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                DataTable dtApplications = new DataTable();

                conn.Open();
                sqlAdapter.Fill(dtApplications);
                conn.Close();

                if (dtApplications.Rows.Count == 0)
                {
                    return null; 
                }

                // Convert the rows to the list of students
                foreach (DataRow dr in dtApplications.Rows)
                {
                    jobApplicationList.Add(new Students
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        Email = dr["Email"].ToString(),
                        PhoneNumber = dr["PhoneNumber"].ToString(),
                        JobPosition = dr["JobPosition"].ToString(),
                        PhotoBase64 = dr["PhotoBase64"].ToString(),
                        ResumeBase64 = dr["ResumeBase64"].ToString(),
                        ApplicationDate = Convert.ToDateTime(dr["ApplicationDate"])
                    });
                }
            }
            return jobApplicationList;
        }

        // Method to insert a student job application
        public bool InsertStudentJobApplication(Students student)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_InsertStudentJobApplication", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                command.Parameters.AddWithValue("@JobPosition", student.JobPosition);
                command.Parameters.AddWithValue("@PhotoBase64", student.PhotoBase64);
                command.Parameters.AddWithValue("@ResumeBase64", student.ResumeBase64);
                command.Parameters.AddWithValue("@ApplicationDate", student.ApplicationDate);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
                connection.Close();
            }

            return rowsAffected > 0;
        }

        //updation of student job application
        public bool UpdateStudentJobApplication(Students student)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateStudentJobApplication", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", student.Id);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                command.Parameters.AddWithValue("@JobPosition", student.JobPosition);
                command.Parameters.AddWithValue("@PhotoBase64", student.PhotoBase64 ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ResumeBase64", student.ResumeBase64 ?? (object)DBNull.Value);
                
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
                connection.Close();
            }

            return rowsAffected > 0;
        }

        //deletion of student job application
        public bool DeleteStudentJobApplication(int id)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(conString))
            {
                SqlCommand command = new SqlCommand("sp_DeleteStudentJobApplication", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
                connection.Close();
            }
            return rowsAffected > 0;
        }
    }
}


