using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByNameAsync(string nome);
    Task<User?> GetByIdAsync(int id);
    Task AddAsync(User user);
}

