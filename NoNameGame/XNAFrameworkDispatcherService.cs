using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xna.Framework;

namespace NoNameGame
{
    public class XNAFrameworkDispatcherService : IApplicationService
    {
        private DispatcherTimer frameworkDispatcherTimer;

        public XNAFrameworkDispatcherService()
        {
            this.frameworkDispatcherTimer = new DispatcherTimer();
            this.frameworkDispatcherTimer.Interval = TimeSpan.FromTicks(333333);
            this.frameworkDispatcherTimer.Tick += frameworkDispatcherTimer_Tick;
            FrameworkDispatcher.Update();
        }
        void frameworkDispatcherTimer_Tick(object sender, EventArgs e) { FrameworkDispatcher.Update(); }

        void IApplicationService.StopService() { this.frameworkDispatcherTimer.Stop(); }

        void IApplicationService.StartService(ApplicationServiceContext context) { this.frameworkDispatcherTimer.Start(); }
    }
}
