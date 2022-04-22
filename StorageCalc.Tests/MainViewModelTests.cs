using Moq;
using StorageCalc.Resources;
using StorageCalc.ViewModels;
using Xunit;

namespace StorageCalc.Tests;

public class MainViewModelTests
{
    Mock<IMessageBoxHelper> _messageBoxMock;
    IMessageBoxHelper _messageBox;
    MainViewModel _sut;

    public MainViewModelTests()
    {
        _messageBoxMock = new Mock<IMessageBoxHelper>();
        _messageBox = _messageBoxMock.Object;

        _sut = new MainViewModel(_messageBox);
    }

    [Theory]
    [InlineData(2, 4, true, false, false, false, false, "7.28 TB", "keine", "de-CH")]
    [InlineData(2, 4, true, false, false, false, false, "7.28 TB", "none", "en-US")]
    [InlineData(2, 1, false, true, false, false, false, "0.91 TB", "1 Platte", "de-CH")]
    [InlineData(3, 2, false, false, true, false, false, "3.64 TB", "1 Platte", "de-CH")]
    [InlineData(4, 1, false, false, false, true, false, "1.82 TB", "2 Platten", "de-CH")]
    [InlineData(4, 1, false, false, false, false, true, "1.82 TB", "Min. 1 Platte", "de-CH")]
    public void Calculate_ReturnsExpectedStrings_WhenGoodDataIsPassed(
        int diskCount, double diskSpace, bool raid0,
        bool raid1, bool raid5, bool raid6, bool raid10,
        string expectedTotalSize, string expectedFaultTolerance, string cultureCode)
    {
        LocalizedStrings.Instance.SetCulture(cultureCode);
        _sut.DiskCount = diskCount;
        _sut.DiskSpace = diskSpace;
        _sut.Raid0 = raid0;
        _sut.Raid1 = raid1;
        _sut.Raid5 = raid5;
        _sut.Raid6 = raid6;
        _sut.Raid10 = raid10;

        if (_sut.CalculateCommand.CanExecute(null))
            _sut.CalculateCommand.Execute(null);

        Assert.Equal(expectedTotalSize, _sut.TotalSpaceText);
        Assert.Equal(expectedFaultTolerance, _sut.FaultToleranceText);
    }

    [Theory]
    [InlineData(1, 1, false, true, false, false, false)]
    [InlineData(1, 1, false, false, true, false, false)]
    [InlineData(1, 1, false, false, false, true, false)]
    [InlineData(1, 1, false, false, false, false, true)]
    public void Calculate_ShowsErrorMessageBox_WhenBadDataIsPassed(
        int diskCount, double diskSpace, bool raid0,
        bool raid1, bool raid5, bool raid6, bool raid10)
    {
        _sut.DiskCount = diskCount;
        _sut.DiskSpace = diskSpace;
        _sut.Raid0 = raid0;
        _sut.Raid1 = raid1;
        _sut.Raid5 = raid5;
        _sut.Raid6 = raid6;
        _sut.Raid10 = raid10;

        var canExecute = _sut.CalculateCommand.CanExecute(null);
        if (canExecute)
            _sut.CalculateCommand.Execute(null);

        Assert.True(canExecute);
        Assert.Equal(string.Empty, _sut.TotalSpaceText);
        Assert.Equal(string.Empty, _sut.FaultToleranceText);
        _messageBoxMock.Verify(m => m.Show(It.IsAny<string>()));
    }

    [Theory]
    [InlineData(2, 4, true, false, false, false, false)]
    [InlineData(2, 1, false, true, false, false, false)]
    [InlineData(3, 2, false, false, true, false, false)]
    [InlineData(4, 1, false, false, false, true, false)]
    [InlineData(4, 1, false, false, false, false, true)]
    public void Calculate_CanExecuteTrue_WhenGoodDataIsPassed(
        int diskCount, double diskSpace, bool raid0,
        bool raid1, bool raid5, bool raid6, bool raid10)
    {
        _sut.DiskCount = diskCount;
        _sut.DiskSpace = diskSpace;
        _sut.Raid0 = raid0;
        _sut.Raid1 = raid1;
        _sut.Raid5 = raid5;
        _sut.Raid6 = raid6;
        _sut.Raid10 = raid10;

        var canExecute = _sut.CalculateCommand.CanExecute(null);
        if (canExecute)
            _sut.CalculateCommand.Execute(null);

        Assert.True(canExecute);
    }

    [Fact]
    public void Calculate_CanExecuteFalse_WhenDataNotSet()
    {
        var result = _sut.CalculateCommand.CanExecute(null);
        Assert.False(result);
    }
}
