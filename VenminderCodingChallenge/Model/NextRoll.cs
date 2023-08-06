namespace VenminderCodingChallenge.Model
{
    public class NextRoll
    {
        public int FrameNumber { get; set; }
        public int RollNumber { get; set; }
        public bool isEndOfGame { get; set; }

        public NextRoll(int frameNumber, int rollNumber, bool isEndOfGame = false)
        {
            FrameNumber = frameNumber;
            RollNumber = rollNumber;
            this.isEndOfGame = isEndOfGame;
        }

    }

    public class FrameDetail
    {
        public int FrameNumber { get; set; }
        public int? Roll1 { get; set; }
        public int? Roll2 { get; set; }
        public int? Roll3 { get; set; }
        public int? FrameScore { get; set; }
    }

    public class BowlingScoreResponse
    {
        public List<FrameDetail> FramesData { get; set; }
        public NextRoll NextRoll { get; set; }
        public BowlingScoreResponse(List<FrameDetail> framesData, NextRoll nextCell) 
        { 
            FramesData = framesData;
            NextRoll= nextCell;
        }
    }
}
