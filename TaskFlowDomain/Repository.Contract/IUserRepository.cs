using System;
using System.Collections.Generic;
using System.Text;
using TaskFlowDomain.Entites;

namespace TaskFlowDomain.Repository.Contract
{
    public interface IUserRepository 
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsAsync(string email);
    }
}
