using System.Net.NetworkInformation;
using VenminderCodingChallenge.Model;

namespace VenminderCodingChallenge.Types
{
    /**
      The 10th frame is a bit different:
        If you roll a strike in the first shot of the 10th frame, you get 2 more shots.
        If you roll a spare in the first two shots of the 10th frame, you get 1 more shot.
        If you leave the 10th frame open after two shots, the game is over and you do not get an additional shot.
        How to Score: The score for the 10th frame is the total number of pins knocked down in the 10th frame.
     */
    public class FrameRolls
    {

        public int FrameNumber { get; set; }
        public List<int> Rolls { get; set; }
        public Boolean IsStrike { get; set; }
        public Boolean IsSpare { get; set; }
        public FrameRolls? NextFrame { get; set; }


        /// <summary>
        /// Create an instance of the FrameRolls
        /// </summary>
        public FrameRolls(int frameNumber, int? rollScore)
        {
            FrameNumber = frameNumber;
            IsStrike = false;
            IsSpare = false;
            Rolls = new List<int>();
            if (rollScore.HasValue) Rolls.Add((int)rollScore);
        }

        public int GetFrameScore()
        {
            var frameScore = 0;

            if (this.IsStrike && FrameNumber != 10)
            {
                frameScore = 10;
                if (NextFrame != null) getStrikeValue(NextFrame, ref frameScore);
            }
            else if (this.IsSpare && FrameNumber != 10)
            {
                frameScore += (10 + (NextFrame == null ? 0 : NextFrame.Rolls[0]));
            }
            else
            {
                frameScore = Rolls.Sum();
            }

            return frameScore;
        }

        private void getStrikeValue(FrameRolls nextFrame, ref int totalScore)
        {
            totalScore += nextFrame.Rolls[0];
            if (nextFrame.IsStrike && nextFrame.FrameNumber != 10)
            {
                if (nextFrame.NextFrame != null) totalScore += nextFrame.NextFrame.Rolls[0];
            }
            else totalScore += nextFrame.Rolls.Count > 1 ? nextFrame.Rolls[1] : 0;
        }

        // if frame number exist, add element to RollNumber list 
        // if frame number doesn't exist, add a new Node to the tail of Frame
        // except the last frame, each frame can only be update ONCE
        public NextRoll CreateFrame(int frameNumber, int rollScore)
        {
            var newFrame = new FrameRolls(frameNumber, rollScore);
            FrameRolls currentFrame = GetLastFrame();
            currentFrame.NextFrame = newFrame;
            // if strike
            if (rollScore == 10)
            {
                newFrame.IsStrike = true;
                return new NextRoll(frameNumber + 1, 1);
            }

            return new NextRoll(frameNumber, 2);
        }

        public NextRoll UpdateFrame(int frameNumber, int rollScore)
        {
            FrameRolls currentFrame = GetLastFrame();
            currentFrame.Rolls.Add(rollScore);
            // if spare
            if (currentFrame.Rolls.Sum() == 10) currentFrame.IsSpare = true;

            return new NextRoll(frameNumber + 1, 1);
        }

        public FrameRolls GetLastFrame()
        {
            FrameRolls tempFrame = this;
            while (tempFrame.NextFrame != null)
            {
                tempFrame = tempFrame.NextFrame;
            }
            return tempFrame;
        }
    }
}
