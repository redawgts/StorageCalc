using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageCalc.Calculators;

public class RaidCalculatorFactory : IRaidCalculatorFactory
{
    private readonly List<IRaidCalculator> _raidCalculators;

    public RaidCalculatorFactory()
    {
        var calcType = typeof(IRaidCalculator);
        _raidCalculators = calcType
            .Assembly
            .ExportedTypes
            .Where(x => calcType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(x => Activator.CreateInstance(x))
            .Cast<IRaidCalculator>()
            .OrderBy(x => x.RaidNumber)
            .ToList();
    }

    public IReadOnlyList<IRaidCalculator> GetAll()
    {
        return _raidCalculators;
    }
}
