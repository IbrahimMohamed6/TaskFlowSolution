using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlowDomain.Repository.Contract
{
    public interface IGenericRepository <T>
    {
        Task<IEnumerable<T>> GetAll();


        Task<T?> GetById(Guid id);

        Task<T> Add(T entity);

        Task<T> Update(T entity);

        Task<int> Delete(T entity);
    }
}
