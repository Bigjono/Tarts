using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Collections.Generic
{
    public static class ListCSVExtension
    {
        
        public static string ToCSV<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0) return null;
            var txt = new StringBuilder();
            //get type from 0th member

             
                //make a new instance of the class name we figured out to get its props
                object o = Activator.CreateInstance<T>();
                //gets all properties
                PropertyInfo[] props = o.GetType().GetProperties();

                //foreach of the properties in class above, write out properties
                //this is the header row
                var headerLine = props.Aggregate("", (current, pi) => current + "\"" + pi.Name.ToLower() + "\",");

                txt.AppendLine(RemoveTrainlingComma(headerLine));

                //this acts as datarow
                foreach (var item in list)
                {
                    //this acts as datacolumn
                    var csvLine = props.Select(pi => GetPropertyValueToString(item, pi)).Aggregate("", (current, whatToWrite) => current + whatToWrite);

                    txt.AppendLine(RemoveTrainlingComma(csvLine));

                }
             

            return txt.ToString();
        }

        public static string ToCSV<T>(this IEnumerable<T> list)
        {
            if (list == null || list.IsEmpty()) return null;
            var txt = new StringBuilder();
            //get type from 0th member


            //make a new instance of the class name we figured out to get its props
            object o = Activator.CreateInstance<T>();
            //gets all properties
            PropertyInfo[] props = o.GetType().GetProperties();

            //foreach of the properties in class above, write out properties
            //this is the header row
            var headerLine = props.Aggregate("", (current, pi) => current + "\"" + pi.Name.ToLower() + "\",");

            txt.AppendLine(RemoveTrainlingComma(headerLine));

            //this acts as datarow
            foreach (var item in list)
            {
                //this acts as datacolumn
                var csvLine = props.Select(pi => GetPropertyValueToString(item, pi)).Aggregate("", (current, whatToWrite) => current + whatToWrite);

                txt.AppendLine(RemoveTrainlingComma(csvLine));

            }


            return txt.ToString();
        }

        public static bool ToCSVFile<T>(this IList<T> list,string csvFilename)
        {
            if (list == null || list.Count == 0) return false;

            //get type from 0th member

            using (var sw = new StreamWriter(csvFilename))
            {
                //make a new instance of the class name we figured out to get its props
                object o = Activator.CreateInstance<T>();
                //gets all properties
                PropertyInfo[] props = o.GetType().GetProperties();

                //foreach of the properties in class above, write out properties
                //this is the header row
                var headerLine = props.Aggregate("", (current, pi) => current + "\"" + pi.Name.ToLower() + "\",");
                
                sw.WriteLine(RemoveTrainlingComma(headerLine));
                
                //this acts as datarow
                foreach (var item in list)
                {
                    //this acts as datacolumn
                    var csvLine =props.Select(pi => GetPropertyValueToString(item, pi)).Aggregate("", (current, whatToWrite) => current + whatToWrite);
                    
                    
                    
                    sw.WriteLine(RemoveTrainlingComma(csvLine));
                     
                }
            }

            return true;
        }


        #region helpers
        private static string GetPropertyValueToString<T>(T item, PropertyInfo pi)
        {
            return "\"" + Convert.ToString(GetValue(pi, item)) + "\"" + ',';
        }

        private static string RemoveTrainlingComma(string value)
        {
            if (value.EndsWith(",")) value = value.Remove(value.Length-1, 1);
            return value;
        }

        private static object GetValue<T>(PropertyInfo pi, T item)
        {
            return item.GetType().GetProperty(pi.Name).GetValue(item, null);
        }

        #endregion


    }
}
