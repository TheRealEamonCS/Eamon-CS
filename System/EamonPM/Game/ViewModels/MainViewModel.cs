
// MainViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using ReactiveUI;
using EamonPM.Game.Views;

namespace EamonPM.Game.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		public UserControl _currentView;

		public bool _isBackButtonActive;

		public string _mainTitle;

		public double _windowWidth;

		public double _windowHeight;

		public Stack<UserControl> ViewStack { get; set; }

		public Stack<string> MainTitleStack { get; set; }

		public Stack<bool> IsBackButtonActiveStack { get; set; }

		public UserControl CurrentView 
		{ 
			get
			{
				return _currentView;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _currentView, value);
			}
		}

		public string MainTitle
		{
			get
			{
				return _mainTitle;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _mainTitle, value);
			}
		}

		public double WindowWidth
		{
			get
			{
				return _windowWidth;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _windowWidth, value);
			}
		}

		public double WindowHeight
		{
			get
			{
				return _windowHeight;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _windowHeight, value);
			}
		}

		public bool IsBackButtonActive
		{
			get
			{
				return _isBackButtonActive;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _isBackButtonActive, value);
			}
		}

		public string ProgramName
		{
			get
			{
				return App.ProgramName;
			}
		}

		public void MainTabControlSelectionChanged(int selectedIndex)
		{
			switch (selectedIndex)
			{
				case 0:

					MainTitle = MainTitleStack.Peek();

					IsBackButtonActive = IsBackButtonActiveStack.Peek();

					break;

				case 1:

					MainTitle = "Settings";

					IsBackButtonActive = false;

					break;

				case 2:

					MainTitle = "About";

					IsBackButtonActive = false;

					break;

				default:

					Debug.Assert(1 == 0);

					break;
			}
		}

		public void EamonPMMainWindowSizeChanged(double windowWidth, double windowHeight)
		{
			WindowWidth = windowWidth;

			WindowHeight = windowHeight;
		}

		public void NavigateTo(UserControl currentView, string mainTitle, bool isBackButtonActive)
		{
			ViewStack.Push(currentView);

			MainTitleStack.Push(mainTitle);

			IsBackButtonActiveStack.Push(isBackButtonActive);

			CurrentView = currentView;

			MainTitle = mainTitle;

			IsBackButtonActive = isBackButtonActive;

			if (currentView is PluginLauncherView)
			{
				App.StartGameThread();
			}
		}

		public MainViewModel()
		{
			ViewStack = new Stack<UserControl>();

			MainTitleStack = new Stack<string>();

			IsBackButtonActiveStack = new Stack<bool>();
		}
	}
}
