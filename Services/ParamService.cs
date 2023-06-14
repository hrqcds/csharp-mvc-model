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

        if (request.IsToSend != null)
        {
            param.IsToSend = request.IsToSend ?? param.IsToSend;
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

    public async Task Import(IFormFile file)
    {
        var f = file.OpenReadStream();
        var reader = new StreamReader(f);

        var param_list = new List<Param>();
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            // estrutura do arquivo: "item1, item2;"
            var values = line!.Split(',');
            if (values.Length > 2)
            {
                Error.Add("Import", new string[] { "Import failed" });
            }
            var param = new Param()
            {
                ValueLog = values[0],
                ValueConverted = values[1].Split(";")[0].Trim()
            };
            param_list.Add(param);
        }

        foreach (var param in param_list)
        {

            var valueLogExist = await paramRepository.GetByValueLog(param.ValueLog);

            if (valueLogExist != null)
            {
                Error.Add("ValueLog", new string[] { $"Value '{param.ValueLog}' Log already exist" });
            }

            var valueConvertedExist = await paramRepository.GetByValueConverted(param.ValueConverted);

            if (valueConvertedExist != null)
            {
                Error.Add("ValueConverted", new string[] { $"Value '{param.ValueConverted}' Converted already exist" });
            }

        }

        if (Error.Count > 0)
            throw new ErrorExceptions("Params in file already exist", 400, Error);

        var result = await paramRepository.Import(param_list);
        if (result == 0)
        {
            Error.Add("Import", new string[] { "Import failed" });
            throw new ErrorExceptions("Import failed", 500, Error);
        }

    }
}