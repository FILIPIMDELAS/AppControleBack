public class VerificationService
{
    private static readonly Dictionary<string, string> _verificationCodes = new Dictionary<string, string>();
    public string GenerateVerificationCode(int length = 6)
    {
        Random random = new Random();
        string code = "";

        for (int i = 0; i < length; i++)
        {
            code += random.Next(0, 10).ToString();
        }

        return code;
    }
}

