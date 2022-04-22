using StorageCalc.Resources;
using System.Windows;

namespace StorageCalc.Calculators;

public class Raid0Calculator : RaidCalculatorBase
{
    public Raid0Calculator()
    {
        RaidNumber = 0;
    }

    public override RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes)
    {
        var useableDiskSpace = diskCount * ConvertDiskSpaceToProperNumber(diskSpaceTerrabytes);
        var faultTolerance = LocalizedStrings.Instance["None"];
        return new RaidCalculatorResult(useableDiskSpace, faultTolerance);
    }
}
