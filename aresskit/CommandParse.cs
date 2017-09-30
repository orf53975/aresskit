using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace aresskit
{
    class CommandParse
    {
        public static void Parse(string command, string[] args)
        {
            string[] class_method = command.Split(Convert.ToChar("::")); // Seperate Administration::isAdmin to (Class: Administration, Method: isAdmin())

            Type thisType = Type.GetType(class_method[0]);
            MethodInfo theMethod = thisType.GetMethod(class_method[1]);
            theMethod.Invoke(null, args);
        }
    }
}
