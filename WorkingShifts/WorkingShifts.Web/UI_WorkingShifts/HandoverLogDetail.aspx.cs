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
        //}


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

    }
}