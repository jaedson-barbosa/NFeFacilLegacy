using System.Collections.Generic;
using System.Linq;

namespace BaseGeral.Sincronizacao.FastServer
{
    struct ParsedUri
    {
        public IReadOnlyList<PathPart> PathParts { get; }

        public ParsedUri(IReadOnlyList<PathPart> pathParts)
        {
            PathParts = pathParts;
        }

        public override string ToString()
        {
            return $"Path={string.Join("/", PathParts.Select(x => x.PartType == PathPart.PathPartType.Argument ? $"{{{x.Value}}}" : x.Value))}";
        }
    }
}