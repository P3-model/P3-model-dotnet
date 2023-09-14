using System.Collections.Generic;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public record DomainModuleOwners(DomainModule Module,
    IReadOnlySet<DevelopmentTeam> DevelopmentOwners,
    IReadOnlySet<BusinessOrganizationalUnit> BusinessOwners);