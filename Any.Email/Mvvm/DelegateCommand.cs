using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Any.Email.Mvvm
{
    /// <summary>
    /// An <see cref="T:System.Windows.Input.ICommand"/> whose delegates can be attached for <see cref="M:Any.Email.Mvvm.DelegateCommand`1.Execute(`0)"/> and <see cref="M:Any.Email.Mvvm.DelegateCommand`1.CanExecute(`0)"/>.
    /// 
    /// </summary>
    /// <typeparam name="T">Parameter type.</typeparam>
    /// <remarks>
    /// The constructor deliberately prevents the use of value types.
    ///             Because ICommand takes an object, having a value type for T would cause unexpected behavior when CanExecute(null) is called during XAML initialization for command bindings.
    ///             Using default(T) was considered and rejected as a solution because the implementor would not be able to distinguish between a valid and defaulted values.
    /// 
    /// <para/>
    /// 
    ///             Instead, callers should support a value type by using a nullable value type and checking the HasValue property before using the Value property.
    /// 
    /// <example>
    /// 
    /// <code>
    /// public MyClass()
    ///             {
    ///                 this.submitCommand = new DelegateCommand&lt;int?&gt;(this.Submit, this.CanSubmit);
    ///             }
    /// 
    ///             private bool CanSubmit(int? customerId)
    ///             {
    ///                 return (customerId.HasValue &amp;&amp; customers.Contains(customerId.Value));
    ///             }
    /// 
    /// </code>
    /// 
    /// </example>
    /// 
    /// </remarks>
    public class DelegateCommand<T> : DelegateCommandBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="T:Any.Email.Mvvm.DelegateCommand`1"/>.
        /// 
        /// </summary>
        /// <param name="executeMethod">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param>
        /// <remarks>
        /// <see cref="M:Any.Email.Mvvm.DelegateCommand`1.CanExecute(`0)"/> will always return true.
        /// </remarks>
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, (Func<T, bool>)(o => true))
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:Any.Email.Mvvm.DelegateCommand`1"/>.
        /// 
        /// </summary>
        /// <param name="executeMethod">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param><param name="canExecuteMethod">Delegate to execute when CanExecute is called on the command. This can be null.</param><exception cref="T:System.ArgumentNullException">When both <paramref name="executeMethod"/> and <paramref name="canExecuteMethod"/> ar <see langword="null"/>.</exception>
        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base((Action<object>)(o => executeMethod((T)o)), (Func<object, bool>)(o => canExecuteMethod((T)o)))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", "DelegateCommandDelegatesCannotBeNull");
            TypeInfo typeInfo = IntrospectionExtensions.GetTypeInfo(typeof(T));
            if (typeInfo.IsValueType && (!typeInfo.IsGenericType || !IntrospectionExtensions.GetTypeInfo(typeof(Nullable<>)).IsAssignableFrom(IntrospectionExtensions.GetTypeInfo(typeInfo.GetGenericTypeDefinition()))))
                throw new InvalidCastException("DelegateCommandInvalidGenericPayloadType");
        }

        private DelegateCommand(Func<T, Task> executeMethod)
            : this(executeMethod, (Func<T, bool>)(o => true))
        {
        }

        private DelegateCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
            : base((Func<object, Task>)(o => executeMethod((T)o)), (Func<object, bool>)(o => canExecuteMethod((T)o)))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod", "DelegateCommandDelegatesCannotBeNull");
        }

        /// <summary>
        /// Factory method to create a new instance of <see cref="T:Any.Email.Mvvm.DelegateCommand`1"/> from an awaitable handler method.
        /// 
        /// </summary>
        /// <param name="executeMethod">Delegate to execute when Execute is called on the command.</param>
        /// <returns>
        /// Constructed instance of <see cref="T:Any.Email.Mvvm.DelegateCommand`1"/>
        /// </returns>
        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod)
        {
            return new DelegateCommand<T>(executeMethod);
        }

        /// <summary>
        /// Factory method to create a new instance of <see cref="T:Any.Email.Mvvm.DelegateCommand`1"/> from an awaitable handler method.
        /// 
        /// </summary>
        /// <param name="executeMethod">Delegate to execute when Execute is called on the command. This can be null to just hook up a CanExecute delegate.</param><param name="canExecuteMethod">Delegate to execute when CanExecute is called on the command. This can be null.</param>
        /// <returns>
        /// Constructed instance of <see cref="T:Any.Email.Mvvm.DelegateCommand`1"/>
        /// </returns>
        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            return new DelegateCommand<T>(executeMethod, canExecuteMethod);
        }

        /// <summary>
        /// Determines if the command can execute by invoked the <see cref="T:System.Func`2"/> provided during construction.
        /// 
        /// </summary>
        /// <param name="parameter">Data used by the command to determine if it can execute.</param>
        /// <returns>
        /// <see langword="true"/> if this command can be executed; otherwise, <see langword="false"/>.
        /// 
        /// </returns>
        public virtual bool CanExecute(T parameter)
        {
            return base.CanExecute((object)parameter);
        }

        /// <summary>
        /// Executes the command and invokes the <see cref="T:System.Action`1"/> provided during construction.
        /// 
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public virtual async Task Execute(T parameter)
        {
            await base.Execute((object)parameter);
        }
    }
}
