using TestDevWebApi.Data;
using TestDevWebApi.Models;
using Microsoft.EntityFrameworkCore;
namespace TestDevWebApi.Services
{
    public class AccountService
    {
        private readonly TestDevDbContext _dbContext;

        public AccountService(TestDevDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<AccountBalanceSummary>> GetAccountBalancesAsync()
        {
      
            var accounts = await _dbContext.Accounts.ToListAsync();

            // Create a dictionary to store the top-level account and its accumulated balance
            var topLevelAccountBalances = new Dictionary<string, decimal?>();

            // Iterate through each account to find the top-level account and accumulate balances
            foreach (var account in accounts)
            {
                var currentAccount = account;
                while (currentAccount.AccParent != null)
                {
                    currentAccount = accounts.FirstOrDefault(x => x.AccNumber == currentAccount.AccParent);
                }

                //Got the top level account for the current account
                var topLevelAccount = currentAccount.AccNumber;


                // Accumulate the balance for the top-level account
                if (topLevelAccountBalances.ContainsKey(topLevelAccount))
                {
                    topLevelAccountBalances[topLevelAccount] += account.Balance;
                }
                else
                {
                    topLevelAccountBalances[topLevelAccount] = account.Balance ?? 0;
                }
            }

            // Convert the dictionary into a list and return
            return topLevelAccountBalances
                .Select(x => new AccountBalanceSummary
                {
                    TopLevelAccount = x.Key,
                    TotalBalance = x.Value
                })
                .OrderBy(y => y.TopLevelAccount)
                .ToList();
        }



        public async Task<List<string>> GetAccountPathsAsync(string topAccountId)
        {
            var paths = new List<string>();
            await FindPathsRecursive(topAccountId, "",0, paths);
            return paths;
        }

 
        private async Task FindPathsRecursive(string currentAccId, string concatenatedPath, decimal? summedBalance ,List<string> paths)
        {
            var currentAccount = await _dbContext.Accounts
                .Where(a => a.AccNumber == currentAccId)
                .FirstOrDefaultAsync();

         
            // Build path and balance
            string BranchPath = string.IsNullOrEmpty(concatenatedPath) ? currentAccId : $"{concatenatedPath}-{currentAccId}";
            decimal? BranchBalance = string.IsNullOrEmpty(concatenatedPath) ? 0 : summedBalance + currentAccount?.Balance ;

            // Get Children
            var children = await _dbContext.Accounts
                .Where(a => a.AccParent == currentAccId)
                .ToListAsync();

            // If it is a leaf, Commit path
            if (!children.Any())
            {
                if (currentAccount.Balance.HasValue)
                    paths.Add($"{BranchPath} = { (int) BranchBalance }");
            }
            else
            {
                // Recurse through children
                foreach (var child in children)
                {
                    await FindPathsRecursive(child.AccNumber, BranchPath, BranchBalance, paths);
                }
            }
        }

    }
}
