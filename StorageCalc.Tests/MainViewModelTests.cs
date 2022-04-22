using Moq;
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
    [InlineData("2","4", true, null, null, null, null, "7.28 TB", "keine")]
    [InlineData("2","1", null, true, null, null, null, "0.91 TB", "1 Platte")]
    [InlineData("3","2", null, null, true, null, null, "3.64 TB", "1 Platte")]
    [InlineData("4","1", null, null, null, true, null, "1.82 TB", "2 Platten")]
    [InlineData("4","1", null, null, null, null, true, "1.82 TB", "Min. 1 Platte")]
    public void Calculate_ReturnsExpectedStrings_WhenGoodDataIsPassed(
        string txtDiskCount, string txtDiskSpace, bool? raid0,
        bool? raid1, bool? raid5, bool? raid6, bool? raid10,
        string expectedTotalSize, string expectedFaultTolerance)
    {
        (string totalSpace, string faultTolerance) = _sut.Calculate(txtDiskCount, txtDiskSpace,
            raid0, raid1, raid5, raid6, raid10);

        Assert.Equal(expectedTotalSize, totalSpace);
        Assert.Equal(expectedFaultTolerance, faultTolerance);
    }

    [Theory]
    [InlineData("1", "1", null, true, null, null, null)]
    [InlineData("1", "1", null, null, true, null, null)]
    [InlineData("1", "1", null, null, null, true, null)]
    [InlineData("1", "1", null, null, null, null, true)]
    [InlineData("abc", "1", true, null, null, null, null)]
    [InlineData("999999999999", "1", true, null, null, null, null)]
    public void Calculate_ShowsErrorMessageBox_WhenBadDataIsPassed(
        string txtDiskCount, string txtDiskSpace, bool? raid0,
        bool? raid1, bool? raid5, bool? raid6, bool? raid10)
    {
        (string totalSpace, string faultTolerance) = _sut.Calculate(txtDiskCount, txtDiskSpace, raid0, raid1, raid5, raid6, raid10);

        Assert.Equal(default(string), totalSpace);
        Assert.Equal(default(string), faultTolerance);
        _messageBoxMock.Verify(m => m.Show(It.IsAny<string>()));
    }
}
