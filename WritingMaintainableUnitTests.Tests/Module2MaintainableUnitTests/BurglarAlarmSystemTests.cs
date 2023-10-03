using NSubstitute;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module2MaintainableUnitTests;

namespace WritingMaintainableUnitTests.Tests.Module2MaintainableUnitTests;

[TestFixture]
public class BurglarAlarmSystemTests
{
    #region Arm

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnArm_SystemIsArmed()
    {
        var sut = new BurglarAlarmSystem(null, null);
        sut.Arm();

        Assert.That(sut.SystemState, Is.EqualTo(BurglarAlarmSystemState.Armed));
    }

    [Test]
    public void DisarmedSystemInTamperAlarmState_OnArm_SystemRemainsDisarmed()
    {
        var alarmSounder = Substitute.For<ICanSoundTheAlarm>();
        var controlRoomNotifier = Substitute.For<ICanNotifyTheControlRoom>();

        var sut = new BurglarAlarmSystem(alarmSounder, controlRoomNotifier);
        sut.Tamper();

        sut.Arm();

        Assert.That(sut.SystemState, Is.EqualTo(BurglarAlarmSystemState.Disarmed));
    }

    #endregion

    #region Disarm

    [Test]
    public void ArmedSystemInNormalAlarmState_OnDisarm_SystemIsDisarmed()
    {
        var sut = new BurglarAlarmSystem(null, null);
        sut.Arm();

        sut.Disarm();

        Assert.That(sut.SystemState, Is.EqualTo(BurglarAlarmSystemState.Disarmed));
    }

    #endregion

    #region BreakIn

    [Test]
    public void ArmedSystemInNormalAlarmState_OnBreakIn_AlarmStateIsAlarm()
    {
        var alarmSounder = Substitute.For<ICanSoundTheAlarm>();
        var controlRoomNotifier = Substitute.For<ICanNotifyTheControlRoom>();

        var sut = new BurglarAlarmSystem(alarmSounder, controlRoomNotifier);
        sut.Arm();

        sut.BreakIn();

        Assert.That(sut.AlarmState, Is.EqualTo(BurglarAlarmState.Alarm));
    }

    [Test]
    public void ArmedSystemInNormalAlarmState_OnBreakIn_AlarmSoundIsMakingNoise()
    {
        var alarmSounder = Substitute.For<ICanSoundTheAlarm>();
        var controlRoomNotifier = Substitute.For<ICanNotifyTheControlRoom>();

        var sut = new BurglarAlarmSystem(alarmSounder, controlRoomNotifier);
        sut.Arm();

        sut.BreakIn();

        alarmSounder.Received().MakeTerribleNoise();
    }

    [Test]
    public void ArmedSystemInNormalAlarmState_OnBreakIn_ControlRoomIsNotifiedAboutBreakInAlarm()
    {
        var alarmSounder = Substitute.For<ICanSoundTheAlarm>();
        var controlRoomNotifier = Substitute.For<ICanNotifyTheControlRoom>();

        var sut = new BurglarAlarmSystem(alarmSounder, controlRoomNotifier);
        sut.Arm();

        sut.BreakIn();

        controlRoomNotifier.Received().NotifyBreakInAlarm();
    }

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnBreakIn_AlarmStateRemainsNormal()
    {
        var sut = new BurglarAlarmSystem(null, null);
        sut.BreakIn();

        Assert.That(sut.AlarmState, Is.EqualTo(BurglarAlarmState.Normal));
    }

    #endregion

    #region Tamper

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnTamper_AlarmStateIsTamper()
    {
        var alarmSounder = Substitute.For<ICanSoundTheAlarm>();
        var controlRoomNotifier = Substitute.For<ICanNotifyTheControlRoom>();

        var sut = new BurglarAlarmSystem(alarmSounder, controlRoomNotifier);
        sut.Tamper();

        Assert.That(sut.AlarmState, Is.EqualTo(BurglarAlarmState.Tamper));
    }

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnTamper_ControlRoomIsNotifiedAboutTamperAlarm()
    {
        var alarmSounder = Substitute.For<ICanSoundTheAlarm>();
        var controlRoomNotifier = Substitute.For<ICanNotifyTheControlRoom>();

        var sut = new BurglarAlarmSystem(alarmSounder, controlRoomNotifier);
        sut.Tamper();

        controlRoomNotifier.Received().NotifyTamperAlarm();
    }

    #endregion
}