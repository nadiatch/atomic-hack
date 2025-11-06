using Microsoft.AspNetCore.Mvc;
using MyApp.Common;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PublicationsController : ControllerBase
    {
        private readonly IDBPublication _publicationDb;

        public PublicationsController()
        {
            _publicationDb = DAO.Instance.Database.Publication;
        }

        /// <summary>
        /// Получить список всех публикаций
        /// </summary>
        /// <returns>Список публикаций</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Publication>), StatusCodes.Status200OK)]
        public ActionResult<List<Publication>> GetAll()
        {
            try
            {
                var publications = _publicationDb.Get();
                return Ok(publications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении списка публикаций", error = ex.Message });
            }
        }

        /// <summary>
        /// Получить публикацию по ID
        /// </summary>
        /// <param name="id">ID публикации</param>
        /// <returns>Публикация</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Publication), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Publication> GetById(int id)
        {
            try
            {
                var publication = _publicationDb.Get(id);
                if (publication == null)
                {
                    return NotFound(new { message = $"Публикация с ID {id} не найдена" });
                }
                return Ok(publication);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении публикации", error = ex.Message });
            }
        }

        /// <summary>
        /// Создать новую публикацию
        /// </summary>
        /// <param name="publication">Данные публикации</param>
        /// <returns>Результат создания</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create([FromBody] Publication publication)
        {
            try
            {
                _publicationDb.Post(publication);
                return CreatedAtAction(nameof(GetById), new { id = publication.PublicationID }, publication);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при создании публикации", error = ex.Message });
            }
        }

        /// <summary>
        /// Обновить данные публикации
        /// </summary>
        /// <param name="id">ID публикации</param>
        /// <param name="publication">Новые данные публикации</param>
        /// <returns>Результат обновления</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] Publication publication)
        {
            try
            {
                var existingPublication = _publicationDb.Get(id);
                if (existingPublication == null)
                {
                    return NotFound(new { message = $"Публикация с ID {id} не найдена" });
                }

                _publicationDb.Put(id, publication);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при обновлении публикации", error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить публикацию
        /// </summary>
        /// <param name="id">ID публикации</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                var existingPublication = _publicationDb.Get(id);
                if (existingPublication == null)
                {
                    return NotFound(new { message = $"Публикация с ID {id} не найдена" });
                }

                _publicationDb.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при удалении публикации", error = ex.Message });
            }
        }
    }
}
