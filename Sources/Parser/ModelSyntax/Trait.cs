namespace P3Model.Parser.ModelSyntax;

public interface Trait
{
    string ElementId => Element.Id;
    ElementBase Element { get; }
}

public interface Trait<out TElement> : Trait 
    where TElement : ElementBase
{
    ElementBase Trait.Element => Element;
    new TElement Element { get; }
}