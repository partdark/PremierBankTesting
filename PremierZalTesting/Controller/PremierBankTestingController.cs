using Microsoft.AspNetCore.Mvc;
using PremierBankTesting.Application.Services;
using PremierBankTesting.Contracts;

namespace PremierBankTesting.Controller
{


    [ApiController]
    [Route("[controller]")]
    public class PremierBankTestingController : ControllerBase
    {
        private readonly IPremierBankTestingServices _premierBankTestingServices;
        private readonly ILogger<PremierBankTestingController> _logger;

        public PremierBankTestingController(IPremierBankTestingServices premierBankTestingServices,
            ILogger<PremierBankTestingController> logger)
        {
            _premierBankTestingServices = premierBankTestingServices;
            _logger = logger;
        }

        [HttpPost("transactions/import")]
        public async Task<ActionResult> ImportDataToDatabase()
        {

            try
            {
                var count = await _premierBankTestingServices.ImportData();
                _logger.LogInformation($"Произошел импорт {count} записей");
                return Ok($"Было импортировано {count} записей");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Ошибка импорта транзакций");
                return StatusCode(400, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("Ошибка импорта транзакций");
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка импорта транзакций");
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }

        }


        [HttpPut("transactions/{id:guid}/process")]

        public async Task<ActionResult> MarkTransaction(Guid id)
        {
            try
            {
                await _premierBankTestingServices.MarkTransactionAsProcessed(id);
                return Ok($"транзакция {id} отмечана как обработанная");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Ресурс не найден");
                return StatusCode(400, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Некорректная операция");
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }


        [HttpGet("transactions/unprocessed")]

        public async Task<ActionResult<List<PremierBankTestingResponse>>> GetAllUnprocessed()
        {
            try
            {
                var unprocessedTransactions = await _premierBankTestingServices.GetAllUnprocessedTransaction();
                var response = unprocessedTransactions.Select(t =>
                    new PremierBankTestingResponse(
                        t.Id,
                        t.Amount,
                        t.Comment,
                        t.Timestamp,
                        t.UserEmail,
                        t.IsProcessed
                        ));
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Ресурс не найден");
                return StatusCode(400, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Некорректная операция");
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("reports/monthly-by-user")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetAllProcessedTransactionByClient()
        {
            try
            {
                var data = await _premierBankTestingServices.GetMonthProcessedTransactionsGroupedByUser();

                if (data.Count == 0)
                {
                    _logger.LogInformation("Не удалось найти записи для формирования отчета");
                }
                else if (data.Count > 0)
                {
                    _logger.LogInformation($"Сформирован отчет с {data.Count} записями ");
                }
                return Ok(data);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Ресурс не найден");
                return StatusCode(400, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Некорректная операция");
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("reports/by-type")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetAllProcessedTransactionByComment()
        {
            try
            {
                var data = await _premierBankTestingServices.GetMonthProcessedTransactionsGroupedByComment();
                return Ok(data);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Ресурс не найден");
                return StatusCode(400, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Некорректная операция");
                return StatusCode(404, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
}