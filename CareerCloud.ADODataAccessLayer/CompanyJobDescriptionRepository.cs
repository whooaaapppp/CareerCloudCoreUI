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
    public class CompanyJobDescriptionRepository : IDataRepository<CompanyJobDescriptionPoco>
    {
        protected readonly string _connstr;
        public CompanyJobDescriptionRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(CompanyJobDescriptionPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Jobs_Descriptions]( [Id], [Job], [Job_Name], [Job_Descriptions] )
                                        VALUES( @Id, @Job, @Job_Name, @Job_Descriptions )";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Job_Name", item.JobName);
                    comm.Parameters.AddWithValue("@Job_Descriptions", item.JobDescriptions);
                    
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public IList<CompanyJobDescriptionPoco> GetAll(params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Job], [Job_Name], [Job_Descriptions], [Time_Stamp]
                                    FROM [dbo].[Company_Jobs_Descriptions]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyJobDescriptionPoco[] companyJobDescriptionPocos = new CompanyJobDescriptionPoco[5000];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    CompanyJobDescriptionPoco companyJobDescriptionPoco = new CompanyJobDescriptionPoco();
                    companyJobDescriptionPoco.Id = sqlReader.GetGuid(0);
                    companyJobDescriptionPoco.Job = sqlReader.GetGuid(1);
                    companyJobDescriptionPoco.JobName = sqlReader.GetString(2);
                    companyJobDescriptionPoco.JobDescriptions = sqlReader.GetString(3);
                    companyJobDescriptionPoco.TimeStamp = (byte[])sqlReader[4];
                    companyJobDescriptionPocos[index] = companyJobDescriptionPoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return companyJobDescriptionPocos.Where(a => a != null).ToList();
            }
        }

        public CompanyJobDescriptionPoco GetSingle(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<CompanyJobDescriptionPoco> companyJobDescriptionPocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return companyJobDescriptionPocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyJobDescriptionPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Jobs_Descriptions]
                                         WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyJobDescriptionPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyJobDescriptionPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"UPDATE [dbo].[Company_Jobs_Descriptions]
                                        SET [Job] = @Job, [Job_Name] = @Job_Name, [Job_Descriptions] = @Job_Descriptions
                                        WHERE [Id] = @Id";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Job_Name", item.JobName);
                    comm.Parameters.AddWithValue("@Job_Descriptions", item.JobDescriptions);
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
        public IList<CompanyJobDescriptionPoco> GetList(Expression<Func<CompanyJobDescriptionPoco, bool>> where, params Expression<Func<CompanyJobDescriptionPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
    
}
