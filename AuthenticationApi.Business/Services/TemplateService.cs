using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AuthenticationApi.Common.Contracts.Services;

namespace AuthenticationApi.Business.Services
{
    public class TemplateService : ITemplateService
    {
        public string Process(string fileName, Dictionary<string, string> values)
        {
            if (File.Exists(fileName))
            {
                var template = new StringBuilder(File.ReadAllText(fileName));

                foreach (var item in values)
                {
                    template.Replace(item.Key, item.Value);
                }
                return template.ToString();
            }
            else
            {
                throw new Exception(string.Format("file {0} not found", fileName));
            }
        }
    }
}