namespace U_RPG.Saving
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object State);
    }
}