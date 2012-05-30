﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.8.1.0
//      SpecFlow Generator Version:1.8.0.0
//      Runtime Version:4.0.30319.239
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Tests
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.8.1.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("PriorityQueue")]
    public partial class PriorityQueueFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PriorityQueue.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "PriorityQueue", "In order to keep track of items by priority\r\nAs a user\r\nI want to be able to get " +
                    "the highest priority item", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Enqueue two items, find highest priority item")]
        [NUnit.Framework.TestCaseAttribute("1", "1", "2", "2", "2", new string[0])]
        [NUnit.Framework.TestCaseAttribute("2", "2", "1", "1", "2", new string[0])]
        [NUnit.Framework.TestCaseAttribute("2", "2", "3", "3", "3", new string[0])]
        [NUnit.Framework.TestCaseAttribute("3", "1", "2", "2", "2", new string[0])]
        public virtual void EnqueueTwoItemsFindHighestPriorityItem(string item, string priority, string other_Item, string other_Priority, string result, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Enqueue two items, find highest priority item", exampleTags);
#line 6
this.ScenarioSetup(scenarioInfo);
#line 7
 testRunner.Given("an exmpty queue");
#line 8
 testRunner.When(string.Format("I enqueue {0} with priority {1}", item, priority));
#line 9
 testRunner.And(string.Format("I enqueue {0} with priority {1}", other_Item, other_Priority));
#line 10
 testRunner.When("I peek in the priority queue");
#line 11
 testRunner.Then(string.Format("I should find {0}", result));
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Cannot enqueue two items with the same priority")]
        [NUnit.Framework.TestCaseAttribute("1", "1", "2", "2", "2", new string[0])]
        public virtual void CannotEnqueueTwoItemsWithTheSamePriority(string item, string priority, string other_Item, string other_Priority, string result, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cannot enqueue two items with the same priority", exampleTags);
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
 testRunner.Given("an empty queue");
#line 21
 testRunner.When(string.Format("I enqueue {0} with priority {1}", item, priority));
#line 22
 testRunner.And(string.Format("I enqueue {0} with priority {1}", item, priority));
#line 23
 testRunner.Then("an error should occur");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
