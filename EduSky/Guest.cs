using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{

    public class Guest : User
    {
        public override string GetRole() => throw new NotImplementedException();

        public User Register(UserRole role)
        {
            throw new NotImplementedException();
        }
    }

}
