namespace Application.Common.Interfaces;

public interface IElectorPadronRepositoryFactory
{
    IElectorPadronRepository Resolve();
}
