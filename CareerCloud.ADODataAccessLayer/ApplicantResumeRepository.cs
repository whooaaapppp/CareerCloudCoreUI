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
    public class ApplicantResumeRepository : IDataRepository<ApplicantResumePoco>
    {
        protected readonly string _connstr;
        public ApplicantResumeRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantResumePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (ApplicantResumePoco item in items)
                {
                    comm.CommandText = @"INSERT INTO [dbo].[Applicant_Resumes]( [Id], [Applicant], [Resume], [Last_Updated] )
                                        VALUES( @Id, @Applicant, @Resume, @Last_Updated )";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Resume", item.Resume);
                    comm.Parameters.AddWithValue("@Last_Updated", item.LastUpdated);
                    //sql open execute connection sequence
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public IList<ApplicantResumePoco> GetAll(params Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            //getall list on the poco
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Applicant], [Resume], [Last_Updated]
                                    FROM [dbo].[Applicant_Resumes]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                ApplicantResumePoco[] appResumePocos = new ApplicantResumePoco[100];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    ApplicantResumePoco appResumePoco = new ApplicantResumePoco();
                    appResumePoco.Id = sqlReader.GetGuid(0);
                    appResumePoco.Applicant = sqlReader.GetGuid(1);
                    appResumePoco.Resume = sqlReader.GetString(2);
                    //handling nullables
                    if(!sqlReader.IsDBNull(3))
                    appResumePoco.LastUpdated = sqlReader.GetDateTime(3);
                    appResumePocos[index] = appResumePoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return appResumePocos.Where(a => a != null).ToList();
            } 
        }

        public ApplicantResumePoco GetSingle(Expression<Func<ApplicantResumePoco, bool>> where, params Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<ApplicantResumePoco> appResumePocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return appResumePocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantResumePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(ApplicantResumePoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Applicant_Resumes]
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantResumePoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(ApplicantResumePoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Applicant_Resumes]
                                        SET [Applicant] = @Applicant, [Resume] = @Resume, [Last_Updated] = @Last_Updated
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Resume", item.Resume);
                    comm.Parameters.AddWithValue("@Last_Updated", item.LastUpdated);
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

        public IList<ApplicantResumePoco> GetList(Expression<Func<ApplicantResumePoco, bool>> where, params Expression<Func<ApplicantResumePoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
}
