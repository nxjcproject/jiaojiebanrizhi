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
    public class StaffService
    {
        /// <summary>
        /// 按组织机构ID获取班组
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static DataTable GetWorkingTeamByOrganizationId(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            Query query = new Query("system_WorkingTeam");
            query.AddCriterion("OrganizationID", organizationId, SqlServerDataAdapter.Infrastruction.CriteriaOperator.Equal);
            query.AddOrderByClause(new SqlServerDataAdapter.Infrastruction.OrderByClause("Name", false));

            return factory.Query(query);
        }

        /// <summary>
        /// 按班组ID获取负责人
        /// </summary>
        /// <param name="workingTeamName"></param>
        /// <returns></returns>
        public static DataTable GetChargeManByWorkingTeamName(string workingTeamName)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT system_StaffInfo.StaffInfoID, system_StaffInfo.Name FROM system_StaffInfo INNER JOIN system_WorkingTeam ON system_StaffInfo.StaffInfoID = system_WorkingTeam.ChargeManID WHERE (system_WorkingTeam.Name = '" + workingTeamName + "')";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <returns></returns>
        public static DataTable GetStaffInfo(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            Query query = new Query("system_StaffInfo");
            query.AddCriterion("OrganizationID", organizationId, SqlServerDataAdapter.Infrastruction.CriteriaOperator.Equal);

            return factory.Query(query);
        }
    }

}
