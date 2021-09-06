using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public class ConfigurationManager
    {
        private static SagaConfiguration SagaConfiguration;
        public static Action<SagaConfiguration> SetConfig = (sagaConfiguration) => SagaConfiguration = sagaConfiguration;
        public static Func<SagaConfiguration> GetConfig = () => SagaConfiguration;
    }
}
