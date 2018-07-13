using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Domain.AggregatesModel;
using Dapper;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Project.API.Application.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private string _constr;
        public ProjectQueries(string constr)
        {
            _constr = constr;
        }

        public async Task<dynamic> GetProjectDetail(int projectId, int userId)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(_constr))
            {
                string sql = @"SELECT
projects.Id
,projects.Company
,projects.Avatar
,projects.ProvinceId
,projects.FinStage
,projects.FinMoney
,projects.Valuation
,projects.FinPercentage
,projects.Introduction
,projects.UserId
,projects.Income
,projects.Revenue
,projects.UserName
,projects.BrokerageOption
,projectvisiblerules.Tags
,projectvisiblerules.Visible
FROM 
projects INNER JOIN projectvisiblerules 
ON projects.Id = projectvisiblerules.ProjectId
WHERE projects.id = @projectId";
                return await mySqlConnection.QueryAsync<dynamic>(sql, new { projectId });
            }
        }

        public async Task<dynamic> GetProjectsByUserId(int userId)
        {
            using (MySqlConnection mySqlConnection = new MySqlConnection(_constr))
            {
                string sql = @"SELECT
projects.Id
,projects.Company
,projects.Avatar
,projects.FinStage
,projects.Introduction
,projects.Tags
,projects.ShowSecurityInfo
,projects.CreateTime
FROM 
projects WHERE projects.UserId =@userId";
                return await mySqlConnection.QueryAsync<dynamic>(sql, new { userId });
            }
        }
    }
}
