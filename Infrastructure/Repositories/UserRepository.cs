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

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;

    public UserRepository(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task AddAsync(User user)
    {
        _ctx.Usuarios.Add(user);
        await _ctx.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _ctx.Usuarios.FindAsync(id);
    }

    public async Task<User?> GetByNameAsync(string nome)
    {
        return await _ctx.Usuarios.FirstOrDefaultAsync(x => x.Nome == nome);
    }
}

