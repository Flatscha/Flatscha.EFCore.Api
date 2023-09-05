using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Flatscha.EFCore.Api.Generators.Base
{
    public abstract class BaseSourceGenerator : ISourceGenerator
    {
        public abstract void Execute(GeneratorExecutionContext context);

        public abstract void Initialize(GeneratorInitializationContext context);

        protected string ReadTemplateFile(string fileName, Assembly assembly = null)
        {
            var fileContent = string.Empty;

            assembly = assembly ?? Assembly.GetExecutingAssembly();
            var path = assembly.GetManifestResourceNames()
                .Where(x => x.EndsWith($".{fileName}"))
                .FirstOrDefault();

            using (var stream = assembly.GetManifestResourceStream(path))
            using (var reader = new StreamReader(stream))
            {
                fileContent = reader.ReadToEnd();
            }

            return fileContent;
        }

        protected INamedTypeSymbol GetClassSymbol(Compilation compilation, ClassDeclarationSyntax classSyntax)
        {
            var model = compilation.GetSemanticModel(classSyntax.SyntaxTree);
            var classSymbol = model.GetDeclaredSymbol(classSyntax)!;
            return ((INamedTypeSymbol)classSymbol);
        }

        protected string GetUsings(List<string> nameSpaces)
        {
            var result = string.Empty;

            foreach (var nameSpace in nameSpaces.Distinct())
            {
                result += $"using {nameSpace};\n";
            }

            return result;
        }

        protected string HandlePropertyContext(string template, IEnumerable<ISymbol> symbols, string start, string end, Func<ISymbol, string, string> setSymbolContextValue)
            => this.HandlePropertyContext(template, symbols, start, end, (field, fieldContextValue, _) => setSymbolContextValue(field, fieldContextValue));

        protected string HandlePropertyContext(string template, IEnumerable<ISymbol> symbols, string start, string end, Func<ISymbol, string, int, string> setSymbolContextValue)
        {
            var fieldContextStart = template.IndexOf(start) + start.Length;
            var fieldContextEnd = template.LastIndexOf(end);
            var fieldContext = template.Substring(fieldContextStart, fieldContextEnd - fieldContextStart);
            fieldContext = Regex.Replace(fieldContext, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);

            var fieldContextValues = string.Empty;

            var i = 0;
            foreach (var field in symbols)
            {
                var fieldContextValue = fieldContext;

                fieldContextValue = setSymbolContextValue(field, fieldContextValue, i);

                fieldContextValues += fieldContextValue;
                i++;
            }

            template = template.Replace(fieldContext, fieldContextValues);
            template = Regex.Replace(template, @$"$\s*{start}", string.Empty, RegexOptions.Multiline);
            template = Regex.Replace(template, @$"\s*{end}", string.Empty, RegexOptions.Multiline);

            return template;
        }
    }
}
