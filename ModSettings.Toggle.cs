namespace WeAreUnited;

partial class ModSettings
{
    public record Toggle(string Name, string Label, bool DefaultValue = false) : 
        Element<bool>(Name, Label, DefaultValue)
    {
        public override float Height { get; set; } = 25f;

        public override void Draw(Rect rect)
        {
            Widgets.CheckboxLabeled(rect, Label, ref value, placeCheckboxNearText: true);
        }
        
        public override void ExposeData()
        {
            Scribe_Values.Look(ref value, Name, defaultValue: DefaultValue);
        }
    }
}