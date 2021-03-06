﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;

namespace DapperContext.Test
{
    [TestFixture]
    public class CodeGenerationFixture
    {
        [Test]
        public void GenerateMethodsTest()
        {
            // This test case is here just to simplify writing code for the T4 template.

            Console.WriteLine(GenerateMethods());
        }

        private string GenerateMethods()
        {
            var cb = new CodeBuilder();
            cb.Indent();
            cb.Indent();

            bool hadOne = false;

            foreach (var entry in GetMethods())
            {
                var method = entry.Item1;
                int? transactionIndex = entry.Item2;

                if (hadOne)
                    cb.AppendLine();
                else
                    hadOne = true;

                var skipParameters = new List<int> { 0 };
                if (transactionIndex.HasValue)
                    skipParameters.Add(transactionIndex.Value);

                cb.Append("public static ");
                GetMethodDeclaration(cb, method, skipParameters);
                cb.AppendLine();
                cb.AppendLine("{");

                cb.Indent();

                cb
                    .Append("return context.Connection.")
                    .Append(method.Name);

                var genericArguments = method.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    cb.Append('<');
                    for (int i = 0; i < genericArguments.Length; i++)
                    {
                        if (i > 0)
                            cb.Append(", ");
                        cb.Append(genericArguments[i].Name);
                    }
                    cb.Append('>');
                }

                cb.Append('(');

                for (var i = 1; i < method.GetParameters().Length; i++)
                {
                    if (i > 1)
                        cb.Append(", ");

                    if (i == transactionIndex)
                        cb.Append("context.Transaction");
                    else
                    {
                        var parameter = method.GetParameters()[i];

                        if (parameter.ParameterType == typeof(CommandDefinition))
                        {
                            cb
                                .Append("CreateCommandDefinition(context, ")
                                .Append(parameter.Name)
                                .Append(')');
                        }
                        else
                        {
                            cb.Append(parameter.Name);
                        }
                    }
                }

                cb.AppendLine(");");

                cb.Unindent();

                cb.AppendLine("}");
            }

            return cb.ToString();
        }

        private void GetMethodDeclaration(CodeBuilder cb, MethodInfo method, IList<int> skipParameters)
        {
            GetTypeName(cb, method.ReturnType, method.ReturnTypeCustomAttributes);

            cb
                .Append(' ')
                .Append(method.Name);

            var genericArguments = method.GetGenericArguments();
            if (genericArguments.Length > 0)
            {
                cb.Append('<');
                for (int i = 0; i < genericArguments.Length; i++)
                {
                    if (i > 0)
                        cb.Append(", ");
                    cb.Append(genericArguments[i].Name);
                }
                cb.Append('>');
            }

            cb.Append("(this IDbContext context");

            for (var i = 0; i < method.GetParameters().Length; i++)
            {
                if (skipParameters.Contains(i))
                    continue;

                cb.Append(", ");

                var parameter = method.GetParameters()[i];

                GetTypeName(cb, parameter.ParameterType, parameter);

                cb
                    .Append(' ')
                    .Append(parameter.Name);

                if (parameter.IsOptional)
                {
                    cb.Append(" = ");
                    GetValueName(cb, parameter.DefaultValue);
                }
            }

            cb.Append(')');
        }

        private void GetValueName(CodeBuilder cb, object value)
        {
            if (value == null)
                cb.Append("null");
            else if (value.Equals(false))
                cb.Append("false");
            else if (value.Equals(true))
                cb.Append("true");
            else if (value is string)
                cb.Append('"').Append((string)value).Append('"');
            else
                throw new NotImplementedException(String.Format("Cannot serialize value '{0}'", value));
        }

        private void GetTypeName(CodeBuilder cb, Type type, ICustomAttributeProvider customAttributeProvider)
        {
            IList<bool> transformFlags = null;
            if (customAttributeProvider != null)
            {
                foreach (DynamicAttribute attribute in customAttributeProvider.GetCustomAttributes(typeof(DynamicAttribute), true))
                {
                    transformFlags = attribute.TransformFlags;
                }
            }

            int typeIndex = 0;
            GetTypeName(cb, type, transformFlags, ref typeIndex);
        }

