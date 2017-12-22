using EasyUIJsonParser;
using SqlServerDataAdapter;
using SqlServerDataAdapter.Infrastruction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WorkingShifts.Infrastructure.Configuration;

namespace WorkingShifts.Service
{
    public class WorkingShiftsService
    {
        /// <summary>
        /// 交接班日志是否已存在
        /// </summary>
        /// <param name="organizationId">组织机构ID</param>
        /// <param name="datetime">日期，格式：yyyy-MM-dd</param>
        /// <param name="shifts">时间班（甲班、乙班、丙班）</param>
        /// <returns></returns>
        public static bool IsShiftLogExisit(string organizationId, string datetime, string shifts)
        {
            int count = 0;
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            DateTime date = DateTime.Parse(datetime);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT COUNT(*)
                                          FROM [dbo].[shift_WorkingTeamShiftLog]
                                         WHERE [OrganizationID] = @organizationId AND
	                                           CONVERT(varchar(10),[ShiftDate],120) = @date AND
	                                           [Shifts] = @shifts";

                command.Parameters.Add(new SqlParameter("organizationId", organizationId));
                command.Parameters.Add(new SqlParameter("date", date.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SqlParameter("shifts", shifts));
                connection.Open();
                count = (int)command.ExecuteScalar();
            }

            return count > 0;
        }

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

            // 检查是否有重复提交
            if (IsShiftLogExisit(organizationId, datetime, shifts))
                throw new ApplicationException("交接班日志已存在。");

            string workingTeamShiftLogID = Guid.NewGuid().ToString();

            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO [dbo].[shift_WorkingTeamShiftLog]
                                                   ([WorkingTeamShiftLogID]
                                                   ,[OrganizationID]
                                                   ,[ShiftDate]
                                                   ,[UpdateDate]
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
                                                   ,@updateDate
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
                command.Parameters.Add(new SqlParameter("updateDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
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
        /// <param name="workingSectionID"></param>
        /// <param name="staffId"></param>
        public static void CreateOperatorLog(string workingTeamShiftLogID, string organizationId, string workingSectionId, string staffId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO [dbo].[shift_OperatorsLog]
                                                   ([WorkingTeamShiftLogID]
                                                   ,[OrganizationID]
                                                   ,[WorkingSectionID]
                                                   ,[StaffID])
                                             VALUES
                                                   (@workingTeamShiftLogID
                                                   ,@organizationID
                                                   ,@workingSectionId
                                                   ,@staffID)";
                command.Parameters.Add(new SqlParameter("workingTeamShiftLogID", workingTeamShiftLogID));
                command.Parameters.Add(new SqlParameter("organizationId", organizationId));
                command.Parameters.Add(new SqlParameter("workingSectionId", workingSectionId));
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

            // 匹配以StaffID_开头的列名，其中后面的部分为GUID的字符串（-替换为_，因为前台javascript不支持-作为ID）。
            // 该GUID就是SectionId
            string reg_idselector = @"StaffID_\w{8}_(\w{4}_){3}\w{12}";
            Regex rgSectionId = new Regex(reg_idselector);

            foreach (string operatorJson in operatorJsons)
            {
                string organizationId = operatorJson.JsonPick("OrganizationID");
                string staffId = "";
                string workingSectionId = "";

                MatchCollection mc = rgSectionId.Matches(operatorJson);

                foreach (Match match in mc)
                {
                    staffId = operatorJson.JsonPick(match.Value);
                    if (string.IsNullOrWhiteSpace(staffId) == false)
                    {
                        staffId = staffId.Split(' ')[0];
                        workingSectionId = match.Value.Substring(match.Value.IndexOf('_') + 1).Replace('_', '-');
                        CreateOperatorLog(workingTeamShiftLogID, organizationId, workingSectionId, staffId);
                    }
                }
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
                                        WHERE   (OrganizationID = @organizationId) AND (ShiftDate > @startTime) AND (ShiftDate < @endTime)
                                        ORDER BY ShiftDate";

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
        /// 根据配置页面的组织机构找到分厂组织机构
        /// </summary>
        /// <param name="myOrganizationId">配置界面组织机构</param>
        /// <returns></returns>
        public static DataTable GetFactoryOrganizationId(string myOrganizationId)
        {

            DataTable result = new DataTable();
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"select A.OrganizationID, A.Name 
                                            from system_Organization A, system_Organization B
                                            where B.OrganizationId = @organizationId
                                            and A.LevelCode like B.LevelCode + '%'
                                            and A.LevelType = 'Factory'
                                            order by A.LevelCode";

                command.Parameters.Add(new SqlParameter("organizationId", myOrganizationId));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }
    }
}
