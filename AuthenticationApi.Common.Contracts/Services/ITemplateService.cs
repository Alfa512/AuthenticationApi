using System.Collections.Generic;

namespace AuthenticationApi.Common.Contracts.Services
{
    public interface ITemplateService
    {
        string Process(string fileName, Dictionary<string, string> values);
    }
}