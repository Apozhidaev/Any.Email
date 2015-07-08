﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Any.Email.Mvvm
{
    /// <summary>
    /// An <see cref="T:System.Windows.Input.ICommand"/> whose delegates can be attached for <see cref="M:Any.Email.Mvvm.DelegateCommandBase.Execute(System.Object)"/> and <see cref="M:Any.Email.Mvvm.DelegateCommandBase.CanExecute(System.Object)"/>.
    /// 
    /// </summary>
    public abstract class DelegateCommandBase : ICommand
    {
        private List<WeakReference> _canExecuteChangedHandlers;
        protected readonly Func<object, Task> _executeMethod;
        protected readonly Func<object, bool> _canExecuteMethod;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute. You must keep a hard
        ///             reference to the handler to avoid garbage collection and unexpected results. See remarks for more information.
        /// 
        /// </summary>
        /// 
        /// <remarks>
        /// When subscribing to the <see cref="E:System.Windows.Input.ICommand.CanExecuteChanged"/> event using
        ///             code (not when binding using XAML) will need to keep a hard reference to the event handler. This is to prevent
        ///             garbage collection of the event handler because the command implements the Weak Event pattern so it does not have
        ///             a hard reference to this handler. An example implementation can be seen in the CompositeCommand and CommandBehaviorBase
        ///             classes. In most scenarios, there is no reason to sign up to the CanExecuteChanged event directly, but if you do, you
        ///             are responsible for maintaining the reference.
        /// 
        /// </remarks>
        /// 
        /// <example>
        /// The following code holds a reference to the event handler. The myEventHandlerReference value should be stored
        ///             in an instance member to avoid it from being garbage collected.
        /// 
        /// <code>
        /// EventHandler myEventHandlerReference = new EventHandler(this.OnCanExecuteChanged);
        ///             command.CanExecuteChanged += myEventHandlerReference;
        /// 
        /// </code>
        /// 
        /// </example>
        public virtual event EventHandler CanExecuteChanged
        {
            add
            {
                WeakEventHandlerManager.AddWeakReferenceHandler(ref this._canExecuteChangedHandlers, value, 2);
            }
            remove
            {
                WeakEventHandlerManager.RemoveWeakReferenceHandler(this._canExecuteChangedHandlers, value);
            }
        }

        /// <summary>
        /// Creates a new instance of a <see cref="T:Any.Email.Mvvm.DelegateCommandBase"/>, specifying both the execute action and the can execute function.
        /// 
        /// </summary>
        /// <param name="executeMethod">The <see cref="T:System.Action"/> to execute when <see cref="M:System.Windows.Input.ICommand.Execute(System.Object)"/> is invoked.</param><param name="canExecuteMethod">The <see cref="T:System.Func`2"/> to invoked when <see cref="M:System.Windows.Input.ICommand.CanExecute(System.Object)"/> is invoked.</param>
        protected DelegateCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", "DelegateCommandDelegatesCannotBeNull");
            this._executeMethod = (Func<object, Task>)(arg =>
            {
                executeMethod(arg);
                return Task.Delay(0);
            });
            this._canExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="T:Any.Email.Mvvm.DelegateCommandBase"/>, specifying both the Execute action as an awaitable Task and the CanExecute function.
        /// 
        /// </summary>
        /// <param name="executeMethod">The <see cref="T:System.Func`2"/> to execute when <see cref="M:System.Windows.Input.ICommand.Execute(System.Object)"/> is invoked.</param><param name="canExecuteMethod">The <see cref="T:System.Func`2"/> to invoked when <see cref="M:System.Windows.Input.ICommand.CanExecute(System.Object)"/> is invoked.</param>
        protected DelegateCommandBase(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", "DelegateCommandDelegatesCannotBeNull");
            this._executeMethod = executeMethod;
            this._canExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Raises <see cref="E:System.Windows.Input.ICommand.CanExecuteChanged"/> on the UI thread so every
        ///             command invoker can requery <see cref="M:System.Windows.Input.ICommand.CanExecute(System.Object)"/>.
        /// 
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            WeakEventHandlerManager.CallWeakReferenceHandlers((object)this, this._canExecuteChangedHandlers);
        }

        /// <summary>
        /// Raises <see cref="E:Any.Email.Mvvm.DelegateCommandBase.CanExecuteChanged"/> on the UI thread so every command invoker
        ///             can requery to check if the command can execute.
        /// 
        /// <remarks>
        /// Note that this will trigger the execution of <see cref="M:Any.Email.Mvvm.DelegateCommandBase.CanExecute(System.Object)"/> once for each invoker.
        /// </remarks>
        /// 
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.OnCanExecuteChanged();
        }

        async void ICommand.Execute(object parameter)
        {
            await this.Execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command with the provided parameter by invoking the <see cref="T:System.Action`1"/> supplied during construction.
        /// 
        /// </summary>
        /// <param name="parameter"/>
        protected async Task Execute(object parameter)
        {
            await this._executeMethod(parameter);
        }

        /// <summary>
        /// Determines if the command can execute with the provided parameter by invoking the <see cref="T:System.Func`2"/> supplied during construction.
        /// 
        /// </summary>
        /// <param name="parameter">The parameter to use when determining if this command can execute.</param>
        /// <returns>
        /// Returns <see langword="true"/> if the command can execute.  <see langword="False"/> otherwise.
        /// </returns>
        protected bool CanExecute(object parameter)
        {
            if (this._canExecuteMethod != null)
                return this._canExecuteMethod(parameter);
            else
                return true;
        }
    }
}