using EasyUIJsonParser;
using EasyUIJsonParser.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkingShifts.Service;

namespace WorkingShifts.Web.UI_WorkingShifts
{
    public partial class HandoverLogDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //[WebMethod]
        //public string GetHandoverLogWithDataGridFormat()
        //{
        //    DataTable dt = WorkingShiftsService.();
        //    return DataGridJsonParser.DataTableToJson(dt);
        //}]

        /// <summary>
        /// 获取交接班日志信息
        /// </summary>
        /// <param name="workingTeamShiftLogId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetWorkingTeamShiftLog(string workingTeamShiftLogId)
        {
            DataTable dt = WorkingShiftsService.GetWorkingTeamShiftLog(workingTeamShiftLogId);
            string json= JsonHelper.DataTableFirstRowToJson(dt);
            return json.Replace("\n", "\\n");
        }

        [WebMethod]
        public static string GetWorkingSectionsWithDataColumnFormat(string organizationId)
        {
            DataTable dt = OperatorService.GetWorkingSections(organizationId);

            StringBuilder sb = new StringBuilder();
            sb.Append("[[");
            sb.Append("{field:'OrganizationID',hidden:true},");
            sb.Append("{field:'Name',width:100},");
            foreach (DataRow dr in dt.Rows)
            {
                string id = dr["WorkingSectionID"].ToString().Trim().Replace('-', '_');
                sb.Append("{field:'StaffName_");
                sb.Append(id);
                sb.Append("',hidden:true},");

                sb.Append("{field:'StaffID_");
                sb.Append(id);
                sb.Append("',width:180,title:'");
                sb.Append(dr["WorkingSectionName"]);
                sb.Append("'},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]]");

            return sb.ToString();
        }

        [WebMethod]
        public static string GetOperatorsLog(string workingTeamShiftLogId)
        {
            DataTable dt = OperatorService.GetOperatorsLogHorizontal(workingTeamShiftLogId);
            return DataGridJsonParser.DataTableToJson(dt);
        }

        /// <summary>
        /// 获取停机记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetMachineHaltLogWithDataGridFormat(string organizationId, string workingTeamShiftLogId)
        {
            DataTable dt = MachineHaltService.GetMachineHaltLog(organizationId, workingTeamShiftLogId);
            return DataGridJsonParser.DataTableToJson(dt);
        }

        /// <summary>
        /// 获取DCS报警记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetDCSWarningLogWithDataGridFormat(string organizationId, string workingTeamShiftLogId)
        {
            DataTable dt = DCSSystemServcie.GetDCSWarningLog(organizationId, workingTeamShiftLogId);
            return DataGridJsonParser.DataTableToJson(dt);
        }

        /// <summary>
        /// 获取能耗报警记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetEnergyConsumptionAlarmLogWithDataGridFormat(string organizationId, string workingTeamShiftLogId)
        {
            DataTable dt = EnergyConsumptionAlarmLogService.GetEnergyConsumptionAlarmLog(organizationId, workingTeamShiftLogId);
            return DataGridJsonParser.DataTableToJson(dt);
        }

        /// <summary>
        /// 获取盘库信息
        /// </summary>
        /// <param name="workingTeamShiftLogId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetStocktakingLogWithDataGridFormat(string workingTeamShiftLogId)
        {
            DataTable dt = StocktakingService.GetBalancedStockingInfo(workingTeamShiftLogId);
            return DataGridJsonParser.DataTableToJson(dt);
        }
    }
}