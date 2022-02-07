using NUnit.Framework;
using UnityAuxiliaryTools.Trigger;

namespace UnityAuxiliartyTools.Tests.Triggers
{
    public class TriggerTests
    {

        [Test]
        public void Triggered_EventCalled()
        {
            // Arrange
            var trigger = new RegularTrigger();
            
            // Act
            var triggered = 0;
            trigger.OnTriggered += () => triggered++;
            trigger.Trigger();
            
            // Assert
            Assert.AreEqual(1, triggered);
        }
        
    }
}