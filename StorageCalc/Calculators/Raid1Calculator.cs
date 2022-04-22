using StorageCalc.Resources;

namespace StorageCalc.Calculators;

public class Raid1Calculator : RaidCalculatorBase
{
    public Raid1Calculator()
    {
        RaidNumber = 1;
    }

    public override RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes)
    {
        if (diskCount != 2)
            return CreateErrorResult(LocalizedStrings.Instance["ExactlyTwoPlatesAreRequired"]);

        var useableDiskSpace = ConvertDiskSpaceToProperNumber(diskSpaceTerrabytes);
        var faultTolerance = LocalizedStrings.Instance["OneDisk"];
        return new RaidCalculatorResult(useableDiskSpace, faultTolerance);
    }
}
