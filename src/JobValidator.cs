namespace WeAreUnited;

public readonly ref partial struct JobValidator(Pawn Recruiter, Pawn Target)
{
    private static bool IsValid(Pawn? pawn) => pawn is
    {
        Downed: false, 
        Dead: false,
#if v1_4
        RaceProps.IsMechanoid: false,
#endif
        RaceProps.Humanlike: true
    };

    public static bool ValidateCapable(Pawn pawn) =>
        (!Settings.ValidateCapability) || (
        pawn.IsCapableOf_WithFailReason(PawnCapacityDefOf.Talking) &&
        pawn.IsSocialWorkEnabled_WithFailReason());

    public static bool IsColonist(Pawn pawn, out Faction? faction) =>
        (faction = pawn.HomeFaction) is { IsPlayer: true };

    public static bool IsCurrentlyColonist(Pawn pawn, out Faction? faction) =>
        (faction = pawn.Faction) is { IsPlayer: true };
    
    public Result Validate()
    {
        if (!IsColonist(Recruiter, out var recruiterFaction) ||
            IsColonist(Target, out var targetFaction))
            return false;

        if (Recruiter.IsSlave) // not sure if it ever will be true
            return false;

        if (!IsCurrentlyColonist(Target, out _))
        {
            if(Target.IsSlave)
                return false;
        }
        
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

    public bool Related => Recruiter.relations?.RelatedPawns.Contains(Target) ?? false;

    public static Result Validate(Pawn? recruiter, Pawn? target)
    {
        if (!IsValid(recruiter) || !IsValid(target)) 
            return false;
        
        var validator = new JobValidator(recruiter!, target!);
        
        return validator.Validate();
    }
    
    public static Job? TryMakeJob(Pawn? recruiter, Thing? thing)
    {
        var target = thing as Pawn;
        if (!Validate(recruiter, target)) return null;
        
        return MakeJob(target!);
    }

    private static Job MakeJob(Pawn target) => 
        JobMaker.MakeJob(Definitions.AskPawnToUnite, target);
}