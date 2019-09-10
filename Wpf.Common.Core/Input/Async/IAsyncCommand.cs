using System.Threading.Tasks;
using System.Windows.Input;

namespace Wpf.Common.Input
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }

}