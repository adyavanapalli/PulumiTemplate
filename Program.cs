using System.Threading.Tasks;
using Pulumi;

/// <summary>
/// TODO: Needs documentation.
/// </summary>
public class Program
{
    /// <summary>
    /// TODO: Needs documentation.
    /// </summary>
    /// <returns>TODO: Needs documentation.</returns>
    public static Task<int> Main()
    {
        return Deployment.RunAsync<PulumiTemplate>();
    }
}
