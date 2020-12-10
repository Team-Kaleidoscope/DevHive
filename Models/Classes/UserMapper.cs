using Models.DTOs;
using System.Reflection;

namespace Models.Classes
{
    public static class Mapper
    {
        /// <summary>
        /// Mapps UserDTO to a User
        /// </summary>
        /// <param name="userDTO">UserDTO that is going to be mapped</param>
        /// <returns>Mapped User</returns>
        public static User UserDtoToUser(UserDTO userDTO)
        {
            User user = new User();
            foreach(PropertyInfo property in user.GetType().GetProperties())
            {
                var neshto = property.GetValue(userDTO, null);
                property.SetValue(user, neshto, null);
            }

            return user;
        }
    }
}
