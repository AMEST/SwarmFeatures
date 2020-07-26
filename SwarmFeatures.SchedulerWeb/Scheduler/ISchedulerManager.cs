using System.Collections.Generic;
using System.Threading.Tasks;
using SwarmFeatures.SwarmControl.DockerEntity;

namespace SwarmFeatures.SchedulerWeb.Scheduler
{
    public interface ISchedulerManager
    {
        /// <summary>
        /// Запуск сервиса (запускает контейнер сервиса)
        /// </summary>
        /// <param name="id">id сервиса в Docker</param>
        /// <returns></returns>
        Task RunService(string id);

        /// <summary>
        /// Остановка сервиса (останавливает все экзепляры контейнера)
        /// </summary>
        /// <param name="id">id сервиса в Docker</param>
        /// <returns></returns>
        Task StopService(string id);

        /// <summary>
        /// Получение списка сервисов для запланированного запуска
        /// </summary>
        /// <returns></returns>
        Task<List<DockerService>> GetScheduledServices();

        /// <summary>
        /// Получение сервиса по id
        /// </summary>
        /// <param name="id">id сервиса в Docker</param>
        /// <returns></returns>
        Task<DockerService> GetScheduledServiceById(string id);

        /// <summary>
        /// Добавление задания в Quartz
        /// </summary>
        /// <param name="id">id сервиса в Docker</param>
        /// <param name="cron">крон правило</param>
        /// <returns></returns>
        Task AddQuartzTask(string id, string cron = "0 * * * * ? *");

        /// <summary>
        /// Удаление задания в Quartz
        /// </summary>
        /// <param name="id">id сервиса в Docker</param>
        /// <returns></returns>
        Task RemoveQuartzTask(string id);

        /// <summary>
        /// Список заданий в quartz
        /// </summary>
        /// <returns></returns>
        Task<List<DockerService>> ListQuartzTasks();
    }
}