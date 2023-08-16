using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTWRK.Tests
{
    public class AggregateFluentWrapper<T> : IAggregateFluent<T>
    {
        private readonly IAggregateFluent<T> _aggregateFluent;

        public AggregateFluentWrapper(IAggregateFluent<T> aggregateFluent)
        {
            _aggregateFluent = aggregateFluent;
        }

        public IMongoDatabase Database => throw new NotImplementedException();

        public AggregateOptions Options => throw new NotImplementedException();

        public IList<IPipelineStageDefinition> Stages => throw new NotImplementedException();

        public IAggregateFluent<TNewResult> AppendStage<TNewResult>(PipelineStageDefinition<T, TNewResult> stage)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> As<TNewResult>(IBsonSerializer<TNewResult> newResultSerializer = null)
        {
            return new AggregateFluentWrapper<TNewResult>(_aggregateFluent.As<TNewResult>(newResultSerializer));
        }

        public IAggregateFluent<AggregateBucketResult<TValue>> Bucket<TValue>(AggregateExpressionDefinition<T, TValue> groupBy, IEnumerable<TValue> boundaries, AggregateBucketOptions<TValue> options = null)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> Bucket<TValue, TNewResult>(AggregateExpressionDefinition<T, TValue> groupBy, IEnumerable<TValue> boundaries, ProjectionDefinition<T, TNewResult> output, AggregateBucketOptions<TValue> options = null)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<AggregateBucketAutoResult<TValue>> BucketAuto<TValue>(AggregateExpressionDefinition<T, TValue> groupBy, int buckets, AggregateBucketAutoOptions options = null)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> BucketAuto<TValue, TNewResult>(AggregateExpressionDefinition<T, TValue> groupBy, int buckets, ProjectionDefinition<T, TNewResult> output, AggregateBucketAutoOptions options = null)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<ChangeStreamDocument<T>> ChangeStream(ChangeStreamStageOptions options = null)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<AggregateCountResult> Count()
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<T> Densify(FieldDefinition<T> field, DensifyRange range, IEnumerable<FieldDefinition<T>> partitionByFields = null)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<T> Densify(FieldDefinition<T> field, DensifyRange range, params FieldDefinition<T>[] partitionByFields)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> Facet<TNewResult>(IEnumerable<AggregateFacet<T>> facets, AggregateFacetOptions<TNewResult> options = null)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> GraphLookup<TFrom, TConnectFrom, TConnectTo, TStartWith, TAsElement, TAs, TNewResult>(IMongoCollection<TFrom> from, FieldDefinition<TFrom, TConnectFrom> connectFromField, FieldDefinition<TFrom, TConnectTo> connectToField, AggregateExpressionDefinition<T, TStartWith> startWith, FieldDefinition<TNewResult, TAs> @as, FieldDefinition<TAsElement, int> depthField, AggregateGraphLookupOptions<TFrom, TAsElement, TNewResult> options = null) where TAs : IEnumerable<TAsElement>
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> Group<TNewResult>(ProjectionDefinition<T, TNewResult> group)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<T> Limit(int limit)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> Lookup<TForeignDocument, TNewResult>(string foreignCollectionName, FieldDefinition<T> localField, FieldDefinition<TForeignDocument> foreignField, FieldDefinition<TNewResult> @as, AggregateLookupOptions<TForeignDocument, TNewResult> options = null)
        {
            return new AggregateFluentWrapper<TNewResult>(_aggregateFluent.Lookup(foreignCollectionName, localField, foreignField, @as, options));
        }

        public IAggregateFluent<TNewResult> Lookup<TForeignDocument, TAsElement, TAs, TNewResult>(IMongoCollection<TForeignDocument> foreignCollection, BsonDocument let, PipelineDefinition<TForeignDocument, TAsElement> lookupPipeline, FieldDefinition<TNewResult, TAs> @as, AggregateLookupOptions<TForeignDocument, TNewResult> options = null) where TAs : IEnumerable<TAsElement>
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<T> Match(FilterDefinition<T> filter)
        {
            return new AggregateFluentWrapper<T>(_aggregateFluent.Match(filter));
        }

        public IAsyncCursor<TOutput> Merge<TOutput>(IMongoCollection<TOutput> outputCollection, MergeStageOptions<TOutput> mergeOptions = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncCursor<TOutput>> MergeAsync<TOutput>(IMongoCollection<TOutput> outputCollection, MergeStageOptions<TOutput> mergeOptions = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> OfType<TNewResult>(IBsonSerializer<TNewResult> newResultSerializer = null) where TNewResult : T
        {
            throw new NotImplementedException();
        }

        public IAsyncCursor<T> Out(IMongoCollection<T> outputCollection, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncCursor<T> Out(string collectionName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncCursor<T>> OutAsync(IMongoCollection<T> outputCollection, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncCursor<T>> OutAsync(string collectionName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> Project<TNewResult>(ProjectionDefinition<T, TNewResult> projection)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> ReplaceRoot<TNewResult>(AggregateExpressionDefinition<T, TNewResult> newRoot)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> ReplaceWith<TNewResult>(AggregateExpressionDefinition<T, TNewResult> newRoot)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<BsonDocument> SetWindowFields<TWindowFields>(AggregateExpressionDefinition<ISetWindowFieldsPartition<T>, TWindowFields> output)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<BsonDocument> SetWindowFields<TPartitionBy, TWindowFields>(AggregateExpressionDefinition<T, TPartitionBy> partitionBy, AggregateExpressionDefinition<ISetWindowFieldsPartition<T>, TWindowFields> output)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<BsonDocument> SetWindowFields<TPartitionBy, TWindowFields>(AggregateExpressionDefinition<T, TPartitionBy> partitionBy, SortDefinition<T> sortBy, AggregateExpressionDefinition<ISetWindowFieldsPartition<T>, TWindowFields> output)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<T> Skip(int skip)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<T> Sort(SortDefinition<T> sort)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<AggregateSortByCountResult<TId>> SortByCount<TId>(AggregateExpressionDefinition<T, TId> id)
        {
            throw new NotImplementedException();
        }

        public void ToCollection(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task ToCollectionAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncCursor<T> ToCursor(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IAsyncCursor<T>> ToCursorAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<T> UnionWith<TWith>(IMongoCollection<TWith> withCollection, PipelineDefinition<TWith, T> withPipeline = null)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> Unwind<TNewResult>(FieldDefinition<T> field, IBsonSerializer<TNewResult> newResultSerializer)
        {
            throw new NotImplementedException();
        }

        public IAggregateFluent<TNewResult> Unwind<TNewResult>(FieldDefinition<T> field, AggregateUnwindOptions<TNewResult> options = null)
        {
            return new AggregateFluentWrapper<TNewResult>(_aggregateFluent.Unwind<TNewResult>(field, options));
        }
    }
}
