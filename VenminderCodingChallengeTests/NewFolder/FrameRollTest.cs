using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenminderCodingChallenge.Model;
using VenminderCodingChallenge.Types;

namespace VenminderCodingChallengeTests.NewFolder
{
    public class FrameRollTest
    {

        [Test]
        public void CreateEmptyFrameRollLinkedList_Should_HaveAllTheProperty()
        {
            var linkedListHead = new FrameRolls(1, null);
            linkedListHead.IsSpare.Should().BeFalse();
            linkedListHead.IsStrike.Should().BeFalse();
            linkedListHead.Rolls.Should().BeEmpty();
        }

        [Test]
        public void CreateStrikeFrameRollLinkedList_Should_HaveAllTheProperty()
        {
            var linkedListHead = new FrameRolls(1, 10);
            linkedListHead.CreateFrame(2, 10);
            var nodeToTest = linkedListHead.NextFrame;
            if (nodeToTest != null)
            {
                nodeToTest.IsSpare.Should().BeFalse();
                nodeToTest.IsStrike.Should().BeTrue();
                nodeToTest.Rolls.Sum().Should().Be(10);
                nodeToTest.GetFrameScore().Should().Be(10);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void CreateSpareFrameRollLinkedList_Should_HaveAllTheProperty()
        {
            var linkedListHead = new FrameRolls(1, 10);
            linkedListHead.CreateFrame(2, 5);
            linkedListHead.UpdateFrame(2, 5);
            var nodeToTest = linkedListHead.NextFrame;

            if (nodeToTest != null)
            {
                nodeToTest.IsSpare.Should().BeTrue();
                nodeToTest.IsStrike.Should().BeFalse();
                nodeToTest.Rolls.Sum().Should().Be(10);
                nodeToTest.GetFrameScore().Should().Be(10);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void CreatePassFrameRollLinkedList_Should_HaveAllTheProperty()
        {
            var linkedListHead = new FrameRolls(1, 10);
            linkedListHead.CreateFrame(2, 4);
            linkedListHead.UpdateFrame(2, 4);
            var nodeToTest = linkedListHead.NextFrame;

            if (nodeToTest != null)
            {
                nodeToTest.IsSpare.Should().BeFalse();
                nodeToTest.IsStrike.Should().BeFalse();
                nodeToTest.Rolls.Sum().Should().Be(8);
                nodeToTest.GetFrameScore().Should().Be(8);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TenthFrameRollLinkedList_Should_HaveAllTheProperty()
        {
            var linkedListHead = new FrameRolls(1, 10);
            linkedListHead.CreateFrame(10, 4);
            linkedListHead.UpdateFrame(10, 4);
            var nodeToTest = linkedListHead.NextFrame;


            if (nodeToTest != null)
            {
                nodeToTest.IsSpare.Should().BeFalse();
                nodeToTest.IsStrike.Should().BeFalse();
                nodeToTest.Rolls.Sum().Should().Be(8);
                nodeToTest.GetFrameScore().Should().Be(8);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TenthFrameIsAStike_Should_HaveAllTheProperty()
        {
            var linkedListHead = new FrameRolls(1, 10);
            linkedListHead.CreateFrame(10, 10);
            linkedListHead.UpdateFrame(10, 5);
            linkedListHead.UpdateFrame(10, 5);
            var nodeToTest = linkedListHead.NextFrame;


            if (nodeToTest != null)
            {
                nodeToTest.IsSpare.Should().BeFalse();
                nodeToTest.IsStrike.Should().BeTrue();
                nodeToTest.Rolls.Sum().Should().Be(20);
                nodeToTest.GetFrameScore().Should().Be(20);
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public void TenthFrameIsASpare_Should_HaveAllTheProperty()
        {
            var linkedListHead = new FrameRolls(1, 10);
            linkedListHead.CreateFrame(10, 4);
            linkedListHead.UpdateFrame(10, 6);
            linkedListHead.UpdateFrame(10, 5);
            var nodeToTest = linkedListHead.NextFrame;


            if (nodeToTest != null)
            {
                nodeToTest.IsSpare.Should().BeTrue();
                nodeToTest.IsStrike.Should().BeFalse();
                nodeToTest.Rolls.Sum().Should().Be(15);
                nodeToTest.GetFrameScore().Should().Be(15);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
