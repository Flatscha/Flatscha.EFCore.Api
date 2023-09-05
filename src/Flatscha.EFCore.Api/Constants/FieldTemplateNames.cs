namespace Flatscha.EFCore.Api.Constants
{
    public class FieldTemplateNames
    {
        public const string Usings = "##USINGS##";
        public const string NameSpace = "##NAMESPACE##";
        public const string ClassName = "##CLASS_NAME##";
        public const string MethodName = "##METHOD_NAME##";
        public const string PropertyName = "##PROPERTY_NAME##";
        public const string FieldName = "##FIELD_NAME##";
        public const string FieldType = "##FIELD_TYPE##";

        public const string FieldContextStart = "##FIELD_CONTEXT_START##";
        public const string FieldContextEnd = "##FIELD_CONTEXT_END##";

        public const string Context = "##CONTEXT##";
        public const string Entity = "##ENTITY##";
        public const string Entities = "##ENTITIES##";

        public const string UpdateEntityStart = "##UPDATE_ENTITY_START##";
        public const string UpdateEntityEnd = "##UPDATE_ENTITY_END##";

        public const string MapEntityStart = "##MAP_ENTITY_START##";
        public const string MapEntityEnd = "##MAP_ENTITY_END##";
    }
}
