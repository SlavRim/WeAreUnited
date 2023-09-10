namespace WeAreUnited;

partial class ModSettings
{
    public record Numeric<T>(
        string Name, 
        string Label, 
        float Minimum = 0f, 
        float Maximum = float.MaxValue, 
        T DefaultValue = default) :
        Element<T>(Name, Label, DefaultValue)
    where T : struct
    {
        public float 
            LabelHeight = 25f,
            FieldHeight = 20f;
        
        public override float Height => + LabelHeight + FieldHeight;

        public override float WidthFactor { get; set; } = 0.5f;

        public string Buffer;
        
        public readonly Listing_Standard Layout = new();

        public override void Draw(Rect rect)
        {
            Layout.Begin(rect);
            
            Widgets.LabelFit(Layout.GetRect(LabelHeight), Label);
            rect = Layout.GetRect(FieldHeight, WidthFactor);
            Widgets.TextFieldNumeric(rect, ref value, ref Buffer, Minimum, Maximum);
            
            Layout.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref value, Name, defaultValue: DefaultValue);
        }
    }
}