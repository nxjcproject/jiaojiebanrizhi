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
    }
}