using NSubstitute;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module2MaintainableUnitTests;

namespace WritingMaintainableUnitTests.Tests.Module2MaintainableUnitTests
{
    [TestFixture]
    public class ArmTests_DRY_SRP : BurglarAlarmSystemBaseTest
    {
        [Test]
        public void DisarmedSystemInNormalAlarmState_OnArm_SystemIsArmed()
        {
            SUTWithoutDependencies.Arm();

            Assert.That(SUTWithoutDependencies.SystemState, Is.EqualTo(BurglarAlarmSystemState.Armed));
        }

        [Test]
        public void DisarmedSystemInTamperAlarmState_OnArm_SystemRemainsDisarmed()
        {
            SUT.Tamper();

            SUT.Arm();

            Assert.That(SUT.SystemState, Is.EqualTo(BurglarAlarmSystemState.Disarmed));
        }
    }

    [TestFixture]
    public class DisarmTests_DRY_SRP : BurglarAlarmSystemBaseTest
    {
        [Test]
        public void ArmedSystemInNormalAlarmState_OnDisarm_SystemIsDisarmed()
        {
            SUTWithoutDependencies.Arm();

            SUTWithoutDependencies.Disarm();

            Assert.That(SUTWithoutDependencies.SystemState, Is.EqualTo(BurglarAlarmSystemState.Disarmed));
        }
    }

    [TestFixture]
    public class BreakInTests_DRY_SRP : BurglarAlarmSystemBaseTest
    {
        [Test]
        public void ArmedSystemInNormalAlarmState_OnBreakIn_AlarmStateIsAlarm()
        {
            SUT.Arm();

            SUT.BreakIn();

            Assert.That(SUT.AlarmState, Is.EqualTo(BurglarAlarmState.Alarm));
        }

        [Test]
        public void ArmedSystemInNormalAlarmState_OnBreakIn_AlarmSoundIsMakingNoise()
        {
            SUT.Arm();

            SUT.BreakIn();

            AlarmSounder.Received().MakeTerribleNoise();
        }

        [Test]
        public void ArmedSystemInNormalAlarmState_OnBreakIn_ControlRoomIsNotifiedAboutBreakInAlarm()
        {
            SUT.Arm();

            SUT.BreakIn();

            ControlRoomNotifier.Received().NotifyBreakInAlarm();
        }

        [Test]
        public void DisarmedSystemInNormalAlarmState_OnBreakIn_AlarmStateRemainsNormal()
        {
            SUTWithoutDependencies.BreakIn();

            Assert.That(SUTWithoutDependencies.AlarmState, Is.EqualTo(BurglarAlarmState.Normal));
        }
    }

    [TestFixture]
    public class TamperTests_DRY_SRP : BurglarAlarmSystemBaseTest
    {
        [Test]
        public void DisarmedSystemInNormalAlarmState_OnTamper_AlarmStateIsTamper()
        {
            SUT.Tamper();

            Assert.That(SUT.AlarmState, Is.EqualTo(BurglarAlarmState.Tamper));
        }

        [Test]
        public void DisarmedSystemInNormalAlarmState_OnTamper_ControlRoomIsNotifiedAboutTamperAlarm()
        {
            SUT.Tamper();

            ControlRoomNotifier.Received().NotifyTamperAlarm();
        }
    }

    public abstract class BurglarAlarmSystemBaseTest
    {
        protected ICanSoundTheAlarm AlarmSounder { get; private set; }
        protected ICanNotifyTheControlRoom ControlRoomNotifier { get; private set; }

        protected BurglarAlarmSystem SUT { get; private set; }
        protected BurglarAlarmSystem SUTWithoutDependencies { get; private set; }

        [SetUp]
        public void SetUp()
        {
            AlarmSounder = Substitute.For<ICanSoundTheAlarm>();
            ControlRoomNotifier = Substitute.For<ICanNotifyTheControlRoom>();

            SUT = new BurglarAlarmSystem(AlarmSounder, ControlRoomNotifier);
            SUTWithoutDependencies = new BurglarAlarmSystem(null, null);
        }
    }
}