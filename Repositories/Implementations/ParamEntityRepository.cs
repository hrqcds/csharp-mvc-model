using Database;
using Microsoft.EntityFrameworkCore;
using Models;
using Utils;

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

    public async Task<List<Param>> GetAll(ParamQueryRequest query)
    {
        return await context
            .Params
            .Where(p => p.ValueLog.Contains(query.ValueLog ?? ""))
            .Where(p => p.ValueConverted.Contains(query.ValueConverted ?? ""))
            .Skip(query.Skip ?? 0)
            .Take(query.Take ?? 10)
            .ToListAsync();
    }

    public async Task<int> Count(ParamQueryRequest query)
    {
        return await context
                    .Params
                    .Where(p => p.ValueLog.Contains(query.ValueLog ?? ""))
                    .Where(p => p.ValueConverted.Contains(query.ValueConverted ?? ""))
                    .Skip(query.Skip ?? 0)
                    .Take(query.Take ?? 10)
                    .CountAsync();
    }

    public async Task<Param?> GetById(string id)
    {
        return await context.Params.FirstOrDefaultAsync(x => x.ID == id);
    }

    public async Task<Param?> GetByValueConverted(string valueConverted)
    {
        return await context.Params.FirstOrDefaultAsync(x => x.ValueConverted == valueConverted);
    }

    public async Task<Param?> GetByValueLog(string valueLog)
    {
        return await context.Params.FirstOrDefaultAsync(x => x.ValueLog == valueLog);
    }

    public async Task<Param?> Update(Param param)
    {
        var p = context.Params.Update(param);
        await context.SaveChangesAsync();
        return p.Entity;
    }
}