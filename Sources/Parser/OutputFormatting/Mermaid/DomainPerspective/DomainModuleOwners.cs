using System.Collections.Generic;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.OutputFormatting.Mermaid.DomainPerspective;

public record DomainModuleOwners(DomainModule Module,
    IReadOnlySet<DevelopmentTeam> DevelopmentOwners,
    IReadOnlySet<BusinessOrganizationalUnit> BusinessOwners);