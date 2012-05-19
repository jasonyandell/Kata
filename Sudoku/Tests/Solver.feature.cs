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
    [NUnit.Framework.DescriptionAttribute("Solver")]
    public partial class SolverFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Solver.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Solver", "In order to solve Sudoku puzzles\r\nAs a library\r\nI want to be able to find step by" +
                    " step solutions to the problem", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
#line 7
 testRunner.Given("this board", "4 . . . . . 8 . 5\r\n. 3 . . . . . . .\r\n. . . 7 . . . . .\r\n. 2 . . . . . 6 .\r\n. . ." +
                    " . 8 . 4 . .\r\n. . . . 1 . . . .\r\n. . . 6 . 3 . 7 .\r\n5 . . 2 . . . . .\r\n1 . 4 . ." +
                    " . . . .", ((TechTalk.SpecFlow.Table)(null)));
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Evaluate a board")]
        [NUnit.Framework.TestCaseAttribute("0", "0", "0", new string[0])]
        [NUnit.Framework.TestCaseAttribute("7", "5", "0", new string[0])]
        [NUnit.Framework.TestCaseAttribute("7", "2", "0", new string[0])]
        [NUnit.Framework.TestCaseAttribute("7", "6", "2", new string[0])]
        [NUnit.Framework.TestCaseAttribute("0", "2", "5", new string[0])]
        public virtual void EvaluateABoard(string row, string col, string digit_Count, string[] exampleTags)
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Evaluate a board", exampleTags);
#line 20
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 21
 testRunner.When("the solver evaluates the board");
#line 22
  testRunner.And("the solver makes the required moves");
#line 23
 testRunner.Then(string.Format("I can place {0} digits at {1},{2}", digit_Count, row, col));
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Play required moves")]
        public virtual void PlayRequiredMoves()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Play required moves", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 33
 testRunner.When("the solver evaluates the board");
#line 34
  testRunner.And("the solver makes the required moves");
#line hidden
#line 35
 testRunner.Then("the board looks like this", "4 . . . . . 8 . 5\r\n. 3 . . . . . . .\r\n. . . 7 . . . . .\r\n. 2 . . . . . 6 .\r\n. . ." +
                    " . 8 . 4 . .\r\n. 4 . . 1 . . . .\r\n. . . 6 . 3 . 7 .\r\n5 . 3 2 . 1 . . .\r\n1 . 4 . ." +
                    " . . . .", ((TechTalk.SpecFlow.Table)(null)));
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion