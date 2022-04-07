namespace TestApi.Dtos
{
    public static class Errors
    {
        public static readonly Error NotCorrectEmailOrPassword = new("Неверный email или пароль.");
        public static readonly Error UserAlreadyExists = new("Пользователь с таким email уже существует.");

        public static readonly Error FaildAccountCreate = new("Ошибка создания лицевого счёта.");
        public static readonly Error FaildAccountTopUp = new("Ошибка пополнения счёта.");
        public static readonly Error FaildTransfer = new("Ошибка перевода средств");
        public static readonly Error FaildAccountConvert = new("Ошибка конвертации счёта");
    }
}