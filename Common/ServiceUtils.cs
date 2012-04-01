using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace TemplateGenerator.Common
{
    /// <summary>
    /// Utility methods for determining service 
    /// </summary>
    public static class ServiceUtils
    {
        /// <summary>
        /// Determines whether [is process running].
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when service name is null or empty.</exception>
        /// <returns>
        /// 	<c>true</c> if [is process running]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsProcessRunning(string serviceName)
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException("serviceName");
            }

            return Process.GetProcessesByName(serviceName).Any(p => p != null);
        }

        /// <summary>
        /// Determines whether [is service running].
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when service name is null or empty.</exception>
        /// <returns>
        /// 	<c>true</c> if [is service running]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsServiceRunning(string serviceName)
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException("serviceName");
            }

            using (var serviceController = new ServiceController(serviceName))
            {
                if (serviceController.Status != ServiceControllerStatus.Stopped ||
                    serviceController.Status != ServiceControllerStatus.Paused)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Existses the specified service name.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when either machine or service name is null or empty.</exception>
        /// <returns>
        /// 	<c>true</c> if [is service running]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasService(string machineName, string serviceName)
        {
            if (machineName == null)
            {
                throw new ArgumentNullException("machineName");
            }
            if (serviceName == null)
            {
                throw new ArgumentNullException("serviceName");
            }

            var scs =
                ServiceController.GetServices(string.IsNullOrEmpty(machineName)
                                                  ? Environment.MachineName
                                                  : machineName);
            return Array.Exists(scs, sc => sc.ServiceName.Equals(serviceName,
                                                                 StringComparison.InvariantCultureIgnoreCase));
        }
    }
}