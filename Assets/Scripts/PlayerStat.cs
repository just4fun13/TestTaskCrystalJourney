namespace Assets.Scripts
{
    public class PlayerStat : UnitStats
    {
        public void GetCrystal() => crystalCount++;
        public void GotDamaged() => lifesCount--;
        public bool IsAlive => lifesCount > 0;
        public int lifesCount { get; private set; } = 3;
        public int crystalCount { get; private set; } = 0;
    }
}
