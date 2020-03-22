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
    public class ApplicantJobApplicationRepository : IDataRepository<ApplicantJobApplicationPoco>
    {
        protected readonly string _connstr;
        public ApplicantJobApplicationRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantJobApplicationPoco[] items)
        {
            using(SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                foreach(ApplicantJobApplicationPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"INSERT INTO [dbo].[Applicant_Job_Applications]( [Id], [Applicant], [Job], [Application_Date] )
                                        VALUES( @Id, @Applicant, @Job, @Application_Date )";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Application_Date", item.ApplicationDate);
                    //sql open execute connection sequence
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public IList<ApplicantJobApplicationPoco> GetAll(params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            //getall list on the poco
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Applicant], [Job], [Application_Date], [Time_Stamp]
                                    FROM [dbo].[Applicant_Job_Applications]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                ApplicantJobApplicationPoco[] appJobApplicationPocos = new ApplicantJobApplicationPoco[1000];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    ApplicantJobApplicationPoco appJobApplicationPoco = new ApplicantJobApplicationPoco();
                    appJobApplicationPoco.Id = sqlReader.GetGuid(0);
                    appJobApplicationPoco.Applicant = sqlReader.GetGuid(1);
                    appJobApplicationPoco.Job = sqlReader.GetGuid(2);
                    appJobApplicationPoco.ApplicationDate = sqlReader.GetDateTime(3);
                    appJobApplicationPoco.TimeStamp = (byte[])sqlReader[4];
                    appJobApplicationPocos[index] = appJobApplicationPoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return appJobApplicationPocos.Where(a => a != null).ToList();
            }
        }

        public ApplicantJobApplicationPoco GetSingle(Expression<Func<ApplicantJobApplicationPoco, bool>> where, params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<ApplicantJobApplicationPoco> appJobApplicationPocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return appJobApplicationPocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantJobApplicationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(ApplicantJobApplicationPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Applicant_Job_Applications]
                                        WHERE Id = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantJobApplicationPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(ApplicantJobApplicationPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Applicant_Job_Applications]
                                        SET [Applicant] = @Applicant, [Job] = @Job, [Application_Date] = @Application_Date
                                        WHERE Id = @Id";
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Application_Date", item.ApplicationDate);
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        /* unimplemented interface methods for future iterations */
        public IList<ApplicantJobApplicationPoco> GetList(Expression<Func<ApplicantJobApplicationPoco, bool>> where, params Expression<Func<ApplicantJobApplicationPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }
    }

}
