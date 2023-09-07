namespace WeAreUnited;

public sealed partial class ModSettings : Verse.ModSettings
{
    public ModSettings()
    {
        Elements = (elements = new()
        {
            AllowNotRelated,
#if v1_4
            AllowLoyal,
#endif
            MinDistanceFromEnemy
        }).AsReadOnly();
    }

    public readonly Listing_Standard Layout = new();
    
    public Toggle 
        AllowNotRelated = new(nameof(AllowNotRelated), "Allow not related", true),
        AllowLoyal = new(nameof(AllowLoyal), "Allow unwavering loyal", false);
    public Numeric<float> 
        MinDistanceFromEnemy = new(nameof(MinDistanceFromEnemy), "Minimum distance from enemy", DefaultValue: 40f);
    
    public void Draw(IElement element) => Draw(element, null);

    public void Draw(Rect rect)
    { 
        rect = new Rect(25, 50, rect.width, rect.height - 16f);
        
        Layout.Begin(rect);

        ForEach(Draw);
        
        Layout.End();
    }
    
    public void ExposeData(IElement element) => element.ExposeData();
    
    public override void ExposeData() => ForEach(ExposeData);
}