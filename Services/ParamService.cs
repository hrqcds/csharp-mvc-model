using Exceptions;
using Generics;
using Models;
using Repositories;
using Utils;

namespace Services;

public class ParamService
{
    private readonly IParamRepository paramRepository;

    private Dictionary<string, string[]> Error { get; set; } = new Dictionary<string, string[]>();

    public ParamService(IParamRepository _paramRepository)
    {
        paramRepository = _paramRepository;
    }

    public async Task<Param> Create(CreateParamRequest request)
    {
        var valueLogExist = await paramRepository.GetByValueLog(request.ValueLog);

        if (valueLogExist != null)
        {
            Error.Add("ValueLog", new string[] { "Value Log already exist" });
        }

        var valueConvertedExist = await paramRepository.GetByValueConverted(request.ValueConverted);

        if (valueConvertedExist != null)
        {
            Error.Add("ValueConverted", new string[] { "Value Converted already exist" });
        }

        if (Error.Count > 0)
            throw new ErrorExceptions("Param already exist", 400, Error);

        var param = new Param()
        {
            ValueLog = request.ValueLog,
            ValueConverted = request.ValueConverted
        };

        return await paramRepository.Create(param);
    }

    public async Task<DataResponse<Param>> GetAll(ParamQueryRequest query)
    {
        var param = await paramRepository.GetAll(query);
        var count = await paramRepository.Count(query);

        return new DataResponse<Param>()
        {
            Data = param,
            Total = count
        };
    }

    public async Task<Param?> GetById(string id)
    {
        var param = await paramRepository.GetById(id);

        if (param == null)
        {
            Error.Add("Param", new string[] { "Param not found" });
            throw new ErrorExceptions("Param not found", 404, Error);
        }

        return param;
    }

    public async Task<Param?> Update(string id, UpdateParamRequest request)
    {
        var param = await paramRepository.GetById(id);

        if (param == null)
        {
            Error.Add("Param", new string[] { "Param not found" });
            throw new ErrorExceptions("Param not found", 404, Error);
        }

        if (request.ValueLog != null)
        {
            var valueLogExist = await paramRepository.GetByValueLog(request.ValueLog);

            if (valueLogExist != null)
            {
                Error.Add("ValueLog", new string[] { "Value Log already exist" });
            }
        }

        if (request.ValueConverted != null)
        {
            var valueConvertedExist = await paramRepository.GetByValueConverted(request.ValueConverted);

            if (valueConvertedExist != null)
            {
                Error.Add("ValueConverted", new string[] { "Value Converted already exist" });
            }
        }

        if (Error.Count > 0)
            throw new ErrorExceptions("Param already exist", 400, Error);

        param.ValueLog = request.ValueLog ?? param.ValueLog;
        param.ValueConverted = request.ValueConverted ?? param.ValueConverted;

        return await paramRepository.Update(param);
    }

    public async Task<Param?> Delete(string id)
    {
        var param = await paramRepository.GetById(id);

        if (param == null)
        {
            Error.Add("Param", new string[] { "Param not found" });
            throw new ErrorExceptions("Param not found", 404, Error);
        }

        return await paramRepository.Delete(param);
    }
}