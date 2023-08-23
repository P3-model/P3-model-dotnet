using System.IO;
using Humanizer;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public record ProcessStep(HierarchyId Id, FileInfo? DescriptionFile) : DomainBuildingBlock(
    Id.LastPart.Humanize(LetterCasing.Title), DescriptionFile)
{
    public new record DependsOnBuildingBlock(ProcessStep Source, DomainBuildingBlock Destination) : 
        DomainBuildingBlock.DependsOnBuildingBlock(Source, Destination), 
        Relation<ProcessStep, DomainBuildingBlock>
    {
        public new ProcessStep Source { get; } = Source;
    }
    
    public record HasNextStep(ProcessStep Source, ProcessStep Destination) : Relation<ProcessStep, ProcessStep>;
}