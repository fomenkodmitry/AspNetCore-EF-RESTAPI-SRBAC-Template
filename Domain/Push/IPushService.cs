using System;
using System.Threading.Tasks;

namespace Domain.Push
{
    public interface IPushService
    {
        Task SendTest(string pushToken);
    }
}