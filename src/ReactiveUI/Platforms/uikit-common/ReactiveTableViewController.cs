﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Foundation;
using NSTableViewController = UIKit.UITableViewController;
using NSTableViewStyle = UIKit.UITableViewStyle;

namespace ReactiveUI
{
    /// <summary>
    /// This is a NSTableViewController that is both an NSTableViewController and has ReactiveObject powers
    /// (i.e. you can call RaiseAndSetIfChanged).
    /// </summary>
    public abstract class ReactiveTableViewController : NSTableViewController,
    IReactiveNotifyPropertyChanged<ReactiveTableViewController>, IHandleObservableErrors, IReactiveObject, ICanActivate
    {
        private Subject<Unit> _activated = new Subject<Unit>();
        private Subject<Unit> _deactivated = new Subject<Unit>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController"/> class.
        /// </summary>
        /// <param name="withStyle">The table view style.</param>
        protected ReactiveTableViewController(NSTableViewStyle withStyle)
            : base(withStyle)
        {
            SetupRxObj();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController"/> class.
        /// </summary>
        /// <param name="nibName">The name.</param>
        /// <param name="bundle">The bundle.</param>
        protected ReactiveTableViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            SetupRxObj();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController"/> class.
        /// </summary>
        /// <param name="handle">The pointer.</param>
        protected ReactiveTableViewController(IntPtr handle)
            : base(handle)
        {
            SetupRxObj();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController"/> class.
        /// </summary>
        /// <param name="t">The object flag.</param>
        protected ReactiveTableViewController(NSObjectFlag t)
            : base(t)
        {
            SetupRxObj();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController"/> class.
        /// </summary>
        /// <param name="coder">The coder.</param>
        protected ReactiveTableViewController(NSCoder coder)
            : base(coder)
        {
            SetupRxObj();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController"/> class.
        /// </summary>
        protected ReactiveTableViewController()
        {
            SetupRxObj();
        }

        /// <inheritdoc/>
        public event PropertyChangingEventHandler PropertyChanging
        {
            add => PropertyChangingEventManager.AddHandler(this, value);
            remove => PropertyChangingEventManager.RemoveHandler(this, value);
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add => PropertyChangedEventManager.AddHandler(this, value);
            remove => PropertyChangedEventManager.RemoveHandler(this, value);
        }

        /// <summary>
        /// Represents an Observable that fires *before* a property is about to
        /// be changed.
        /// </summary>
        public IObservable<IReactivePropertyChangedEventArgs<ReactiveTableViewController>> Changing => this.GetChangingObservable();

        /// <summary>
        /// Represents an Observable that fires *after* a property has changed.
        /// </summary>
        public IObservable<IReactivePropertyChangedEventArgs<ReactiveTableViewController>> Changed => this.GetChangedObservable();

        /// <inheritdoc/>
        public IObservable<Exception> ThrownExceptions => this.GetThrownExceptionsObservable();

        /// <inheritdoc/>
        public IObservable<Unit> Activated => _activated.AsObservable();

        /// <inheritdoc/>
        public IObservable<Unit> Deactivated => _deactivated.AsObservable();

        /// <summary>
        /// When this method is called, an object will not fire change
        /// notifications (neither traditional nor Observable notifications)
        /// until the return value is disposed.
        /// </summary>
        /// <returns>An object that, when disposed, reenables change
        /// notifications.</returns>
        public IDisposable SuppressChangeNotifications()
        {
            return IReactiveObjectExtensions.SuppressChangeNotifications(this);
        }

        /// <inheritdoc/>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _activated.OnNext(Unit.Default);
            this.ActivateSubviews(true);
        }

        /// <inheritdoc/>
        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            _deactivated.OnNext(Unit.Default);
            this.ActivateSubviews(false);
        }

        /// <inheritdoc/>
        void IReactiveObject.RaisePropertyChanging(PropertyChangingEventArgs args)
        {
            PropertyChangingEventManager.DeliverEvent(this, args);
        }

        /// <inheritdoc/>
        void IReactiveObject.RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChangedEventManager.DeliverEvent(this, args);
        }

        private void SetupRxObj()
        {
        }
    }

    /// <summary>
    /// This is a NSTableViewController that is both an NSTableViewController and has ReactiveObject powers
    /// (i.e. you can call RaiseAndSetIfChanged).
    /// </summary>
    /// <typeparam name="TViewModel">The view model type.</typeparam>
    public abstract class ReactiveTableViewController<TViewModel> : ReactiveTableViewController, IViewFor<TViewModel>
        where TViewModel : class
    {
        private TViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController{TViewModel}"/> class.
        /// </summary>
        /// <param name="withStyle">The table view style.</param>
        protected ReactiveTableViewController(NSTableViewStyle withStyle)
            : base(withStyle)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController{TViewModel}"/> class.
        /// </summary>
        /// <param name="nibName">The name.</param>
        /// <param name="bundle">The bundle.</param>
        protected ReactiveTableViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController{TViewModel}"/> class.
        /// </summary>
        /// <param name="handle">The pointer.</param>
        protected ReactiveTableViewController(IntPtr handle)
            : base(handle)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController{TViewModel}"/> class.
        /// </summary>
        /// <param name="t">The object flag.</param>
        protected ReactiveTableViewController(NSObjectFlag t)
            : base(t)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController{TViewModel}"/> class.
        /// </summary>
        /// <param name="coder">The coder.</param>
        protected ReactiveTableViewController(NSCoder coder)
            : base(coder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveTableViewController{TViewModel}"/> class.
        /// </summary>
        protected ReactiveTableViewController()
        {
        }

        /// <inheritdoc/>
        public TViewModel ViewModel
        {
            get => _viewModel;
            set => this.RaiseAndSetIfChanged(ref _viewModel, value);
        }

        /// <inheritdoc/>
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel)value;
        }
    }
}
