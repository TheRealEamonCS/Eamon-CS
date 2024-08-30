
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

		public string _mainTitle;

		public double _windowWidth;

		public double _windowHeight;

		public bool _isBackButtonActive;

		public bool _isBackArrowActive;

		public bool _isBackArrowDarkActive;

		public virtual Stack<UserControl> ViewStack { get; set; }

		public virtual Stack<string> MainTitleStack { get; set; }

		public virtual Stack<bool> IsBackButtonActiveStack { get; set; }

		public virtual UserControl CurrentView 
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

		public virtual string MainTitle
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

		public virtual double WindowWidth
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

		public virtual double WindowHeight
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

		public virtual bool IsBackButtonActive
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

		public virtual bool IsBackArrowActive
		{
			get
			{
				return _isBackArrowActive;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _isBackArrowActive, value);
			}
		}

		public virtual bool IsBackArrowDarkActive
		{
			get
			{
				return _isBackArrowDarkActive;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _isBackArrowDarkActive, value);
			}
		}

		public virtual string ProgramName
		{
			get
			{
				return App.ProgramName;
			}
		}

		public virtual void MainTabControlSelectionChanged(int selectedIndex)
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

		public virtual void EamonPMMainWindowSizeChanged(double windowWidth, double windowHeight)
		{
			Debug.Assert(windowWidth > 0 && windowHeight > 0);

			WindowWidth = windowWidth;

			WindowHeight = windowHeight;
		}

		public virtual void NavigateTo(UserControl currentView, string mainTitle, bool isBackButtonActive)
		{
			Debug.Assert(currentView != null && !string.IsNullOrWhiteSpace(mainTitle));

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
