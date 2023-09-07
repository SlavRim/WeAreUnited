namespace WeAreUnited;

public static class WorkGiver_AskForReunion
{
    public static bool ThrowReasonIfIncapable(Pawn pawn)
    {
        if (IsCapable(pawn))
            return true;

        JobFailReasonIs_Incapable(pawn, PawnCapacityDefOf.Talking);

        return false;
    }

    public static void JobFailReasonIs_Incapable(Pawn pawn, PawnCapacityDef capacity)
    {
        var capacityName = capacity.label;
        var argument = pawn.Named(Translations.PawnArgument);
        
        var reason = Translations.Incapable.Translate(capacityName, argument);
        
        JobFailReason.Is(reason);
    }

    public static bool IsCapable(Pawn pawn) =>
        pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking);

    private static bool IsValid(Pawn? pawn) => pawn is { Downed: false, Dead: false, RaceProps.Humanlike: true };
    
    public static bool CanDoJob(Pawn? pawn, Pawn? target)
    {
        if (!IsValid(pawn) || !IsValid(target)) 
            return false;

        if (!ThrowReasonIfIncapable(pawn) || !ThrowReasonIfIncapable(target))
            return false;

        var targetFaction = target.HomeFaction;

        if (pawn.HomeFaction is not { IsPlayer: true } pawnFaction) // allowed to only permanent colonist
            return false;

        if (targetFaction == pawnFaction) // already in the same faction
            return false;
        
        if (!pawn.relations.RelatedPawns.Contains(target)) // pawn has no relations with target
        {
            // if target has no faction
            // or target is an ancient who has no home
            var allow = targetFaction is null || targetFaction == Faction.OfAncients;
            
            if(!allow) return false;
        }
        
        if (target.HostileTo(pawn))
            return false;
        
#if v1_4
        if (target is not { guest.Recruitable: true }) // disallow non recruitable
        {
            JobFailReason.Is(Translations.Unrecruitable.Translate());
            return false;  
        }
#endif
        
        if (TrainableUtility.TrainedTooRecently(target))
        {
            JobFailReason.Is(Translations.Recently.Translate());
            return false;
        }
        
        if (!pawn.CanReserve(target, ignoreOtherReservations: true) || 
            GenAI.EnemyIsNear(target, MinDistFromEnemy))
            return false;
        
        return true;
    }
    
    public static Job? JobOnThing(Pawn? pawn, Thing? thing)
    {
        if (!CanDoJob(pawn, thing as Pawn)) return null;
        
        return JobMaker.MakeJob(Definitions.AskPawnToUnite, thing);
    }

    public static float MinDistFromEnemy = 40f;
}