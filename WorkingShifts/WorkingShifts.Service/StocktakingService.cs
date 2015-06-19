using EasyUIJsonParser;
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
    public class StocktakingService
    {
        /// <summary>
        /// 从实时增量累计表中获取原始盘库信息
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static DataTable GetOriginalStocktakingInfo(string organizationId, bool getCurrentShiftData = true)
        {
            DataTable result = new DataTable();

            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT (CASE WHEN @shifts = 'Last' THEN RealtimeIncrementCumulant.CumulantLastClass WHEN @shifts = 'Current' THEN RealtimeIncrementCumulant.CumulantClass END)AS Data, 
		                                        RealtimeIncrementCumulant.OrganizationID, RealtimeIncrementCumulant.VariableId, material_MaterialDetail.Name, material_MaterialDetail.Unit, system_Organization.Name AS OrganizationName
                                          FROM  material_MaterialDetail INNER JOIN
		                                        tz_Material ON material_MaterialDetail.KeyID = tz_Material.KeyID AND 
		                                        tz_Material.OrganizationID LIKE @organizationId + '%' LEFT JOIN
		                                        RealtimeIncrementCumulant ON material_MaterialDetail.VariableId = RealtimeIncrementCumulant.VariableId AND
		                                        tz_Material.OrganizationID = RealtimeIncrementCumulant.OrganizationID INNER JOIN
		                                        system_Organization ON tz_Material.OrganizationID = system_Organization.OrganizationID
                                      ORDER BY system_Organization.OrganizationID";

                string shifts = "";
                if (getCurrentShiftData)
                    shifts = "Current";
                else
                    shifts = "Last";

                command.Parameters.Add(new SqlParameter("organizationId", organizationId));
                command.Parameters.Add(new SqlParameter("shifts", shifts));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }

        /// <summary>
        /// 创建盘库信息（单条）
        /// </summary>
        /// <param name="workingTeamShiftLogId"></param>
        /// <param name="organizationId"></param>
        /// <param name="shift"></param>
        /// <param name="updateDate"></param>
        /// <param name="variableId"></param>
        /// <param name="dataValue"></param>
        /// <param name="remark"></param>
        public static void CreateStocktakingInfo(string workingTeamShiftLogId, string organizationId, string shift, DateTime updateDate, string variableId, string dataValue, string remark)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO [dbo].[balance_BalanceMartieralsClass]
                                                   ([DataItemId]
                                                   ,[Class]
                                                   ,[WorkingTeamShiftLogID]
                                                   ,[OrganizationID]
                                                   ,[UpdateDate]
                                                   ,[VariableId]
                                                   ,[DataValue]
                                                   ,[Remark])
                                             VALUES
                                                   (@dataItemId
                                                   ,@class
                                                   ,@workingTeamShiftLogID
                                                   ,@organizationID
                                                   ,@updateDate
                                                   ,@variableId
                                                   ,@dataValue
                                                   ,@remark)";

                command.Parameters.Add(new SqlParameter("dataItemId", Guid.NewGuid()));
                command.Parameters.Add(new SqlParameter("class", shift));
                command.Parameters.Add(new SqlParameter("workingTeamShiftLogID", workingTeamShiftLogId));
                command.Parameters.Add(new SqlParameter("organizationID", organizationId));
                command.Parameters.Add(new SqlParameter("updateDate", updateDate));
                command.Parameters.Add(new SqlParameter("variableId", variableId));
                command.Parameters.Add(new SqlParameter("dataValue", dataValue));
                command.Parameters.Add(new SqlParameter("remark", remark));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 创建盘库信息
        /// </summary>
        /// <param name="workingTeamShiftLogID"></param>
        /// <param name="shift"></param>
        /// <param name="json"></param>
        public static void SaveStocktakingInfo(string workingTeamShiftLogID, string shift, string json)
        {
            DateTime datetime = DateTime.Now;
            string[] stocktakingJsons = json.JsonPickArray("rows");
            string organizationId = "", variableId = "", dataValue = "", remark = "";

            foreach (string stocktakingJson in stocktakingJsons)
            {
                organizationId = stocktakingJson.JsonPick("OrganizationID");
                variableId = stocktakingJson.JsonPick("VariableId");
                dataValue = stocktakingJson.JsonPick("DataValue");
                remark = stocktakingJson.JsonPick("Remark");

                if (string.IsNullOrWhiteSpace(dataValue))
                    continue;

                CreateStocktakingInfo(workingTeamShiftLogID, organizationId, shift, datetime, variableId, dataValue, remark);
            }
        }

        /// <summary>
        /// 获取平衡后的物料信息
        /// </summary>
        /// <param name="workingTeamShiftLogID"></param>
        /// <returns></returns>
        public static DataTable GetBalancedStockingInfo(string workingTeamShiftLogID)
        {
            DataTable result = new DataTable();

            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"  SELECT [O].[Name] AS [OrganizationName],
                                                 [M].[Name] AS [Name],
		                                         [M].[Unit] AS [Unit],
                                                 (CASE WHEN [W].[Shifts] = '甲班' THEN [B].[First]
                                                       WHEN [W].[Shifts] = '乙班' THEN [B].[Second]
			                                           WHEN [W].[Shifts] = '丙班' THEN [B].[Third] END) AS [Data],
                                                 (CASE WHEN [W].[Shifts] = '甲班' THEN [B].[FirstB]
                                                       WHEN [W].[Shifts] = '乙班' THEN [B].[SecondB]
			                                           WHEN [W].[Shifts] = '丙班' THEN [B].[ThirdB] END) AS [DataValue],
		                                         [F].[Remark]
                                            FROM [balance_Energy] AS [B] INNER JOIN
	                                             [tz_Balance] AS [T] ON [B].[KeyId] = [T].[BalanceId] INNER JOIN
		                                         [shift_WorkingTeamShiftLog] AS [W] ON [T].[TimeStamp] =  CONVERT(varchar(100), [W].[ShiftDate], 23) INNER JOIN
		                                         [material_MaterialDetail] AS [M] ON [B].[VariableId] = [M].[VariableId] INNER JOIN
		                                         [system_Organization] AS [O] ON [B].[OrganizationID] = [O].[OrganizationID] LEFT JOIN
		                                         [balance_BalanceMartieralsClass] AS [F] ON [B].[OrganizationID] = [F].[OrganizationID] AND [B].[VariableId] = [F].[VariableId]
                                           WHERE [W].[WorkingTeamShiftLogID] = @workingTeamShiftLogID
                                        ORDER BY [O].[OrganizationID]";

                command.Parameters.Add(new SqlParameter("workingTeamShiftLogID", workingTeamShiftLogID));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }
    }
}