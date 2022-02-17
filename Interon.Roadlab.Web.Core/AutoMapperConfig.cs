using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Interon.Roadlab.Web.App.Interfaces;
 

namespace Interon.Roadlab.Web.App
{
    public class AutoMapperConfig:IRunAtInit

    {
        public void Execute()
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();
            LoadStandardMappings(types);
            LoadCustomMappings(types);
        }

        private void LoadCustomMappings(Type[] types)
        {
            var maps = (from t in types
                from i in t.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICustomMappings) &&
                      !t.IsAbstract &&
                      !t.IsInterface
                select (ICustomMappings) Activator.CreateInstance(t)).ToArray();
            foreach (var map in maps)
            {
                map.CreateMappings(Mapper.Configuration);
            }
        }

        private void LoadStandardMappings(Type[] types)
        {
            var maps = (from t in types
                from i in t.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                      !t.IsAbstract &&
                      !t.IsInterface
                select new
                {
                    Source      = i.GetGenericArguments()[0],
                    Destination = t

                }).ToArray();
            foreach (var map in maps)
            {
                Mapper.CreateMap(map.Source, map.Destination);
            }

        }
    }
}