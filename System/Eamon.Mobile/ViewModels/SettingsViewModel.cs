
// SettingsViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Polenter.Serialization;

namespace Eamon.Mobile.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		public string _foregroundColorName;

		public string _backgroundColorName;

		public string _fontFamily;

		public long _fontSize;

		public long _outputBufMaxSize;

		public bool _keepKeyboardVisible;

		[ExcludeFromSerialization]
		public List<string> ColorNames { get; set; }

		[ExcludeFromSerialization]
		public List<string> FontFamilies { get; set; }

		[ExcludeFromSerialization]
		public List<long> FontSizes { get; set; }

		[ExcludeFromSerialization]
		public List<long> OutputBufMaxSizes { get; set; }

		public event EventHandler SettingsChanged;

		public virtual string ForegroundColorName
		{
			get
			{
				return _foregroundColorName;
			}
			set
			{
				SetProperty(ref _foregroundColorName, value);

				OnSettingsChanged(new EventArgs());
			}
		}

		public virtual string BackgroundColorName
		{
			get
			{
				return _backgroundColorName;
			}
			set
			{
				SetProperty(ref _backgroundColorName, value);

				OnSettingsChanged(new EventArgs());
			}
		}

		public virtual string FontFamily
		{
			get
			{
				return _fontFamily;
			}
			set
			{
				SetProperty(ref _fontFamily, value);

				OnSettingsChanged(new EventArgs());
			}
		}

		public virtual long FontSize
		{
			get
			{
				return _fontSize;
			}
			set
			{
				SetProperty(ref _fontSize, value);

				OnSettingsChanged(new EventArgs());
			}
		}

		public virtual long OutputBufMaxSize
		{
			get
			{
				return _outputBufMaxSize;
			}
			set
			{
				SetProperty(ref _outputBufMaxSize, value);
			}
		}

		public virtual bool KeepKeyboardVisible
		{
			get
			{
				return _keepKeyboardVisible;
			}
			set
			{
				SetProperty(ref _keepKeyboardVisible, value);
			}
		}

		/// <summary></summary>
		/// <param name="args"></param>
		public virtual void OnSettingsChanged(EventArgs args)
		{
			var handler = SettingsChanged;

			if (handler != null)
			{
				handler(this, args);
			}
		}

		public SettingsViewModel()
		{
			Title = "Settings";

			ColorNames = typeof(Color).GetRuntimeFields().Where(f => f.IsPublic && f.IsStatic).Select(f => f.Name).ToList();

			FontFamilies = new List<string>()		// Hardcoded to Android for right now; might need to obtain this using a callback into platform-specific code
			{
				"monospace"
			};

			FontSizes = new List<long>()
			{
				6, 7, 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72
			};

			OutputBufMaxSizes = new List<long>()
			{
				8192, 16384, 32768, 65536, 131072, 262144
			};

			ForegroundColorName = "Black";

			BackgroundColorName = "White";

			FontFamily = "monospace";     // Hardcoded to Android for right now; might need to obtain this using a callback into platform-specific code

			FontSize = 12;

			OutputBufMaxSize = 32768;

			KeepKeyboardVisible = true;

			App.SettingsViewModel = this;
		}
	}
}