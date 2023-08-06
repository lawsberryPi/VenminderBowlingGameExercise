using FluentAssertions;
using System.Linq;
using VenminderCodingChallenge.Model;
using VenminderCodingChallenge.Repositories;
using VenminderCodingChallenge.Types;

namespace VenminderCodingChallengeTests.Repositories
{
    public class RepositoryTests
    {
        [Test]
        public void AddFirstFrameThatIsOpen_Should_GetCorrectNextCell()
        {
            var bowlingRepoUnderTest = new BowlingScoreRepository();
            var result = bowlingRepoUnderTest.ProcessFrame(1, 1, 7);
            var frameScore = new List<int>() { 10 };
            var expectedNextCell1 = new NextRoll(1, 2);
            var expectedNextCell2 = new NextRoll(2, 1);

            var result1 = bowlingRepoUnderTest.ProcessFrame(1, 2, 3);

            result.NextRoll.Should().BeEquivalentTo(expectedNextCell1);
            result1.NextRoll.Should().BeEquivalentTo(expectedNextCell2);
            result1.FramesData.Select(x => x.FrameScore).ToList().Should().BeEquivalentTo(frameScore);
        }

        [Test]
        public void FirstFrameIsStrike_Should_SkipSecondRoll()
        {
            var bowlingScoreRepositoryUnderTest = new BowlingScoreRepository();
            var result = bowlingScoreRepositoryUnderTest.ProcessFrame(1, 1, 10);
            var frameScore = new List<int>() { 10 };
            var expectedNextCell1 = new NextRoll(2, 1);

            result.NextRoll.Should().BeEquivalentTo(expectedNextCell1);
            result.FramesData.Select(x => x.FrameScore).ToList().Should().BeEquivalentTo(frameScore);
        }

        [Test]
        public void FirstFrameIsStrikeSecondFrameIsNotStrike_Should_ReturnCorrectScore()
        {
            var bowlingScoreRepositoryUnderTest = new BowlingScoreRepository();
            var strikeFrame = bowlingScoreRepositoryUnderTest.ProcessFrame(1, 1, 10);
            var resultSecondFrameFirstRoll = bowlingScoreRepositoryUnderTest.ProcessFrame(strikeFrame.NextRoll.FrameNumber, strikeFrame.NextRoll.RollNumber, 4);
            var resultSecondFrameSecondRoll = bowlingScoreRepositoryUnderTest.ProcessFrame(resultSecondFrameFirstRoll.NextRoll.FrameNumber, resultSecondFrameFirstRoll.NextRoll.RollNumber
                , 5);

            // because the first frame is strike, therefore the first frame score is 10 plus the next two rolls
            // second frame is a pass, therefore score is 9
            var frameScore = new List<int>() { 19, 9 };
            var expectedNextCell1 = new NextRoll(3, 1);

            resultSecondFrameSecondRoll.NextRoll.Should().BeEquivalentTo(expectedNextCell1);
            resultSecondFrameSecondRoll.FramesData.Select(x => x.FrameScore).ToList().Should().BeEquivalentTo(frameScore);
        }

        [Test]
        public void FirstFrameIsStrikeSecondFrameIsStrikeThirdFrameIsNotStrike_Should_ReturnCorrectScore()
        {
            var bowlingScoreRepositoryUnderTest = new BowlingScoreRepository();
            var strikeFrame = bowlingScoreRepositoryUnderTest.ProcessFrame(1, 1, 10);
            var strikeSecondFrame = bowlingScoreRepositoryUnderTest.ProcessFrame(strikeFrame.NextRoll.FrameNumber, strikeFrame.NextRoll.RollNumber, 10);
            var notStrikeThirdFrameFirstRoll = bowlingScoreRepositoryUnderTest.ProcessFrame(strikeSecondFrame.NextRoll.FrameNumber, strikeSecondFrame.NextRoll.RollNumber
                , 5);
            var notStrikeThirdFrameSecondRoll = bowlingScoreRepositoryUnderTest.ProcessFrame(notStrikeThirdFrameFirstRoll.NextRoll.FrameNumber, notStrikeThirdFrameFirstRoll.NextRoll.RollNumber
    , 3);

            // because the first frame is strike, therefore the first frame score is 10 plus the next two rolls
            // second frame is a pass, therefore score is 9
            var frameScore = new List<int>() { 25, 18, 8 };
            var expectedNextCell1 = new NextRoll(4, 1);

            notStrikeThirdFrameSecondRoll.NextRoll.Should().BeEquivalentTo(expectedNextCell1);
            notStrikeThirdFrameSecondRoll.FramesData.Select(x => x.FrameScore).ToList().Should().BeEquivalentTo(frameScore);
        }

