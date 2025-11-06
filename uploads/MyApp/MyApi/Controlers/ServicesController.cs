using Microsoft.AspNetCore.Mvc;
using MyApp.Common;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ServicesController : ControllerBase
    {
        private readonly IDBServices _servicesDb;

        public ServicesController()
        {
            _servicesDb = DAO.Instance.Database.Services;
        }

        /// <summary>
        /// Получить список всех услуг
        /// </summary>
        /// <returns>Список услуг</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Services>), StatusCodes.Status200OK)]
        public ActionResult<List<Services>> GetAll()
        {
            try
            {
                var services = _servicesDb.Get();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении списка услуг", error = ex.Message });
            }
        }

        /// <summary>
        /// Получить услугу по ID
        /// </summary>
        /// <param name="id">ID услуги</param>
        /// <returns>Услуга</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Services), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Services> GetById(int id)
        {
            try
            {
                var service = _servicesDb.Get(id);
                if (service == null)
                {
                    return NotFound(new { message = $"Услуга с ID {id} не найдена" });
                }
                return Ok(service);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении услуги", error = ex.Message });
            }
        }

        /// <summary>
        /// Создать новую услугу
        /// </summary>
        /// <param name="service">Данные услуги</param>
        /// <returns>Результат создания</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create([FromBody] Services service)
        {
            try
            {
                _servicesDb.Post(service);
                return CreatedAtAction(nameof(GetById), new { id = service.ServicesID }, service);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при создании услуги", error = ex.Message });
            }
        }

        /// <summary>
        /// Обновить данные услуги
        /// </summary>
        /// <param name="id">ID услуги</param>
        /// <param name="service">Новые данные услуги</param>
        /// <returns>Результат обновления</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] Services service)
        {
            try
            {
                var existingService = _servicesDb.Get(id);
                if (existingService == null)
                {
                    return NotFound(new { message = $"Услуга с ID {id} не найдена" });
                }

                _servicesDb.Put(id, service);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при обновлении услуги", error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить услугу
        /// </summary>
        /// <param name="id">ID услуги</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                var existingService = _servicesDb.Get(id);
                if (existingService == null)
                {
                    return NotFound(new { message = $"Услуга с ID {id} не найдена" });
                }

                _servicesDb.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при удалении услуги", error = ex.Message });
            }
        }
    }
}
