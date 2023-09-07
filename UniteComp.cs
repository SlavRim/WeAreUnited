namespace WeAreUnited;

public class UniteComp : ThingComp
{
    public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
    {
        var options = base.CompFloatMenuOptions(selPawn);
        
        var option = GetMenuOption(selPawn, parent as Pawn);
        if (option is null) return options;
        
        if(options is IList<FloatMenuOption> { IsReadOnly: false } list)
            list.Add(option);
        else options = options.Append(option);
        
        return options;
    }
    
    public static FloatMenuOption? GetMenuOption(Pawn? pawn, Pawn? target)
    {
        JobFailReason.Clear();
        
        var job = JobValidator.TryGetJob(pawn, target);

        void OptionAction()
        {
            if (job is null) return;
            pawn?.jobs?.TryTakeOrderedJob(job, JobTag.Misc);
        }

        var translation = Translations.FloatMenu_NotRelated;

        if (target is { relations: var relations })
            if (relations.RelatedPawns.Contains(pawn))
                translation = Translations.FloatMenu;
        
        FloatMenuOption option = new(translation.Translate(target.LabelShort), OptionAction);

        option = FloatMenuUtility.DecoratePrioritizedTask(option, pawn, target);
        
        if (job is not null)
            return option;

        if (!JobFailReason.HaveReason)
            return null;
        
        var failText = Translations.JobFail.Translate(Translations.Verb.Translate(), target.LabelShort, target);
        option.Label = $"{failText} ({JobFailReason.Reason})";
        option.Disabled = true;
        
        return option;
    }
}