        [Test]
        public void FirstFrameIsStrikeSecondFrameIsStrikeThirdFrameIsSpare_Should_ReturnCorrectScore()
        {
            var bowlingScoreRepositoryUnderTest = new BowlingScoreRepository();
            var strikeFrame = bowlingScoreRepositoryUnderTest.ProcessFrame(1, 1, 10);
            var strikeSecondFrame = bowlingScoreRepositoryUnderTest.ProcessFrame(strikeFrame.NextRoll.FrameNumber, strikeFrame.NextRoll.RollNumber, 10);
            var notStrikeThirdFrameFirstRoll = bowlingScoreRepositoryUnderTest.ProcessFrame(strikeSecondFrame.NextRoll.FrameNumber, strikeSecondFrame.NextRoll.RollNumber
                , 5);
            var notStrikeThirdFrameSecondRoll = bowlingScoreRepositoryUnderTest.ProcessFrame(notStrikeThirdFrameFirstRoll.NextRoll.FrameNumber, notStrikeThirdFrameFirstRoll.NextRoll.RollNumber
    , 5);

            var frameScore = new List<int>() { 25, 20, 10 };
            var expectedNextCell1 = new NextRoll(4, 1);

            notStrikeThirdFrameSecondRoll.NextRoll.Should().BeEquivalentTo(expectedNextCell1);
            notStrikeThirdFrameSecondRoll.FramesData.Select(x => x.FrameScore).ToList().Should().BeEquivalentTo(frameScore);
        }

        [Test]
        public void FirstFrameIsStrikeSecondFrameIsSpareThirdFrameIsStrike_Should_ReturnCorrectScore()
        {
            var bowlingScoreRepositoryUnderTest = new BowlingScoreRepository();
            var strikeFrame = bowlingScoreRepositoryUnderTest.ProcessFrame(1, 1, 10);
            var spareSecondFrameRoll1 = bowlingScoreRepositoryUnderTest.ProcessFrame(strikeFrame.NextRoll.FrameNumber, strikeFrame.NextRoll.RollNumber, 5);
            var spareSecondFrameRoll2 = bowlingScoreRepositoryUnderTest.ProcessFrame(spareSecondFrameRoll1.NextRoll.FrameNumber, spareSecondFrameRoll1.NextRoll.RollNumber, 5);

            var strikeThirdFrameFirstRoll = bowlingScoreRepositoryUnderTest.ProcessFrame(spareSecondFrameRoll2.NextRoll.FrameNumber, spareSecondFrameRoll2.NextRoll.RollNumber
                , 10);

            var frameScore = new List<int>() { 20, 20, 10 };
            var expectedNextCell1 = new NextRoll(4, 1);

            strikeThirdFrameFirstRoll.NextRoll.Should().BeEquivalentTo(expectedNextCell1);
            strikeThirdFrameFirstRoll.FramesData.Select(x => x.FrameScore).ToList().Should().BeEquivalentTo(frameScore);
        }

        [Test]
        public void FirstFrameIsSpareSecondFrameIsSpareThirdFrameIsStrike_Should_ReturnCorrectScore()
        {
            var bowlingScoreRepositoryUnderTest = new BowlingScoreRepository();
            var firstSpareFrameRoll1 = bowlingScoreRepositoryUnderTest.ProcessFrame(1, 1, 6);
            var firstSpareFrameRoll2 = bowlingScoreRepositoryUnderTest.ProcessFrame(firstSpareFrameRoll1.NextRoll.FrameNumber, firstSpareFrameRoll1.NextRoll.RollNumber, 4);
            var spareSecondFrameRoll1 = bowlingScoreRepositoryUnderTest.ProcessFrame(firstSpareFrameRoll2.NextRoll.FrameNumber, firstSpareFrameRoll2.NextRoll.RollNumber, 5);
            var spareSecondFrameRoll2 = bowlingScoreRepositoryUnderTest.ProcessFrame(spareSecondFrameRoll1.NextRoll.FrameNumber, spareSecondFrameRoll1.NextRoll.RollNumber, 5);

            var strikeThirdFrameFirstRoll = bowlingScoreRepositoryUnderTest.ProcessFrame(spareSecondFrameRoll2.NextRoll.FrameNumber, spareSecondFrameRoll2.NextRoll.RollNumber
                , 10);

            var frameScore = new List<int>() { 15, 20, 10 };
            var expectedNextCell1 = new NextRoll(4, 1);

            strikeThirdFrameFirstRoll.NextRoll.Should().BeEquivalentTo(expectedNextCell1);
            strikeThirdFrameFirstRoll.FramesData.Select(x => x.FrameScore).ToList().Should().BeEquivalentTo(frameScore);
        }

