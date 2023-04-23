using System.Collections.Generic;
using TMRDataManager.Library.Models;

namespace TMRDataManager.Library.DataAccess
{
    public interface IUserData
    {
        void CreateUser(UserModel user);
        List<UserModel> GetUserById(string Id);
    }
}