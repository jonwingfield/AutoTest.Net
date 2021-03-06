﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AutoTest.Core.TestRunners.TestRunners;
using AutoTest.Core.Messaging;
using AutoTest.Core.Configuration;
using Rhino.Mocks;
using AutoTest.Core.Caching.Projects;
using AutoTest.Core.FileSystem;
using AutoTest.Messages;
using System.IO;
using AutoTest.TestRunners.Shared.AssemblyAnalysis;

namespace AutoTest.Test.Core.TestRunners
{
    [TestFixture]
    public class NUnitTestRunnerTest
    {
        private NUnitTestRunner _runner;
        private IMessageBus _bus;
        private IConfiguration _configuration;
        private IAssemblyReader _assemblyReader;
        private IFileSystemService _fsService;

        [SetUp]
        public void SetUp()
        {
            _bus = MockRepository.GenerateMock<IMessageBus>();
            _configuration = MockRepository.GenerateMock<IConfiguration>();
			_assemblyReader = MockRepository.GenerateMock<IAssemblyReader>();
            _fsService = MockRepository.GenerateMock<IFileSystemService>();

            _runner = new NUnitTestRunner(_bus, _configuration, _assemblyReader, _fsService);
        }
		
		[Test]
		public void Should_check_for_nunit_test_framework_reference()
		{
            _fsService.Stub(f => f.FileExists("")).IgnoreArguments().Return(true);
            _assemblyReader.Stub(a => a.GetTargetFramework("")).Return(new Version());
			_assemblyReader.Stub(r => r.GetReferences("")).Return(new string[] { "nunit.framework" });
            var change = "";
            _runner.CanHandleTestFor(change).ShouldBeTrue();
		}
    }
}
