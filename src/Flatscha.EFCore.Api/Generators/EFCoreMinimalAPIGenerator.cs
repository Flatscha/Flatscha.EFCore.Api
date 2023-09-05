using Flatscha.EFCore.Api.Constants;
using Flatscha.EFCore.Api.Generators.Base;
using Flatscha.EFCore.Api.SyntaxReceiver;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System.Text;
using System.Diagnostics;

namespace Flatscha.EFCore.Api.Generators
{
    [Generator]
    public class EFCoreMinimalAPIGenerator : BaseSourceGenerator
    {
        public override void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!Debugger.IsAttached) { Debugger.Launch(); }
#endif
            context.RegisterForSyntaxNotifications(() => new EFCoreSyntaxReceiver());
        }

        public override void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxContextReceiver is EFCoreSyntaxReceiver syntaxReceiver)) { return; }

            var template = this.ReadTemplateFile(EmbeddedFileNames.EFCore);
            var templateEntity = this.ReadTemplateFile(EmbeddedFileNames.EFCoreEntity);

            foreach (var dbContext in syntaxReceiver.Context
                .Select(c => this.GetClassSymbol(context.Compilation, c)))
            {
                this.GenerateMappings(template, templateEntity, dbContext, context);
            }
        }

        private void GenerateMappings(string template, string templateEntity, INamedTypeSymbol dbContext, GeneratorExecutionContext context)
        {
            var className = $"MinimalAPI{dbContext.Name}Extensions";
            var containingNamespace = typeof(EFCoreMinimalAPIGenerator).Namespace;

            var entities = new List<ISymbol>();

            foreach (var set in dbContext.GetMembers()
                .Where(x => x.Kind == SymbolKind.Property)
                .Select(x => (IPropertySymbol)x))
            {
                this.GenerateEntityMapping(set, templateEntity, containingNamespace, ref entities, dbContext, context);
            }

            var syntax = template.Replace(FieldTemplateNames.NameSpace, containingNamespace);
            syntax = syntax.Replace(FieldTemplateNames.ClassName, className);
            syntax = syntax.Replace(FieldTemplateNames.MethodName, $"Map{dbContext.Name}");

            syntax = this.HandlePropertyContext(syntax, entities, FieldTemplateNames.MapEntityStart, FieldTemplateNames.MapEntityEnd, (symbol, fieldContext) =>
            {
                fieldContext = fieldContext.Replace(FieldTemplateNames.Entity, symbol.Name);
                return fieldContext;
            });

            context.AddSource($"{className}.g.cs", SourceText.From(syntax, Encoding.UTF8));
        }

        private void GenerateEntityMapping(IPropertySymbol set, string template, string containingNamespace, ref List<ISymbol> entities, INamedTypeSymbol dbContext, GeneratorExecutionContext context)
        {
            if (!set.IsVirtual) { return; }

            var type = (INamedTypeSymbol)set.Type;
            if (type.TypeArguments.Count() != 1) { return; }

            var entityType = type.TypeArguments.Single();

            entities.Add(entityType);

            var nameSpaces = new List<string>()
            {
                entityType.ContainingNamespace.ToDisplayString(),
                dbContext.ContainingNamespace.ToDisplayString(),
            };

            var syntaxEntity = template.Replace(FieldTemplateNames.Usings, this.GetUsings(nameSpaces));
            syntaxEntity = syntaxEntity.Replace(FieldTemplateNames.NameSpace, containingNamespace);
            syntaxEntity = syntaxEntity.Replace(FieldTemplateNames.Context, dbContext.Name);

            var className = $"Map{entityType.Name}Extensions";

            syntaxEntity = syntaxEntity.Replace(FieldTemplateNames.ClassName, className);
            syntaxEntity = syntaxEntity.Replace(FieldTemplateNames.Entity, entityType.Name);
            syntaxEntity = syntaxEntity.Replace(FieldTemplateNames.Entities, set.Name);

            syntaxEntity = this.HandlePropertyContext(syntaxEntity, entityType.GetMembers().Where(x => x.Kind == SymbolKind.Property), FieldTemplateNames.UpdateEntityStart, FieldTemplateNames.UpdateEntityEnd, (symbol, fieldContext) =>
            {
                fieldContext = fieldContext.Replace(FieldTemplateNames.PropertyName, symbol.Name);
                return fieldContext;
            });

            context.AddSource($"{className}.g.cs", SourceText.From(syntaxEntity, Encoding.UTF8));
        }
    }
}
