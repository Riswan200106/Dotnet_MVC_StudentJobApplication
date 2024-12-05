using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentJobApplication.DAL;
using StudentJobApplication.Models;   

namespace StudentJobApplication.Controllers
{
    public class StudentController : Controller
    {
        StudentJobApplicationDAL _studentDAL = new StudentJobApplicationDAL();  

        /// <summary>
        /// Displays the list of all student job applications
        /// </summary>
        /// <returns> return to the index view with list of job application list if it is success
        /// if an error occur return the empty list with an error message 
        /// </returns>
        public ActionResult Index()
        {
            try
            {
                List<Students> JobApplicationList = _studentDAL.GetAllJobApplications();
                if (JobApplicationList == null)
                {
                    JobApplicationList = new List<Students>();
                }
                return View(JobApplicationList);
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = "Error retrieving job applications: " + exception.Message;

                return View(new List<Students>()); 
            }
        }

        /// <summary>
        /// Displays the Create view for submitting a student job application.
        /// </summary>
        /// <returns> Returns the Create view. </returns>
        public ActionResult Create()
        {
            return View(); 
        }

        /// <summary>
        /// Handles the post request of the student job application
        /// </summary>
        /// <param name="student">The model object contains the form data</param>
        /// <param name="Photo"> The uploading photo file</param>
        /// <param name="Resume"> The uploading resume file</param>
        /// <returns> Redirects to the Index action on success. 
        /// If an error occurs, returns to the Create view with an error message.
        /// </returns>
        [HttpPost]
        public ActionResult Create(Students student, HttpPostedFileBase Photo, HttpPostedFileBase Resume)
        {
            bool IsInserted = false;

            try
            {
                if (ModelState.IsValid)
                {
                    student.ApplicationDate = DateTime.Now;
                    if (Photo != null && Photo.ContentLength > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            Photo.InputStream.CopyTo(memoryStream);
                            student.PhotoBase64 = Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }
                    if (Resume != null && Resume.ContentLength > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            Resume.InputStream.CopyTo(memoryStream);
                            student.ResumeBase64 = Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }
                    IsInserted = _studentDAL.InsertStudentJobApplication(student);

                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Student job application submitted successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to submit the job application.";
                    }
                }
                return RedirectToAction("Index"); 
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = exception.Message;  
                return View();  
            }
        }

        /// <summary>
        /// Display the edit view for the updation of the student job application
        /// </summary>
        /// <param name="id"> The unique identifier of the student job application to be updated. </param>
        /// <returns> Returns the Edit view with the existing student's details. 
        /// If the student is not found, redirects to the Index view with an informational message.
        /// </returns>
        public ActionResult Edit(int id)
        {
            List<Students> jobApplications = _studentDAL.GetAllJobApplications();
            Students student = jobApplications.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                TempData["InfoMessage"] = "Student is not available with Id" + id.ToString();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        /// <summary>
        /// Handles the submission of updated data of student job application
        /// </summary>
        /// <param name="student"> student object contains the updated student job application details</param>
        /// <param name="Photo"> The uploaded photo </param>
        /// <param name="Resume"> The uploaded resume</param>
        /// <returns> return to the index view if edit is successful with a success message
        /// return to the edit if fails, and show a error message
        /// </returns>
        [HttpPost]
        public ActionResult Edit(Students student, HttpPostedFileBase Photo, HttpPostedFileBase Resume)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please correct the errors in the form.";
                    return View(student);
                }

                if (Photo != null && Photo.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        Photo.InputStream.CopyTo(memoryStream);
                        student.PhotoBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }

                if (Resume != null && Resume.ContentLength > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        Resume.InputStream.CopyTo(memoryStream);
                        student.ResumeBase64 = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
                bool isUpdated = _studentDAL.UpdateStudentJobApplication(student);

                if (isUpdated)
                {
                    TempData["SuccessMessage"] = "Student details updated successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to update the student details.";
                    return View(student);
                }
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = $"An error occurred: {exception.Message}";
                return View(student);
            }
        }

        /// <summary>
        /// Display the delete confirmation view of the details to be deleted
        /// </summary>
        /// <param name="id"> unique identifier that is need to be deleted</param>
        /// <returns> return the delete confirmation view with details
        /// if no id is founded it return to index with an error message
        /// </returns>
        public ActionResult Delete(int id)
        {
            Students student = _studentDAL.GetAllJobApplications().FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("Index");
            }

            return View(student);
        }

        /// <summary>
        /// Handles the deletion of student details of selected id
        /// </summary>
        /// <param name="id"> unique identifier of student job application to be deleted</param>
        /// <returns> Returns to the index view after attempting the deletion
        /// if deleted show the success message otherwise show the error message
        /// </returns>
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {               
                bool isDeleted = _studentDAL.DeleteStudentJobApplication(id);

                if (isDeleted)
                {
                    TempData["SuccessMessage"] = " Student application details deleted successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to delete the student job application.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["ErrorMessage"] = exception.Message;
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Displays the details of the selected id students application details
        /// </summary>
        /// <param name="id">unique identifier to be selected</param>
        /// <returns> return the detail view of student application details
        /// if student not founded it show an error message and redirect to index view
        /// </returns>
        public ActionResult Details(int id)
        {
            Students student = _studentDAL.GetAllJobApplications().FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("Index");
            }

            return View(student);
        }
    }
}
