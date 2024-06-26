using Riok.Mapperly.Abstractions;
using SparepartManagementSystem.Domain;

namespace SparepartManagementSystem.Repository.Tests;

[Mapper]
public partial class MapperlyMapper
{
    public partial NumberSequence Copy(NumberSequence source);
    public partial Role Copy(Role source);
}