        [Test]
        public void FirstFrameIsSpareSecondFrameIsSpareThirdFrameIsOpen_Should_ReturnCorrectScore()
        {
            var bowlingScoreRepositoryUnderTest = new BowlingScoreRepository();
            var firstSpareFrameRoll1 = bowlingScoreRepositoryUnderTest.ProcessFrame(1, 1, 6);
            var firstSpareFrameRoll2 = bowlingScoreRepositoryUnderTest.ProcessFrame(firstSpareFrameRoll1.NextRoll.FrameNumber, firstSpareFrameRoll1.NextRoll.RollNumber, 4);
            var spareSecondFrameRoll1 = bowlingScoreRepositoryUnderTest.ProcessFrame(firstSpareFrameRoll2.NextRoll.FrameNumber, firstSpareFrameRoll2.NextRoll.RollNumber, 5);
            var spareSecondFrameRoll2 = bowlingScoreRepositoryUnderTest.ProcessFrame(spareSecondFrameRoll1.NextRoll.FrameNumber, spareSecondFrameRoll1.NextRoll.RollNumber, 5);

            var openThirdFrameFirstRoll1 = bowlingScoreRepositoryUnderTest.ProcessFrame(spareSecondFrameRoll2.NextRoll.FrameNumber, spareSecondFrameRoll2.NextRoll.RollNumber
                , 3);
            var openeThirdFrameFirstRoll2 = bowlingScoreRepositoryUnderTest.ProcessFrame(openThirdFrameFirstRoll1.NextRoll.FrameNumber, spareSecondFrameRoll1.NextRoll.RollNumber
    , 2);

            var frameScore = new List<int>() { 15, 13, 5 };
            var expectedNextCell1 = new NextRoll(4, 1);

            openeThirdFrameFirstRoll2.NextRoll.Should().BeEquivalentTo(expectedNextCell1);
            openeThirdFrameFirstRoll2.FramesData.Select(x => x.FrameScore).ToList().Should().BeEquivalentTo(frameScore);
        }

        [Test]
        public void RunExampleFromProblemDescription_Should_ReturnCorrectScore()
        {
            var testInput = new List<int>() { 3, 7, 3, 5, 2, 8, 1, 4, 6, 2, 4, 8, 0, 8, 0, 8, 2, 10, 1, 7 };
            var firstFrame = new List<int>() { 1, 1, 4 };
            var result = GenerateFramesFromList(testInput, firstFrame);
            result.GetTotalScore().Should().Be(110);
        }

        [Test]
        public void AllStike_Should_Return300()
        {
            int[] testInput = new int[11];
            Array.Fill(testInput, 10);
            var firstFrame = new List<int>() { 1, 1, 10 };
            var result = GenerateFramesFromList(testInput.ToList(), firstFrame);
            result.GetTotalScore().Should().Be(300);
        }

        [Test]
        public void AllSpare_Should_Return150()
        {
            int[] testInput = new int[20];
            Array.Fill(testInput, 5);
            var firstFrame = new List<int>() { 1, 1, 5 };
            var result = GenerateFramesFromList(testInput.ToList(), firstFrame);
            result.GetTotalScore().Should().Be(150);
        }

        [Test]
        public void ClearScore_Should_ReturnEmptyHead()
        {
            int[] testInput = new int[20];
            Array.Fill(testInput, 5);
            var firstFrame = new List<int>() { 1, 1, 5 };
            var result = GenerateFramesFromList(testInput.ToList(), firstFrame);
            result.resetGame();
            result.GetTotalScore().Should().Be(0);
        }

        private BowlingScoreRepository GenerateFramesFromList(List<int> input, List<int> firstFrame)
        {
            var bowlingScoreRepositoryUnderTest = new BowlingScoreRepository();
            var processResult = bowlingScoreRepositoryUnderTest.ProcessFrame(firstFrame[0], firstFrame[1], firstFrame[2]);

            input.ToList().ForEach(eachRollScore =>
            {
                var nextRoll = processResult.NextRoll;
                processResult = bowlingScoreRepositoryUnderTest.ProcessFrame(nextRoll.FrameNumber, nextRoll.RollNumber, eachRollScore);
            });

            return bowlingScoreRepositoryUnderTest;
        }
    }
}