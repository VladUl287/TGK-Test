using TestApi.Database.Models;
using TestApi.Dtos;

namespace TestApi.Services.Contracts
{
    public interface IAccountService
    {
        Task<IEnumerable<PersonalAccount>> Get(int userId);

        Task<PersonalAccount?> Create(AccountModel account);

        Task<PersonalAccount?> TopUp(TopUpModel topUpModel);

        Task<PersonalAccount?> Convert(ConvertModel convertModel);

        Task<bool> Transfer(TransferModel transferModel);
    }
}