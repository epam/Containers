/*
  Copyright 2021 EPAM Systems, Inc

  See the NOTICE file distributed with this work for additional information
  regarding copyright ownership. Licensed under the Apache License,
  Version 2.0 (the "License"); you may not use this file except in compliance
  with the License.  You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

  Unless required by applicable law or agreed to in writing, software
  distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
  WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the
  License for the specific language governing permissions and limitations under
  the License.
 */
﻿using System;
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