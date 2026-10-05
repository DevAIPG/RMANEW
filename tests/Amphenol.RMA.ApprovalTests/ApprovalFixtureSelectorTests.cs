using Xunit;

public class ApprovalFixtureSelectorTests
{
    [Fact]
    public void AutomaticSelectionDoesNotReuseRequestsAcrossScenarios()
    {
        var selector = new ApprovalFixtureSelector((_, _) => new[] { 101, 102, 103 }, _ => null);

        Assert.Equal(101, selector.Select("RMA_TEST_ROLLBACK_ID"));
        Assert.Equal(102, selector.Select("RMA_TEST_CONCURRENT_ID"));
        Assert.Equal(103, selector.Select("RMA_TEST_M10_FAILURE_ID"));
    }

    [Fact]
    public void ManualOverridesAreReservedBeforeTheirScenarioRuns()
    {
        var selector = new ApprovalFixtureSelector((_, excluded) =>
        {
            Assert.Contains(101, excluded);
            return new[] { 101, 102 };
        }, key => key == "RMA_TEST_CONCURRENT_ID" ? "101" : null);

        Assert.Equal(102, selector.Select("RMA_TEST_ROLLBACK_ID"));
        Assert.Equal(101, selector.Select("RMA_TEST_CONCURRENT_ID"));
    }

    [Fact]
    public void DuplicateOverridesFailBeforeLookingForCandidates()
    {
        var selector = new ApprovalFixtureSelector((_, _) => throw new Exception("Must not query"),
            key => key is "RMA_TEST_ROLLBACK_ID" or "RMA_TEST_CONCURRENT_ID" ? "101" : null);

        var error = Assert.Throws<InvalidOperationException>(() => selector.Select("RMA_TEST_M10_FAILURE_ID"));
        Assert.Contains("configured more than once", error.Message);
    }

    [Theory]
    [InlineData("unknown")]
    [InlineData("0")]
    [InlineData("-1")]
    public void InvalidOverrideExplainsHowToEnableAutomaticSelection(string value)
    {
        var selector = new ApprovalFixtureSelector((_, _) => throw new Exception("Must not query"),
            key => key == "RMA_TEST_ROLLBACK_ID" ? value : null);

        var error = Assert.Throws<InvalidOperationException>(() => selector.Select("RMA_TEST_ROLLBACK_ID"));
        Assert.Contains("Unset it to use automatic selection", error.Message);
    }

    [Fact]
    public void ExhaustedCandidatesFailWithSetupInstructions()
    {
        var selector = new ApprovalFixtureSelector((_, _) => new[] { 101 }, _ => null);
        selector.Select("RMA_TEST_ROLLBACK_ID");

        var error = Assert.Throws<InvalidOperationException>(() => selector.Select("RMA_TEST_CONCURRENT_ID"));
        Assert.Contains("No eligible unused test request", error.Message);
        Assert.Contains("below $20,000", error.Message);
    }

    [Fact]
    public void DirectorScenarioUsesItsOwnEligibilitySearch()
    {
        var selector = new ApprovalFixtureSelector((name, _) =>
            name == "RMA_TEST_BYPASS_DIRECTOR_ID" ? new[] { 201 } : new[] { 101 }, _ => null);

        Assert.Equal(101, selector.Select("RMA_TEST_BYPASS_ID"));
        Assert.Equal(201, selector.Select("RMA_TEST_BYPASS_DIRECTOR_ID"));
    }
}
