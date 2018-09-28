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
        public IEnumerable<Employee> Get()
        {
           using(EmployeeDBEntities entities= new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }
        //Get ID
        public HttpResponseMessage Get(int id)
        {
            using(EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity= entities.Employees.FirstOrDefault(e => e.ID == id);
                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"Employee with ID "+id
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
            catch(Exception ex)
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
                   var entity= entities.Employees.FirstOrDefault((e => e.ID == id));

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
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

    }
}
