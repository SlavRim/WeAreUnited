namespace WeAreUnited;

partial class ModSettings
{
    public abstract record Element(string Name, string Label) : IElement
    {
        public virtual float Height { get; set; }
        public virtual float WidthFactor { get; set; } = 1f;
        
        public abstract void Draw(Rect rect);
        public abstract void Reset();
        public abstract void ExposeData();
    }
    
    public abstract record Element<T>(string Name, string Label, T DefaultValue) : Element(Name, Label), IElement<T>
    {
        protected T value = DefaultValue;

        public virtual T Value
        {
            get => value; 
            set => this.value = value;
        }

        public override void Reset() => Value = DefaultValue;

        public static implicit operator T(Element<T> element) => element.Value;
    }
}