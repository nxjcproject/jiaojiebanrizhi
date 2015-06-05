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
        public static string GetShiftTime(string organizationId, string shift)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            string mySql = @"SELECT StartTime,EndTime
                                FROM system_ShiftDescription
                                WHERE OrganizationID=@OrganizationID
                                AND [Shifts]=@Shifts";
            SqlParameter[] parameters = { new SqlParameter("OrganizationID", organizationId), new SqlParameter("Shifts", shift) };
            DataTable table = factory.Query(mySql, parameters);
            string time = "{\"startTime\":\"" + table.Rows[0]["StartTime"].ToString().Trim() + "\",\"endTime\":\"" + table.Rows[0]["EndTime"].ToString().Trim() + "\"}";
            return time;
        }
    }
}
