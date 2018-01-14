using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using OrchardCore.Entities;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Models;

namespace OrchardCore.Workflows.Activities
{
    public abstract class Activity : Entity, IActivity
    {
        public abstract string Name { get; }
        public abstract LocalizedString Category { get; }
        public abstract LocalizedString Description { get; }

        public abstract IEnumerable<Outcome> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext);

        public virtual Task<bool> CanExecuteAsync(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return Task.FromResult(CanExecute(workflowContext, activityContext));
        }

        public virtual bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return true;
        }

        public virtual Task<ActivityExecutionResult> ExecuteAsync(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return Task.FromResult(Execute(workflowContext, activityContext));
        }

        public virtual ActivityExecutionResult Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return ActivityExecutionResult.Noop();
        }

        public virtual Task<ActivityExecutionResult> ResumeAsync(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return Task.FromResult(Resume(workflowContext, activityContext));
        }

        public virtual ActivityExecutionResult Resume(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return ActivityExecutionResult.Noop();
        }

        public virtual Task OnInputReceivedAsync(WorkflowContext workflowContext, IDictionary<string, object> input)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnWorkflowStartingAsync(WorkflowContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public virtual Task OnWorkflowStartedAsync(WorkflowContext context)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnWorkflowResumingAsync(WorkflowContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public virtual Task OnWorkflowResumedAsync(WorkflowContext context)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnActivityExecutingAsync(WorkflowContext workflowContext, ActivityContext activityContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public virtual Task OnActivityExecutedAsync(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            return Task.CompletedTask;
        }

        protected Outcome Outcome(LocalizedString name)
        {
            return new Outcome(name);
        }

        protected IEnumerable<Outcome> Outcomes(params LocalizedString[] names)
        {
            return names.Select(x => new Outcome(x));
        }

        protected ActivityExecutionResult Outcomes(params string[] names)
        {
            return ActivityExecutionResult.FromOutcomes(names);
        }

        protected ActivityExecutionResult Outcomes(IEnumerable<string> names)
        {
            return ActivityExecutionResult.FromOutcomes(names);
        }

        protected ActivityExecutionResult Halt()
        {
            return ActivityExecutionResult.Halt();
        }

        protected ActivityExecutionResult Noop()
        {
            return ActivityExecutionResult.Noop();
        }

        protected virtual T GetProperty<T>(Func<T> defaultValue = null, [CallerMemberName]string name = null)
        {
            var item = Properties[name];
            return item != null ? item.ToObject<T>() : defaultValue != null ? defaultValue() : default(T);
        }

        protected virtual void SetProperty<T>(T value, [CallerMemberName]string name = null)
        {
            Properties[name] = JToken.FromObject(value);
        }
    }
}