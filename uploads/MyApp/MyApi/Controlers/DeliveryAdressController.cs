using Microsoft.AspNetCore.Mvc;
using MyApp.Common;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DeliveryAdressController : ControllerBase
    {
        private readonly IDBDeliveryAdress _adressDb;

        public DeliveryAdressController()
        {
            _adressDb = DAO.Instance.Database.DeliveryAdress;
        }

        /// <summary>
        /// Получить список всех адресов доставки
        /// </summary>
        /// <returns>Список адресов доставки</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DeliveryAdress>), StatusCodes.Status200OK)]
        public ActionResult<List<DeliveryAdress>> GetAll()
        {
            try
            {
                var adresses = _adressDb.Get();
                return Ok(adresses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении списка адресов доставки", error = ex.Message });
            }
        }

        /// <summary>
        /// Получить адрес доставки по ID
        /// </summary>
        /// <param name="id">ID адреса доставки</param>
        /// <returns>Адрес доставки</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DeliveryAdress), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DeliveryAdress> GetById(int id)
        {
            try
            {
                var adress = _adressDb.Get(id);
                if (adress == null)
                {
                    return NotFound(new { message = $"Адрес доставки с ID {id} не найден" });
                }
                return Ok(adress);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении адреса доставки", error = ex.Message });
            }
        }

        /// <summary>
        /// Создать новый адрес доставки
        /// </summary>
        /// <param name="adress">Данные адреса доставки</param>
        /// <returns>Результат создания</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create([FromBody] DeliveryAdress adress)
        {
            try
            {
                _adressDb.Post(adress);
                return CreatedAtAction(nameof(GetById), new { id = adress.DeliveryAdressID }, adress);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при создании адреса доставки", error = ex.Message });
            }
        }

        /// <summary>
        /// Обновить данные адреса доставки
        /// </summary>
        /// <param name="id">ID адреса доставки</param>
        /// <param name="adress">Новые данные адреса доставки</param>
        /// <returns>Результат обновления</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] DeliveryAdress adress)
        {
            try
            {
                var existingAdress = _adressDb.Get(id);
                if (existingAdress == null)
                {
                    return NotFound(new { message = $"Адрес доставки с ID {id} не найден" });
                }

                _adressDb.Put(id, adress);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при обновлении адреса доставки", error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить адрес доставки
        /// </summary>
        /// <param name="id">ID адреса доставки</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                var existingAdress = _adressDb.Get(id);
                if (existingAdress == null)
                {
                    return NotFound(new { message = $"Адрес доставки с ID {id} не найден" });
                }

                _adressDb.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при удалении адреса доставки", error = ex.Message });
            }
        }
    }
}
