using Microsoft.EntityFrameworkCore;
using EKZ.Models;

namespace EKZ.Services
{
    /// <summary>
    /// Сервис для работы с данными в базе данных, предоставляет универсальные методы CRUD
    /// и специфичные для приложения запросы.
    /// </summary>
    public class DataService
    {
        private readonly MyDbContext _context;

        /// <summary>
        /// Конструктор класса DataService.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public DataService(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Создает новую сущность в базе данных.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <param name="entity">Экземпляр сущности.</param>
        /// <returns>Созданная сущность.</returns>
        public async Task<T> CreateAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Получает сущность по её идентификатору.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Найденная сущность или null.</returns>
        public async Task<T?> GetByIdAsync<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Получает все сущности определённого типа.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <returns>Список сущностей.</returns>
        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Обновляет сущность в базе данных.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <param name="entity">Экземпляр сущности с обновленными данными.</param>
        /// <returns>Обновленная сущность.</returns>
        public async Task<T?> UpdateAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Удаляет сущность по её идентификатору.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>True, если удаление успешно; иначе False.</returns>
        public async Task<bool> DeleteAsync<T>(int id) where T : class
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return false;

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Получает список заявок клиента по его идентификатору.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента.</param>
        /// <returns>Список заявок клиента.</returns>
        public async Task<List<Request>> GetRequestsByClientIdAsync(int clientId)
        {
            return await _context.Requests
                .Where(r => r.ClientID == clientId)
                .ToListAsync();
        }

        /// <summary>
        /// Получает список ремонтов по идентификатору заявки.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <returns>Список ремонтов с включением связанных данных (услуги, сотрудники).</returns>
        public async Task<List<Repair>> GetRepairsByRequestIdAsync(int requestId)
        {
            return await _context.Repairs
                .Where(r => r.RequestID == requestId)
                .Include(r => r.Service)   // Включает данные об услугах
                .Include(r => r.Employee) // Включает данные о сотрудниках
                .ToListAsync();
        }

        /// <summary>
        /// Получает список продаж клиента по его идентификатору.
        /// </summary>
        /// <param name="clientId">Идентификатор клиента.</param>
        /// <returns>Список продаж с включением связанных данных (автомобили, сотрудники).</returns>
        public async Task<List<Sale>> GetSalesByClientIdAsync(int clientId)
        {
            return await _context.Sales
                .Where(s => s.ClientID == clientId)
                .Include(s => s.Car)       // Включает данные об автомобилях
                .Include(s => s.Employee)  // Включает данные о сотрудниках
                .ToListAsync();
        }
    }
}
