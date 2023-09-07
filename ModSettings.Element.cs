namespace WeAreUnited;

partial class ModSettings
{
    private readonly List<IElement> elements;
    public readonly IReadOnlyList<IElement> Elements;
    
    private void ForEach(Action<IElement> action)
    {
        if (elements is null) return;
        
        foreach (var element in elements)
        {
            try
            {
                action(element);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    
    public void Draw(IElement element, float? height, float widthFactor = 1f)
    {
        Layout.Gap();
        var rect = Layout.GetRect(height ?? element.Height, widthFactor);
         
        element.Draw(rect);
    }
    
    public interface IElement : IExposable
    {
        float Height { get; }
        float WidthFactor { get; }
        
        void Draw(Rect rect);
    }
    
    public abstract record Element<T>(string Name, string Label, T DefaultValue) : IElement
    {
        public T Value;
        
        public virtual float Height { get; set; }
        public virtual float WidthFactor { get; set; } = 1f;

        public abstract void Draw(Rect rect);

        public virtual void Reset() => Value = DefaultValue;
        
        public abstract void ExposeData();

        public static implicit operator T(Element<T> element) => element.Value;
    }
}