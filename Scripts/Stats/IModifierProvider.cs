using System.Collections.Generic;

namespace U_RPG.Stats
{
    // Interface for weapons. Calculate additive and percentage damages.
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}