using EasyUIJsonParser;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            string energyConsumptionAlarmLogs = json.JsonPick("energyConsumptionAlarmLogs");
            string performToObjectives = json.JsonPick("performToObjectives");
            string problemsAndSettlements = json.JsonPick("problemsAndSettlements");
            string equipmentSituation = json.JsonPick("equipmentSituation");
            string advicesToNextShift = json.JsonPick("advicesToNextShift");

            // 创建交接班日志主表记录
            string workingTeamShiftLogID = WorkingShiftsService.CreateShiftLog(organizationId, time, shifts, workingTeam, chargeMan, performToObjectives, problemsAndSettlements, equipmentSituation, advicesToNextShift);
            // 添加操作员记录
            WorkingShiftsService.CreateOperatorLogFromJson(workingTeamShiftLogID, operators);
            // 更新停机原因
            MachineHaltService.UpdateMachineHaltLogFromJson(workingTeamShiftLogID, haltLogs);
            // 更新DCS报警记录
            DCSSystemServcie.UpdateDCSWarningLogFromJson(workingTeamShiftLogID, dcsWarningLogs);
            // 更新能耗报警记录
            EnergyConsumptionAlarmLogService.UpdateEnergyConsumptionAlarmLogFromJson(workingTeamShiftLogID, energyConsumptionAlarmLogs);

            return "success";
        }

        #endregion

        #region Read

        /// <summary>
        /// 获取班组
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetWorkingTeamWithComboboxFormat(string organizationId)
        {
            DataTable dt = StaffService.GetWorkingTeamByOrganizationId(organizationId);
            return ComboboxJsonParser.DataTableToJson(dt, "Name", "ChargeManID");
        }

        /// <summary>
        /// 依据班组获取负责人
        /// </summary>
        /// <param name="workingTeamName"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetChargeManByWorkingTeamNameWithComboboxFormat(string workingTeamName)
        {
            DataTable dt = StaffService.GetChargeManByWorkingTeamName(workingTeamName);
            if (dt.Rows.Count == 0)
                return "";
            return "{\"ID\":\"" + dt.Rows[0]["StaffInfoID"].ToString() + "\", \"Name\":\"" + dt.Rows[0]["Name"].ToString() + "\"}";
        }

        /// <summary>
        /// 获取职工信息
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取DCS系统信息
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetDCSSystemWithDataGridFormat(string organizationId)
        {
            DataTable dt = DCSSystemServcie.GetDCSSystemByOrganizationId(organizationId);
            return DataGridJsonParser.DataTableToJson(dt, "Name", "OrganizationID");
        }
        //获取班次时间
        [WebMethod]
        public static string GetShiftTimeInfo(string organizationId,string shift)
        {
            string json=ShiftTimeService.GetShiftTime(organizationId, shift);
            return json;
        }

        /// <summary>
        /// 获取DCS报警记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetDCSWarningLogWithDataGridFormat(string organizationId,string startTime,string endTime)
        {
            DataTable dt = DCSSystemServcie.GetDCSWarningLog(organizationId,startTime,endTime);
            return DataGridJsonParser.DataTableToJson(dt);
        }

        /// <summary>
        /// 获取停机记录原因
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetMachineHaltReasonsWithCombotreeFormat()
        {
            DataTable dt = MachineHaltService.GetMachineHaltReasons();
            string json=TreeJsonParser.DataTableToJsonByLevelCode(dt, "MachineHaltReasonID", "ReasonText", "Remarks");
            return json;
           // return EasyUIJsonParser.ComboboxJsonParser.DataTableToJson(dt, "MachineHaltReasonID", "ReasonText", "Remarks");
        }

        /// <summary>
        /// 获取停机记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetMachineHaltLogWithDataGridFormat(string organizationId,string startTime,string endTime)
        {
            DataTable dt = MachineHaltService.GetMachineHaltLog(organizationId,startTime,endTime);
            return DataGridJsonParser.DataTableToJson(dt);
        }

        /// <summary>
        /// 获取能耗报警记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetEnergyConsumptionAlarmLogWithDataGridFormat(string organizationId, string startTime, string endTime)
        {
            DataTable dt = EnergyConsumptionAlarmLogService.GetEnergyConsumptionAlarmLog(organizationId, startTime,endTime);
            return DataGridJsonParser.DataTableToJson(dt);
        }
        /// <summary>
        /// 读取分厂的OrganizationID
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetAppSettingValue()
        {
            return ConfigurationManager.AppSettings["StationId"];
        }

        #endregion

    }
}