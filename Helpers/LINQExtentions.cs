﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class LINQExtentions
    {
        public static IEnumerable<object> GetFieldsFromObjectList(this IEnumerable<object> list, string fields)
        {
            IEnumerable<string> parsedFields = fields.Split(',').AsEnumerable();
            List<object> selectQuery = new List<object>();

            PropertyInfo[] props;
            object obj;

            foreach (var item in list)
            {
                props = list.GetType().GetProperties();
                obj = new ExpandoObject();

                foreach (var field in parsedFields)
                {
                    var fieldCapitalized = field.First().ToString().ToUpper() + field.Substring(1);
                    var propValue = (
                                from prop in props
                                where prop.Name == fieldCapitalized
                                select prop.GetValue(list, null)
                        );

                    ((IDictionary<string, object>)obj).Add(fieldCapitalized, propValue.FirstOrDefault());
                }

                selectQuery.Add(obj);
            }

            return selectQuery;

        }


    }
}