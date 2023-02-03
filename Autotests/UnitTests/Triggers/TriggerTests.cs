using NUnit.Framework;
using AsyncReactAwait.Trigger;

namespace AsyncReactAwait.Tests.Triggers
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
            trigger.Triggered += () => triggered++;
            trigger.Trigger();
            
            // Assert
            Assert.AreEqual(1, triggered);
        }
        
    }
}