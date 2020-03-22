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
    public class CompanyJobEducationRepository : IDataRepository<CompanyJobEducationPoco>
    {
        protected readonly string _connstr;
        public CompanyJobEducationRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(CompanyJobEducationPoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Job_Educations]( [Id], [Job], [Major], [Importance] )
                                        VALUES( @Id, @Job, @Major, @Importance )";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Major", item.Major);
                    comm.Parameters.AddWithValue("@Importance", item.Importance);
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public IList<CompanyJobEducationPoco> GetAll(params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Job], [Major], [Importance], [Time_Stamp]
                                    FROM [dbo].[Company_Job_Educations]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyJobEducationPoco[] companyJobEducationPocos = new CompanyJobEducationPoco[10000];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    CompanyJobEducationPoco companyJobEducationPoco = new CompanyJobEducationPoco();
                    companyJobEducationPoco.Id = sqlReader.GetGuid(0);
                    companyJobEducationPoco.Job = sqlReader.GetGuid(1);
                    companyJobEducationPoco.Major = sqlReader.GetString(2);
                    companyJobEducationPoco.Importance = sqlReader.GetInt16(3);
                    companyJobEducationPoco.TimeStamp = (byte[])sqlReader[4];
                    companyJobEducationPocos[index] = companyJobEducationPoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return companyJobEducationPocos.Where(a => a != null).ToList();
            }
        }
        public CompanyJobEducationPoco GetSingle(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<CompanyJobEducationPoco> companyJobEducationPocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return companyJobEducationPocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(CompanyJobEducationPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Job_Educations]
                                    WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();

                }
            }
        }

        public void Update(params CompanyJobEducationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyJobEducationPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Company_Job_Educations]
                                        SET [Job] = @Job, [Major] = @Major, [Importance] = @Importance
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Major", item.Major);
                    comm.Parameters.AddWithValue("@Importance", item.Importance);
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
        public IList<CompanyJobEducationPoco> GetList(Expression<Func<CompanyJobEducationPoco, bool>> where, params Expression<Func<CompanyJobEducationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

    }
}
