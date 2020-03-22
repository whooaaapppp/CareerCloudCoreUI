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
    
    public class SystemCountryCodeRepository : IDataRepository<SystemCountryCodePoco>
    {
        protected readonly string _connstr;
        public SystemCountryCodeRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SystemCountryCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(SystemCountryCodePoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[System_Country_Codes]( [Code], [Name] )
                                        VALUES( @Code, @Name )";
                    comm.Parameters.AddWithValue("@Code", item.Code);
                    comm.Parameters.AddWithValue("@Name", item.Name);
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public IList<SystemCountryCodePoco> GetAll(params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Code], [Name]
                                    FROM [dbo].[System_Country_Codes]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                SystemCountryCodePoco[] systemCountryCodePocos = new SystemCountryCodePoco[100];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    SystemCountryCodePoco systemCountryCodePoco = new SystemCountryCodePoco();
                    systemCountryCodePoco.Code = sqlReader.GetString(0);
                    systemCountryCodePoco.Name = sqlReader.GetString(1);
                    systemCountryCodePocos[index] = systemCountryCodePoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return systemCountryCodePocos.Where(a => a != null).ToList();

            }
        }

        public SystemCountryCodePoco GetSingle(Expression<Func<SystemCountryCodePoco, bool>> where, params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<SystemCountryCodePoco> systemCountryCodePocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return systemCountryCodePocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SystemCountryCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (SystemCountryCodePoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[System_Country_Codes]
                                        WHERE [Code] = @Code";
                    comm.Parameters.AddWithValue("@Code", item.Code);
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SystemCountryCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (SystemCountryCodePoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[System_Country_Codes]
                                        SET [Name] = @Name
                                        WHERE [Code] = @Code";
                    comm.Parameters.AddWithValue("@Code", item.Code);
                    comm.Parameters.AddWithValue("@Name", item.Name);
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
        public IList<SystemCountryCodePoco> GetList(Expression<Func<SystemCountryCodePoco, bool>> where, params Expression<Func<SystemCountryCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
}
