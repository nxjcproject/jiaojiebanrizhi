using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkingShifts.Service;
using EasyUIJsonParser;

namespace WorkingShifts.Web.UI_DispatchingSummary
{
    public partial class DispatchingLog_SubmitStatus : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if DEBUG
                // 调试用,自定义的数据授权
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_yc", "zc_nxjc_qtx" };
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#endif
            }
        }
        [WebMethod]
        public static string GetDispatchingSubmitStatus(string myDate)
        {
            DataTable dt = WorkingShifts.Service.DispatchingLog.DispatchingLog_SubmitStatus.GetDispatchingSubmitStatus(myDate, GetDataValidIdGroup("ProductionOrganization"));
            return DataGridJsonParser.DataTableToJson(dt);
        }
    }
}