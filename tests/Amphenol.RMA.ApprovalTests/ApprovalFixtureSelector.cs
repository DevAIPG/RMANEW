internal sealed class ApprovalFixtureSelector
{
    internal static readonly string[] Names =
    {
        "RMA_TEST_ROLLBACK_ID", "RMA_TEST_CONCURRENT_ID", "RMA_TEST_BYPASS_ID",
        "RMA_TEST_M10_FAILURE_ID", "RMA_TEST_EDIT_PROTECTION_ID",
        "RMA_TEST_VALIDATION_FAILURE_ID", "RMA_TEST_EDIT_LIFECYCLE_ID",
        "RMA_TEST_BYPASS_DIRECTOR_ID"
    };

    private readonly object _gate = new();
    private readonly HashSet<int> _used = new();
    private readonly Func<string, IReadOnlyCollection<int>, IEnumerable<int>> _findCandidates;
    private readonly Func<string, string> _readOverride;

    public ApprovalFixtureSelector(Func<string, IReadOnlyCollection<int>, IEnumerable<int>> findCandidates,
        Func<string, string> readOverride)
    {
        _findCandidates = findCandidates;
        _readOverride = readOverride;
    }

    public int Select(string name)
    {
        lock (_gate)
        {
            if (!Names.Contains(name)) throw new ArgumentException("Unknown approval fixture.", nameof(name));
            var overrides = new Dictionary<string, int>();
            foreach (var key in Names)
            {
                var value = _readOverride(key);
                if (string.IsNullOrWhiteSpace(value)) continue;
                if (!int.TryParse(value, out var explicitId) || explicitId <= 0)
                    throw new InvalidOperationException($"{key} must be a positive CSEXSW_Rma.Id. Unset it to use automatic selection.");
                if (overrides.ContainsValue(explicitId))
                    throw new InvalidOperationException($"Fixture overrides must use different request IDs; Id={explicitId} is configured more than once.");
                overrides.Add(key, explicitId);
            }

            int id;
            if (overrides.TryGetValue(name, out var configured))
                id = configured;
            else
            {
                var excluded = _used.Concat(overrides.Values).Distinct().ToArray();
                id = _findCandidates(name, excluded).FirstOrDefault(x => x > 0 && !excluded.Contains(x));
                if (id == 0)
                    throw new InvalidOperationException(
                        $"No eligible unused test request was found for {name}. Create a fresh submitted pending USD request with valid lines and an assigned approver " +
                        (name == "RMA_TEST_BYPASS_DIRECTOR_ID" ? "at or above $20,000 assigned to the quality director." : "below $20,000.") +
                        " The test does not create or reset existing requests automatically.");
            }
            if (!_used.Add(id))
                throw new InvalidOperationException($"Fixture Id={id} was already used in this test run. Choose a different request or unset {name}.");
            return id;
        }
    }
}
