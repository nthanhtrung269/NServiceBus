namespace NServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extensibility;
    using Persistence;
    using Sagas;

    class CustomFinderAdapter<TSagaData, TMessage> : SagaFinder where TSagaData : IContainSagaData
    {
        public override async Task<IContainSagaData> Find(IServiceProvider builder, SagaFinderDefinition finderDefinition, SynchronizedStorageSession storageSession, ContextBag context, object message, IReadOnlyDictionary<string, string> messageHeaders)
        {
            var customFinderType = (Type) finderDefinition.Properties["custom-finder-clr-type"];

            var finder = (IFindSagas<TSagaData>.Using<TMessage>) builder.GetService(customFinderType);

            return await finder
                .FindBy((TMessage) message, storageSession, context)
                .ThrowIfNull()
                .ConfigureAwait(false);
        }
    }
}