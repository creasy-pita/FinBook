using Microsoft.EntityFrameworkCore;
using System;
using User.API.Data;
using User.API.Model;
using Xunit;

namespace User.API.UnitTests
{
    public class UserControllerUnitTest
    {
        private AppUserDbContext _userContext;

        private AppUserDbContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<AppUserDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var userContext = new AppUserDbContext(options);
            userContext.Users .Add( new AppUser { Id = 1, Name = "creasypita" });
            userContext.SaveChanges();
            return userContext;
        }

        [Fact]
        public void Get_ReturnRightUser_WithExpectedParameters()
        {
            var context = GetUserContext();

        }
    }
}
