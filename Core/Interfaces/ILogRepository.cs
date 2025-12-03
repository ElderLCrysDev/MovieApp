using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces;
public interface ILogRepository
{
    Task AddAsync(LogEntry entry);
    Task<(IEnumerable<LogEntry> Items, int Total)> GetPagedAsync(int page, int pageSize);
}
