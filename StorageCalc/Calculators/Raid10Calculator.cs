using StorageCalc.Resources;

namespace StorageCalc.Calculators;

public class Raid10Calculator : RaidCalculatorBase
{
    public Raid10Calculator()
    {
        RaidNumber = 10;
    }

    public override RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes)
    {
        if (diskCount % 2 != 0 || diskCount < 4)
            return CreateErrorResult(LocalizedStrings.Instance["AtLeastFourPlatesAndEvenNumber"]);

        var useableDiskSpace = (diskCount - 2) * ConvertDiskSpaceToProperNumber(diskSpaceTerrabytes);
        var faultTolerance = LocalizedStrings.Instance["MinOneDisk"];
        return new RaidCalculatorResult(useableDiskSpace, faultTolerance);
    }
}