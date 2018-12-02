using System.Threading.Tasks;

namespace PW.Core.Conventions
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
