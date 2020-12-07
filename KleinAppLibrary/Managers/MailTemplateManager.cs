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
            output.Add(TemplateType.Confirm, _razorEngine.Compile("Hi! If you'd like to subscribe to receive daily data analysis for @Model.StationName, click the button below <button>Subscribe</button>!"));
            // ++++++ ANALYSIS TEMPLATE ++++++ //
            return output;
        }

        public IRazorEngineCompiledTemplate GetTemplate(TemplateType type) => _templates[type];
    }
}