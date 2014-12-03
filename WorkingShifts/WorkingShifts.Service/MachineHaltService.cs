using EasyUIJsonParser;
using SqlServerDataAdapter;
using SqlServerDataAdapter.Infrastruction;
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
    public class MachineHaltService
    {
        /// <summary>
        /// 获取停机原因列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMachineHaltReasons()
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            Query query = new Query("system_MachineHaltReason");

            return factory.Query(query);
        }

        /// <summary>
        /// 获取停机记录
        /// </summary>
        /// <param name="organizationId">组织机构ID</param>
        /// <param name="workingTeamShiftLogID">交接班日志ID</param>
        /// <returns></returns>
        public static DataTable GetMachineHaltLog(string organizationId, string workingTeamShiftLogID = "")
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            Query query = new Query("shift_MachineHaltLog");
            query.AddCriterion("OrganizationID", organizationId, CriteriaOperator.Equal);

            if (string.IsNullOrWhiteSpace(workingTeamShiftLogID))
                query.AddCriterion("WorkingTeamShiftLogID", null, CriteriaOperator.NULL);
            else
                query.AddCriterion("WorkingTeamShiftLogID", workingTeamShiftLogID, CriteriaOperator.Equal);

            DataTable dt = factory.Query(query);
            return dt;
        }

        /// <summary>
        /// 更新指定停机记录
        /// </summary>
        /// <param name="machineHaltLogID">指定停机记录ID</param>
        /// <param name="workingTeamShiftLogID"></param>
        /// <param name="reasonId"></param>
        /// <param name="reasonText"></param>
        /// <param name="remarks"></param>
        public static void UpdateMachineHaltLog(string machineHaltLogID, string workingTeamShiftLogID, string reasonId, string reasonText, string remarks)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"UPDATE [dbo].[shift_MachineHaltLog]
                                           SET [WorkingTeamShiftLogID] = @workingTeamShiftLogID
                                              ,[ReasonID] = @reasonID
                                              ,[ReasonText] = @reasonText
                                              ,[Remarks] = @remarks
                                         WHERE [MachineHaltLogID] = @machineHaltLogID";

                command.Parameters.Add(new SqlParameter("machineHaltLogID", machineHaltLogID));
                command.Parameters.Add(new SqlParameter("workingTeamShiftLogID", workingTeamShiftLogID));
                command.Parameters.Add(new SqlParameter("reasonID", reasonId));
                command.Parameters.Add(new SqlParameter("reasonText", reasonText));
                command.Parameters.Add(new SqlParameter("remarks", remarks));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 从JSON更新指定停机记录（辅助方法）
        /// </summary>
        /// <param name="workingTeamShiftLogID"></param>
        /// <param name="json"></param>
        public static void UpdateMachineHaltLogFromJson(string workingTeamShiftLogID, string json)
        {
            string[] haltLogJsons = json.JsonPickArray("rows");
            foreach (string haltLogJson in haltLogJsons)
            {
                string machineHaltLogId = haltLogJson.JsonPick("MachineHaltLogID");
                string reasonId = haltLogJson.JsonPick("ReasonID");
                string reasonText = haltLogJson.JsonPick("ReasonText");
                string remarks = haltLogJson.JsonPick("Remarks");

                UpdateMachineHaltLog(machineHaltLogId, workingTeamShiftLogID, reasonId, reasonText, remarks);
            }
        }
    }
}
