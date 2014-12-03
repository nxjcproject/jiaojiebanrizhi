﻿using EasyUIJsonParser;
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
    public class DCSSystemServcie
    {
        /// <summary>
        /// 根据组织机构ID（分厂）获取DCS系统
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static DataTable GetDCSSystemByOrganizationId(string organizationId)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT * 
                                        FROM [dbo].[system_Organization_Instrumentation] 
                                        WHERE LEN([LevelCode]) = 7 AND [LevelCode] LIKE (
                                            (SELECT [LevelCode] FROM [dbo].[system_Organization_Instrumentation] 
                                            WHERE [OrganizationID] = @organizationId) 
                                            + '%' 
                                        )";
                command.Parameters.Add(new SqlParameter("organizationId", organizationId));

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 获取DCS报警记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="workingTeamShiftLogID"></param>
        /// <returns></returns>
        public static DataTable GetDCSWarningLog(string organizationId, string workingTeamShiftLogID = "")
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;

            ISqlServerDataFactory factory = new SqlServerDataFactory(connectionString);
            Query query = new Query("shift_DCSWarningLog");
            query.AddCriterion("OrganizationID", organizationId, CriteriaOperator.Equal);

            if (string.IsNullOrWhiteSpace(workingTeamShiftLogID))
                query.AddCriterion("WorkingTeamShiftLogID", null, CriteriaOperator.NULL);
            else
                query.AddCriterion("WorkingTeamShiftLogID", workingTeamShiftLogID, CriteriaOperator.Equal);

            return factory.Query(query);
        }

        /// <summary>
        /// 更新DCS报警记录
        /// </summary>
        /// <param name="dcsWarningLogId"></param>
        /// <param name="workingTeamShiftLogID"></param>
        /// <param name="handleInformation"></param>
        /// <param name="remarks"></param>
        public static void UpdateDCSWarningLog(string dcsWarningLogId, string workingTeamShiftLogID, string handleInformation, string remarks)
        {
            string connectionString = ConnectionStringFactory.NXJCConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"UPDATE [dbo].[shift_DCSWarningLog]
                                           SET [WorkingTeamShiftLogID] = @workingTeamShiftLogID
                                              ,[HandleInformation] = @handleInformation
                                              ,[Remarks] = @remarks
                                         WHERE [DCSWarningLogID] = @dcsWarningLogId";

                command.Parameters.Add(new SqlParameter("dcsWarningLogId", dcsWarningLogId));
                command.Parameters.Add(new SqlParameter("workingTeamShiftLogID", workingTeamShiftLogID));
                command.Parameters.Add(new SqlParameter("handleInformation", handleInformation));
                command.Parameters.Add(new SqlParameter("remarks", remarks));

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 从JSON更新DCS报警记录（辅助方法）
        /// </summary>
        /// <param name="workingTeamShiftLogID"></param>
        /// <param name="json"></param>
        public static void UpdateDCSWarningLogFromJson(string workingTeamShiftLogID, string json)
        {
            string[] dcsWarningLogJsons = json.JsonPickArray("rows");
            foreach (string dcsWarningLogJson in dcsWarningLogJsons)
            {
                string dcsWarningLogId = dcsWarningLogJson.JsonPick("DCSWarningLogID");
                string handleInformation = dcsWarningLogJson.JsonPick("HandleInformation");
                string remarks = dcsWarningLogJson.JsonPick("Remarks");

                UpdateDCSWarningLog(dcsWarningLogId, workingTeamShiftLogID, handleInformation, remarks);
            }
        }
    }
}
