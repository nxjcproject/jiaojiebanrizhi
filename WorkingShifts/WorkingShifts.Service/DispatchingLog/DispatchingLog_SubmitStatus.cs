using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EasyUIJsonParser;
using SqlServerDataAdapter;
using SqlServerDataAdapter.Infrastruction;
using WorkingShifts.Infrastructure.Configuration;

namespace WorkingShifts.Service.DispatchingLog
{
    public class DispatchingLog_SubmitStatus
    {
        private static readonly string _connStr = ConnectionStringFactory.NXJCConnectionString;
        private static readonly ISqlServerDataFactory _dataFactory = new SqlServerDataFactory(_connStr);
        public static DataTable GetDispatchingSubmitStatus(string myDate, List<string> myOrganisationIdList)
        {
            string m_Sql = @"Select 
                    A.OrganizationID as OrganizationId, 
                    A.Name as Name,
                    (case when B.OrganizationID != '' then 1 else 0 end) as SubmitStatus
                    from system_Organization A
                    left join shift_DispatchingLog B on A.OrganizationID = B.OrganizationID
                       and B.DispatchingDate = '{3}'
					where A.Enabled = {1} 
                    and len(A.LevelCode) = {2}
                    and A.OrganizationID in {0}";
            string m_SqlCondition = "";                 //数据数据授权

            //tz_Formula B, formula_FormulaDetail C
            if (myOrganisationIdList != null)                //数据授权约束
            {
                for (int i = 0; i < myOrganisationIdList.Count; i++)
                {
                    if (i == 0)
                    {
                        m_SqlCondition = "'" + myOrganisationIdList[i] + "'";
                    }
                    else
                    {
                        m_SqlCondition = m_SqlCondition + ",'" + myOrganisationIdList[i] + "'";
                    }
                }
            }

            if (m_SqlCondition != "")
            {
                m_Sql = string.Format(m_Sql, "(" + m_SqlCondition + ")", "1", "3", myDate);
            }
            else
            {
                m_Sql = string.Format(m_Sql, "A.OrganizationID <> A.OrganizationID", "1", "3", myDate);
            }

            try
            {
                DataTable m_Result = _dataFactory.Query(m_Sql);
                return m_Result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
