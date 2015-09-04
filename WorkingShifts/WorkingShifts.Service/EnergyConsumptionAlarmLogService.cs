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
    public class EnergyConsumptionAlarmLogService
    {
        /// <summary>
        /// 获取能耗报警记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="workingTeamShiftLogID"></param>
        /// <returns></returns>
        public static DataTable GetEnergyConsumptionAlarmLog(string organizationId, string workingTeamShiftLogID)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            //Query query = new Query("shift_EnergyConsumptionAlarmLog");
            //query.AddCriterion("OrganizationID", organizationId, CriteriaOperator.Equal);

            //if (string.IsNullOrWhiteSpace(workingTeamShiftLogID))
            //    query.AddCriterion("WorkingTeamShiftLogID", null, CriteriaOperator.NULL);
            //else
            //    query.AddCriterion("WorkingTeamShiftLogID", workingTeamShiftLogID, CriteriaOperator.Equal);
            string mySql = @"SELECT A.* ,B.Name as ProductLineName
                                FROM shift_EnergyConsumptionAlarmLog AS A,system_Organization AS B
                                WHERE A.OrganizationID=B.OrganizationID
                                AND B.LevelCode LIKE (select LevelCode from system_Organization where OrganizationID='{0}')+'%'
                                AND WorkingTeamShiftLogID = '{1}'";

            DataTable dt = factory.Query(string.Format(mySql, organizationId, workingTeamShiftLogID));
            return dt;
        }
        /// <summary>
        /// 获取能耗报警记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="workingTeamShiftLogID"></param>
        /// <returns></returns>
        public static DataTable GetEnergyConsumptionAlarmLog(string organizationId,string startTime,string endTime, string workingTeamShiftLogID = "")
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            //Query query = new Query("shift_EnergyConsumptionAlarmLog");
            //query.AddCriterion("OrganizationID", organizationId, CriteriaOperator.Equal);

            //if (string.IsNullOrWhiteSpace(workingTeamShiftLogID))
            //    query.AddCriterion("WorkingTeamShiftLogID", null, CriteriaOperator.NULL);
            //else
            //    query.AddCriterion("WorkingTeamShiftLogID", workingTeamShiftLogID, CriteriaOperator.Equal);
            string mySql = @"SELECT A.* ,B.Name as ProductLineName
                                FROM shift_EnergyConsumptionAlarmLog AS A,system_Organization AS B
                                WHERE A.OrganizationID=B.OrganizationID
                                AND B.LevelCode LIKE (select LevelCode from system_Organization where OrganizationID='{2}')+'%'
                                AND(A.StartTime>=CONVERT(varchar(10),GETDATE(),20)+' {0}'
                                AND A.StartTime<=CONVERT(varchar(10),GETDATE(),20)+' {1}')";
            if (endTime == "24:00")
                endTime = "23:59";
            DataTable dt = factory.Query(string.Format(mySql, startTime, endTime, organizationId));
            //DataTable dt = factory.Query(string.Format(mySql, organizationId));
            return dt;
        }
        /// <summary>
        /// 更新能耗报警记录
        /// </summary>
        /// <param name="energyConsumptionAlarmLogId"></param>
        /// <param name="workingTeamShiftLogID"></param>
        /// <param name="reason"></param>
        public static void UpdateEnergyConsumptionAlarmLog(string energyConsumptionAlarmLogId, string workingTeamShiftLogID, string reason)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"UPDATE [dbo].[shift_EnergyConsumptionAlarmLog]
                                           SET [WorkingTeamShiftLogID] = @workingTeamShiftLogID
                                              ,[Reason] = @reason
                                         WHERE [EnergyConsumptionAlarmLogID] = @energyConsumptionAlarmLogId";

                command.Parameters.Add(new SqlParameter("energyConsumptionAlarmLogId", energyConsumptionAlarmLogId));
                command.Parameters.Add(new SqlParameter("workingTeamShiftLogID", workingTeamShiftLogID));
                command.Parameters.Add(new SqlParameter("reason", reason));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 从JSON更新能耗报警记录（辅助方法）
        /// </summary>
        /// <param name="workingTeamShiftLogID"></param>
        /// <param name="json"></param>
        public static void UpdateEnergyConsumptionAlarmLogFromJson(string workingTeamShiftLogID, string json)
        {
            string[] energyConsumptionAlarmLogJsons = json.JsonPickArray("rows");
            foreach (string energyConsumptionAlarmLogJson in energyConsumptionAlarmLogJsons)
            {
                string energyConsumptionAlarmLogId = energyConsumptionAlarmLogJson.JsonPick("EnergyConsumptionAlarmLogID");
                string reason = energyConsumptionAlarmLogJson.JsonPick("Reason");

                UpdateEnergyConsumptionAlarmLog(energyConsumptionAlarmLogId, workingTeamShiftLogID, reason);
            }
        }
    }
}