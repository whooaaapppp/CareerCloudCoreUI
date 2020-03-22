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
    public class CompanyJobSkillRepository : IDataRepository<CompanyJobSkillPoco>
    {
        protected readonly string _connstr;
        public CompanyJobSkillRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyJobSkillPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Job_Skills]( [Id], [Job], [Skill], [Skill_Level], [Importance] )
                                        VALUES( @Id, @Job, @Skill, @Skill_Level, @Importance )";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Skill", item.Skill);
                    comm.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
                    comm.Parameters.AddWithValue("@Importance", item.Importance);
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public IList<CompanyJobSkillPoco> GetAll(params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                comm.CommandText = @"SELECT [Id], [Job], [Skill], [Skill_Level], [Importance], [Time_Stamp]
                                    FROM [dbo].[Company_Job_Skills]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyJobSkillPoco[] companyJobSkillPocos = new CompanyJobSkillPoco[10000];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    CompanyJobSkillPoco companyJobSkillPoco = new CompanyJobSkillPoco();
                    companyJobSkillPoco.Id = sqlReader.GetGuid(0);
                    companyJobSkillPoco.Job = sqlReader.GetGuid(1);
                    companyJobSkillPoco.Skill = sqlReader.GetString(2);
                    companyJobSkillPoco.SkillLevel = sqlReader.GetString(3);
                    companyJobSkillPoco.Importance = sqlReader.GetInt32(4);
                    companyJobSkillPoco.TimeStamp = (byte[])sqlReader[5];
                    companyJobSkillPocos[index] = companyJobSkillPoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return companyJobSkillPocos.Where(a => a != null).ToList();
            }
        }
        public CompanyJobSkillPoco GetSingle(Expression<Func<CompanyJobSkillPoco, bool>> where, params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<CompanyJobSkillPoco> companyJobSkillPocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return companyJobSkillPocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(CompanyJobSkillPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Job_Skills]
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyJobSkillPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyJobSkillPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Company_Job_Skills]
                                        SET [Job] = @Job, [Skill] = @Skill, [Skill_Level] = @Skill_Level, [Importance] = @Importance
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Job", item.Job);
                    comm.Parameters.AddWithValue("@Skill", item.Skill);
                    comm.Parameters.AddWithValue("@Skill_Level", item.SkillLevel);
                    comm.Parameters.AddWithValue("@Importance", item.Importance);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        /* unimplemented interface methods for future iterations */
        public IList<CompanyJobSkillPoco> GetList(Expression<Func<CompanyJobSkillPoco, bool>> where, params Expression<Func<CompanyJobSkillPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
        public void CallStoredProc(string name, params Tuple<string, string>[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
