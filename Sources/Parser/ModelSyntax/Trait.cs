namespace P3Model.Parser.ModelSyntax;

public interface Trait
{
    string ElementId => Element.Id;
    Element Element { get; }
}

public interface Trait<out TElement> : Trait 
    where TElement : class, Element
{
    Element Trait.Element => Element;
    new TElement Element { get; }
}