using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _02_RegistrationFramework
{
    static class DataRegister
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class ModelAnnotationAttribute : Attribute
        {

        }

        public static string xml_file_path { get; set; }
        static Type objecttype;
        static DataRegister()
        {
            xml_file_path = "outfile.xml";

            //setup object type from current assembly
            Assembly a = Assembly.GetExecutingAssembly();
            Type [] currenttypes = a.GetTypes();
            
            objecttype = currenttypes.Where(t => t.GetCustomAttribute<ModelAnnotationAttribute>() != null).FirstOrDefault();
            if (objecttype == null)
            {
                throw new ArgumentException("There is no suitable Model Class in this Assembly. " +
                    "Create a Model Class and place DataRegister.ModelAnnotation");
            }
        }

        public static void CollectData()
        {
            var currentitem = Activator.CreateInstance(objecttype);

            PropertyInfo[] props = objecttype.GetProperties();
            foreach (var item in props)
            {
                Console.WriteLine($"Enter <{item.Name.ToUpper()}>");
                string data = Console.ReadLine();
                Type proptype = item.PropertyType;
                if (proptype != typeof(string))
                {
                    MethodInfo parsemethod = proptype.GetMethods().Where(t => t.Name.Contains("Parse")).FirstOrDefault();
                    var converted_data = parsemethod.Invoke(proptype, new object[] { data });
                    item.SetValue(currentitem, converted_data);
                }
                else
                {
                    item.SetValue(currentitem, data);
                }
            }

            WriteOutToXML(ObjectToXElement(currentitem));

        }

        static XElement ObjectToXElement(object o)
        {
            XElement mainelement = new XElement(objecttype.Name);

            PropertyInfo[] props = o.GetType().GetProperties();
            foreach (var item in props)
            {
                XElement childelement = new XElement(item.Name, item.GetValue(o).ToString());
                mainelement.Add(childelement);
            }

            return mainelement;
        }

        static void WriteOutToXML(XElement newelement)
        {
            //xml not exist? Create!
            if (!File.Exists(xml_file_path))
            {
                XDocument xdoc = new XDocument();
                XElement rootelement = new XElement(objecttype.Name + "s");
                xdoc.Add(rootelement);
                rootelement.Add(newelement);
                xdoc.Save(xml_file_path);
            }
            else
            {
                XDocument xdoc = null;
                try
                {
                    xdoc = XDocument.Load(xml_file_path);
                }
                catch (Exception)
                {
                    //not valid xml :( 
                    File.Delete(xml_file_path);
                    xdoc = new XDocument();
                    XElement rootelement = new XElement(objecttype.Name + "s");
                    xdoc.Add(rootelement);
                }
                finally
                {
                    xdoc.Element(objecttype.Name + "s").Add(newelement);
                    xdoc.Save(xml_file_path);
                }
            }
        }




    }
}
