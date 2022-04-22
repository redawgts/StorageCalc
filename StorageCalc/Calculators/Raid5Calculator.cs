using StorageCalc.Resources;

namespace StorageCalc.Calculators;

public class Raid5Calculator : RaidCalculatorBase
{
    public Raid5Calculator()
    {
        RaidNumber = 5;
    }

    public override RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes)
    {
        if (diskCount < 3)
            return CreateErrorResult(LocalizedStrings.Instance["ExactlyTwoPlatesAreRequired"]);

        var useableDiskSpace = (diskCount - 1) * ConvertDiskSpaceToProperNumber(diskSpaceTerrabytes);
        var faultTolerance = LocalizedStrings.Instance["OneDisk"];
        return new RaidCalculatorResult(useableDiskSpace, faultTolerance);
    }
}
