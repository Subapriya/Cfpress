using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.WindowsAzure;
using Microsoft.VisualStudio.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.Diagnostics.ServiceRuntime;

namespace CFPress.UmbracoMVCApplication
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            var config = DiagnosticMonitor.GetDefaultInitialConfiguration();

            config.DiagnosticInfrastructureLogs.ScheduledTransferLogLevelFilter = LogLevel.Error;
            config.DiagnosticInfrastructureLogs.ScheduledTransferPeriod = TimeSpan.FromMinutes(5);

            DiagnosticMonitor.Start("DiagnosticsConnectionString", config);

            return base.OnStart();
            return base.OnStart();
        }
    }
}
