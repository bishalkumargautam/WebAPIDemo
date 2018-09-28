using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDAL;

namespace PragimWebAPIDemo.Controllers
{
    public class EmployeesController : ApiController
    {
        //Get 
        public HttpResponseMessage Get(string gender = "All")
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    switch (gender.ToLower())
                    {
                        case "all":
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());

                            }
                        case "male":
                            {
                                return Request.CreateResponse(HttpStatusCode.OK,
                                    entities.Employees.Where(e => e.Gender == gender).ToList());
                            }
                        case "female":
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(e => e.Gender == gender).ToList());
                            }
                        default:
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request can be either 'male', 'female' or 'all'");

                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        //Get ID
        public HttpResponseMessage Get(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with ID " + id
                        + " not found in the database");
                }
            }
        }
        // Post
        public HttpResponseMessage Post([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        //Delete
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault((e => e.ID == id));

                    if (entity != null)
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK, "Employee with id " + id + " deleted");
                        return message;
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id " + id + " not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        //Put
        public HttpResponseMessage Put([FromUri] int id, [FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee with Id " + id + " is not present.");
                    }
                    entity.FirstName = employee.FirstName;
                    entity.LastName = employee.LastName;
                    entity.Salary = employee.Salary;
                    entity.Gender = employee.Gender;
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Employee with Id " + id + " is updated");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
