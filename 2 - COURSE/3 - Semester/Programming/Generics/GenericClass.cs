using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics
{
    class GenericClass<T>
    {
        T data;   
        
        public GenericClass(T value)
        {
            data = value;
        }

        public T GetData() 
        { 
            return data; 
        }
    }
}
