using Microsoft.EntityFrameworkCore;
using Company.CRUD.MVC.BLL.Interfaces;
using Company.CRUD.MVC.DAL.Data.Contexts;
using Company.CRUD.MVC.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.CRUD.MVC.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await  _context.Employees.Include(E => E.WorkFor).ToListAsync();
            }
            else 
            {
                return await _context.Set<T>().ToListAsync();
            }
        }
        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

       
        public async Task<int> AddAsync(T entity)
        {
             _context.Add(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(T entity)
        {

            _context.Update(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(T entity)
        {

            _context.Remove(entity);
            return await _context.SaveChangesAsync();
        }

       
    }
}
