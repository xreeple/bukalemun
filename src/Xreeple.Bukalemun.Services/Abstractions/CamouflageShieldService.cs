using Xreeple.Bukalemun.Services.Options;

namespace Xreeple.Bukalemun.Services.Abstractions
{
    public interface CamouflageShieldService
    {
        string Mask(string input, CamouflageShieldOptions options);
    }
}
