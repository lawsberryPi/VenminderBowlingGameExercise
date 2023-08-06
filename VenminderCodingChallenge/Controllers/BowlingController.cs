using Microsoft.AspNetCore.Mvc;
using VenminderCodingChallenge.Model;

namespace VenminderCodingChallenge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BowlingController : ControllerBase
    {

        private readonly ILogger<BowlingController> _logger;
        private readonly IBowlingScoreRepository _bowlingScoreRepository;

        public BowlingController(ILogger<BowlingController> logger, IBowlingScoreRepository bowlingScoreRepository)
        {
            _logger = logger;
            _bowlingScoreRepository = bowlingScoreRepository;
        }

        [HttpDelete]
        public bool Delete()
        {
            return _bowlingScoreRepository.resetGame();
        }

        [HttpGet]
        public int Get()
        {
            return _bowlingScoreRepository.GetTotalScore();
        }

        [HttpPost]
        public BowlingScoreResponse Post([FromForm] int frameNumber, [FromForm] int rollNumber, [FromForm] int rollScore)
        {

            Console.WriteLine(frameNumber);
            var returnValue = _bowlingScoreRepository.ProcessFrame(frameNumber, rollNumber, rollScore);
            return returnValue;
        }
    }
}