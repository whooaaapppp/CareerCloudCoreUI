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
    public class SystemLanguageCodeRepository : IDataRepository<SystemLanguageCodePoco>
    {
        protected readonly string _connstr;
        public SystemLanguageCodeRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SystemLanguageCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(SystemLanguageCodePoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[System_Language_Codes]( [LanguageID], [Name], [Native_Name] )
                                        VALUES( @LanguageID, @Name, @Native_Name )";
                    comm.Parameters.AddWithValue("@LanguageID", item.LanguageID);
                    comm.Parameters.AddWithValue("@Name", item.Name);
                    comm.Parameters.AddWithValue("@Native_Name", item.NativeName);
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        

        public IList<SystemLanguageCodePoco> GetAll(params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [LanguageID], [Name], [Native_Name]
                FROM [dbo].[System_Language_Codes]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                SystemLanguageCodePoco[] systemLanguageCodePocos = new SystemLanguageCodePoco[100];
                //while sql reader has something to read
                while (sqlReader.Read())
                {
                    SystemLanguageCodePoco systemLanguageCodePoco = new SystemLanguageCodePoco();
                    systemLanguageCodePoco.LanguageID = sqlReader.GetString(0);
                    systemLanguageCodePoco.Name = sqlReader.GetString(1);
                    systemLanguageCodePoco.NativeName = sqlReader.GetString(2);
                    systemLanguageCodePocos[index] = systemLanguageCodePoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return systemLanguageCodePocos.Where(a => a != null).ToList();

            }
        }
        public SystemLanguageCodePoco GetSingle(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<SystemLanguageCodePoco> systemLanguageCodePocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return systemLanguageCodePocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SystemLanguageCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SystemLanguageCodePoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[System_Language_Codes]
                                        WHERE [LanguageID] = @LanguageID";
                    comm.Parameters.AddWithValue("@LanguageID", item.LanguageID);

                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SystemLanguageCodePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach (SystemLanguageCodePoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[System_Language_Codes]
                                       SET [Name] = @Name
                                          ,[Native_Name] = @Native_Name
                                       WHERE [LanguageID] = @LanguageID";
                    comm.Parameters.AddWithValue("@LanguageID", item.LanguageID);
                    comm.Parameters.AddWithValue("@Name", item.Name);
                    comm.Parameters.AddWithValue("@Native_Name", item.NativeName);
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
        public IList<SystemLanguageCodePoco> GetList(Expression<Func<SystemLanguageCodePoco, bool>> where, params Expression<Func<SystemLanguageCodePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
}
