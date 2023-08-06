using VenminderCodingChallenge.Model;
using VenminderCodingChallenge.Types;

namespace VenminderCodingChallenge.Repositories
{
    public class BowlingScoreRepository : IBowlingScoreRepository
    {
        private FrameRolls _frameRolls;

        public BowlingScoreRepository()
        {
            // initilize the Linked List with the first frame's frame number = 1 
            // as the header, it will have no element inside of Rolls
            _frameRolls = new FrameRolls(1, null);
        }

        public int GetTotalScore()
        {
            var totalScore = 0;
            var currentFrame = _frameRolls;
            while (currentFrame != null)
            {
                totalScore += currentFrame.GetFrameScore();
                currentFrame = currentFrame.NextFrame;
            }
            return totalScore;
        }

        public bool ResetGame()
        {
            _frameRolls = new FrameRolls(1, null);
            return true;
        }

        public BowlingScoreResponse ProcessFrame(int frameNumber, int rollNumber, int rollScore)
        {
            // first frame first roll is special because
            // the root is initialized in the constructor with empty Roll
            NextRoll nextCell;
            if (frameNumber == 1)
            {
                nextCell = processFirstFrame(rollScore, rollNumber);
            }
            else if (rollNumber == 1)
            {
                nextCell = _frameRolls.CreateFrame(frameNumber, rollScore);
                if (frameNumber == 10) processLastFrame(frameNumber, rollNumber, ref nextCell);
            }
            else
            {
                nextCell = _frameRolls.UpdateFrame(frameNumber, rollScore);
                if (frameNumber == 10) processLastFrame(frameNumber, rollNumber, ref nextCell);
            }

            var scores = populateFrameDetails();
            return new BowlingScoreResponse(scores, nextCell);
        }

        private List<FrameDetail> populateFrameDetails()
        {
            var frameDetails = new List<FrameDetail>();
            var currentFrame = _frameRolls;
            while (currentFrame != null)
            {
                var frameDetail = new FrameDetail()
                {
                    FrameNumber = currentFrame.FrameNumber,
                    Roll1 = currentFrame.Rolls.ElementAt(0),
                    Roll2 = currentFrame.Rolls.Count > 1 ? currentFrame.Rolls[1] : null,
                    Roll3 = currentFrame.Rolls.Count > 2 ? currentFrame.Rolls[2] : null,
                    FrameScore = currentFrame.GetFrameScore()
                };
                frameDetails.Add(frameDetail);
                currentFrame = currentFrame.NextFrame;
            }
            return frameDetails;
        }

        private void processLastFrame(int frameNumber, int rollNumber, ref NextRoll nextRoll)
        {
            var tenthFrame = _frameRolls.GetLastFrame();
            if (tenthFrame.IsStrike && rollNumber == 1)
            {
                nextRoll = new NextRoll(frameNumber, 2);
            }
            else if (tenthFrame.IsSpare || tenthFrame.IsStrike)
            {
                // if 10th roll has strike or spare, there will be a third roll
                if (rollNumber == 2) nextRoll = new NextRoll(frameNumber, 3);
                //  roll number == 3 UI checks for frame number == 11 to determine if the game is finished
                else nextRoll = new NextRoll(11, 0, true);
            }
            else
            {
                if (rollNumber == 2) nextRoll = new NextRoll(11, 0, true);
            }
        }

        private NextRoll processFirstFrame(int rollScore, int rollNumber)
        {
            _frameRolls.Rolls.Add(rollScore);
            if (rollNumber == 1)
            {
                // if the first frame is a strike
                if (rollScore == 10)
                {
                    _frameRolls.IsStrike = true;
                    return new NextRoll(2, 1);
                }
                return new NextRoll(1, 2);
            }
            // second roll
            else
            {
                // spare in first frame
                if (_frameRolls.Rolls.Sum() == 10) _frameRolls.IsSpare = true;
                return new NextRoll(2, 1);
            }
        }
    }
}
