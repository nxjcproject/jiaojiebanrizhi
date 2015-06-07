using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkingShifts.Service;
using System.Data;
using EasyUIJsonParser;

namespace WorkingShifts.Web.UI_DispatchingSummary
{
    public partial class DispatchingLog_Branch : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
#if DEBUG
                // 调试用,自定义的数据授权
                List<string> m_DataValidIdItems = new List<string>() { "zc_nxjc_qtx"};
                AddDataValidIdGroup("ProductionOrganization", m_DataValidIdItems);
#endif
                this.OrganisationTree.Organizations = GetDataValidIdGroup("ProductionOrganization");                 //向web用户控件传递数据授权参数
                this.OrganisationTree.PageName = "DispatchingLog_Branch.aspx";
                this.OrganisationTree.LeveDepth = 3;                                         //设定levelcode层次深度（层次码的位数）
            }
        }
    }
}