        private void GetTypeName(CodeBuilder cb, Type type, IList<bool> transformFlags, ref int typeIndex)
        {
            if (type.IsNested && !type.IsGenericParameter)
            {
                int nestedTypeIndex = 0;
                GetTypeName(cb, type.DeclaringType, null, ref nestedTypeIndex);
                cb.Append('.');
            }

            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                GetTypeName(cb, underlyingType, transformFlags, ref typeIndex);
                cb.Append('?');
                return;
            }

            if (transformFlags != null && transformFlags[typeIndex])
            {
                cb.Append("dynamic");
                return;
            }

            string primitiveType = GetPrimitiveType(type);
            if (primitiveType != null)
            {
                cb.Append(primitiveType);
            }
            else if (type.IsGenericParameter)
            {
                cb.Append(type.Name);
            }
            else if (type.IsGenericType)
            {
                string genericTypeName = type.GetGenericTypeDefinition().Name;
                int index = genericTypeName.IndexOf('`');
                genericTypeName = genericTypeName.Substring(0, index);

                cb
                    .Append(genericTypeName)
                    .Append('<');

                for (int i = 0; i < type.GenericTypeArguments.Length; i++)
                {
                    if (i > 0)
                        cb.Append(", ");
                    typeIndex++;
                    GetTypeName(cb, type.GenericTypeArguments[i], transformFlags, ref typeIndex);
                }
                cb.Append('>');
            }
            else
            {
                cb.Append(type.Name);
            }
        }

        private string GetPrimitiveType(Type type)
        {
            if (type == typeof(bool))
                return "bool";
            if (type == typeof(byte))
                return "byte";
            if (type == typeof(sbyte))
                return "sbyte";
            if (type == typeof(char))
                return "char";
            if (type == typeof(decimal))
                return "decimal";
            if (type == typeof(double))
                return "double";
            if (type == typeof(float))
                return "float";
            if (type == typeof(int))
                return "int";
            if (type == typeof(uint))
                return "uint";
            if (type == typeof(long))
                return "long";
            if (type == typeof(ulong))
                return "ulong";
            if (type == typeof(object))
                return "object";
            if (type == typeof(short))
                return "short";
            if (type == typeof(ushort))
                return "ushort";
            if (type == typeof(string))
                return "string";
            return null;
        }

        private List<Tuple<MethodInfo, int?>> GetMethods()
        {
            var methods = new List<Tuple<MethodInfo, int?>>();

            foreach (var type in typeof(CommandDefinition).Assembly.GetTypes())
            {
                if (type.GetCustomAttribute<ExtensionAttribute>() == null)
                    continue;

                foreach (var method in type.GetMethods())
                {
                    if (method.GetCustomAttribute<ExtensionAttribute>() == null)
                        continue;

                    var parameters = method.GetParameters();
                    if (parameters[0].ParameterType != typeof(IDbConnection))
                        continue;

                    int? transactionIndex = null;
                    for (int i = 1; i < parameters.Length; i++)
                    {
                        if (parameters[i].ParameterType == typeof(IDbTransaction))
                            transactionIndex = i;
                    }

                    methods.Add(Tuple.Create(method, transactionIndex));
                }
            }

            return methods;
        }

        private class CodeBuilder
        {
            private const string IndentText = "    ";

            private readonly StringBuilder _sb = new StringBuilder();
            private int _indent;
            private bool _hadIndent;

            public void Indent()
            {
                _indent++;
            }

            public void Unindent()
            {
                _indent--;
            }

            private void WriteIndent()
            {
                if (_hadIndent)
                    return;

                for (int i = 0; i < _indent; i++)
                {
                    _sb.Append(IndentText);
                }

                _hadIndent = true;
            }

            public CodeBuilder Append(string text)
            {
                WriteIndent();
                _sb.Append(text);

                return this;
            }

            public CodeBuilder Append(char text)
            {
                WriteIndent();
                _sb.Append(text);

                return this;
            }

            public CodeBuilder AppendLine()
            {
                return AppendLine(null);
            }

            public CodeBuilder AppendLine(string text)
            {
                if (!String.IsNullOrEmpty(text))
                    WriteIndent();

                _sb.AppendLine(text);
                _hadIndent = false;

                return this;
            }

            public override string ToString()
            {
                return _sb.ToString();
            }
        }
    }
}
