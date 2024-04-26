namespace WeAreUnited;

partial class JobDriver_AskToUnite
{
    public static float 
        MaxOpinion = 100f,
        MaxSkillLevel = SkillRecord.MaxLevel;
    
    private static PawnRelationDef? mostImportantRelation;
    public static PawnRelationDef MostImportantRelation => 
        mostImportantRelation ??= DefDatabase<PawnRelationDef>.AllDefs.MaxBy(x => x.importance);

    private static float? maxImportance;
    public static float MaxImportance => maxImportance ??= MostImportantRelation.importance;

    public virtual float GetRelationFactor(PawnRelationDef relation) =>
        relation.importance / MaxImportance;
    
    public virtual float RelationsMultiplier => Settings.RelationsMultiplier;
    public virtual float RelationsFactor => 
        Target.GetRelations(Actor).Sum(GetRelationFactor) * RelationsMultiplier;

    public virtual float OpinionMultiplier => Settings.OpinionMultiplier;
    public virtual float OpinionFactor => 
        Target.relations.OpinionOf(Actor) / MaxOpinion * OpinionMultiplier;

    public virtual float SkillMultiplier => Settings.SkillMultiplier;
    public virtual float SkillFactor => 
        Actor.skills.GetSkill(SkillDefOf.Social).Level / MaxSkillLevel * SkillMultiplier;

    public virtual float Factor =>
        RelationsFactor + OpinionFactor + SkillFactor;
}