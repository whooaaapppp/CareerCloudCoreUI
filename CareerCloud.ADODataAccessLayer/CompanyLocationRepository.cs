using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace CareerCloud.ADODataAccessLayer
{

    
    public class CompanyLocationRepository : IDataRepository<CompanyLocationPoco>
    {
        protected readonly string _connstr;
        public CompanyLocationRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyLocationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(CompanyLocationPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Locations]( [Id], [Company], [Country_Code], [State_Province_Code], [Street_Address], [City_Town], [Zip_Postal_Code] )
                                        VALUES( @Id, @Company, @Country_Code, @State_Province_Code, @Street_Address, @City_Town, @Zip_Postal_Code )";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                    comm.Parameters.AddWithValue("@State_Province_Code", item.Province);
                    comm.Parameters.AddWithValue("@Street_Address", item.Street);
                    comm.Parameters.AddWithValue("@City_Town", item.City);
                    comm.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);
                    
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public IList<CompanyLocationPoco> GetAll(params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Company], [Country_Code], [State_Province_Code], [Street_Address], [City_Town], [Zip_Postal_Code], [Time_Stamp]
                                    FROM [dbo].[Company_Locations]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyLocationPoco[] companyLocationPocos = new CompanyLocationPoco[1000];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    CompanyLocationPoco companyLocationPoco = new CompanyLocationPoco();
                    companyLocationPoco.Id = sqlReader.GetGuid(0);
                    companyLocationPoco.Company = sqlReader.GetGuid(1);
                    companyLocationPoco.CountryCode = sqlReader.GetString(2);
                    //handling nullable var entries to empty strings
                    companyLocationPoco.Province = sqlReader.IsDBNull(3) ? String.Empty : sqlReader.GetString(3);
                    companyLocationPoco.Street = sqlReader.IsDBNull(4) ? String.Empty : sqlReader.GetString(4);
                    companyLocationPoco.City = sqlReader.IsDBNull(5) ? String.Empty : sqlReader.GetString(5);
                    companyLocationPoco.PostalCode = sqlReader.IsDBNull(6) ? String.Empty : sqlReader.GetString(6);
                    companyLocationPoco.TimeStamp = (byte[])sqlReader[7];
                    companyLocationPocos[index] = companyLocationPoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return companyLocationPocos.Where(a => a != null).ToList();
            }
        }
        public CompanyLocationPoco GetSingle(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<CompanyLocationPoco> companyLocationPocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return companyLocationPocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyLocationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyLocationPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Locations]
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyLocationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyLocationPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"UPDATE [dbo].[Company_Locations]
                                        SET [Company] = @Company, [Country_Code] = @Country_Code, [State_Province_Code] = @State_Province_Code, [Street_Address] = @Street_Address, [City_Town] = @City_Town, [Zip_Postal_Code] = @Zip_Postal_Code
                                        WHERE [Id] = @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                    comm.Parameters.AddWithValue("@State_Province_Code", item.Province);
                    comm.Parameters.AddWithValue("@Street_Address", item.Street);
                    comm.Parameters.AddWithValue("@City_Town", item.City);
                    comm.Parameters.AddWithValue("@Zip_Postal_Code", item.PostalCode);
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        /* unimplemented interface methods for future iterations */
        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }
        public IList<CompanyLocationPoco> GetList(Expression<Func<CompanyLocationPoco, bool>> where, params Expression<Func<CompanyLocationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
}
