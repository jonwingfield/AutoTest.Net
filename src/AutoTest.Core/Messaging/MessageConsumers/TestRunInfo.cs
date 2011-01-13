using System;
using System.Linq;
using AutoTest.Core.Caching.Projects;
using System.Collections.Generic;
using AutoTest.Messages;
namespace AutoTest.Core.Messaging.MessageConsumers
{
	public class TestRunInfo
	{
		private List<TestToRun> _testsToRun;
        private List<TestToRun> _membersToRun;
        private List<TestToRun> _namespacesToRun;
        private List<TestRunner> _onlyRunTestsFor;
        private List<TestRunner> _rerunAllWhenFinishedFor;
		
		public Project Project { get; private set; }
		public string Assembly { get; private set; }
		
		public TestRunInfo(Project project, string assembly)
		{
			Project = project;
			Assembly = assembly;
            _testsToRun = new List<TestToRun>();
            _membersToRun = new List<TestToRun>();
            _namespacesToRun = new List<TestToRun>();
            _onlyRunTestsFor = new List<TestRunner>();
            _rerunAllWhenFinishedFor = new List<TestRunner>();
		}

        public bool OnlyRunSpcifiedTestsFor(TestRunner runner)
        {
            if (_onlyRunTestsFor.Contains(TestRunner.Any))
                return true;
            return _onlyRunTestsFor.Contains(runner);
        }

        public void ShouldOnlyRunSpcifiedTestsFor(TestRunner runner)
        {
            if (!_onlyRunTestsFor.Exists(r => r.Equals(runner)))
                _onlyRunTestsFor.Add(runner);
        }

        public bool RerunAllTestWhenFinishedFor(TestRunner runner)
        {
            if (_rerunAllWhenFinishedFor.Contains(TestRunner.Any))
                return true;
            return _rerunAllWhenFinishedFor.Contains(runner);
        }

        public void ShouldRerunAllTestWhenFinishedFor(TestRunner runner)
        {
            if (!_rerunAllWhenFinishedFor.Contains(runner))
                _rerunAllWhenFinishedFor.Add(runner);
        }

        public bool RerunAllTestWhenFinishedForAny()
        {
            return _rerunAllWhenFinishedFor.Count > 0;
        }

        public void AddTestsToRun(TestToRun[] tests) { _testsToRun.AddRange(tests); }
        public TestToRun[] GetTests() { return _testsToRun.ToArray(); }
        public string[] GetTestsFor(TestRunner runner) { return getRunItemsFor(runner, _testsToRun); }

        public void AddMembersToRun(TestToRun[] members) { _membersToRun.AddRange(members); }
        public TestToRun[] GetMembers() { return _membersToRun.ToArray(); }
        public string[] GetMembersFor(TestRunner runner) { return getRunItemsFor(runner, _membersToRun); }

        public void AddNamespacesToRun(TestToRun[] namespaces) { _namespacesToRun.AddRange(namespaces); }
        public TestToRun[] GetNamespaces() { return _namespacesToRun.ToArray(); }
        public string[] GetNamespacesFor(TestRunner runner) { return getRunItemsFor(runner, _namespacesToRun); }

        private string[] getRunItemsFor(TestRunner runner, List<TestToRun> runList)
        {
            var query = from t in runList
                        where t.Runner.Equals(runner) || t.Runner.Equals(TestRunner.Any)
                        select t.Test;
            return query.ToArray();
        }
	}
}

