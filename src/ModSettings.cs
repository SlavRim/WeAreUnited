namespace WeAreUnited;

public sealed partial class ModSettings : Verse.ModSettings
{
    public ModSettings()
    {
        Elements = (elements = new()
        {
            ValidateCapability,
            AllowNotRelated,
#if !v1_3
            AllowLoyal,
#endif
            MinDistanceFromEnemy,
            
            RelationsMultiplier,
            SkillMultiplier,
            OpinionMultiplier
        }).AsReadOnly();
    }
    
    public Toggle 
        ValidateCapability = new(nameof(ValidateCapability), "Talking capability required", true),
        AllowNotRelated = new(nameof(AllowNotRelated), "Allow not related", true),
        AllowLoyal = new(nameof(AllowLoyal), "Allow unwavering loyal", false);
    public Numeric<float> 
        MinDistanceFromEnemy = new(nameof(MinDistanceFromEnemy), "Minimum distance from enemy", DefaultValue: 40f),
        RelationsMultiplier = new(nameof(RelationsMultiplier), "Relations multiplier", DefaultValue: 0.25f),
        SkillMultiplier = new(nameof(SkillMultiplier), "Skill multiplier", DefaultValue: 0.5f),
        OpinionMultiplier = new(nameof(OpinionMultiplier), "Opinion multiplier", DefaultValue: 0.5f);
    
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