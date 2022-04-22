using FluentAssertions;
using Moq;
using StorageCalc.Calculators;
using StorageCalc.Resources;
using StorageCalc.ViewModels;
using System.Collections.Generic;
using Xunit;

namespace StorageCalc.Tests.ViewModels;

public class MainViewModelTests
{
    Mock<IMessageBoxHelper> _messageBoxMock;
    IMessageBoxHelper _messageBox;
    Mock<IRaidCalculatorFactory> _raidCalculatorFactoryMock;
    IRaidCalculatorFactory _raidCalculatorFactory;
    Mock<IRaidCalculator> _raidCalculatorMock;
    IRaidCalculator _raidCalculator;
    RaidCalculatorResult _raidCalculatorResult = new(1.23, "none");

    MainViewModel _sut;

    public MainViewModelTests()
    {
        _messageBoxMock = new Mock<IMessageBoxHelper>();
        _messageBox = _messageBoxMock.Object;

        _raidCalculatorMock = new Mock<IRaidCalculator>();
        _raidCalculatorMock.Setup(x => x.Calculate(It.IsAny<int>(), It.IsAny<double>())).Returns(_raidCalculatorResult);
        _raidCalculator = _raidCalculatorMock.Object;

        _raidCalculatorFactoryMock = new Mock<IRaidCalculatorFactory>();
        _raidCalculatorFactoryMock.Setup(x => x.GetAll()).Returns(new List<IRaidCalculator> { _raidCalculator });
        _raidCalculatorFactory = _raidCalculatorFactoryMock.Object;

        _sut = new MainViewModel(_messageBox, _raidCalculatorFactory);
        _sut.SelectedCalculator = _raidCalculator;
    }

    [Theory]
    [InlineData(0, 1, true)]
    [InlineData(1, 0, true)]
    [InlineData(2, 2, false)]
    public void CalculateCommand_CannotExecute_WhenParameterIsInvalid(int diskCount, double diskSpace, bool hasSelectedRaid)
    {
        _sut.DiskCount = diskCount;
        _sut.DiskSpace = diskSpace;
        if (!hasSelectedRaid)
            _sut.SelectedCalculator = null;

        var result = _sut.CalculateCommand.CanExecute(null);

        result.Should().BeFalse();
    }

    [Fact]
    public void Calculators_IsNotEmpty_WhenClassIsInitialized()
    {
        Assert.NotEmpty(_sut.Calculators);
    }

    [Fact]
    public void CalculateCommand_UsesResultsFromSelectedCalculator_WhenSelectedCalculatorIsNotNull()
    {
        _sut.DiskCount = 2;
        _sut.DiskSpace = 2;

        _sut.CalculateCommand.Execute(null);

        _sut.FaultToleranceText.Should().Be(_raidCalculatorResult.FaultTolerance);
        _sut.TotalSpaceText.Should().Be($"{_raidCalculatorResult.UseableDiskSpace} TB");
    }

    [Fact]
    public void CalculateCommand_ShowsErrorMessage_WhenErrorIsReturnedFromCalculator()
    {
        _raidCalculatorMock.Setup(x => x.Calculate(It.IsAny<int>(), It.IsAny<double>())).Returns(new RaidCalculatorResult(default, default, "This is an error"));
        _sut.DiskCount = 2;
        _sut.DiskSpace = 2;

        _sut.CalculateCommand.Execute(null);

        _sut.FaultToleranceText.Should().BeNullOrWhiteSpace();
        _sut.TotalSpaceText.Should().BeNullOrWhiteSpace();
        _messageBoxMock.Verify(x => x.Show("This is an error"));
    }
}
