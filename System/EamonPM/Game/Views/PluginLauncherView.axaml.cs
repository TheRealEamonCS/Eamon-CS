
// PluginLauncherView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using EamonPM.Game.ViewModels;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Views
{
	public partial class PluginLauncherView : UserControl
	{
		public virtual IList<string> CommandList { get; set; }

		public virtual long CommandListIndex { get; set; }

		public virtual void OutputScrollViewer_PointerWheelChanged(object sender, PointerWheelEventArgs e)
		{
			InputTextBoxLoseFocus();
		}

		public virtual void OutputScrollViewer_PointerPressed(object sender, PointerPressedEventArgs e)
		{
			InputTextBoxLoseFocus();
		}

		public virtual void OutputTextBlock_GotFocus(object sender, GotFocusEventArgs e)
		{
			if (e != null)
			{
				e.Handled = true;
			}
		}

		public virtual void OutputTextBlock_PointerPressed(object sender, PointerPressedEventArgs e)
		{
			InputTextBoxLoseFocus();
		}

		public virtual void InputTextBox_GotFocus(object sender, GotFocusEventArgs e)
		{
			if (DataContext is PluginLauncherViewModel viewModel)
			{
				if (App.ProgramName == "EamonPM.Android" && viewModel.KeepKeyboardVisible)
				{
					App.ShowKeyboardFunc?.Invoke();
				}
			}
		}

		public virtual void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			if (DataContext is PluginLauncherViewModel viewModel)
			{
				if (App.ProgramName == "EamonPM.Android" && viewModel.KeepKeyboardVisible)
				{
					App.HideKeyboardFunc?.Invoke();
				}
			}
		}

		public virtual void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (DataContext is PluginLauncherViewModel viewModel)
			{
				var inputText = viewModel.InputText ?? "";

				var origInputText = viewModel.OrigInputText ?? "";

				if (inputText.Length > origInputText.Length)
				{
					var ch = inputText[inputText.Length - 1];

					var ch01 = ch;

					if (App.InputBufSize > 0 && inputText.Length <= App.InputBufSize)
					{
						if (App.InputModifyCharFunc != null)
						{
							ch = App.InputModifyCharFunc(ch);
						}

						var validChar = true;

						if (App.InputValidCharFunc != null)
						{
							validChar = App.InputValidCharFunc(ch);
						}

						if (validChar)
						{
							if (ch != '\0')
							{
								if (ch != ch01)
								{
									SetInputTextNoEvents(string.Format("{0}{1}", origInputText, ch));
								}
							}
							else
							{
								SetInputTextNoEvents(origInputText);
							}

							var termChar = false;

							if (App.InputTermCharFunc != null)
							{
								termChar = App.InputTermCharFunc(ch);
							}

							if (termChar)
							{
								App.DispatcherUIThreadPost(() =>
								{
									InputTextBoxEntryCompleted();
								});
							}
						}
						else
						{
							SetInputTextNoEvents(origInputText);
						}
					}
					else
					{
						SetInputTextNoEvents(origInputText);
					}
				}
			}
		}

		public virtual void InputTextBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (DataContext is PluginLauncherViewModel viewModel)
			{
				if (e.Key == Key.Up && gEngine.EnableCommandHistory)
				{
					if (CommandListIndex + 1 < CommandList.Count)
					{
						InputTextBox.Text = CommandList[(int)CommandListIndex + 1];

						CommandListIndex++;

						InputTextBox.CaretIndex = InputTextBox.Text.Length;

						InputTextBox.Focus();
					}
				}
				else if (e.Key == Key.Down && gEngine.EnableCommandHistory)
				{
					if (CommandListIndex > -1)
					{
						InputTextBox.Text = CommandListIndex > 0 ? CommandList[(int)CommandListIndex - 1] : string.Empty;

						CommandListIndex--;

						InputTextBox.CaretIndex = InputTextBox.Text.Length;

						InputTextBox.Focus();
					}
				}
				else if (e.Key == Key.Escape)
				{
					InputTextBox.Text = string.Empty;

					CommandListIndex = -1;

					InputTextBox.Focus();
				}
				else if (e.Key == Key.Enter)
				{
					InputTextBoxEntryCompleted();

					if (App.ProgramName != "EamonPM.Android" || viewModel.KeepKeyboardVisible)
					{
						// Keep focus
						InputTextBox.Focus();
					}
					else
					{
						// Clear focus and hide keyboard
						InputTextBoxLoseFocus();
						App.HideKeyboardFunc?.Invoke();
					}
				}
			}
		}

		public virtual void OutputScrollViewerScrollToEnd()
		{
			App.DispatcherUIThreadPost(() =>
			{
				OutputScrollViewer.ScrollToEnd();
			});
		}

		public virtual void InputTextBoxLoseFocus()
		{
			InputTextBox.IsEnabled = false;

			InputTextBox.IsEnabled = true;
		}

		public virtual void InputTextBoxEntryCompleted()
		{
			if (DataContext is PluginLauncherViewModel viewModel)
			{
				if (viewModel.InputText.Length > 0 || App.InputEmptyAllowed)
				{
					if (!App.FinishInputSet)         // Xamarin Forms bug workaround ???
					{
						App.FinishInputSet = true;

						if (viewModel.InputText.Length == 0 && App.InputEmptyVal != null)
						{
							SetInputTextNoEvents(App.InputEmptyVal);
						}

						var inputText = Regex.Replace(viewModel.InputText, @"\s+", " ").Trim();

						if (inputText.Length > 0 && gEngine.EnableCommandHistory)
						{
							if (CommandList.Count > 0)
							{
								var index = CommandList.ToList().FindIndex(s =>	s.Equals(inputText, StringComparison.OrdinalIgnoreCase));

								if (index >= 0)
								{
									CommandList.RemoveAt(index);
								}

								CommandList.Insert(0, inputText);

								if (CommandList.Count > 100)
								{
									CommandList.RemoveAt(100);
								}
							}
							else
							{
								CommandList.Add(inputText);
							}
						}

						CommandListIndex = -1;

						App.DispatcherUIThreadPost(() =>
						{
							if (App.ProgramName == "EamonPM.Android" && !viewModel.KeepKeyboardVisible)
							{
								InputTextBoxLoseFocus();
							}

							App.FinishInput.Set();
						});
					}
				}
			}
		}

		public virtual void SetInputTextWatermark(string value, bool useFloating)
		{
			InputTextBox.Watermark = value;

			InputTextBox.UseFloatingWatermark = useFloating;
		}

		public virtual async void SetInputTextNoEvents(string value)
		{
			if (DataContext is PluginLauncherViewModel viewModel)
			{
				if (viewModel.InputText == null || !viewModel.InputText.Equals(value, StringComparison.Ordinal))
				{
					InputTextBox.TextChanged -= InputTextBox_TextChanged;

					viewModel.InputText = value;

					InputTextBox.CaretIndex = InputTextBox.Text.Length;

					await Task.Yield();

					InputTextBox.TextChanged += InputTextBox_TextChanged;
				}
			}
		}

		public PluginLauncherView()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(PluginLauncherViewModel));

			CommandList = new List<string>();

			CommandListIndex = -1;

			OutputScrollViewer.AddHandler(InputElement.PointerPressedEvent, OutputScrollViewer_PointerPressed, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

			OutputTextBlock.AddHandler(InputElement.PointerPressedEvent, OutputTextBlock_PointerPressed, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

			var viewModel = DataContext as PluginLauncherViewModel;

			Debug.Assert(viewModel != null);

			viewModel.PropertyChanged += (o, e) =>
			{
				if (e != null && e.PropertyName == "OutputText")
				{
					OutputScrollViewerScrollToEnd();
				}
			};
		}
	}
}
