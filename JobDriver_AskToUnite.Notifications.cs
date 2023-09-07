namespace WeAreUnited;

partial class JobDriver_AskToUnite
{
    public virtual void NotifySucceeded(float chance)
    {
        var mote = Translations.SucceededMote.Translate(chance.ToStringPercent());
        var message = Translations.SucceededMessage.Translate(Target.LabelShort);
        
        ThrowNotification(mote, message);
    }

    public virtual void NotifyFailed(float chance)
    {
        var mote = Translations.FailedMote.Translate(chance.ToStringPercent());
        var message = Translations.FailedMessage.Translate(Target.LabelShort);
        
        ThrowNotification(mote, message);
    }

    public virtual void ThrowNotification(TaggedString mote, TaggedString message, MessageTypeDef? messageType = null)
    {
        ThrowMote(mote);
        ThrowMessage(message, messageType);
    }

    public virtual void ThrowMessage(TaggedString message, MessageTypeDef? messageType = null)
    {
        messageType ??= MessageTypeDefOf.NeutralEvent;
        
        Messages.Message(message, messageType);
    }

    public virtual void ThrowMote(TaggedString mote) => 
        MoteMaker.ThrowText((Actor.DrawPos + Target.DrawPos) / 2f, Actor.Map, mote, 8f);
}