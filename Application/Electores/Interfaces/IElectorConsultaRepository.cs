using Domain.Entities;

namespace Application.Electores.Interfaces;

public interface IElectorConsultaRepository
{
    Task RegistrarAsync(ElectorConsulta consulta, CancellationToken cancellationToken = default);
}
