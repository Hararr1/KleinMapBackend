using KleinMapLibrary.Enums;
using RazorEngineCore;

namespace KleinMapLibrary.Interfaces
{
    public interface IMailTemplateManager
    {
        IRazorEngineCompiledTemplate GetTemplate(TemplateType type);
    }
}