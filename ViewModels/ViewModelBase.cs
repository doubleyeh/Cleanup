using Cleanup.Utils;
using ReactiveUI;

namespace Cleanup.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        protected static bool IsAdmin => AppUtil.IsRunningAsAdministrator();
    }
}
