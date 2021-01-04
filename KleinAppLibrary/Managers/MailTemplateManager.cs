using KleinMapLibrary.Enums;
using KleinMapLibrary.Interfaces;
using RazorEngineCore;
using System.Collections.Generic;

namespace KleinMapLibrary.Managers
{
    public class MailTemplateManager : IMailTemplateManager
    {
        private readonly IRazorEngine _razorEngine;
        private readonly IDictionary<TemplateType, IRazorEngineCompiledTemplate> _templates;
        public MailTemplateManager(IRazorEngine razorEngine)
        {
            _razorEngine = razorEngine;
            _templates = InitializeTemplates();
        }
        private IDictionary<TemplateType, IRazorEngineCompiledTemplate> InitializeTemplates()
        {
            Dictionary<TemplateType, IRazorEngineCompiledTemplate> output = new Dictionary<TemplateType, IRazorEngineCompiledTemplate>();
            // ++++++ CONFIRM TEMPLATE ++++++ //
            output.Add(TemplateType.Confirm, _razorEngine.Compile(
                "<h1 style=\"margin: 0 auto;width: max-content; color: #3abbfb;\">KleinApp</h1>" +
                "<h2 style=\"text-align: center;\">Hi! If you'd like to subscribe to receive daily data analysis for @Model.StationsCount selected stations, click the button below or go to the link!</h2>" +
                "<form style=\"display: flex; margin-top: 10px;\">" +
                    "<a href=\"@Model.KleinAppAddress\" style=\"width: 220px; font-size: 30px; font-weight: bold; color: white; " +
                        "background-color: #00b08c; border: none; border-radius: 5px; margin: 0 auto; text-align: center; text-decoration: none;\">" +
                        "Subscribe" +
                    "</a>" +
                "</form>"));
            // ++++++ ANALYSIS TEMPLATE ++++++ //
            output.Add(TemplateType.Analysis, _razorEngine.Compile("" +
                "<h1 style=\"margin: 0 auto;width: max-content; color: #3abbfb;\">KleinApp daily e-mail</h1>" +
                "@foreach(var station in Model.Stations) {" +
                "<p>@station?.stationName</p>}"));
            return output;
        }

        public IRazorEngineCompiledTemplate GetTemplate(TemplateType type) => _templates[type];
    }
}