using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LogRepository : ILogRepository
{
    private readonly AppDbContext _ctx;

    public LogRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task AddAsync(LogEntry entry)
    {
        _ctx.Logs.Add(entry);
        await _ctx.SaveChangesAsync();
    }

    public async Task<(IEnumerable<LogEntry> Items, int Total)> GetPagedAsync(int page, int pageSize)
    {
        var query = _ctx.Logs.AsQueryable();
        var total = await query.CountAsync();
        var items = await query.OrderByDescending(x => x.DataHoraRequisicao)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();
        return (items, total);
    }
}
