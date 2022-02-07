using NUnit.Framework;
using UnityAuxiliaryTools.Trigger;
using UnityAuxiliaryTools.Trigger.Factory;

namespace UnityAuxiliartyTools.Tests.Triggers
{
    public class TriggerFactoryTests
    {
        [Test]
        public void CreateRegularTrigger_RegularTriggerCreated()
        {
            // Arrange
            var factory = new TriggerFactory();
            
            // Act
            var regularTrigger = factory.CreateRegularTrigger();
            
            // Assert
            Assert.IsTrue(regularTrigger is RegularTrigger);
        }
    }
}