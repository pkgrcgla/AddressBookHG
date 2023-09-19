using AddressBookHG_DL.ContextInfo;
using AddressBookHG_DL.InterfaceOfRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_DL.ImplementationsOfRepos
{
    public class Repository<T, Tid> : IRepository<T, Tid>
          where T : class, new()
    {
        protected readonly AddressbookContext _context;

        public Repository(AddressbookContext context)
        {
            _context = context; // DI
        }

        public int Add(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Delete(T entity)
        {
            try
            {
                _context.Set<T>().Remove(entity);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? whereFilter = null, string[]? joinTablesName = null)
        {

            try
            {

                IQueryable<T>? query;

                if (whereFilter != null)
                {
                    // // select * from tabloAdi where ile koşullar eklenir

                    query = _context.Set<T>().Where(whereFilter);
                }
                else
                {
                    // select * from tabloAdi
                    query = _context.Set<T>();
                }

                if (joinTablesName != null)
                {
                    foreach (var item in joinTablesName)
                    {
                        query = query.Include(item);
                        //join'e bakalım
                    }
                }

                // return query.AsNoTracking();
                return query; //program.cs içinde ayarladık

            }
            catch (Exception)
            {

                throw;
            }
        }

        public T GetByCondition(Expression<Func<T, bool>>? whereFilter = null, string[]? joinTablesName = null)
        {
            try
            {

                IQueryable<T>? query;

                if (whereFilter != null)
                {
                    // // select top 1 * from tabloAdi where ile koşullar eklenir

                    query = _context.Set<T>().Where(whereFilter);
                }
                else
                {
                    // select top 1 * from tabloAdi
                    query = _context.Set<T>();
                }

                if (joinTablesName != null)
                {
                    foreach (var item in joinTablesName)
                    {
                        query = query.Include(item);
                        //join'e bakalım
                    }
                }

                //return query.AsNoTracking().FirstOrDefault();
                return query.FirstOrDefault(); //program.cs içinde ayarladık

            }
            catch (Exception)
            {

                throw;
            }


        }

        public T GetbyId(Tid id)
        {

            try
            {
                return _context.Set<T>().Find(id);
            }
            catch (Exception)
            {

                throw;
            }
        }



        public int Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                return _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
