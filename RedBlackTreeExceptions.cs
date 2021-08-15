using System;
using System.Runtime.Serialization;

[Serializable]
public class DuplicateValueException : Exception
{
    // Constructors
    public DuplicateValueException(string message)
        : base(message)
    { }
}