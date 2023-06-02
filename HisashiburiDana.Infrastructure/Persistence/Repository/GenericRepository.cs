using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using HisashiburiDana.Application.Abstractions.Infrastucture.Persistence.IRepository;
using HisashiburiDana.Domain.Common;
using HisashiburiDana.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HisashiburiDana.Infrastructure.Persistence.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity: class
    {
        protected readonly DynamoDBContext _context;
        protected GenericRepository(IAmazonDynamoDB client)
        {
            _context = new DynamoDBContext(client);
        }

        public async Task<TEntity?> GetById(string Id)
        {
            return await _context.LoadAsync<TEntity>(Id);
        }

        public async Task<TEntity?> Get(string columnName, ScanOperator ope, string value)
        {
            var result = await  _context.ScanAsync<TEntity>(
                new List<ScanCondition>() 
                {
                    new ScanCondition(columnName, ope, value)
                }              
                ).GetRemainingAsync();
            return result.FirstOrDefault();
            
        }

        public async Task<List<TEntity>> GetAll()
        {
            var result = await _context.ScanAsync<TEntity>(
            new List<ScanCondition>()
            { }
                ).GetRemainingAsync();
            return result;
        }

        public async Task<List<TEntity>> GetAll(string columnName, ScanOperator ope, string value)
        {
            var result = await _context.ScanAsync<TEntity>(
                new List<ScanCondition>()
                {
                    new ScanCondition(columnName, ope, value)
                }
                ).GetRemainingAsync();
            return result;
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _context.SaveAsync(entity);
        }

        
        public async void Delete(TEntity entity)
        {
            await _context.DeleteAsync(entity);
        }

        

        public void Update(TEntity entity)
        {
             _context.SaveAsync(entity);
        }

        
       

    }
}
