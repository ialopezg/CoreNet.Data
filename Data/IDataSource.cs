using System;

namespace CoreNet.Data
{
    /// <summary>
    /// Interfaz for data source objects.
    /// </summary>
    public interface IDataSource : IDisposable
    {
        /// <summary>
        /// Gets the data source type.
        /// </summary>
        DataSourceType DataSourceType { get; }

        /// <summary>
        /// Adds a <see cref="bool"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, bool value);

        /// <summary>
        /// Adds a <see cref="byte"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, byte value);

        /// <summary>
        /// Adds a <see cref="byte[]"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, byte[] value);

        /// <summary>
        /// Adds a <see cref="DateTime"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, DateTime value);

        /// <summary>
        /// Adds a <see cref="decimal"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, decimal value);

        /// <summary>
        /// Adds a <see cref="double"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, double value);

        /// <summary>
        /// Adds a <see cref="float"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, float value);

        /// <summary>
        /// Adds a <see cref="int"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, int value);

        /// <summary>
        /// Adds a <see cref="long"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, long value);

        /// <summary>
        /// Adds a <see cref="object"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, object value);

        /// <summary>
        /// Adds a <see cref="short"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, short value);

        /// <summary>
        /// Adds a <see cref="string"/> parameter type.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value</param>
        void AddParameter(string name, string value);

        /// <summary>
        /// Confirm current transaction.
        /// </summary>
        void ConfirmTransaction();

        /// <summary>
        /// Rollback current transaction.
        /// </summary>
        void UndoTransaction();
    }
}