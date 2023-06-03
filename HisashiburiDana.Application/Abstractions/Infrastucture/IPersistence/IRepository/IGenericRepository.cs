using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Application.Abstractions.Infrastucture.Persistence.IRepository
{
    public interface IGenericRepository<TEntity>  where TEntity : class 
    {
        Task<TEntity?> GetById(string Id);
        Task<TEntity?> Get(string columnName, ScanOperator ope, string value);
        Task<List<TEntity>> GetAll();
        Task<List<TEntity>> GetAll(string columnName, ScanOperator ope, string value);
        Task InsertAsync(TEntity entity);
       
        void Delete(TEntity entity);

        void Update(TEntity entity);
     
       
    }
}
