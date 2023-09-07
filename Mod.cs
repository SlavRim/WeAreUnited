global using Mod = WeAreUnited.Mod;

namespace WeAreUnited;

public sealed class Mod : Verse.Mod
{
    public static Mod Instance { get; private set; }
    
    public Mod(ModContentPack content) : base(content)
    {
        Instance = this;
    }
}
