﻿using FluentAssertions;
using StorageCalc.Calculators;
using StorageCalc.Resources;
using Xunit;

namespace StorageCalc.Tests.Calculators;

public class Raid0CalculatorTests
{
    Raid0Calculator _sut = new();

    [Theory]
    [InlineData(2, 4, "de-CH", 7.28, "keine")]
    [InlineData(2, 4, "en-US", 7.28, "none")]
    public void Calculate_ReturnsExpectedValues_WhenCalled(
        int diskCount, double diskSpace, string cultureCode,
        double expectedTotalSpace, string expectedFaultTolerance)
    {
        LocalizedStrings.Instance.SetCulture(cultureCode);

        var result = _sut.Calculate(diskCount, diskSpace);

        result.Should().NotBeNull();
        result.ErrorMessage.Should().BeNullOrWhiteSpace();
        result.UseableDiskSpace.Should().BeApproximately(expectedTotalSpace, 0.01);
        result.FaultTolerance.Should().Be(expectedFaultTolerance);
    }

    [Fact]
    public void RaidNumber_IsSetToZero()
    {
        _sut.RaidNumber.Should().Be(0);
    }

    [Fact]
    public void Name_IsRaidFollowedByZero()
    {
        _sut.Name.Should().Be("RAID 0");
    }
}
