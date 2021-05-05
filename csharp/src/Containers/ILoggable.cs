using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPAM.Deltix.Containers{
	/// <summary>
	/// Severity
	/// </summary>
	public enum Severity
	{
		/// <summary>
		/// Info message
		/// </summary>
		Info,
		/// <summary>
		/// Warning message
		/// </summary>
		Warning,
		/// <summary>
		/// Error message
		/// </summary>
		Error
	}
	/// <summary>
	/// Interface for logging
	/// </summary>
	public interface ILoggable
	{
		/// <summary>
		/// Logging(object, severity, exception, message)
		/// </summary>
		event Action<Object, Severity, Exception, IReadOnlyString> Logging;

		/// <summary>
		/// Minimal value of severity for sent messages. (Info less than Warning, Warning less than Error). Default value is Warning.
		/// </summary>
		Severity MinimalSeverityToLog
		{
			get;
			set;
		}
	}
}
