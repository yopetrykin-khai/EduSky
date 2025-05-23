using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduSky
{
    public interface IDataService<T>
    {
        List<T> Load();
        void Save(List<T> items);
    }
}
