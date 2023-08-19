using Microsoft.AspNetCore.Mvc;
using sonun_sonu.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using sonun_sonu.Data;
using System;

namespace sonun_sonu.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO MD_WORKERS (WM_ID, WM_NAME, WM_SURNAME, WM_CODE) VALUES (@WM_ID, @WM_NAME, @WM_SURNAME, @WM_CODE)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@WM_ID", customer.WM_ID);
                        command.Parameters.AddWithValue("@WM_NAME", customer.WM_NAME);
                        command.Parameters.AddWithValue("@WM_SURNAME", customer.WM_SURNAME);
                        command.Parameters.AddWithValue("@WM_CODE", customer.WM_CODE);

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            // Hata işleme kodunu buraya ekleyin (isteğe bağlı)
                            // Örneğin, hata mesajını yakalayabilir veya loglayabilirsiniz.

                            return RedirectToAction("Index");
                        }
                    }
                }

                return RedirectToAction("Index");
            }

            return View(customer);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM MD_WORKERS WHERE WM_ID = @WM_ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WM_ID", id);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeCustomerData(string txtWMID, string txtWMCode, string txtWMName, string txtWMSurname)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE MD_WORKERS SET WM_CODE = @WMCode, WM_NAME = @WMName, WM_SURNAME = @WMSurname WHERE WM_ID = @WMID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WMCode", txtWMCode);
                    command.Parameters.AddWithValue("@WMName", txtWMName);
                    command.Parameters.AddWithValue("@WMSurname", txtWMSurname);
                    command.Parameters.AddWithValue("@WMID", txtWMID);

                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM MD_WORKERS WHERE WM_ID = @WM_ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WM_ID", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int wmId;
                            if (int.TryParse(reader["WM_ID"].ToString(), out wmId))
                            {
                                int wmCode;
                                if (int.TryParse(reader["WM_CODE"].ToString(), out wmCode))
                                {
                                    Customer customer = new Customer
                                    {
                                        WM_ID = wmId,
                                        WM_CODE = wmCode,
                                        WM_NAME = (string)reader["WM_NAME"],
                                        WM_SURNAME = (string)reader["WM_SURNAME"],
                                    };

                                    return View(customer);
                                }
                                else
                                {
                                    // WM_CODE değeri geçerli bir int'e dönüştürülemediği durumu işleyebilirsiniz.
                                    // Hata işleme veya varsayılan değer atama gibi bir yaklaşım kullanabilirsiniz.
                                }
                            }
                            else
                            {
                                // WM_ID değeri geçerli bir int'e dönüştürülemediği durumu işleyebilirsiniz.
                                // Hata işleme veya varsayılan değer atama gibi bir yaklaşım kullanabilirsiniz.
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(Customer customer)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "UPDATE MD_WORKERS SET WM_NAME = @WM_NAME, WM_SURNAME = @WM_SURNAME, WM_CODE = @WM_CODE WHERE WM_ID = @WM_ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@WM_ID", customer.WM_ID);
                        command.Parameters.AddWithValue("@WM_NAME", customer.WM_NAME);
                        command.Parameters.AddWithValue("@WM_SURNAME", customer.WM_SURNAME);
                        command.Parameters.AddWithValue("@WM_CODE", customer.WM_CODE);

                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            // Hata işleme kodunu buraya ekleyin (isteğe bağlı)
                            // Örneğin, hata mesajını yakalayabilir veya loglayabilirsiniz.
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index(string keyword)
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM MD_WORKERS WHERE LOWER(CAST(WM_NAME AS NVARCHAR(MAX))) LIKE LOWER(@Keyword) OR LOWER(CAST(WM_SURNAME AS NVARCHAR(MAX))) LIKE LOWER(@Keyword) OR CAST(WM_ID AS NVARCHAR(MAX)) LIKE @Keyword OR CAST(WM_CODE AS NVARCHAR(MAX)) LIKE @Keyword";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int wmId;
                            if (int.TryParse(reader["WM_ID"].ToString(), out wmId))
                            {
                                int wmCode;
                                if (int.TryParse(reader["WM_CODE"].ToString(), out wmCode))
                                {
                                    Customer customer = new Customer
                                    {
                                        WM_ID = wmId,
                                        WM_CODE = wmCode,
                                        WM_NAME = reader["WM_NAME"].ToString(),
                                        WM_SURNAME = reader["WM_SURNAME"].ToString(),
                                    };

                                    customers.Add(customer);
                                }
                                else
                                {
                                    // WM_CODE değeri geçerli bir int'e dönüştürülemediği durumu işleyebilirsiniz.
                                    // Hata işleme veya varsayılan değer atama gibi bir yaklaşım kullanabilirsiniz.
                                }
                            }
                            else
                            {
                                // WM_ID değeri geçerli bir int'e dönüştürülemediği durumu işleyebilirsiniz.
                                // Hata işleme veya varsayılan değer atama gibi bir yaklaşım kullanabilirsiniz.
                            }
                        }
                    }
                }
            }

            return View(customers);
        }
    }
}
