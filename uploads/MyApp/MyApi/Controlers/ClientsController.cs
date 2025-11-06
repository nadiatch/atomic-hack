using Microsoft.AspNetCore.Mvc;
using MyApp.Common;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ClientsController : ControllerBase
    {
        private readonly IDBClient _clientDb;

        public ClientsController()
        {
            _clientDb = DAO.Instance.Database.Client;
        }

        /// <summary>
        /// Получить список всех клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Client>), StatusCodes.Status200OK)]
        public ActionResult<List<Client>> GetAll()
        {
            try
            {
                var clients = _clientDb.Get();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении списка клиентов", error = ex.Message });
            }
        }

        /// <summary>
        /// Получить клиента по ID
        /// </summary>
        /// <param name="id">ID клиента</param>
        /// <returns>Клиент</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Client> GetById(int id)
        {
            try
            {
                var client = _clientDb.Get(id);
                if (client == null)
                {
                    return NotFound(new { message = $"Клиент с ID {id} не найден" });
                }
                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении клиента", error = ex.Message });
            }
        }

        /// <summary>
        /// Создать нового клиента
        /// </summary>
        /// <param name="client">Данные клиента</param>
        /// <returns>Результат создания</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create([FromBody] Client client)
        {
            try
            {
                _clientDb.Post(client);
                return CreatedAtAction(nameof(GetById), new { id = client.ClientID }, client);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при создании клиента", error = ex.Message });
            }
        }

        /// <summary>
        /// Обновить данные клиента
        /// </summary>
        /// <param name="id">ID клиента</param>
        /// <param name="client">Новые данные клиента</param>
        /// <returns>Результат обновления</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] Client client)
        {
            try
            {
                var existingClient = _clientDb.Get(id);
                if (existingClient == null)
                {
                    return NotFound(new { message = $"Клиент с ID {id} не найден" });
                }

                _clientDb.Put(id, client);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при обновлении клиента", error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <param name="id">ID клиента</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                var existingClient = _clientDb.Get(id);
                if (existingClient == null)
                {
                    return NotFound(new { message = $"Клиент с ID {id} не найден" });
                }

                _clientDb.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при удалении клиента", error = ex.Message });
            }
        }
    }
}
