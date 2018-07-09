using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace Project.Domain.AggregatesModel
{
    public class ProjectProperty : ValueObject
    {
        public ProjectProperty(string key, string text, string value)
        {
            this.Text = text;
            this.Key = key;
            this.Value = value;
        }

        /// <summary>
        /// 项目id
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Value { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
