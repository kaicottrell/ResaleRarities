using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class NotificationService
    {
        public event Action<string, string> OnShowNotification;

        public void ShowNotification(string message, string type)
        {
            OnShowNotification?.Invoke(message, type);
        }

        

    }
}
