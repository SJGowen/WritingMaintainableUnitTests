namespace WritingMaintainableUnitTests.Module2MaintainableUnitTests
{
    public class BurglarAlarmSystem
    {
        private readonly ICanSoundTheAlarm _alarmSounder;
        private readonly ICanNotifyTheControlRoom _controlRoomNotifier;

        public BurglarAlarmState AlarmState { get; private set; }
        public BurglarAlarmSystemState SystemState { get; private set; }

        public BurglarAlarmSystem(ICanSoundTheAlarm alarmSounder, ICanNotifyTheControlRoom controlRoomNotifier)
        {
            _alarmSounder = alarmSounder;
            _controlRoomNotifier = controlRoomNotifier;
        }

        public void Arm()
        {
            if (AlarmState == BurglarAlarmState.Tamper)
                return;

            SystemState = BurglarAlarmSystemState.Armed;
        }

        public void Disarm()
        {
            SystemState = BurglarAlarmSystemState.Disarmed;
        }

        public void BreakIn()
        {
            if (SystemState != BurglarAlarmSystemState.Armed)
                return;

            AlarmState = BurglarAlarmState.Alarm;

            _alarmSounder.MakeTerribleNoise();
            _controlRoomNotifier.NotifyBreakInAlarm();
        }

        public void Tamper()
        {
            AlarmState = BurglarAlarmState.Tamper;
            _controlRoomNotifier.NotifyTamperAlarm();
        }
    }

    public enum BurglarAlarmSystemState
    {
        Disarmed = 0,
        Armed = 1
    }

    public enum BurglarAlarmState
    {
        Normal = 0,
        Alarm = 2,
        Tamper = 3
    }

    public interface ICanSoundTheAlarm
    {
        void MakeTerribleNoise();
    }

    public interface ICanNotifyTheControlRoom
    {
        void NotifyBreakInAlarm();
        void NotifyTamperAlarm();
    }
}