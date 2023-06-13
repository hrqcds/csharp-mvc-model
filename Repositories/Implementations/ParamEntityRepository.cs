using Database;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories.Implementations;

public class ParamEntityRepository : IParamRepository
{
    private readonly Context context;

    public ParamEntityRepository(Context _context)
    {
        context = _context;
    }

    public async Task<Param> Create(Param param)
    {
        var p = context.Params.Add(param);
        await context.SaveChangesAsync();
        return p.Entity;
    }

    public async Task<Param?> GetByValueConverted(string valueConverted)
    {
        return await context.Params.FirstOrDefaultAsync(x => x.ValueConverted == valueConverted);
    }

    public async Task<Param?> GetByValueLog(string valueLog)
    {
        return await context.Params.FirstOrDefaultAsync(x => x.ValueLog == valueLog);
    }
}