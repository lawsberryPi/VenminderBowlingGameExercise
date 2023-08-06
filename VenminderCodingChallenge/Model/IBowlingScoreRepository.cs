namespace VenminderCodingChallenge.Model
{
    public interface IBowlingScoreRepository
    {
        int GetTotalScore();
        BowlingScoreResponse ProcessFrame(int frameNumber, int rollNumber, int rollScore);
        bool resetGame();
    }
}
