using RimWorld.Planet;
using Verse.AI.Group;

namespace WeAreUnited;

public partial class JobDriver_AskToUnite : JobDriver
{
    public UniteComp_Properties Properties => Actor.def.GetCompProperties<UniteComp_Properties>();
    
    public Pawn Actor => pawn;
    private Pawn? target;
    public Pawn Target => target ??= job.GetTarget(TargetIdx).Pawn;

    public static int WaitTicks = 5f.SecondsToTicks();
    
    public static readonly TargetIndex TargetIdx = TargetIndex.A;
    
    public override bool TryMakePreToilReservations(bool errorOnFailed) => 
        Actor.Reserve(Target, job);
    
    public override IEnumerable<Toil> MakeNewToils()
    {
        yield return Toils_Interpersonal.GotoInteractablePosition(TargetIdx);

        var toil = Toils_General.Wait(WaitTicks, TargetIdx)
            .WithProgressBarToilDelay(TargetIdx, WaitTicks);
        toil.AddPreInitAction(ForceTargetToWait);
        
        yield return toil;
        
        yield return Toils_General.Do(TryRecruit);
        
        yield return Toils_Interpersonal.SetLastInteractTime(TargetIdx);
        
        yield return Toils_General.Do(End);
    }
    
    public virtual void TryRecruit()
    {   
        var chance = Factor;
        if (!Rand.Chance(chance))
        {
            NotifyFailed(chance);
            return;
        }

        Recruit();
        NotifySucceeded(chance);
    }

    public virtual void Recruit()
    {
        var target = Target;
        if (target.GetCaravan() is { } caravan)
        {
            target.GetLord()?.Notify_PawnLost(target, PawnLostCondition.LeftVoluntarily);
            caravan.RemovePawn(target);
        }

        if (target.roping is { } rope)
        {
            rope.DropRopes();
        }
        
        InteractionWorker_RecruitAttempt.DoRecruit(Actor, target);
    } 

    public virtual void ForceTargetToWait() => PawnUtility.ForceWait(Target, WaitTicks, Actor);

    public virtual void End() => EndJobWith(JobCondition.Succeeded);
}