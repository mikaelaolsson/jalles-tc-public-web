using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Dashboards;

namespace Jalles.Web.App_Plugins.StartDashboard;

[Weight(-10)]
public class StartDashboard : IDashboard
{
    public string Alias => "startDashboard";
    public string View => "/App_Plugins/StartDashboard/dashboard.html";
    public string[] Sections => new[] { Constants.Applications.Content };

    public IAccessRule[] AccessRules
    {
        get
        {
            return new IAccessRule[]
            {
                new AccessRule {Type = AccessRuleType.Grant, Value = Constants.Security.AdminGroupAlias},
                new AccessRule {Type = AccessRuleType.Grant, Value = Constants.Security.EditorGroupAlias},
                new AccessRule {Type = AccessRuleType.Grant, Value = Constants.Security.TranslatorGroupAlias}
            };
        }
    }
}
