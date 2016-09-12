namespace RMS.AppServiceLayer.Zktime.Enums
{
    public enum ShiftProfileStatus
    {
        Valid = 0,
        LateIn = 1,
        EarlyOut = 2,
        EarlyIn = 3,
        LateOut = 4,
        MissingClockIn = 5,
        MissingClockOut = 6,
        NoShow = 7,
        OverStayed = 8,
        Training = 9
    }
}