using SqlServerDataAdapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkingShifts.Infrastructure.Configuration;

namespace WorkingShifts.Service
{
    public class DCSSystemServcie
    {
        /// <summary>
        /// 根据组织机构ID（分厂）获取DCS系统
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static DataTable GetDCSSystemByOrganizationId(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT * 
                                        FROM [dbo].[system_Organization_Instrumentation] 
                                        WHERE LEN([LevelCode]) = 7 AND [LevelCode] LIKE (
                                            (SELECT [LevelCode] FROM [dbo].[system_Organization_Instrumentation] 
                                            WHERE [OrganizationID] = @organizationId) 
                                            + '%' 
                                        )";
                command.Parameters.Add(new SqlParameter("organizationId", organizationId));

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
            }

            return ds.Tables[0];
        }
    }
}
