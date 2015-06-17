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
    public static class OperatorService
    {
        /// <summary>
        /// 获取分厂工段
        /// </summary>
        /// <param name="organizationId">组织机构ID（分厂）</param>
        /// <returns></returns>
        public static DataTable GetWorkingSections(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT *
                                          FROM [system_WorkingSection]
                                         WHERE [OrganizationID] LIKE @organizationId + '%'
                                      ORDER BY [Type], [DisplayIndex]";

                command.Parameters.Add(new SqlParameter("organizationId", organizationId));

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }

            return dt;
        }

        /// <summary>
        /// 获取分厂生产线
        /// </summary>
        /// <param name="organizationId">织机构ID（分厂）</param>
        /// <returns></returns>
        public static DataTable GetProductionLines(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT *
                                          FROM [system_Organization]
                                         WHERE [OrganizationID] LIKE @organizationId + '%' AND
                                               [Type] IS NOT NULL
                                      ORDER BY [OrganizationID]";

                command.Parameters.Add(new SqlParameter("organizationId", organizationId));

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
            }

            return dt;
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
                command.CommandText = @"SELECT [shift_OperatorsLog].*, [system_StaffInfo].[Name] AS [StaffName], [system_Organization].[Name] AS [ProductionLineName]
                                          FROM [system_StaffInfo] INNER JOIN
                                               [shift_OperatorsLog] ON [system_StaffInfo].[StaffInfoID] = [shift_OperatorsLog].[StaffID] INNER JOIN
                                               [system_Organization] ON [shift_OperatorsLog].[OrganizationID] = [system_Organization].[OrganizationID]
                                         WHERE [shift_OperatorsLog].[WorkingTeamShiftLogID] = @workingTeamShiftLogId";

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
            var columns = (from x in verticalResult.Rows.Cast<DataRow>() select x["WorkingSectionID"].ToString().Trim().Replace('-', '_')).Distinct();

            // 添加工序环节列
            foreach (var column in columns)
            {
                string colname = column;
                result.Columns.Add("StaffName_" + colname);
                result.Columns.Add("StaffID_" + colname);
            }

            // 获取纵表中生产线的ID
            var productionLineIds = (from x in verticalResult.Rows.Cast<DataRow>() select x["OrganizationID"].ToString()).Distinct();

            Dictionary<string, DataRow> rowDictionary = new Dictionary<string, DataRow>();

            // 每个生产线的ID对应横表的一行
            foreach (var productionLineId in productionLineIds)
            {
                DataRow row = result.NewRow();
                row["OrganizationID"] = productionLineId;
                result.Rows.Add(row);
                rowDictionary.Add(productionLineId, row);
            }

            // 将纵表转换为横标数据
            foreach (DataRow verticalRow in verticalResult.Rows)
            {
                string organizationId = verticalRow["OrganizationID"].ToString();
                string sectionName = verticalRow["WorkingSectionID"].ToString().Trim().Replace('-', '_');
                string colname = sectionName;

                rowDictionary[organizationId]["Name"] = verticalRow["ProductionLineName"];
                rowDictionary[organizationId]["StaffName_" + colname] = verticalRow["StaffName"];
                rowDictionary[organizationId]["StaffId_" + colname] = verticalRow["StaffId"] + " " + verticalRow["StaffName"];
            }

            return result;
        }

        /// <summary>
        /// 按班组获取最近一次操作员记录（纵表）
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="workingTeamShiftLogId"></param>
        /// <returns></returns>
        public static DataTable GetLastOperatorsLogVertical(string organizationId, string workingTeam)
        {
            DataTable result = new DataTable();
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT [shift_OperatorsLog].*, [system_StaffInfo].[Name] AS [StaffName], [system_Organization].[Name] AS [ProductionLineName]
                                          FROM [system_StaffInfo] INNER JOIN
                                               [shift_OperatorsLog] ON [system_StaffInfo].[StaffInfoID] = [shift_OperatorsLog].[StaffID] INNER JOIN
                                               [system_Organization] ON [shift_OperatorsLog].[OrganizationID] = [system_Organization].[OrganizationID]
                                         WHERE [shift_OperatorsLog].[WorkingTeamShiftLogID] = (
		                                         SELECT TOP(1) [WorkingTeamShiftLogID]
		                                           FROM [shift_WorkingTeamShiftLog]
		                                          WHERE [WorkingTeam] = @workingTeam AND
		   	                                            [OrganizationID] = @organizationId
	                                           ORDER BY [UpdateDate] DESC
                                                )";

                command.Parameters.Add(new SqlParameter("organizationId", organizationId));
                command.Parameters.Add(new SqlParameter("workingTeam", workingTeam));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }

        /// <summary>
        /// 按班组获取最近一次操作员记录（横表）
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="workingTeam"></param>
        /// <returns></returns>
        public static DataTable GetLastOperatorsLogHorizontal(string organizationId, string workingTeam)
        {
            DataTable verticalResult = GetLastOperatorsLogVertical(organizationId, workingTeam);
            DataTable result = new DataTable();

            // DCSID列
            result.Columns.Add("OrganizationID");
            // DCS名称列
            result.Columns.Add("Name");

            // 获取工序环节（转换后每个工序环节对应两列，分别为职工ID和职工NAME）
            var columns = (from x in verticalResult.Rows.Cast<DataRow>() select x["WorkingSectionID"].ToString().Trim().Replace('-', '_')).Distinct();

            // 添加工序环节列
            foreach (var column in columns)
            {
                string colname = column;
                result.Columns.Add("StaffName_" + colname);
                result.Columns.Add("StaffID_" + colname);
            }

            // 获取纵表中生产线的ID
            var productionLineIds = (from x in verticalResult.Rows.Cast<DataRow>() select x["OrganizationID"].ToString()).Distinct();

            Dictionary<string, DataRow> rowDictionary = new Dictionary<string, DataRow>();

            //// 每个生产线的ID对应横表的一行
            //foreach (var productionLineId in productionLineIds)
            //{
            //    DataRow row = result.NewRow();
            //    row["OrganizationID"] = productionLineId;
            //    result.Rows.Add(row);
            //    rowDictionary.Add(productionLineId, row);
            //}
            DataTable productionLines = GetProductionLine(organizationId);
            foreach (DataRow productionLine in productionLines.Rows)
            {
                DataRow row = result.NewRow();
                row["OrganizationID"] = productionLine["OrganizationID"].ToString();
                row["Name"] = productionLine["Name"].ToString();
                result.Rows.Add(row);
                rowDictionary.Add(productionLine["OrganizationID"].ToString(), row);
            }

            // 将纵表转换为横标数据
            foreach (DataRow verticalRow in verticalResult.Rows)
            {
                string id = verticalRow["OrganizationID"].ToString();
                string sectionName = verticalRow["WorkingSectionID"].ToString().Trim().Replace('-', '_');
                string colname = sectionName;

                rowDictionary[id]["Name"] = verticalRow["ProductionLineName"];
                rowDictionary[id]["StaffName_" + colname] = verticalRow["StaffName"];
                rowDictionary[id]["StaffId_" + colname] = verticalRow["StaffId"] + " " + verticalRow["StaffName"];
            }

            return result;
        }

        private static DataTable GetProductionLine(string organizationId)
        {
            DataTable result = new DataTable();
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT [OrganizationID], [Name]
                                          FROM [system_Organization]
                                         WHERE [OrganizationID] LIKE @organizationId + '%' AND [Type] IS NOT NULL
                                      ORDER BY [OrganizationID]";

                command.Parameters.Add(new SqlParameter("organizationId", organizationId));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }
    }
}
