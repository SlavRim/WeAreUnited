global using static WeAreUnited.Extensions;

namespace WeAreUnited;

public static partial class Extensions
{
    public static ModSettings Settings => Mod.Instance.Settings;

    public static Rect GetRect(this Listing_Standard layout, float height, float widthFactor)
    {
        return layout.GetRect(height
#if v1_4
        , widthFactor
#endif
        );
    }
    
    public static bool IsHomelessFaction(this Faction? faction) => 
        faction is null ||
        faction == Faction.OfAncients;
    
    public static bool IsCapableOf(this Pawn pawn, PawnCapacityDef capacity) =>
        pawn.health.capacities.CapableOf(capacity);
    
    public static bool IsCapableOf_WithFailReason(this Pawn pawn, PawnCapacityDef capacity)
    {   
        if (pawn.IsCapableOf(capacity))
            return true;

        var reason = GetIncapableReason(pawn, capacity);
        JobFailReason.Is(reason);

        return false;
    }

    public static string GetIncapableReason(this Pawn pawn, PawnCapacityDef capacity) =>
        Translations.Incapable.Translate(capacity.label, pawn.Named(Translations.PawnArgument));
}