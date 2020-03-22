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
    
    public class ApplicantWorkHistoryRepository : IDataRepository<ApplicantWorkHistoryPoco>
    {
        protected readonly string _connstr;
        public ApplicantWorkHistoryRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantWorkHistoryPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(ApplicantWorkHistoryPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"INSERT INTO [dbo].[Applicant_Work_History]( [Id], [Applicant], [Company_Name], [Country_Code], [Location], [Job_Title], [Job_Description], [Start_Month], [Start_Year], [End_Month], [End_Year] )
                                        VALUES( @Id, @Applicant, @Company_Name, @Country_Code, @Location, @Job_Title, @Job_Description, @Start_Month, @Start_Year, @End_Month, @End_Year )";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                    comm.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                    comm.Parameters.AddWithValue("@Location", item.Location);
                    comm.Parameters.AddWithValue("@Job_Title", item.JobTitle);
                    comm.Parameters.AddWithValue("@Job_Description", item.JobDescription);
                    comm.Parameters.AddWithValue("@Start_Month", item.StartMonth);
                    comm.Parameters.AddWithValue("@Start_Year", item.StartYear);
                    comm.Parameters.AddWithValue("@End_Month", item.EndMonth);
                    comm.Parameters.AddWithValue("@End_Year", item.EndYear);
                    //extract rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public IList<ApplicantWorkHistoryPoco> GetAll(params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Applicant], [Company_Name], [Country_Code], [Location], [Job_Title], [Job_Description], [Start_Month], [Start_Year], [End_Month], [End_Year], [Time_Stamp]
                                    FROM [dbo].[Applicant_Work_History]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                ApplicantWorkHistoryPoco[] appWorkHistoryPocos = new ApplicantWorkHistoryPoco[1000];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    ApplicantWorkHistoryPoco appWorkHistoryPoco = new ApplicantWorkHistoryPoco();
                    appWorkHistoryPoco.Id = sqlReader.GetGuid(0);
                    appWorkHistoryPoco.Applicant = sqlReader.GetGuid(1);
                    appWorkHistoryPoco.CompanyName = sqlReader.GetString(2);
                    appWorkHistoryPoco.CountryCode = sqlReader.GetString(3);
                    appWorkHistoryPoco.Location = sqlReader.GetString(4);
                    appWorkHistoryPoco.JobTitle = sqlReader.GetString(5);
                    appWorkHistoryPoco.JobDescription = sqlReader.GetString(6);
                    appWorkHistoryPoco.StartMonth = sqlReader.GetInt16(7);
                    appWorkHistoryPoco.StartYear = sqlReader.GetInt32(8);
                    appWorkHistoryPoco.EndMonth = sqlReader.GetInt16(9);
                    appWorkHistoryPoco.EndYear = sqlReader.GetInt32(10);
                    appWorkHistoryPoco.TimeStamp = (byte[])sqlReader[11];
                    appWorkHistoryPocos[index] = appWorkHistoryPoco;
                    index++;

                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return appWorkHistoryPocos.Where(a => a != null).ToList();
            }
        }
        public ApplicantWorkHistoryPoco GetSingle(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<ApplicantWorkHistoryPoco> appWorkHistoryPocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return appWorkHistoryPocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantWorkHistoryPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (ApplicantWorkHistoryPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Applicant_Work_History]
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantWorkHistoryPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (ApplicantWorkHistoryPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"UPDATE [dbo].[Applicant_Work_History]
                                        SET [Applicant] = @Applicant, [Company_Name] = @Company_Name, [Country_Code] = @Country_Code, [Location] = @Location, [Job_Title] = @Job_Title, [Job_Description] = @Job_Description, [Start_Month] = @Start_Month, [Start_Year] = @Start_Year, [End_Month] = @End_Month, [End_Year] = @End_Year
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Company_Name", item.CompanyName);
                    comm.Parameters.AddWithValue("@Country_Code", item.CountryCode);
                    comm.Parameters.AddWithValue("@Location", item.Location);
                    comm.Parameters.AddWithValue("@Job_Title", item.JobTitle);
                    comm.Parameters.AddWithValue("@Job_Description", item.JobDescription);
                    comm.Parameters.AddWithValue("@Start_Month", item.StartMonth);
                    comm.Parameters.AddWithValue("@Start_Year", item.StartYear);
                    comm.Parameters.AddWithValue("@End_Month", item.EndMonth);
                    comm.Parameters.AddWithValue("@End_Year", item.EndYear);
                    //extract rows affected
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
        public IList<ApplicantWorkHistoryPoco> GetList(Expression<Func<ApplicantWorkHistoryPoco, bool>> where, params Expression<Func<ApplicantWorkHistoryPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
}
