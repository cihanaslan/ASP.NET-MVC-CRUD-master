using Microsoft.Extensions.Configuration;
using sonun_sonu;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using sonun_sonu.Models;


namespace sonun_sonu.Data
{
    public class CustomerRepository

    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Customer> GetAllCustomers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM MD_WORKERS", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Customer> customers = new List<Customer>();

                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                WM_ID = (int)reader["WM_ID"],
                                WM_NAME = (string)reader["WM_NAME"],
                                WM_SURNAME = (string)reader["WM_SURNAME"],
                                WM_CODE = (int)reader["WM_CODE"]
                            };

                            customers.Add(customer);
                        }

                        return customers;
                    }
                }
            }
        }
        public List<Customer> SearchCustomers(string keyword)
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM MD_WORKERS WHERE WM_NAME LIKE @Keyword OR WM_SURNAME LIKE @Keyword";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer
                            {
                                
                                WM_NAME = (string)reader["WM_NAME"],
                                WM_SURNAME = (string)reader["WM_SURNAME"]
                            };

                            if (reader["WM_ID"] != DBNull.Value)
                            {
                                int wmId;
                                if (int.TryParse(reader["WM_ID"].ToString(), out wmId))
                                {
                                    customer.WM_ID = wmId;
                                }
                                else
                                {
                                    // WM_CODE değeri geçerli bir int'e dönüştürülemediği durumu işleyebilirsiniz.
                                    // Hata işleme veya varsayılan değer atama gibi bir yaklaşım kullanabilirsiniz.
                                }
                            }

                            if (reader["WM_CODE"] != DBNull.Value)
                            {
                                int wmCode;
                                if (int.TryParse(reader["WM_CODE"].ToString(), out wmCode))
                                {
                                    customer.WM_CODE = wmCode;
                                }
                                else
                                {
                                    // WM_CODE değeri geçerli bir int'e dönüştürülemediği durumu işleyebilirsiniz.
                                    // Hata işleme veya varsayılan değer atama gibi bir yaklaşım kullanabilirsiniz.
                                }
                            }

                            customers.Add(customer);
                        }


                    }
                }
            }

            return customers;
        }
    }
}
