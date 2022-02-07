using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityAuxiliaryTools.Extensions
{
    public static class ExceptionExtensions
    {
        // TODO: cover with autotests
        public static string ToErrorString(this Exception exception)
        {
            var errorStringBuilder = new StringBuilder();
            if (exception is AggregateException)
            {
                errorStringBuilder.Append("Aggregate exception. Inner exceptions:");
                var exceptionsList = new Queue<Exception>();
                exceptionsList.Enqueue(exception);
                while (exceptionsList.Any())
                {
                    var processingException = exceptionsList.Dequeue();
                    if (processingException is AggregateException processingAggregateException)
                    {
                        foreach (var innerException in processingAggregateException.InnerExceptions)
                        {
                            exceptionsList.Enqueue(innerException);
                        }
                    }
                    else
                    {
                        errorStringBuilder.AppendLine();
                        errorStringBuilder.Append(GetNotAggregateErrorString(processingException));
                    }
                }
            }
            else
            {
                errorStringBuilder.Append(GetNotAggregateErrorString(exception));
            }

            return errorStringBuilder.ToString();
        }

        public static string GetErrorMessage(this Exception exception)
        {
            var processingException = exception;
            while (processingException is AggregateException)
            {
                processingException = processingException.InnerException;
            }

            return processingException != null ? processingException.Message : "Invalid error";
        }

        private static string GetNotAggregateErrorString(Exception exception)
        {
            var errorStringBuilder = new StringBuilder();
            errorStringBuilder.Append(exception.Message);
            errorStringBuilder.AppendLine();
            errorStringBuilder.Append(exception.StackTrace);
            return errorStringBuilder.ToString();
        }
    }
}