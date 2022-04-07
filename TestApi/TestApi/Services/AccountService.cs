using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestApi.Database;
using TestApi.Database.Models;
using TestApi.Dtos;
using TestApi.Services.Contracts;

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

        public async Task<PersonalAccount?> Create(AccountModel account)
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
                personalAcc.DateCreate = DateTime.UtcNow;
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

            if (account.CurrencyId != topUpModel.CurrencyId)
            {
                var rate = await dbContext.ExchangeRates
                    .Where(e => e.CurrencyId == account.CurrencyId && e.SecondCurrencyId == topUpModel.CurrencyId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (rate is null)
                {
                    return null;
                }

                topUpModel.Value = rate.Exchange(topUpModel.Value);
            }

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                account.Value += topUpModel.Value;
                await dbContext.SaveChangesAsync();

                var report = new Report
                {
                    UserId = account.UserId,
                    PersonalAccountId = account.Number,
                    TransferValue = topUpModel.Value,
                    AccountValue = account.Value,
                    Credited = true,
                    AccountCurrencyId = account.CurrencyId,
                    CurrencyId = topUpModel.CurrencyId,
                    DateTransfer = DateTime.UtcNow
                };

                await dbContext.Reports.AddAsync(report);
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return account;
            }
            catch
            {
                await transaction.RollbackAsync();
                return null;
            }
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
                    if (account.Number == toAccount.Number)
                    {
                        return false;
                    }

                    using var transaction = await dbContext.Database.BeginTransactionAsync();
                    try
                    {
                        var value = transferModel.Value;
                        account.Value -= value;

                        if (account.CurrencyId != toAccount.CurrencyId)
                        {
                            var rate = await dbContext.ExchangeRates
                                .Where(e => e.CurrencyId == account.CurrencyId && e.SecondCurrencyId == toAccount.CurrencyId)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

                            if (rate is null)
                            {
                                throw new DbUpdateException();
                            }

                            value = rate.Exchange(value);
                        }

                        toAccount.Value += value;
                        await dbContext.SaveChangesAsync();

                        var fromUserReport = new Report
                        {
                            UserId = account.UserId,
                            PersonalAccountId = account.Number,
                            TransferValue = transferModel.Value,
                            AccountValue = account.Value,
                            AccountCurrencyId = account.CurrencyId,
                            CurrencyId = account.CurrencyId,
                            DateTransfer = DateTime.UtcNow
                        };
                        var toUserReport = new Report
                        {
                            UserId = toAccount.UserId,
                            PersonalAccountId = toAccount.Number,
                            TransferValue = transferModel.Value,
                            AccountValue = toAccount.Value,
                            Credited = true,
                            AccountCurrencyId = toAccount.CurrencyId,
                            CurrencyId = account.CurrencyId,
                            DateTransfer = DateTime.UtcNow
                        };

                        await dbContext.Reports.AddRangeAsync(fromUserReport, toUserReport);
                        await dbContext.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return true;
                    }
                    catch (DbUpdateException)
                    {
                        await transaction.RollbackAsync();
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
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (rate is not null)
            {
                account.Value = rate.Exchange(account.Value);
                account.CurrencyId = convertModel.CurrencyId;

                await dbContext.SaveChangesAsync();
            }

            return account;
        }
    }
}