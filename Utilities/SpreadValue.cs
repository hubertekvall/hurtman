using Godot;

namespace Hurtman.Utilities;

public struct SpreadValue
{
    public static double GetRandomSpread(double baseValue, double spread)
    {
        return GD.RandRange(baseValue - (baseValue * spread), baseValue + (baseValue * spread));
    }
}