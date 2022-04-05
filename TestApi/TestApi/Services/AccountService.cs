using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestApi.Database;
using TestApi.Database.Models;
using TestApi.Services.Contracts;
using TestApi.ViewModels;

namespace TestApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly DatabaseContext dbContext;

        public AccountService(DatabaseContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PersonalAccount>> Get(int userId)
        {
            return await dbContext.PersonalAccounts
                .Include(e => e.Currency)
                .Where(e => e.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PersonalAccount?> CreateAccount(AccountModel account)
        {
            var count = await dbContext.PersonalAccounts
                .CountAsync(e => e.UserId == account.UserId);

            if (count >= 5)
            {
                return null;
            }

            var personalAcc = mapper.Map<PersonalAccount>(account);

            try
            {
                await dbContext.PersonalAccounts.AddAsync(personalAcc);
                await dbContext.SaveChangesAsync();

                return personalAcc;
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        public async Task<PersonalAccount?> TopUp(TopUpModel topUpModel)
        {
            var account = await dbContext.PersonalAccounts
                .FirstOrDefaultAsync(e => e.Number == topUpModel.AccountNumber && e.UserId == topUpModel.UserId);

            if (account is null)
            {
                return null;
            }

            account.Value += topUpModel.Value;
            await dbContext.SaveChangesAsync();

            return account;
        }

        public async Task<bool> Transfer(TransferModel transferModel)
        {
            var account = await dbContext.PersonalAccounts
                .FirstOrDefaultAsync(e => e.Number == transferModel.FromAccountNumber && e.UserId == transferModel.UserId);

            if (account is not null && account.Value >= transferModel.Value)
            {
                var toAccount = await dbContext.PersonalAccounts.FirstOrDefaultAsync(e => e.Number == transferModel.ToAccountNumber);

                if (toAccount is not null)
                {
                    using var transaction = await dbContext.Database.BeginTransactionAsync();

                    try
                    {
                        var value = transferModel.Value;
                        account.Value -= value;

                        if (account.CurrencyId != toAccount.CurrencyId)
                        {
                            var rate = await dbContext.ExchangeRates
                                .Where(e => e.CurrencyId == account.CurrencyId && e.SecondCurrencyId == toAccount.CurrencyId)
                                .Select(e => e.Rate)
                                .FirstOrDefaultAsync();

                            if (rate != default)
                            {
                                value *= rate;
                            }
                        }

                        toAccount.Value += value;
                        await dbContext.SaveChangesAsync();

                        var report = new Report
                        {
                            UserId = account.UserId,
                            ToUserId = toAccount.UserId,
                            PersonalAccountId = account.Number,
                            ToPersonalAccountId = toAccount.Number,
                            Value = transferModel.Value,
                            CurrencyId = account.CurrencyId,
                            DateTransfer = DateTime.UtcNow
                        };

                        await dbContext.Reports.AddAsync(report);
                        await dbContext.SaveChangesAsync();

                        await dbContext.Database.CommitTransactionAsync();

                        return true;
                    }
                    catch (DbUpdateException)
                    {
                        await dbContext.Database.RollbackTransactionAsync();
                    }
                }
            }

            return false;
        }

        public async Task<PersonalAccount?> Convert(ConvertModel convertModel)
        {
            var account = await dbContext.PersonalAccounts
                .FirstOrDefaultAsync(e => e.Number == convertModel.AccountNumber && e.UserId == convertModel.UserId);

            if (account is null || account.CurrencyId == convertModel.CurrencyId)
            {
                return null;
            }

            var rate = await dbContext.ExchangeRates
                .Where(e => e.CurrencyId == account.CurrencyId && e.SecondCurrencyId == convertModel.CurrencyId)
                .Select(e => e.Rate)
                .FirstOrDefaultAsync();

            if (rate != default)
            {
                account.Value *= rate;
                account.CurrencyId = convertModel.CurrencyId;

                await dbContext.SaveChangesAsync();
            }

            return account;
        }
    }
}