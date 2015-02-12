﻿using System;
using System.Linq;
using Typewriter.CodeModel;
using Typewriter.CodeModel.Attributes;
using Type = Typewriter.CodeModel.Type;

namespace Typewriter.Generation
{
    public class Extensions
    {
        [Property("(extension) string name", "The name of the $context (camelCased)")]
        public static string name(Item itemInfo)
        {
            if (itemInfo.Name.Length > 1)
                return itemInfo.Name.Substring(0, 1).ToLowerInvariant() + itemInfo.Name.Substring(1);
            return itemInfo.Name.ToLowerInvariant();
        }

        [Property("(extension) string Class", "The class name of the $context")]
        public static string Class(Field fieldInfo)
        {
            return Class(fieldInfo.Type);
        }

        [Property("(extension) string Default", "The default value of the $context")]
        public static string Default(Field fieldInfo)
        {
            return Default(fieldInfo.Type);
        }

        [Property("(extension) string Type", "The type name")]
        public static string Type(Type typeInfo)
        {
            return typeInfo.ToString();
        }

        [Property("(extension) string Class", "The class name of the $context")]
        public static string Class(Type typeInfo)
        {
            var type = typeInfo.ToString();
            return type.EndsWith("[]") ? type.Substring(0, type.Length - 2) : type;
        }

        [Property("(extension) bool IsDate", "Determines if the $context is a DateTime")]
        public static bool IsDate(Field fieldInfo)
        {
            var type = fieldInfo.Type.ToString();
            return type == "Date";
        }

        [Property("(extension) string Default", "The default value of the $context")]
        public static string Default(Type typeInfo)
        {
            var type = typeInfo.ToString();

            if (type.EndsWith("[]")) return "[]";
            if (type == "boolean") return "false";
            if (type == "number") return "0";
            if (type == "void") return "void(0)";

            return "null";
        }

        [Property("(extension) string Route", "Returns the value of the RouteAttribute of the $context and the RoutePrefixAttribute of the current class")]
        public static string Route(Method methodInfo)
        {
            var parent = methodInfo.Parent as Item;
            var route = methodInfo.Attributes.FirstOrDefault(a => a.Name == "Route");
            var routePrefix = parent != null ? parent.Attributes.FirstOrDefault(a => a.Name == "RoutePrefix") : null;

            if (route == null && routePrefix == null) return null;
            
            return string.Concat(routePrefix != null ? routePrefix.Value + "/" : null, route != null ? route.Value : null);
        }
    }
}