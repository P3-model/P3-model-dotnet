namespace P3Model.Parser.ModelSyntax;

public interface Trait { }

public interface Trait<out TElement> : Trait 
    where TElement : Element
{
    TElement Element { get; }
}