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

    
    public class CompanyJobRepository : IDataRepository<CompanyJobPoco>
    {
        protected readonly string _connstr;
        public CompanyJobRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params CompanyJobPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyJobPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"INSERT INTO [dbo].[Company_Jobs]( [Id], [Company], [Profile_Created], [Is_Inactive], [Is_Company_Hidden] )
                                        VALUES( @Id, @Company, @Profile_Created, @Is_Inactive, @Is_Company_Hidden )";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@Profile_Created", item.ProfileCreated);
                    comm.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                    comm.Parameters.AddWithValue("@Is_Company_Hidden", item.IsCompanyHidden);
                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        public IList<CompanyJobPoco> GetAll(params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Company], [Profile_Created], [Is_Inactive], [Is_Company_Hidden], [Time_Stamp]
                                    FROM [dbo].[Company_Jobs]";
                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                CompanyJobPoco[] companyJobPocos = new CompanyJobPoco[5000];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    CompanyJobPoco companyJobPoco = new CompanyJobPoco();
                    companyJobPoco.Id = sqlReader.GetGuid(0);
                    companyJobPoco.Company = sqlReader.GetGuid(1);
                    companyJobPoco.ProfileCreated = sqlReader.GetDateTime(2);
                    companyJobPoco.IsInactive = sqlReader.GetBoolean(3);
                    companyJobPoco.IsCompanyHidden = sqlReader.GetBoolean(4);
                    companyJobPoco.TimeStamp = (byte[])sqlReader[5];
                    companyJobPocos[index] = companyJobPoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return companyJobPocos.Where(a => a != null).ToList();
            }
        }
        public CompanyJobPoco GetSingle(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<CompanyJobPoco> companyJobPocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return companyJobPocos.Where(where).FirstOrDefault();
        }

        public void Remove(params CompanyJobPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyJobPoco item in items)
                {
                    comm.CommandText = @"DELETE FROM [dbo].[Company_Jobs]
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params CompanyJobPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (CompanyJobPoco item in items)
                {
                    comm.CommandText = @"UPDATE [dbo].[Company_Jobs]
                                        SET [Company] = @Company, [Profile_Created] = @Profile_Created, [Is_Inactive] = @Is_Inactive, [Is_Company_Hidden] = @Is_Company_Hidden
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Company", item.Company);
                    comm.Parameters.AddWithValue("@Profile_Created", item.ProfileCreated);
                    comm.Parameters.AddWithValue("@Is_Inactive", item.IsInactive);
                    comm.Parameters.AddWithValue("@Is_Company_Hidden", item.IsCompanyHidden);
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
        public IList<CompanyJobPoco> GetList(Expression<Func<CompanyJobPoco, bool>> where, params Expression<Func<CompanyJobPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
}
