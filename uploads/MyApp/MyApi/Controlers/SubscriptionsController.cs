using Microsoft.AspNetCore.Mvc;
using MyApp.Common;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly IDBSubscriptions _subscriptionsDb;

        public SubscriptionsController()
        {
            _subscriptionsDb = DAO.Instance.Database.Subscriptions;
        }

        /// <summary>
        /// Получить список всех подписок
        /// </summary>
        /// <returns>Список подписок</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Subscriptions>), StatusCodes.Status200OK)]
        public ActionResult<List<Subscriptions>> GetAll()
        {
            try
            {
                var subscriptions = _subscriptionsDb.Get();
                return Ok(subscriptions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении списка подписок", error = ex.Message });
            }
        }

        /// <summary>
        /// Получить подписку по ID
        /// </summary>
        /// <param name="id">ID подписки</param>
        /// <returns>Подписка</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Subscriptions), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Subscriptions> GetById(int id)
        {
            try
            {
                var subscription = _subscriptionsDb.Get(id);
                if (subscription == null)
                {
                    return NotFound(new { message = $"Подписка с ID {id} не найдена" });
                }
                return Ok(subscription);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении подписки", error = ex.Message });
            }
        }

        /// <summary>
        /// Создать новую подписку
        /// </summary>
        /// <param name="subscription">Данные подписки</param>
        /// <returns>Результат создания</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create([FromBody] Subscriptions subscription)
        {
            try
            {
                // Валидация дат
                if (subscription.DateStart < subscription.Date)
                {
                    return BadRequest(new { message = "Дата начала подписки не может быть раньше даты оформления" });
                }

                if (subscription.DateEnd.HasValue && subscription.DateEnd.Value < subscription.DateStart)
                {
                    return BadRequest(new { message = "Дата окончания подписки не может быть раньше даты начала" });
                }

                if (subscription.Price <= 0)
                {
                    return BadRequest(new { message = "Цена подписки должна быть положительной" });
                }

                _subscriptionsDb.Post(subscription);
                return CreatedAtAction(nameof(GetById), new { id = subscription.SubscriptionsID }, subscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при создании подписки", error = ex.Message });
            }
        }

        /// <summary>
        /// Обновить данные подписки
        /// </summary>
        /// <param name="id">ID подписки</param>
        /// <param name="subscription">Новые данные подписки</param>
        /// <returns>Результат обновления</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] Subscriptions subscription)
        {
            try
            {
                var existingSubscription = _subscriptionsDb.Get(id);
                if (existingSubscription == null)
                {
                    return NotFound(new { message = $"Подписка с ID {id} не найдена" });
                }

                // Валидация дат
                if (subscription.DateStart < subscription.Date)
                {
                    return BadRequest(new { message = "Дата начала подписки не может быть раньше даты оформления" });
                }

                if (subscription.DateEnd.HasValue && subscription.DateEnd.Value < subscription.DateStart)
                {
                    return BadRequest(new { message = "Дата окончания подписки не может быть раньше даты начала" });
                }

                if (subscription.Price <= 0)
                {
                    return BadRequest(new { message = "Цена подписки должна быть положительной" });
                }

                _subscriptionsDb.Put(id, subscription);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при обновлении подписки", error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить подписку
        /// </summary>
        /// <param name="id">ID подписки</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                var existingSubscription = _subscriptionsDb.Get(id);
                if (existingSubscription == null)
                {
                    return NotFound(new { message = $"Подписка с ID {id} не найдена" });
                }

                _subscriptionsDb.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при удалении подписки", error = ex.Message });
            }
        }
    }
}
