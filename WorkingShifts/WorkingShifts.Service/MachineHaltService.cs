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
            string mySql = @"SELECT  RTRIM(LTRIM(LevelCode)) AS LevelCode, ReasonText, RTRIM(LTRIM(MachineHaltReasonID)) AS MachineHaltReasonID
                                FROM system_MachineHaltReason
                                WHERE Enabled='true'
                                order by LevelCode";
            return factory.Query(mySql);
        }

        /// <summary>
        /// 获取停机记录
        /// </summary>
        /// <param name="organizationId">组织机构ID</param>
        /// <param name="workingTeamShiftLogID">交接班日志ID</param>
        /// <returns></returns>
        public static DataTable GetMachineHaltLog(string organizationId, string startTime, string endTime, string workingTeamShiftLogID = "")
        {
            DataTable dt = new DataTable();
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"SELECT [A].* ,B.Name as ProductLineName
                                          FROM [shift_MachineHaltLog] AS [A], [system_Organization] AS [B]
                                         WHERE [A].[OrganizationID] = [B].[OrganizationID] AND 
                                               [B].[LevelCode] LIKE (SELECT [LevelCode] FROM [system_Organization] WHERE [OrganizationID] = @organizationId) + '%' AND 
                                               [A].[HaltTime] >= @startTime AND
                                               [A].[HaltTime] < @endTime
                                         order by [A].[HaltTime]";

                command.Parameters.Add(new SqlParameter("organizationId", organizationId));
                command.Parameters.Add(new SqlParameter("startTime", startTime));
                command.Parameters.Add(new SqlParameter("endTime", endTime));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        /// <summary>
        /// 获取停机记录
        /// </summary>
        /// <param name="organizationId">组织机构ID</param>
        /// <param name="workingTeamShiftLogID">交接班日志ID</param>
        /// <returns></returns>
        public static DataTable GetMachineHaltLog(string organizationId,string workingTeamShiftLogID)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            //Query query = new Query("shift_MachineHaltLog");
            //query.AddCriterion("OrganizationID", organizationId, CriteriaOperator.Equal);

            //if (string.IsNullOrWhiteSpace(workingTeamShiftLogID))
            //    query.AddCriterion("WorkingTeamShiftLogID", null, CriteriaOperator.NULL);
            //else
            //    query.AddCriterion("WorkingTeamShiftLogID", workingTeamShiftLogID, CriteriaOperator.Equal);
            string mySql;
            if (string.IsNullOrWhiteSpace(workingTeamShiftLogID))
            {
                mySql = @"SELECT A.* 
                                FROM shift_MachineHaltLog AS A,system_Organization AS B
                                WHERE A.OrganizationID=B.OrganizationID
                                AND B.LevelCode LIKE (select LevelCode from system_Organization where OrganizationID=@OrganizationID)+'%'
                                AND CONVERT(varchar(10),A.HaltTime,20)=CONVERT(varchar(10),GETDATE(),20)";
            }
            else
            {
                mySql = @"SELECT A.* 
                                FROM shift_MachineHaltLog AS A,system_Organization AS B
                                WHERE A.OrganizationID=B.OrganizationID
                                AND B.LevelCode LIKE (select LevelCode from system_Organization where OrganizationID=@OrganizationID)+'%'                               
                                AND WorkingTeamShiftLogID='" + workingTeamShiftLogID + "'";  
            }

            SqlParameter parameters =new SqlParameter("OrganizationID", organizationId);
            DataTable dt = factory.Query(mySql,parameters);
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
