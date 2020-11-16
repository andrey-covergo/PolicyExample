using System;
using System.Linq;
using GraphQL;

namespace PolicyExample.GraphQL.Client
{
    public class GraphQlException : Exception
    {
        public GraphQLError[]? Errors { get; }

        public GraphQlException(GraphQLError[]? errors)
        {
            Errors = errors;
        }
        public GraphQlException(string message):base(message)
        {
        }

        public override string ToString()
        {
            return Errors?.FirstOrDefault()?.ToString() + base.ToString();
        }
    }
}