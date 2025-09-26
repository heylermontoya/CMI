using NUnit.Framework;

namespace CMI.Api.Tests
{
    [SetUpFixture]
    public class GlobalTestSetup
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            // Configuración global si es necesario
        }
    }
}
