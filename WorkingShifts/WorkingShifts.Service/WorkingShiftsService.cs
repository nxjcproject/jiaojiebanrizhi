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

        /// <summary>
        /// 按交接班记录ID获取交接班日志
        /// </summary>
        /// <param name="workingTeamShiftLogId"></param>
        /// <returns></returns>
        public static DataTable GetWorkingTeamShiftLog(string workingTeamShiftLogId)
        {
            DataTable result = new DataTable();
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT   system_StaffInfo.Name AS ChargeManName, shift_WorkingTeamShiftLog.*
                                        FROM     shift_WorkingTeamShiftLog INNER JOIN
                                                 system_StaffInfo ON shift_WorkingTeamShiftLog.ChargeManID = system_StaffInfo.StaffInfoID
                                        WHERE   (shift_WorkingTeamShiftLog.WorkingTeamShiftLogID = @workingTeamShiftLogId)";

                command.Parameters.Add(new SqlParameter("workingTeamShiftLogId", workingTeamShiftLogId));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }

        /// <summary>
        /// 按分厂与起止时间查询交接班日志
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static DataTable GetWorkingTeamShiftLogs(string organizationId, DateTime startTime, DateTime endTime)
        {
            DataTable result = new DataTable();
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT   shift_WorkingTeamShiftLog.*
                                        FROM      shift_WorkingTeamShiftLog
                                        WHERE   (OrganizationID = @organizationId) AND (ShiftDate > @startTime) AND (ShiftDate < @endTime)";

                command.Parameters.Add(new SqlParameter("organizationId", organizationId));
                command.Parameters.Add(new SqlParameter("startTime", startTime));
                command.Parameters.Add(new SqlParameter("endTime", endTime));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }

        /// <summary>
        /// 按交接班记录ID获取操作员记录（纵表）
        /// </summary>
        /// <param name="workingTeamShiftLogId"></param>
        /// <returns></returns>
        public static DataTable GetOperatorsLogVertical(string workingTeamShiftLogId)
        {
            DataTable result = new DataTable();
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT   shift_OperatorsLog.*, system_StaffInfo.Name AS StaffName, 
                                                        system_Organization.Name AS DCSName
                                        FROM      system_StaffInfo INNER JOIN
                                                        shift_OperatorsLog ON system_StaffInfo.StaffInfoID = shift_OperatorsLog.StaffID INNER JOIN
                                                        system_Organization ON 
                                                        shift_OperatorsLog.OrganizationID = system_Organization.OrganizationID
                                        WHERE   (shift_OperatorsLog.WorkingTeamShiftLogID = @workingTeamShiftLogId)";

                command.Parameters.Add(new SqlParameter("workingTeamShiftLogId", workingTeamShiftLogId));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }

        /// <summary>
        /// 按交接班记录ID获取操作员记录（横表）
        /// </summary>
        /// <param name="workingTeamShiftLogId"></param>
        /// <returns></returns>
        public static DataTable GetOperatorsLogHorizontal(string workingTeamShiftLogId)
        {
            DataTable verticalResult = GetOperatorsLogVertical(workingTeamShiftLogId);
            DataTable result = new DataTable();

            // DCSID列
            result.Columns.Add("OrganizationID");
            // DCS名称列
            result.Columns.Add("Name");

            // 获取工序环节（转换后每个工序环节对应两列，分别为职工ID和职工NAME）
            var columns = (from x in verticalResult.Rows.Cast<DataRow>() select x["WorkingSectionName"].ToString()).Distinct();

            // 添加工序环节列
            foreach (var column in columns)
            {
                string colname = GetColumnName(column);
                result.Columns.Add("StaffName_" + colname);
                result.Columns.Add("StaffID_" + colname);
            }

            // 获取纵表中DCS的ID
            var dcsids = (from x in verticalResult.Rows.Cast<DataRow>() select x["OrganizationID"].ToString()).Distinct();

            Dictionary<string, DataRow> rowDictionary = new Dictionary<string, DataRow>();

            // 每个DCS的ID对应横表的一行
            foreach (var dcsid in dcsids)
            {
                DataRow row = result.NewRow();
                row["OrganizationID"] = dcsid;
                result.Rows.Add(row);
                rowDictionary.Add(dcsid, row);
            }

            // 将纵表转换为横标数据
            foreach (DataRow verticalRow in verticalResult.Rows)
            {
                string organizationId = verticalRow["OrganizationID"].ToString();
                string sectionName = verticalRow["WorkingSectionName"].ToString();
                string colname = GetColumnName(sectionName);

                rowDictionary[organizationId]["Name"] = verticalRow["DCSName"];
                rowDictionary[organizationId]["StaffName_" + colname] = verticalRow["StaffName"];
                rowDictionary[organizationId]["StaffId_" + colname] = verticalRow["StaffId"] + " " + verticalRow["StaffName"];
            }

            return result;
        }

        private static string GetColumnName(string sectionName)
        {
            switch (sectionName)
            {
                case "石灰石破碎": return "SHSPS";
                case "煤粉制备": return "MFZB";
                case "生料制备": return "SHENGLIAOZB";
                case "熟料制备": return "SHULIAOZB";
                case "水泥粉磨": return "SHUINIZB";
                case "辅助生产": return "FZZB";
                default: return "";
            }
        }
    }
}
