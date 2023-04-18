using System.Collections.Generic;
using TMRDataManager.Library.Models;

namespace TMRDataManager.Library.DataAccess
{
    public interface IUserData
    {
        List<UserModel> GetUserById(string Id);
    }
}