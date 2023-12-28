﻿using CheckedAppProject.DATA.CheckedAppDbContext;
using CheckedAppProject.DATA.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheckedAppProject.DATA.DbServices.Repository
{
    public class UserRepository : IUserRepository
    {
        private UserItemContext _userItemContext;

        public UserRepository(UserItemContext userItemContext)
        {
            _userItemContext = userItemContext;
        }

        public async Task<User?> GetUserAsync(Func<IQueryable<User>, IQueryable<User>> customQuery)
        {
            var query = _userItemContext.Users.AsQueryable();

            query = customQuery(query);

            return await query
                .Include(u => u.ItemList)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<User>> GetAllUsersDataAsync()
        {
            var users = await _userItemContext
                .Users
                .Include(e => e.ItemList)
                .ToListAsync();

            return users;
        }
        public async Task<bool> DeleteUserAsync(Func<IQueryable<User>, IQueryable<User>> customQuery)
        {
            var query = _userItemContext.Users.AsQueryable();

            query = customQuery(query);

            var userToDelete = await query.FirstOrDefaultAsync();

            if (userToDelete != null)
            {
                _userItemContext.Users.Remove(userToDelete);
                await _userItemContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> EditUserData(User userData) // do poprawki, nie zapisuje nowych danych dto nowy trzeba zrobic bez warunków
        {
            var dbUser = await _userItemContext.Users
                .FirstOrDefaultAsync(u => u.UserId == userData.UserId);

            if (dbUser != null)
            {
                dbUser.UserName = userData.UserName ?? dbUser.UserName;
                dbUser.UserSurname = userData.UserSurname ?? dbUser.UserSurname;
                dbUser.UserEmail = userData.UserEmail ?? dbUser.UserEmail;
                dbUser.Password = userData.Password ?? dbUser.Password;
                dbUser.UserAge = userData.UserAge != 0 ? userData.UserAge : dbUser.UserAge;
                dbUser.UserSex = userData.UserSex ?? dbUser.UserSex;
                dbUser.UserLogged = userData.UserLogged;

                await _userItemContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task AddUserAsync(User userData)
        {
            try
            {
                _userItemContext.Users.Add(userData);
                await _userItemContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }// dokończyć merge sprawdzenie
    }
}
