using EasyUIJsonParser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkingShifts.Service;

namespace WorkingShifts.Web.UI_WorkingShifts
{
    public partial class HandoverLoger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region Create

        /// <summary>
        /// 创建交接班日志
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        [WebMethod]
        public static string CreateWorkingTeamShiftLog(string organizationId, string json)
        {
            string time = json.JsonPick("time");
            string shifts = json.JsonPick("shifts");
            string workingTeam = json.JsonPick("workingTeam");
            string chargeMan = json.JsonPick("chargeMan");
            string operators = json.JsonPick("operators");
            string haltLogs = json.JsonPick("haltLogs");
            string dcsWarningLogs = json.JsonPick("dcsWarningLogs");
            string performToObjectives = json.JsonPick("performToObjectives");
            string problemsAndSettlements = json.JsonPick("problemsAndSettlements");
            string equipmentSituation = json.JsonPick("equipmentSituation");
            string advicesToNextShift = json.JsonPick("advicesToNextShift");

            string[] operatorJsons = operators.JsonPickArray("rows");

            return "";
        }

        #endregion

        #region Read

        [WebMethod]
        public static string GetWorkingTeamWithComboboxFormat(string organizationId)
        {
            DataTable dt = StaffService.GetWorkingTeamByOrganizationId(organizationId);
            return ComboboxJsonParser.DataTableToJson(dt, "Name", "ChargeManID");
        }

        [WebMethod]
        public static string GetChargeManByWorkingTeamNameWithComboboxFormat(string workingTeamName)
        {
            DataTable dt = StaffService.GetChargeManByWorkingTeamName(workingTeamName);
            if (dt.Rows.Count == 0)
                return "";
            return "{\"ID\":\"" + dt.Rows[0]["StaffInfoID"].ToString() + "\", \"Name\":\"" + dt.Rows[0]["Name"].ToString() + "\"}";
        }

        [WebMethod]
        public static string GetStaffInfoWithComboboxFormat(string organizationId)
        {
            DataTable dt = StaffService.GetStaffInfo(organizationId);
            dt.Columns["StaffInfoID"].ColumnName = "StaffID";
            dt.Columns["Name"].ColumnName = "StaffName";
            dt.Columns.Add("Combined");
            foreach (DataRow dr in dt.Rows)
            {
                dr["Combined"] = dr["StaffID"] + "  " + dr["StaffName"];
            }
            return ComboboxJsonParser.DataTableToJson(dt, "StaffID", "StaffName", "Combined");
        }

        [WebMethod]
        public static string GetDCSSystemWithDataGridFormat(string organizationId)
        {
            DataTable dt = DCSSystemServcie.GetDCSSystemByOrganizationId(organizationId);
            return DataGridJsonParser.DataTableToJson(dt, "Name", "OrganizationID");
        }

        #endregion

    }
}