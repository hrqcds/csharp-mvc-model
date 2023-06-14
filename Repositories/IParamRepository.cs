using Models;
using Utils;

namespace Repositories;

public interface IParamRepository
{
    Task<Param> Create(Param param);

    Task<Param?> Update(Param param);

    Task<Param?> Delete(Param id);

    Task<List<Param>> GetAll(ParamQueryRequest query);

    Task<int> Count(ParamQueryRequest query);

    Task<Param?> GetById(string id);

    Task<Param?> GetByValueLog(string valueLog);

    Task<Param?> GetByValueConverted(string valueConverted);
}