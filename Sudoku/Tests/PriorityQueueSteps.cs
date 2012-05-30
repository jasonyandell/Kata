using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Tests
{
    [Binding]
    public class PriorityQueueSteps
    {
        [Given("an empty queue")]
        public void GivenAnEmptyQueue()
        {
            ScenarioContext.Current.Pending();
        }

        [When("I enqueue (.*) with priority (.*)")]
        public void WhenIEnqueueItemWithPriorityP(string item, int priority)
        {
            ScenarioContext.Current.Pending();
        }

        [When("I enqueue peek in the priority queue")]
        public void WhenIPeekInThePriorityQueue()
        {
            ScenarioContext.Current.Pending();
        }

        [Then("I should find (.*)")]
        public void ThenIShouldFind(string key)
        {
            ScenarioContext.Current.Pending();
        }

        [Then("an error should occur")]
        public void ThenAnErrorShouldOccur()
        {
            Assert.IsNotNull(ScenarioContext.Current.TestError);
        }

    }
}
