using Project.Domain.Events;
using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Domain.AggregatesModel
{
    public class Project:Entity, IAggregateRoot
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        // public override int Id { get; protected set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 项目基本介绍
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 项目logo
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 原bpfile
        /// </summary>
        public string OriginBPFile { get; set; }
        /// <summary>
        /// 转化后bffile
        /// </summary>
        public string FormatBPFile { get; set; }

        /// <summary>
        /// 是否显示敏感信息
        /// </summary>
        public bool ShowSecurityInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ProvinceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CityId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 融资比例
        /// </summary>
        public int FinPercentage { get; set; }

        /// <summary>
        /// 融资阶段
        /// </summary>
        public int FinStage { get; set; }
        /// <summary>
        /// 融资金额
        /// </summary>
        public string FinMoney { get; set; }
        /// <summary>
        /// 收入
        /// </summary>
        public int Income { get; set; }

        /// <summary>
        /// 利润
        /// </summary>
        public int Revenue { get; set; }

        /// <summary>
        /// 估值
        /// </summary>
        public string Valuation { get; set; }
        /// <summary>
        /// 项目标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 佣金分配方式 枚举  线下 商议 等比例分配
        /// </summary>
        public string BrokerageOption { get; set; }


        /// <summary>
        /// 原项目id
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 引用项目id
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// 项目枚举属性
        /// </summary>
        public List<ProjectProperty> Properties { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public List<ProjectVisibleRules> ProjectVisibleRule { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ProjectContributor> Contributors { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ProjectViewer> Viewers { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public Project CloneProject(Project source =null)
        {
            if (source == null) return this;
            var newProject = new Project
            {

                AreaId = source.AreaId,
                BrokerageOption = source.BrokerageOption,
                Avatar = source.Avatar,
                CityId = source.CityId,
                Company = source.Company,
                CityName = source.CityName,
                CreateTime = source.CreateTime,
                AreaName = source.AreaName,
                FinMoney = source.FinMoney,
                FinPercentage = source.FinPercentage,
                FinStage = source.FinStage,
                FormatBPFile = source.FormatBPFile,
                Income = source.Income,
                Introduction = source.Introduction,
                OriginBPFile = source.OriginBPFile,
                ProvinceId = source.ProvinceId,
                ProvinceName = source.ProvinceName,
                Contributors = null,
                ProjectVisibleRule = null,
                Properties = null,
                Viewers = null,
                ReferenceId = source.ReferenceId,
                RegisterTime = source.RegisterTime,
                SourceId = source.SourceId,
                Revenue = source.Revenue,
                Tags = source.Tags,
                UpdateTime = source.UpdateTime,
                UserId = source.UserId,
                Id = source.Id,

                Valution = source.Valution

            };
            foreach(var item in source.Properties)
            {
                newProject.Properties.Add(new ProjectProperty(item.Key, item.Text, item.Value));
            }

            return newProject;
        }

        public Project ContributorFork(int contributorId, Project source = null)
        {
            if (source == null)
                source = this;
            var newProject = CloneProject(source);
            newProject.UserId = contributorId;
            newProject.SourceId = source.SourceId == 0 ? source.Id : source.SourceId;
            //TBD
            newProject.ReferenceId = source.ReferenceId == 0 ? source.Id : source.ReferenceId;
            newProject.UpdateTime = DateTime.Now;
            return newProject;
        }

        public Project()
        {
            this.Viewers = new List<ProjectViewer>();
            this.Contributors = new List<ProjectContributor>();
            this.AddDomainEvent(new ProjectCreatedEvent { Project = this });
        }

        public void AddViewer(int userId, string userName, string avatar)
        {
            var viewer = new ProjectViewer
            {
                UserId = userId,
                UserName = userName,
                Avatar = avatar
            };
            if (!Viewers.Any(v => v.UserId == userId))
            {
                this.AddDomainEvent(new ProjectViewedEvent { Viewer = viewer });
                Viewers.Add(viewer);
            }
        }

        public void AddContributor(ProjectContributor contributor)
        {
            if (!Contributors.Any(v => v.Id == contributor.Id))
            {
                this.AddDomainEvent(new ProjectJoinedEvent { Contributor = contributor });
                Contributors.Add(contributor);
            }
        }
    }
}
