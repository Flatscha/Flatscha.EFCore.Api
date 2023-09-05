using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace Flatscha.EFCore.Api.SyntaxReceiver
{
    public class EFCoreSyntaxReceiver : ISyntaxContextReceiver
    {
        public List<ClassDeclarationSyntax> Context { get; } = new();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is ClassDeclarationSyntax cds)
            {
                if (cds.BaseList?.Types.Any(x => x.ToString() == "DbContext") ?? false)
                {
                    this.Context.Add(cds);
                }
            }
        }
    }
}
