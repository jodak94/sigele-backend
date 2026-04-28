using Application.Operadores.Interfaces;

namespace Application.Common.Interfaces;

public interface IOperadorElectorRepositoryFactory
{
    IOperadorElectorRepository Resolve();
}
