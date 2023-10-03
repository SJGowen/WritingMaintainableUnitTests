using NSubstitute;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module2MaintainableUnitTests;

namespace WritingMaintainableUnitTests.Tests.Module2MaintainableUnitTests;

[TestFixture]
public class ArmTests_DRY_SRP_DAMP
{
    [Test]
    public void DisarmedSystemInNormalAlarmState_OnArm_SystemIsArmed()
    {
        var testFactory = new BurglarAlarmSystemTestFactory();
        var sut = testFactory.CreateSystemInNormalStateWithoutDependencies();

        sut.Arm();

        Assert.That(sut.SystemState, Is.EqualTo(BurglarAlarmSystemState.Armed));
    }

    [Test]
    public void DisarmedSystemInTamperAlarmState_OnArm_SystemRemainsDisarmed()
    {
        var testFactory = new BurglarAlarmSystemTestFactory();
        var sut = testFactory.CreateDisarmedSystemWithTamperAlarm();

        sut.Arm();

        Assert.That(sut.SystemState, Is.EqualTo(BurglarAlarmSystemState.Disarmed));
    }
}

[TestFixture]
public class DisarmTests_DRY_SRP_DAMP
{
    [Test]
    public void ArmedSystemInNormalAlarmState_OnDisarm_SystemIsDisarmed()
    {
        var testFactory = new BurglarAlarmSystemTestFactory();
        var sut = testFactory.CreateArmedSystemWithoutDependencies();

        sut.Disarm();

        Assert.That(sut.SystemState, Is.EqualTo(BurglarAlarmSystemState.Disarmed));
    }
}

[TestFixture]
public class BreakInTests_DRY_SRP_DAMP
{
    [Test]
    public void ArmedSystemInNormalAlarmState_OnBreakIn_AlarmStateIsAlarm()
    {
        var testFactory = new BurglarAlarmSystemTestFactory();
        var sut = testFactory.CreateArmedSystem();

        sut.BreakIn();

        Assert.That(sut.AlarmState, Is.EqualTo(BurglarAlarmState.Alarm));
    }

    [Test]
    public void ArmedSystemInNormalAlarmState_OnBreakIn_AlarmSoundIsMakingNoise()
    {
        var testFactory = new BurglarAlarmSystemTestFactory();
        var sut = testFactory.CreateArmedSystem();

        sut.BreakIn();

        testFactory.AlarmSounder.Received().MakeTerribleNoise();
    }

    [Test]
    public void ArmedSystemInNormalAlarmState_OnBreakIn_ControlRoomIsNotifiedAboutBreakInAlarm()
    {
        var testFactory = new BurglarAlarmSystemTestFactory();
        var sut = testFactory.CreateArmedSystem();

        sut.BreakIn();

        testFactory.ControlRoomNotifier.Received().NotifyBreakInAlarm();
    }

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnBreakIn_AlarmStateRemainsNormal()
    {
        var testFactory = new BurglarAlarmSystemTestFactory();
        var sut = testFactory.CreateSystemInNormalStateWithoutDependencies();

        sut.BreakIn();

        Assert.That(sut.AlarmState, Is.EqualTo(BurglarAlarmState.Normal));
    }
}

[TestFixture]
public class TamperTests_DRY_SRP_DAMP
{
    [Test]
    public void DisarmedSystemInNormalAlarmState_OnTamper_AlarmStateIsTamper()
    {
        var testFactory = new BurglarAlarmSystemTestFactory();
        var sut = testFactory.CreateSystemInNormalState();

        sut.Tamper();

        Assert.That(sut.AlarmState, Is.EqualTo(BurglarAlarmState.Tamper));
    }

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnTamper_ControlRoomIsNotifiedAboutTamperAlarm()
    {
        var testFactory = new BurglarAlarmSystemTestFactory();
        var sut = testFactory.CreateSystemInNormalState();

        sut.Tamper();

        testFactory.ControlRoomNotifier.Received().NotifyTamperAlarm();
    }
}

public class BurglarAlarmSystemTestFactory
{
    public ICanSoundTheAlarm AlarmSounder { get; }
    public ICanNotifyTheControlRoom ControlRoomNotifier { get; }

    public BurglarAlarmSystemTestFactory()
    {
        AlarmSounder = Substitute.For<ICanSoundTheAlarm>();
        ControlRoomNotifier = Substitute.For<ICanNotifyTheControlRoom>();
    }

    private static BurglarAlarmSystem CreateBurglarAlarmSystem(ICanSoundTheAlarm alarmSounder,
        ICanNotifyTheControlRoom controlRoomNotifier)
    {
        return new BurglarAlarmSystem(alarmSounder, controlRoomNotifier);
    }

    public BurglarAlarmSystem CreateSystemInNormalState()
    {
        return CreateBurglarAlarmSystem(AlarmSounder, ControlRoomNotifier);
    }

    public BurglarAlarmSystem CreateSystemInNormalStateWithoutDependencies()
    {
        return CreateBurglarAlarmSystem(null, null);
    }

    public BurglarAlarmSystem CreateArmedSystem()
    {
        var burglarAlarmSystem = CreateSystemInNormalState();
        burglarAlarmSystem.Arm();

        return burglarAlarmSystem;
    }

    public BurglarAlarmSystem CreateArmedSystemWithoutDependencies()
    {
        var burglarAlarmSystem = CreateSystemInNormalStateWithoutDependencies();
        burglarAlarmSystem.Arm();

        return burglarAlarmSystem;
    }

    public BurglarAlarmSystem CreateDisarmedSystemWithTamperAlarm()
    {
        var burglarAlarmSystem = CreateSystemInNormalState();
        burglarAlarmSystem.Tamper();

        return burglarAlarmSystem;
    }
}