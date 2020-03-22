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
    public class ApplicantSkillRepository : IDataRepository<ApplicantSkillPoco>
    {

        protected readonly string _connstr;
        public ApplicantSkillRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params ApplicantSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (ApplicantSkillPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"INSERT INTO [dbo].[Applicant_Skills]( [Id], [Applicant], [Skill], [Skill_Level], [Start_Month], [Start_Year], [End_Month], [End_Year] )
                                        VALUES( @Id, @Applicant, @Skill, @Skill_Level, @Start_Month, @Start_Year, @End_Month, @End_Year )";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Skill", item.Skill);
                    comm.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
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
        public IList<ApplicantSkillPoco> GetAll(params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            //getall list on the poco
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Applicant], [Skill], [Skill_Level], [Start_Month], [Start_Year], [End_Month], [End_Year], [Time_Stamp]
                                    FROM [dbo].[Applicant_Skills]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                ApplicantSkillPoco[] appSkillPocos = new ApplicantSkillPoco[500];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    ApplicantSkillPoco appSkillPoco = new ApplicantSkillPoco();
                    appSkillPoco.Id = sqlReader.GetGuid(0);
                    appSkillPoco.Applicant = sqlReader.GetGuid(1);
                    appSkillPoco.Skill = sqlReader.GetString(2);
                    appSkillPoco.SkillLevel = sqlReader.GetString(3);
                    appSkillPoco.StartMonth = sqlReader.GetByte(4);
                    appSkillPoco.StartYear = sqlReader.GetInt32(5);
                    appSkillPoco.EndMonth = sqlReader.GetByte(6);
                    appSkillPoco.EndYear = sqlReader.GetInt32(7);
                    appSkillPoco.TimeStamp = (byte[])sqlReader[8];
                    appSkillPocos[index] = appSkillPoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return appSkillPocos.Where(a => a != null).ToList();
            }
        }

        public ApplicantSkillPoco GetSingle(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<ApplicantSkillPoco> appSkillPocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return appSkillPocos.Where(where).FirstOrDefault();
        }

        public void Remove(params ApplicantSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(ApplicantSkillPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Applicant_Skills]
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params ApplicantSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (ApplicantSkillPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Applicant_Skills]
                                        SET [Applicant] = @Applicant, [Skill] = @Skill, [Skill_Level] = @Skill_Level, [Start_Month] = @Start_Month, [Start_Year] = @Start_Year, [End_Month] = @End_Month, [End_Year] = @End_Year
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Applicant", item.Applicant);
                    comm.Parameters.AddWithValue("@Skill", item.Skill);
                    comm.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
                    comm.Parameters.AddWithValue("@Start_Month", item.StartMonth);
                    comm.Parameters.AddWithValue("@Start_Year", item.StartYear);
                    comm.Parameters.AddWithValue("@End_Month", item.EndMonth);
                    comm.Parameters.AddWithValue("@End_Year", item.EndYear);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        /* unimplemented interface methods for future iterations */
        public IList<ApplicantSkillPoco> GetList(Expression<Func<ApplicantSkillPoco, bool>> where, params Expression<Func<ApplicantSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
