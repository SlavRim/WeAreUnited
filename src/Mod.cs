global using Mod = WeAreUnited.Mod;

namespace WeAreUnited;

public sealed class Mod : Verse.Mod
{
    public static Mod Instance { get; private set; }

    private new ModSettings modSettings;
    public ModSettings Settings => modSettings ??= GetSettings<ModSettings>();
    
    public Mod(ModContentPack content) : base(content)
    {
        Instance = this;
    }

    public override string SettingsCategory() => GenText.SplitCamelCase(nameof(WeAreUnited));

    public override void DoSettingsWindowContents(Rect rect) => Settings.Draw(rect);
}