using Microsoft.AspNetCore.Mvc;
using MyApp.Common;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CatalogsController : ControllerBase
    {
        private readonly IDBCatalogs _catalogsDb;

        public CatalogsController()
        {
            _catalogsDb = DAO.Instance.Database.Catalogs;
        }

        /// <summary>
        /// Получить список всех каталогов
        /// </summary>
        /// <returns>Список каталогов</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Catalogs>), StatusCodes.Status200OK)]
        public ActionResult<List<Catalogs>> GetAll()
        {
            try
            {
                var catalogs = _catalogsDb.Get();
                return Ok(catalogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении списка каталогов", error = ex.Message });
            }
        }

        /// <summary>
        /// Получить каталог по ID
        /// </summary>
        /// <param name="id">ID каталога</param>
        /// <returns>Каталог</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Catalogs), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Catalogs> GetById(int id)
        {
            try
            {
                var catalog = _catalogsDb.Get(id);
                if (catalog == null)
                {
                    return NotFound(new { message = $"Каталог с ID {id} не найден" });
                }
                return Ok(catalog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при получении каталога", error = ex.Message });
            }
        }

        /// <summary>
        /// Создать новый каталог
        /// </summary>
        /// <param name="catalog">Данные каталога</param>
        /// <returns>Результат создания</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create([FromBody] Catalogs catalog)
        {
            try
            {
                // Валидация дат
                if (catalog.DateEnd.HasValue && catalog.DateEnd.Value < catalog.DateStart)
                {
                    return BadRequest(new { message = "Дата окончания каталога не может быть раньше даты начала" });
                }

                _catalogsDb.Post(catalog);
                return CreatedAtAction(nameof(GetById), new { id = catalog.CatalogsID }, catalog);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при создании каталога", error = ex.Message });
            }
        }

        /// <summary>
        /// Обновить данные каталога
        /// </summary>
        /// <param name="id">ID каталога</param>
        /// <param name="catalog">Новые данные каталога</param>
        /// <returns>Результат обновления</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(int id, [FromBody] Catalogs catalog)
        {
            try
            {
                var existingCatalog = _catalogsDb.Get(id);
                if (existingCatalog == null)
                {
                    return NotFound(new { message = $"Каталог с ID {id} не найден" });
                }

                // Валидация дат
                if (catalog.DateEnd.HasValue && catalog.DateEnd.Value < catalog.DateStart)
                {
                    return BadRequest(new { message = "Дата окончания каталога не может быть раньше даты начала" });
                }

                _catalogsDb.Put(id, catalog);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = "Ошибка валидации данных", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при обновлении каталога", error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить каталог
        /// </summary>
        /// <param name="id">ID каталога</param>
        /// <returns>Результат удаления</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            try
            {
                var existingCatalog = _catalogsDb.Get(id);
                if (existingCatalog == null)
                {
                    return NotFound(new { message = $"Каталог с ID {id} не найден" });
                }

                _catalogsDb.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ошибка при удалении каталога", error = ex.Message });
            }
        }
    }
}
