namespace Cake.Prca.Tests
{
    using System;
    using Prca.Issues;

    public class FakeRuleUrlResolver : BaseRuleUrlResolver<FakeRuleDescription>
    {
        protected override bool TryGetRuleDescription(string rule, FakeRuleDescription ruleDescription)
        {
            ruleDescription = new FakeRuleDescription { Rule = rule };
            return true;
        }
    }
}
