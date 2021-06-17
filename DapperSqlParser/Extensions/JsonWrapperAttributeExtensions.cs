﻿using System;
using System.Linq;
using DapperSqlParser.Services;

namespace DapperSqlParser.Extensions
{
    public static class JsonWrapperAttributeExtensions
    {
        public static JsonWrapperAttribute GetAttributeCustom<T>() where T : class
        {
            try
            {
                return ((JsonWrapperAttribute)typeof(T).GetCustomAttributes(typeof(JsonWrapperAttribute), false).FirstOrDefault());
            }
            catch (SystemException)
            {
                return null;
            }
        }
    }
}