namespace WeAreUnited;

public readonly ref partial struct JobValidator(Pawn Recruiter, Pawn Target)
{
    private static bool IsValid(Pawn? pawn) => pawn is
    {
        Downed: false, 
        Dead: false, 
        RaceProps.Humanlike: true
    };

    public static bool ValidateCapable(Pawn pawn) =>
        pawn.IsCapableOf_WithFailReason(PawnCapacityDefOf.Talking);

    public static bool IsColonist(Pawn pawn, out Faction? faction) =>
        (faction = pawn.HomeFaction) is { IsPlayer: true };
    
    public Result Validate()
    {
        if (!IsColonist(Recruiter, out var recruiterFaction) ||
            IsColonist(Target, out var targetFaction))
            return false;
        
        if (!Related)
        {
            bool allow = Settings.AllowNotRelated;
            allow &= targetFaction.IsHomelessFaction();
            
            if(!allow) return false;
        }
        
        if (!Reservable || EnemiesNearby)
            return false;
        
        if (!ValidateCapable(Recruiter) || !ValidateCapable(Target))
            return false;

        if (!Recruitable)
            return Translations.Unrecruitable.Translate();
        
        if (AttemptedRecently)
            return Translations.Recently.Translate();
        
        if (Hostile)
            return false;
        
        return true;
    }

    public bool Recruitable =>
#if v1_4
        Target is { guest.Recruitable: true } || Settings.AllowLoyal;
#else
        true;
#endif

    /// Expensive Invocation
    public bool Hostile => Target.HostileTo(Recruiter);
    
    public bool AttemptedRecently => TrainableUtility.TrainedTooRecently(Target);

    public bool EnemiesNearby => GenAI.EnemyIsNear(Target, Settings.MinDistanceFromEnemy);

    public bool Reservable => Recruiter.CanReserve(Target, ignoreOtherReservations: true);

    public bool Related => Recruiter.relations.RelatedPawns.Contains(Target);

    public static Result Validate(Pawn? recruiter, Pawn? target)
    {
        if (!IsValid(recruiter) || !IsValid(target)) 
            return false;
        
        var validator = new JobValidator(recruiter, target);
        
        return validator.Validate();
    }
    
    public static Job? TryGetJob(Pawn? pawn, Thing? thing)
    {
        if (!Validate(pawn, thing as Pawn)) return null;
        
        return JobMaker.MakeJob(Definitions.AskPawnToUnite, thing);
    }
}