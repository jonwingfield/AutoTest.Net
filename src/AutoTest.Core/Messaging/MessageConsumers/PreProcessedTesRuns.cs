﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AutoTest.Core.Messaging.MessageConsumers
{
    public class PreProcessedTesRuns
    {
        public Action<AutoTest.TestRunners.Shared.Targeting.Platform, Action<ProcessStartInfo>> ProcessWrapper { get; private set; }
        public RunInfo[] RunInfos { get; private set; }

        public PreProcessedTesRuns(Action<AutoTest.TestRunners.Shared.Targeting.Platform, Action<ProcessStartInfo>> processWrapper, RunInfo[] runinfos)
        {
            ProcessWrapper = processWrapper;
            RunInfos = runinfos;
        }
    }
}
