using TestApi.Database.Models;
using TestApi.ViewModels;

namespace TestApi.Services.Contracts
{
    public interface IAccountService
    {
        Task<IEnumerable<PersonalAccount>> Get(int userId);
        Task<PersonalAccount?> CreateAccount(AccountModel account);
        Task<PersonalAccount?> Convert(ConvertModel convertModel);
        Task<bool> Transfer(TransferModel transferModel);
    }
}