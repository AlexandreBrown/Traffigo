using CodeBits;

namespace TraffiGo.Modeles
{
    public static class Generator
    {
        public static string RandomPassword()
        {
            return PasswordGenerator.Generate(8);
        }
    }
}
