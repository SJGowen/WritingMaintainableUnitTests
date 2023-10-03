using NSubstitute;
using NUnit.Framework;
using WritingMaintainableUnitTests.Module2MaintainableUnitTests;

namespace WritingMaintainableUnitTests.Tests.Module2MaintainableUnitTests;

[TestFixture]
public class BurglarAlarmSystemTests_DRY
{
    [SetUp]
    public void SetUp()
    {
        _alarmSounder = Substitute.For<ICanSoundTheAlarm>();
        _controlRoomNotifier = Substitute.For<ICanNotifyTheControlRoom>();

        _sut = new BurglarAlarmSystem(_alarmSounder, _controlRoomNotifier);
        _sutWithoutDependencies = new BurglarAlarmSystem(null, null);
    }

    #region Arm

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnArm_SystemIsArmed()
    {
        _sutWithoutDependencies.Arm();

        Assert.That(_sutWithoutDependencies.SystemState, Is.EqualTo(BurglarAlarmSystemState.Armed));
    }

    [Test]
    public void DisarmedSystemInTamperAlarmState_OnArm_SystemRemainsDisarmed()
    {
        _sut.Tamper();

        _sut.Arm();

        Assert.That(_sut.SystemState, Is.EqualTo(BurglarAlarmSystemState.Disarmed));
    }

    #endregion

    #region Disarm

    [Test]
    public void ArmedSystemInNormalAlarmState_OnDisarm_SystemIsDisarmed()
    {
        _sutWithoutDependencies.Arm();

        _sutWithoutDependencies.Disarm();

        Assert.That(_sutWithoutDependencies.SystemState, Is.EqualTo(BurglarAlarmSystemState.Disarmed));
    }

    #endregion

    #region BreakIn

    [Test]
    public void ArmedSystemInNormalAlarmState_OnBreakIn_AlarmStateIsAlarm()
    {
        _sut.Arm();

        _sut.BreakIn();

        Assert.That(_sut.AlarmState, Is.EqualTo(BurglarAlarmState.Alarm));
    }

    [Test]
    public void ArmedSystemInNormalAlarmState_OnBreakIn_AlarmSoundIsMakingNoise()
    {
        _sut.Arm();

        _sut.BreakIn();

        _alarmSounder.Received().MakeTerribleNoise();
    }

    [Test]
    public void ArmedSystemInNormalAlarmState_OnBreakIn_ControlRoomIsNotifiedAboutBreakInAlarm()
    {
        _sut.Arm();

        _sut.BreakIn();

        _controlRoomNotifier.Received().NotifyBreakInAlarm();
    }

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnBreakIn_AlarmStateRemainsNormal()
    {
        _sutWithoutDependencies.BreakIn();

        Assert.That(_sutWithoutDependencies.AlarmState, Is.EqualTo(BurglarAlarmState.Normal));
    }

    #endregion

    #region Tamper

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnTamper_AlarmStateIsTamper()
    {
        _sut.Tamper();

        Assert.That(_sut.AlarmState, Is.EqualTo(BurglarAlarmState.Tamper));
    }

    [Test]
    public void DisarmedSystemInNormalAlarmState_OnTamper_ControlRoomIsNotifiedAboutTamperAlarm()
    {
        _sut.Tamper();

        _controlRoomNotifier.Received().NotifyTamperAlarm();
    }

    #endregion

    private ICanSoundTheAlarm _alarmSounder;
    private ICanNotifyTheControlRoom _controlRoomNotifier;
    private BurglarAlarmSystem _sut;
    private BurglarAlarmSystem _sutWithoutDependencies;
}