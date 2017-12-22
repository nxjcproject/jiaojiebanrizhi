using EasyUIJsonParser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkingShifts.Service;

namespace WorkingShifts.Web.UI_WorkingShifts
{
    public partial class HandoverLoger : WebStyleBaseForEnergy.webStyleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.InitComponts();
#if DEBUG
            mPageOpPermission = "1111";
#endif
            ShiftDateTime.Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");          //交接班日志统一为打开填写交接班日志页面的这一刻起
        }

        /// <summary>
        /// 增删改查权限控制
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static char[] AuthorityControl()
        {
            return mPageOpPermission.ToArray();
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
            // todo: 事务处理应放在服务层，重构时应优化。
            if (mPageOpPermission.ToArray()[1] == '1')
            {
                string time = json.JsonPick("time");
                string shifts = json.JsonPick("shifts");
                string workingTeam = json.JsonPick("workingTeam");
                string chargeMan = json.JsonPick("chargeMan");
                string operators = json.JsonPick("operators");
                string haltLogs = json.JsonPick("haltLogs");
                string dcsWarningLogs = json.JsonPick("dcsWarningLogs");
                string energyConsumptionAlarmLogs = json.JsonPick("energyConsumptionAlarmLogs");
                string stocktakingInfos = json.JsonPick("stocktakingInfos");
                string performToObjectives = json.JsonPick("performToObjectives");
                string problemsAndSettlements = json.JsonPick("problemsAndSettlements");
                string equipmentSituation = json.JsonPick("equipmentSituation");
                string advicesToNextShift = json.JsonPick("advicesToNextShift");

                using (TransactionScope tsCope = new TransactionScope())
                {
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
                    // 添加盘库信息
                    StocktakingService.SaveStocktakingInfo(workingTeamShiftLogID, shifts, stocktakingInfos);

                    tsCope.Complete();
                }

                return "success";
            }
            else
            {
                return "noright";
            }
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

        /// <summary>
        /// 获取班次信息
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetShiftsInfo(string organizationId)
        {
            return ShiftTimeService.GetShiftSchedule(organizationId);
        }

        /// <summary>
        /// 获取DCS报警记录
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetDCSWarningLogWithDataGridFormat(string organizationId,string startTime,string endTime)
        {

            DataTable dt = DCSSystemServcie.GetDCSWarningLog(organizationId, startTime, endTime);
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
            DataTable dt = MachineHaltService.GetMachineHaltLog(organizationId, startTime, endTime);
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
            DataTable dt = EnergyConsumptionAlarmLogService.GetEnergyConsumptionAlarmLog(organizationId, startTime, endTime);
            return DataGridJsonParser.DataTableToJson(dt);
        }

        /// <summary>
        /// 获取原始盘库信息
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string GetOriginalStocktakingInfoWithDataGridFormat(string organizationId, bool getCurrentShiftData)
        {
            DataTable dt = StocktakingService.GetOriginalStocktakingInfo(organizationId, getCurrentShiftData);
            return DataGridJsonParser.DataTableToJson(dt);
        }

        /// <summary>
        /// 读取分厂的OrganizationID
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public static string GetFactoryOrganizationId()
        {
            string m_OrganizationId = ConfigurationManager.AppSettings["StationId"];
            DataTable m_OrganizationIdTable = WorkingShiftsService.GetFactoryOrganizationId(m_OrganizationId);
            string m_OrganizationIdString = EasyUIJsonParser.ComboboxJsonParser.DataTableToJson(m_OrganizationIdTable);
            return m_OrganizationIdString;
        }

        #endregion

        #region 操作员相关

        /// <summary>
        /// 获取工段信息
        /// </summary>
        /// <param name="organizationId">组织机构ID（分厂）</param>
        /// <returns>DataGrid列信息</returns>
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
                sb.Append("',");
                sb.Append("formatter:function(value,row){return row.StaffName_" + id + ";},");
                sb.Append("editor:{type:'combobox',options:{valueField:'StaffID',textField:'Combined',data:logerData.getStaffInfo()}}");
                sb.Append("},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]]");

            return sb.ToString();
        }

        [WebMethod]
        public static string GetOperatorsLog(string workingTeamShiftLogId)
        {
            workingTeamShiftLogId = "3E7B0F32-12F7-4C37-96FC-13830181906B";
            DataTable dt = OperatorService.GetOperatorsLogHorizontal(workingTeamShiftLogId);
            return DataGridJsonParser.DataTableToJson(dt);
        }


        [WebMethod]
        public static string GetLastOperatorsLog(string organizationId, string workingTeam)
        {
            DataTable dt = OperatorService.GetLastOperatorsLogHorizontal(organizationId, workingTeam);
            return DataGridJsonParser.DataTableToJson(dt);
        }

        #endregion

    }
}