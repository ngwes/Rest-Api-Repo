using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiRepo.PresentationServices
{
    public class UriBuilderFactory : IUriBuilderFactory
    {
        private readonly IReadOnlyDictionary<Type, IUriBuilderService> _uriBuilders;

        public UriBuilderFactory()
        {
            var uriBuilerProvider = typeof(IUriBuilderService);
            _uriBuilders = uriBuilerProvider.Assembly.ExportedTypes
                .Where(x => uriBuilerProvider.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                //if you have some of the UriBuilder with parameterless constructor,
                //uncomment the following and comment the remaining part
                //.Select(x=> {
                //var p = new object[] { };
                //var parameterLessConstructor = x.GetConstructors().SingleOrDefault(c => c.GetParameters().Length == 0);
                //return parameterLessConstructor is null ? Activator.CreateInstance(x) : Activator.CreateInstance(x, p);
                //})
                //.Cast<IUriBuilderService>()
                //.ToDictionary(x => nameof(x), x => x);
                .Select(Activator.CreateInstance)
                .Cast<IUriBuilderService>()
                .ToImmutableDictionary(x=> x.ResourceType, x=>x);
        }

        public IUriBuilderService CreateBuilder<TResource>()
        {
            if (_uriBuilders.TryGetValue(typeof(TResource), out var builder))
                return builder;
            else
                throw new ArgumentOutOfRangeException();
        }
    }
}
