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

        public PremierBankTestingController(IPremierBankTestingServices premierBankTestingServices)
        {
            _premierBankTestingServices = premierBankTestingServices;
        }

        [HttpPost("transactions/import")]
        public async Task<ActionResult> ImportDataToDatabase()
        {

            try
            {
                var count = await _premierBankTestingServices.ImportData();
                return Ok($"Было импортировано {count} записей");
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }

        }


        [HttpPut("transactions/{id:guid}/process")]

        public async Task<ActionResult> MarkTransaction(Guid id)
        {
            try
            {
                await _premierBankTestingServices.MarkTransactionAsProcessed(id);
                return Ok($"transaction {id} marked as Processed");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }

        [HttpGet("\"reports/monthly-by-client")]
        public async Task<ActionResult<Dictionary<string, decimal>>> GetAllProcessedTransactionByClient()
        {
            try
            {
                var data = await _premierBankTestingServices.GetMonthProcessedTransactionsGroupedByUser();
                return Ok(data);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
        }
    }
}