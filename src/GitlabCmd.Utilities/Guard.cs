﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitlabCmd.Utilities
{
    internal static class Guard
    {
        public static void PathExists(string arg, string argName)
        {
            if (string.IsNullOrEmpty(arg) || !(File.Exists(arg) || Directory.Exists(arg)))
                throw new ArgumentException($"ArgumentException: Path not valid {arg}. Parameter name {argName}");
        }

        public static void NotEmpty(string arg, string argName, string message = null)
        {
            if (string.IsNullOrEmpty(arg))
                if (string.IsNullOrEmpty(message))
                    throw new ArgumentException($"ArgumentException: {argName} string not valid.");
                else
                    throw new ArgumentException($"{message}");
        }

        public static void NotEmpty<T>(IEnumerable<T> arg, string argName)
        {
            if (arg == null || !arg.Any())
                throw new ArgumentException($"ArgumentException: sequence {argName} is empty or null");
        }

        public static void NotNull<T>(T arg, string argName)
            where T : class
        {
            if (arg == null)
                throw new ArgumentException($"ArgumentException: {argName} is null");
        }

        public static void IsOfType<T>(Type type, string argName)
            where T : class
        {
            if (typeof(T).IsAssignableFrom(type))
                throw new ArgumentException(
                    $"Type in {argName} must implement/derive from {typeof(T).Name}");
        }

        public static void IsOfType<T>(IEnumerable<Type> types, string argName)
            where T : class
        {
            if (!types.Select(s => typeof(T).IsAssignableFrom(s)).All(s => s))
                throw new ArgumentException(
                    $"All types in {argName} must implement/derive from {typeof(T).Name}");
        }

        public static void IsTrue(bool condition, string message)
        {
            if (!condition)
                throw new ArgumentException($"condition not satisfied: {message}");
        }
    }
}
