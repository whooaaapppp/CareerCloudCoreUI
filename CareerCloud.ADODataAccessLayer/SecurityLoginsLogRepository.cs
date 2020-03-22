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
    public class SecurityLoginsLogRepository : IDataRepository<SecurityLoginsLogPoco>
    {
        protected readonly string _connstr;
        public SecurityLoginsLogRepository()
        {
            //create Config Builder
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            _connstr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
        }

        public void Add(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach(SecurityLoginsLogPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"INSERT INTO [dbo].[Security_Logins_Log]( [Id], [Login], [Source_IP], [Logon_Date], [Is_Succesful] )
                                        VALUES( @Id, @Login, @Source_IP, @Logon_Date, @Is_Succesful)";

                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Source_IP", item.SourceIP);
                    comm.Parameters.AddWithValue("@Logon_Date", item.LogonDate);
                    comm.Parameters.AddWithValue("@Is_Succesful", item.IsSuccesful);

                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
        
        public IList<SecurityLoginsLogPoco> GetAll(params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;
                comm.CommandText = @"SELECT [Id], [Login], [Source_IP], [Logon_Date], [Is_Succesful]
                                    FROM [dbo].[Security_Logins_Log]";

                connection.Open();
                int index = 0;
                SqlDataReader sqlReader = comm.ExecuteReader();
                SecurityLoginsLogPoco[] securityLoginsLogPocos = new SecurityLoginsLogPoco[10000];
                //while sqlreader has something to read
                while (sqlReader.Read())
                {
                    SecurityLoginsLogPoco securityLoginsLogPoco = new SecurityLoginsLogPoco();
                    securityLoginsLogPoco.Id = sqlReader.GetGuid(0);
                    securityLoginsLogPoco.Login = sqlReader.GetGuid(1);
                    securityLoginsLogPoco.SourceIP = sqlReader.GetString(2);
                    securityLoginsLogPoco.LogonDate = sqlReader.GetDateTime(3);
                    securityLoginsLogPoco.IsSuccesful = sqlReader.GetBoolean(4);
                    securityLoginsLogPocos[index] = securityLoginsLogPoco;
                    index++;
                }
                connection.Close();
                //return only non-null poco object -> a and pass it to list
                return securityLoginsLogPocos.Where(a => a != null).ToList();
            }
        }
        public SecurityLoginsLogPoco GetSingle(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            /* https://docs.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=netframework-4.8 */
            IQueryable<SecurityLoginsLogPoco> securityLoginsLogPocos = GetAll().AsQueryable();
            /* return first element of a sequence or a default value if the seq contains no elements that satisfy the where predicate */
            return securityLoginsLogPocos.Where(where).FirstOrDefault();
        }

        public void Remove(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (SecurityLoginsLogPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"DELETE FROM [dbo].[Security_Logins_Log]
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);

                    //rows affected
                    connection.Open();
                    int rowAffected = comm.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Update(params SecurityLoginsLogPoco[] items)
        {
            using (SqlConnection connection = new SqlConnection(_connstr))
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = connection;

                foreach (SecurityLoginsLogPoco item in items)
                {
                    //prepping the sql query
                    comm.CommandText = @"UPDATE [dbo].[Security_Logins_Log]
                                        SET [Login] = @Login, [Source_IP] = @Source_IP, [Logon_Date] = @Logon_Date, [Is_Succesful] = @Is_Succesful
                                        WHERE [Id] = @Id";
                    comm.Parameters.AddWithValue("@Id", item.Id);
                    comm.Parameters.AddWithValue("@Login", item.Login);
                    comm.Parameters.AddWithValue("@Source_IP", item.SourceIP);
                    comm.Parameters.AddWithValue("@Logon_Date", item.LogonDate);
                    comm.Parameters.AddWithValue("@Is_Succesful", item.IsSuccesful);

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
        public IList<SecurityLoginsLogPoco> GetList(Expression<Func<SecurityLoginsLogPoco, bool>> where, params Expression<Func<SecurityLoginsLogPoco, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }
    }
}
