using System;
using System.Collections.Generic;
using System.Linq;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

public class DomainBuildingBlocks
{
    private readonly Dictionary<DomainModule, List<DomainBuildingBlock>> _moduleToBuildingBlocks;

    public DomainBuildingBlocks(IEnumerable<DomainModule.ContainsBuildingBlock> relations) =>
        _moduleToBuildingBlocks = relations
            .GroupBy(r => r.DomainModule,
                r => r.BuildingBlock,
                (module, buildingBlocks) => (Module: module, BuildingBlock: buildingBlocks))
            .ToDictionary(g => g.Module,
                g => g.BuildingBlock.ToList());

    public IReadOnlyCollection<DomainBuildingBlock> GetFor(DomainModule module)
    {
        if (_moduleToBuildingBlocks.TryGetValue(module, out var buildingBlocks))
            return buildingBlocks.AsReadOnly();
        return Array.Empty<DomainBuildingBlock>();
    }
}