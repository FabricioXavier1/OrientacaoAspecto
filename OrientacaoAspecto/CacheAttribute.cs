using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Serialization;


namespace OrientacaoAspecto
{
    [PSerializable]
    public sealed class CacheAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            // Constrói o ID de cache.
            var stringBuilder = new StringBuilder();
            AppendCallInformation(args, stringBuilder);
            var cacheKey = stringBuilder.ToString();

            // Obtém o valor do cache.
            var cachedValue = MemoryCache.Default.Get(cacheKey);

            if (cachedValue != null)
            {
                // Se caso o valor já exista no cache, nem executa o método. Retorno o valor do cache imediatamente.
                args.ReturnValue = cachedValue;
                args.FlowBehavior = FlowBehavior.Return;
            }
            else
            {
                // Se o valor não está no cache, mantenha a execução do método, mas salve no cache para que possa ser utilizado no futuro.
                args.MethodExecutionTag = cacheKey;
                args.FlowBehavior = FlowBehavior.Continue;
            }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            var cacheKey = (string)args.MethodExecutionTag;
            MemoryCache.Default[cacheKey] = args.ReturnValue;
        }

        private static void AppendCallInformation(MethodExecutionArgs args, StringBuilder stringBuilder)
        {
            // Append tipo e nome do método.
            var declaringType = args.Method.DeclaringType;
            Formatter.AppendTypeName(stringBuilder, declaringType);
            stringBuilder.Append('.');
            stringBuilder.Append(args.Method.Name);

            // Append generic arguments.
            if (args.Method.IsGenericMethod)
            {
                var genericArguments = args.Method.GetGenericArguments();
                Formatter.AppendGenericArguments(stringBuilder, genericArguments);
            }

            // Append argumentos.
            Formatter.AppendArguments(stringBuilder, args.Arguments);
        }
    }
}
