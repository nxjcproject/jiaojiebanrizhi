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
    public partial class HandoverLog : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if DEBUG
                // 调试用,自定义的数据授权
                List<string> m_DataValidIdItems = new List<string>() { "O0101", "O0102" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#endif
                this.OrganisationTree.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
                this.OrganisationTree.PageName = "Edit.aspx";
            }
        }

        [WebMethod]
        public static string GetWorkingTeamShiftLogsWithDataGridFormat(string organizationId, string startTime, string endTime)
        {
            DataTable dt = WorkingShiftsService.GetWorkingTeamShiftLogs(organizationId, DateTime.Parse(startTime), DateTime.Parse(endTime));
            return DataGridJsonParser.DataTableToJson(dt);
        }
    }
}