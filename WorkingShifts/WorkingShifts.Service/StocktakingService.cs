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
        public static DataTable GetOriginalStocktakingInfo(string organizationId)
        {
            DataTable result = new DataTable();

            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT   DISTINCT RealtimeIncrementCumulant.*, RealtimeIncrementCumulant.CumulantLastClass AS DataValue, material_MaterialDetail.Name,  material_MaterialDetail.Unit
                                        FROM     RealtimeIncrementCumulant RIGHT JOIN
                                                        material_MaterialDetail ON RealtimeIncrementCumulant.VariableId = material_MaterialDetail.VariableId
                                        WHERE    RealtimeIncrementCumulant.OrganizationID LIKE @organizationId + '%'";

                command.Parameters.Add(new SqlParameter("organizationId", organizationId));

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(result);
                }
            }

            return result;
        }

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

        public static void SaveStocktakingInfo(string workingTeamShiftLogID, string shift, string json)
        {
            DateTime datetime = DateTime.Now;
            string[] stocktakingJsons = json.JsonPickArray("rows");
            foreach (string stocktakingJson in stocktakingJsons)
            {
                string organizationId = stocktakingJson.JsonPick("OrganizationID");
                string variableId = stocktakingJson.JsonPick("VariableId");
                string dataValue = stocktakingJson.JsonPick("DataValue");
                string remark = stocktakingJson.JsonPick("Remark");

                CreateStocktakingInfo(workingTeamShiftLogID, organizationId, shift, datetime, variableId, dataValue, remark);
            }
        }
    }
}