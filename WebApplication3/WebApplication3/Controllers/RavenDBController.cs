using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;
namespace WebApplication3.Controllers 
{
    public class RavenDBController : Controller
    {
        //Lấy Dữ Liệu Từ DataBase hiển thị ra màn hình
        public ActionResult Index()
        {
            //kết nối DB
            List<Company> model = new List<Company>();
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "testDB"
            })
            {
                documentstore.Initialize();
                using (var session = documentstore.OpenSession())
                {
                    model = session.Query<Company>().ToList();
                }
            
            }
                return View(model);
        }
       //Tìm Kiếm Dữ Liệu từ DataBase
       public ActionResult List(string searchBy,string search)
        {
           
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "testDB"
            })
            //Lưu trữ Document/Data
            {
                documentstore.Initialize();

                using (var session = documentstore.OpenSession())
                {
                    if (searchBy == "Name")
                    {
                        return View(session.Query<Company>().Where(x => x.Name.StartsWith(search) || search == null).ToList());
                    }
                    else
                    {
                        return View(session.Query<Company>().Where(x => x.Id.StartsWith(search)|| search == null).ToList());
                    }
                }
            }
            

        }
        [HttpGet]
        //Tạo View Thhêm Mới
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        // Gửi Dữ Liệu Tạo Thành Viên Mới
        public ActionResult Create(Company model)
        {
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "testDB"
            })
            {
                documentstore.Initialize();
                using (var session = documentstore.OpenSession())
                {
                    session.Store(model);
                    session.SaveChanges();
                    
                }

            }
            return View(model);
        }
        //Chi Tiết Thông Tin Nhân Viên
        [HttpGet]
        public ActionResult Details(int Id)
        {
            Company company = new Company();
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "testDB"
            })
            {
                documentstore.Initialize();
                using (var session = documentstore.OpenSession())
                {
                    company = session.Load<Company>(Id.ToString());
                }
            }
                return View(company);

        }
        //Lấy Dữ Liệu Để Chỉnh Sữa
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            Company company = new Company();
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "testDB"
            })
            {
                documentstore.Initialize();
                using (var session = documentstore.OpenSession())
                {
                    company = session.Load<Company>(Id.ToString());
                }
            }
            return View(company);
        }
        //Chỉnh Sữa Dữ Liệu
        [HttpPost]
        public ActionResult Edit(Company model)
        {
            Company result = new Company();
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "testDB"
            })
            {
                documentstore.Initialize();
                using (var session = documentstore.OpenSession())
                {
                    result = session.Load<Company>(model.Id);
                    if (result != null)
                    {
                        result.Id = model.Id;
                        result.Name = model.Name;
                        result.DOB = model.DOB;
                        result.Sex = model.Sex;
                        result.Role = model.Role;
                        result.Salary = model.Salary;
                        result.Address = model.Address;
                        result.Phone = model.Phone;
                        result.CMND = model.CMND;
                        session.SaveChanges();
                    }
                }
            }
            return Redirect("/RavenDB/index");
        }
        //Xoá Dữ Liệu
        public ActionResult Delete(int Id)
        {
            Company company = new Company();
            using (var documentstore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "testDB"
            })
            {
                documentstore.Initialize();
                using (var session = documentstore.OpenSession())
                {
                    company = session.Load<Company>(Id.ToString());
                    session.Delete<Company>(company);
                    session.SaveChanges();
                }
            }
            return Redirect("/RavenDB/index");
        }
    }
}