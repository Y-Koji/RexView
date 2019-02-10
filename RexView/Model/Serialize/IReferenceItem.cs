namespace RexView.Model.Serialize
{
    public interface IReferenceItem
    {
        string Id { get; set; }
        int Index { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }
}
