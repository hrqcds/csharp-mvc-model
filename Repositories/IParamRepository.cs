using Models;

namespace Repositories;

public interface IParamRepository
{
    Task<Param> Create(Param param);

    Task<Param?> GetByValueLog(string valueLog);

    Task<Param?> GetByValueConverted(string valueConverted);

}