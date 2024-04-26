namespace WeAreUnited;

partial class ModSettings
{
    public readonly Listing_Standard Layout = new();
    
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

        void Reset();
        
        void Draw(Rect rect);
    }
    
    public interface IElement<T> : IElement
    {
        T Value { get; set; }
    }
}