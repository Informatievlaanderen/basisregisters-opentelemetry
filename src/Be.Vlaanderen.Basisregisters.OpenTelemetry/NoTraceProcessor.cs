using System;
using System.Diagnostics;
using OpenTelemetry;

namespace Be.Vlaanderen.Basisregisters.OpenTelemetry
{
    internal sealed class NoTraceProcessor : BaseProcessor<Activity>
    {
        private readonly Predicate<string> _predicate;

        public NoTraceProcessor(Predicate<string> predicate)
        {
            _predicate = predicate;
        }

        public override void OnEnd(Activity activity)
        {
            if (_predicate(activity.DisplayName))
            {
                activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
            }
        }
    }
}
