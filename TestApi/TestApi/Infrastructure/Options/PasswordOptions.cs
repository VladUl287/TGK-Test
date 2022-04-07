namespace TestApi.Infrastructure.Options
{
    public class PasswordOptions
    {
        public const string Position = "Password";

        public string HashSecret { get; set; } = string.Empty;
    }
}
