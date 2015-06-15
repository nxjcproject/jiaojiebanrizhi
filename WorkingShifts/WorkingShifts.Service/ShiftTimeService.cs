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
    public class ShiftTimeService
    {
        /// <summary>
        /// 按照组织机构ID（分厂）获取时间班计划
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static string GetShiftSchedule(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            string sql = @"SELECT *
                             FROM [system_ShiftDescription]
                            WHERE [OrganizationID] = @organizationID
                         ORDER BY [StartTime]";
            SqlParameter[] parameters = { new SqlParameter("organizationID", organizationId) };
            DataTable table = factory.Query(sql, parameters);

            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (DataRow dr in table.Rows)
            {
                sb.Append("\"");
                sb.Append(dr["Shifts"].ToString().Trim());
                sb.Append("\":{\"startTime\":\"");
                sb.Append(dr["StartTime"].ToString().Trim());
                sb.Append("\",\"endTime\":\"");
                sb.Append(dr["EndTime"].ToString().Trim());
                sb.Append("\"},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
    }
}