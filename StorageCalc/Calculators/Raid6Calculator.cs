﻿using StorageCalc.Resources;

namespace StorageCalc.Calculators;

public class Raid6Calculator : RaidCalculatorBase
{
    public Raid6Calculator()
    {
        RaidNumber = 6;
    }

    public override RaidCalculatorResult Calculate(int diskCount, double diskSpaceTerrabytes)
    {
        if (diskCount < 4)
            return CreateErrorResult(LocalizedStrings.Instance["AtLeastFourPlatesRequired"]);

        var useableDiskSpace = (diskCount - 2) * ConvertDiskSpaceToProperNumber(diskSpaceTerrabytes);
        var faultTolerance = LocalizedStrings.Instance["TwoDisks"];
        return new RaidCalculatorResult(useableDiskSpace, faultTolerance);
    }
}
