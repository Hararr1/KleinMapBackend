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
                "<h2 style=\"font-size: 1.5rem; text-align: center; color: #0096FF;bmargin: 10px auto;\">" +
                "@station?.stationName" +
                "</h2>" +
                "<table style=\"border: 1px solid black; background: #041723; border-radius: 5px; margin: 0 auto;\"> " +
                "   <thead>" +
                "       <th style=\"font-size: 1.2rem; background-color: #5fdce6; color: #000000;height: 30px;\">Type</th>" +
                "       <th style=\"font-size: 1.2rem; background-color: #5fdce6; color: #000000;height: 30px;\">Last Update</th>" +
                "       <th style=\"font-size: 1.2rem; background-color: #5fdce6; color: #000000;height: 30px;\">State</th>" +
                "       <th style=\"font-size: 1.2rem; background-color: #5fdce6; color: #000000;height: 30px;\">Worst</th>" +
                "       <th style=\"font-size: 1.2rem; background-color: #5fdce6; color: #000000;height: 30px;\">Value</th>" +
                "   </thead>" +
                "   <tbody>" +
                "       @foreach(var sensor in station?.sensors) {" +
                "       <tr>" +
                "          <td style=\"padding: 7px 3px; font-size: 1rem; text-align: center; color: white;\">@sensor?.param?.paramName</td>  " +
                "          <td style=\"padding: 7px 3px; font-size: 1rem; text-align: center; color: white;\">@sensor?.lastUpdate</td>  " +
                "          <td style=\"padding: 7px 3px; font-size: 1rem; text-align: center; color: white;\">@sensor?.state</td>  " +
                "          <td style=\"padding: 7px 3px; font-size: 1rem; text-align: center; color: white;\">@sensor?.worstValue</td>  " +
                "          <td style=\"padding: 7px 3px; font-size: 1rem; text-align: center; color: white;\">@sensor?.currentValue</td>  " +
                "       </tr> }" +
                "   </tbody> " +
                "</table>}"));
            return output;
        }

        public IRazorEngineCompiledTemplate GetTemplate(TemplateType type) => _templates[type];
    }
}