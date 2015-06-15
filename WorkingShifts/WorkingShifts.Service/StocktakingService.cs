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
    }
}