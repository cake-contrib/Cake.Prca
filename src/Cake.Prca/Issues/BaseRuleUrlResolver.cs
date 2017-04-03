namespace Cake.Prca.Issues
{
    using System;

    /// <summary>
    /// Base class for retrieving an URL linking to a site containing help for a rule.
    /// </summary>
    /// <typeparam name="T">Type of the rule description</typeparam>
    public abstract class BaseRuleUrlResolver<T>
        where T : BaseRuleDescription, new()
    {
        /// <summary>
        /// Returns an URL linking to a site describing a specific rule.
        /// </summary>
        /// <param name="rule">Code of the rule for which the URL should be retrieved.</param>
        /// <returns>URL linking to a site describing the rule, or <c>null</c> if <paramref name="rule"/>
        /// could not be parsed.</returns>
        public Uri ResolveRuleUrl(string rule)
        {
            rule.NotNullOrWhiteSpace(nameof(rule));

            var ruleDescription = new T { Rule = rule };
            return !this.TryGetRuleDescription(rule, ruleDescription) ? null : this.GetRuleUri(ruleDescription);
        }

        /// <summary>
        /// Parses a rule into a <see cref="BaseRuleDescription"/>.
        /// </summary>
        /// <param name="rule">Rule which should be parsed.</param>
        /// <param name="ruleDescription">Description of the rule.</param>
        /// <returns><c>true</c> if rule could by parsed successfully, otherwise <c>false</c>.</returns>
        protected abstract bool TryGetRuleDescription(string rule, T ruleDescription);

        /// <summary>
        /// Gets a <see cref="Uri"/> linking to a site containing help for a
        /// specific <see cref="BaseRuleDescription"/>.
        /// </summary>
        /// <param name="ruleDescription">Description of the rule.</param>
        /// <returns>Uri linking to a site containing help for the rule.</returns>
        protected abstract Uri GetRuleUri(T ruleDescription);
    }
}