using EasyUIJsonParser;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkingShifts.Infrastructure.Configuration;

namespace WorkingShifts.Service
{
    public class WorkingShiftsService
    {
        /// <summary>
        /// 创建交接班日志
        /// </summary>
        /// <param name="organizationId">组织机构ID</param>
        /// <param name="datetime">交接班时间</param>
        /// <param name="shifts">班次</param>
        /// <param name="workingTeam">班组</param>
        /// <param name="chargeManId">负责人ID</param>
        /// <param name="performToObjectives">本班生产计划完成情况</param>
        /// <param name="problemsAndSettlements">本班出现的问题及处理情况</param>
        /// <param name="equipmentSituation">本班设备运行情况</param>
        /// <param name="advicesToNextShift">下班工作重点及建议</param>
        /// <returns>交接班日志ID</returns>
        public static string CreateShiftLog(string organizationId, string datetime, string shifts, string workingTeam, string chargeManId, string performToObjectives, string problemsAndSettlements, string equipmentSituation, string advicesToNextShift)
        {
            string workingTeamShiftLogID = Guid.NewGuid().ToString();

            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO [dbo].[shift_WorkingTeamShiftLog]
                                                   ([WorkingTeamShiftLogID]
                                                   ,[OrganizationID]
                                                   ,[ShiftDate]
                                                   ,[Shifts]
                                                   ,[WorkingTeam]
                                                   ,[ChargeManID]
                                                   ,[PerformToObjectives]
                                                   ,[ProblemsAndSettlements]
                                                   ,[EquipmentSituation]
                                                   ,[AdvicesToNextShift])
                                             VALUES
                                                   (@workingTeamShiftLogID
                                                   ,@organizationID
                                                   ,@shiftDate
                                                   ,@shifts
                                                   ,@workingTeam
                                                   ,@chargeManID
                                                   ,@performToObjectives
                                                   ,@problemsAndSettlements
                                                   ,@equipmentSituation
                                                   ,@advicesToNextShift)";
                command.Parameters.Add(new SqlParameter("workingTeamShiftLogID", workingTeamShiftLogID));
                command.Parameters.Add(new SqlParameter("organizationId", organizationId));
                command.Parameters.Add(new SqlParameter("shiftDate", datetime));
                command.Parameters.Add(new SqlParameter("shifts", shifts));
                command.Parameters.Add(new SqlParameter("workingTeam", workingTeam));
                command.Parameters.Add(new SqlParameter("chargeManId", chargeManId));
                command.Parameters.Add(new SqlParameter("performToObjectives", performToObjectives));
                command.Parameters.Add(new SqlParameter("problemsAndSettlements", problemsAndSettlements));
                command.Parameters.Add(new SqlParameter("equipmentSituation", equipmentSituation));
                command.Parameters.Add(new SqlParameter("advicesToNextShift", advicesToNextShift));

                connection.Open();
                command.ExecuteNonQuery();
            }

            return workingTeamShiftLogID;
        }

        /// <summary>
        /// 创建操作员记录
        /// </summary>
        /// <param name="workingTeamShiftLogID"></param>
        /// <param name="organizationId"></param>
        /// <param name="workingSectionName"></param>
        /// <param name="staffId"></param>
        public static void CreateOperatorLog(string workingTeamShiftLogID, string organizationId, string workingSectionName, string staffId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO [dbo].[shift_OperatorsLog]
                                                   ([WorkingTeamShiftLogID]
                                                   ,[OrganizationID]
                                                   ,[WorkingSectionName]
                                                   ,[StaffID])
                                             VALUES
                                                   (@workingTeamShiftLogID
                                                   ,@organizationID
                                                   ,@workingSectionName
                                                   ,@staffID)";
                command.Parameters.Add(new SqlParameter("workingTeamShiftLogID", workingTeamShiftLogID));
                command.Parameters.Add(new SqlParameter("organizationId", organizationId));
                command.Parameters.Add(new SqlParameter("workingSectionName", workingSectionName));
                command.Parameters.Add(new SqlParameter("staffID", staffId));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 从JSON创建操作员记录（辅助方法）
        /// </summary>
        /// <param name="workingTeamShiftLogID"></param>
        /// <param name="json"></param>
        public static void CreateOperatorLogFromJson(string workingTeamShiftLogID, string json)
        {
            string[] operatorJsons = json.JsonPickArray("rows");

            foreach (string operatorJson in operatorJsons)
            {
                string organizationId = operatorJson.JsonPick("OrganizationID");
                string staffId = "";

                // 石灰石破碎
                staffId = operatorJson.JsonPick("StaffID_SHSPS");
                if (string.IsNullOrWhiteSpace(staffId) == false)
                    CreateOperatorLog(workingTeamShiftLogID, organizationId, "石灰石破碎", staffId);

                // 煤粉制备
                staffId = operatorJson.JsonPick("StaffID_MFZB");
                if (string.IsNullOrWhiteSpace(staffId) == false)
                    CreateOperatorLog(workingTeamShiftLogID, organizationId, "煤粉制备", staffId);

                // 生料制备
                staffId = operatorJson.JsonPick("StaffID_SHENGLIAOZB");
                if (string.IsNullOrWhiteSpace(staffId) == false)
                    CreateOperatorLog(workingTeamShiftLogID, organizationId, "生料制备", staffId);

                // 熟料制备
                staffId = operatorJson.JsonPick("StaffID_SHULIAOZB");
                if (string.IsNullOrWhiteSpace(staffId) == false)
                    CreateOperatorLog(workingTeamShiftLogID, organizationId, "熟料制备", staffId);

                // 水泥粉磨
                staffId = operatorJson.JsonPick("StaffID_SHUINIZB");
                if (string.IsNullOrWhiteSpace(staffId) == false)
                    CreateOperatorLog(workingTeamShiftLogID, organizationId, "水泥粉磨", staffId);

                // 辅助生产
                staffId = operatorJson.JsonPick("StaffID_FZZB");
                if (string.IsNullOrWhiteSpace(staffId) == false)
                    CreateOperatorLog(workingTeamShiftLogID, organizationId, "辅助生产", staffId);
            }
        }
    }
}
