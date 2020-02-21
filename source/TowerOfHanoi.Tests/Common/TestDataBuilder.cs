using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using NSubstitute;

namespace TowerOfHanoi.Tests.Common
{
  public abstract class TestDataBuilder
  {
    private static bool _enableRecursiveBuildDetection;
    private static ThreadLocal<Stack<Type>> _buildInvocationStack;

    public static int MinimumBuildDepthTriggeringRecursionDetection = 100;

    public static bool EnableRecursiveBuildDetection
    {
      get => _enableRecursiveBuildDetection;
      set
      {
        if (value == _enableRecursiveBuildDetection)
          return;
        _enableRecursiveBuildDetection = value;
        _buildInvocationStack?.Dispose();
        _buildInvocationStack = value ? new ThreadLocal<Stack<Type>>(() => new Stack<Type>(), false) : null;
      }
    }

    private static long _constructionCounter;

    private protected static void BeginValidConstruction()
    {
      Interlocked.Increment(ref _constructionCounter);
    }

    private protected static void EndValidConstruction()
    {
      Interlocked.Decrement(ref _constructionCounter);
    }

    private protected static void BeginBuild(Type type)
    {
      if (EnableRecursiveBuildDetection)
      {
        var stack = _buildInvocationStack.Value;
        stack.Push(type);
      }
    }

    private protected static void EndBuild(Type type)
    {
      if (EnableRecursiveBuildDetection)
      {
        var stack = _buildInvocationStack.Value;

        if (stack.Count < 1)
          throw new InvalidOperationException("Stack imbalance detected when tracking build invocations for recursions; Stack had no elements to pop.");

        var popped = stack.Pop();

        if (popped != type)
          throw new InvalidOperationException($"Stack imbalance detected when tracking build invocations for recursions; Stack popped wrong type, expected {type} but got {popped}");

        _buildInvocationStack.Value = stack;
        EnsureNotInRecursiveBuild();
      }
    }

    private static void EnsureNotInRecursiveBuild()
    {
      var stack = _buildInvocationStack.Value;

      if (stack.Count < MinimumBuildDepthTriggeringRecursionDetection)
        return;

      var contents = stack.ToArray();
      var counters = contents.GroupBy(type => type, type => 1)
       .Select(hits => new KeyValuePair<Type, int>(hits.Key, hits.Count()))
       .ToArray();


      if (counters.Count(pair => pair.Value > 5) > 1)
      {
        var recursiveLoop = counters.Where(pair => pair.Value > 5).ToArray();
        throw new InvalidOperationException("Recursive build loop detected involving " + string.Join(", ", recursiveLoop.Select(pair => $"'{pair.Key.Name}'")));
      }
    }

    protected TestDataBuilder()
    {
      if (Interlocked.Read(ref _constructionCounter) == 0L)
        throw new NotSupportedException(
          $"Constructing a test data builder by using 'new {GetType().Name}(...)' is not supported, please use one of the static methods (MyDataBuilder.Create*/MyDataBuilder.Build*) methods instead.");
    }
  }

  public abstract class TestDataBuilder<TBuilder, TWorkingModel, TEntity> : TestDataBuilder
    where TBuilder : TestDataBuilder<TBuilder, TWorkingModel, TEntity>, new()
    where TWorkingModel : class
    where TEntity : class
  {
    private static readonly bool CanSubstitute = typeof(TWorkingModel).GetTypeInfo()
     .IsInterface;

    private static readonly bool HasDefaultConstructor = typeof(TWorkingModel).GetTypeInfo() switch
    {
      { IsInterface: true } => false,
      { IsAbstract: true } => false,
      { IsGenericType: true, IsConstructedGenericType: false } => false,
      var x when x.GetConstructor(
                   BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly,
                   null,
                   Type.EmptyTypes,
                   new ParameterModifier[0])
                 != null
      => true,
      _ => false
    };

    public static TBuilder Create()
    {
      try
      {
        BeginValidConstruction();
        return new TBuilder();
      }
      finally
      {
        EndValidConstruction();
      }
    }

    public static TBuilder CreateWithDefaultProps()
    {
      return Create().WithDefaultProps();
    }

    public static TBuilder CreateWithRandomProps()
    {
      return Create().WithRandomProps();
    }

    public static TEntity[] BuildRandomCollection(Action<TBuilder> customSetup = null, int count = 5)
    {
      var result = new List<TEntity>();

      for (int i = 0; i < count; i++)
        result.Add(BuildRandom(customSetup));

      return result.ToArray();
    }

    public static TEntity BuildRandom()
    {
      return BuildRandom(null);
    }

    public static TEntity BuildDefault()
    {
      return BuildDefault(null);
    }

    public static TEntity BuildRandom(Action<TBuilder> customSetup)
    {
      return Create()
       .WithRandomProps()
       .Build(customSetup);
    }

    public static TEntity BuildDefault(Action<TBuilder> customSetup)
    {
      return Create()
       .WithDefaultProps()
       .Build(customSetup);
    }

    private readonly TBuilder _self;
    private bool _building;
    private readonly List<Action<WorkingModelReference>> _intraBuildMutations;
    private readonly List<Action<WorkingModelReference>> _mutations;

    protected TestDataBuilder()
    {
      _self = this as TBuilder;
      _mutations = new List<Action<WorkingModelReference>>();
      _intraBuildMutations = new List<Action<WorkingModelReference>>();
    }

    protected virtual TWorkingModel CreateWorkingModel()
    {
      if (HasDefaultConstructor)
        return Activator.CreateInstance<TWorkingModel>();

      if (CanSubstitute)
        return Substitute.For<TWorkingModel>();

      throw new NotSupportedException(
        $"Cannot create an instance of {typeof(TWorkingModel)}, please override {GetType()}.{nameof(CreateWorkingModel)}");
    }

    protected internal virtual TBuilder WithProp(Action<TWorkingModel> mutation)
    {
      (_building ? _intraBuildMutations : _mutations).Add(wrapped => mutation(wrapped.Value));
      return _self;
    }

    protected virtual TBuilder WithPropByReference(Action<WorkingModelReference> mutation)
    {
      (_building ? _intraBuildMutations : _mutations).Add(mutation);
      return _self;
    }


    /// <summary>
    /// Use this to configure an entity to behave correctly for non-specific usages in tests, but with random values.
    /// </summary>
    public virtual TBuilder WithRandomProps()
    {
      throw new NotImplementedException("You need to implement this if you expect to get a valid random entity");
    }

    /// <summary>
    /// Use this to configure an entity to behave correctly for non-specific usages in tests, for example, a stored event migration could by default
    /// return the stored event passed in as a parameter to the migrate method to produce the correct behavior.
    /// </summary>
    public virtual TBuilder WithDefaultProps()
    {
      return _self;
    }

    public virtual TEntity Build(Action<TBuilder> alterationsForThisBuild = null)
    {
      if (_building)
        throw new InvalidOperationException("Builder does not support recursive builds, but it could :D");
      _building = true;
      BeginBuild(typeof(TEntity));

      try
      {
        var workingModelReference = new WorkingModelReference { Value = CreateWorkingModel() };
        IEnumerable<Action<WorkingModelReference>> mutations = _mutations;

        if (alterationsForThisBuild != null)
        {
          alterationsForThisBuild(_self);

          if (_intraBuildMutations.Count > 0)
          {
            mutations = mutations.Concat(_intraBuildMutations.ToArray());
            _intraBuildMutations.Clear();
          }
        }

        foreach (var mutation in mutations)
        {
          mutation(workingModelReference);
          // Don't change this to a foreach, as the collection will be modified during the iteration.
          for (int i = 0; i < _intraBuildMutations.Count; i++)
            _intraBuildMutations[i](workingModelReference);

          _intraBuildMutations.Clear();
        }

        return ConvertFromWorkingModelToEntity(workingModelReference.Value);
      }
      finally
      {
        EndBuild(typeof(TEntity));
        _building = false;
      }
    }

    protected abstract TEntity ConvertFromWorkingModelToEntity(TWorkingModel workingModel);

    protected sealed class WorkingModelReference
    {
      public TWorkingModel Value { get; set; }
    }
  }
  public abstract class TestDataBuilder<TBuilder, TEntity> : TestDataBuilder<TBuilder, TEntity, TEntity>
    where TBuilder : TestDataBuilder<TBuilder, TEntity>, new()
    where TEntity : class
  {

    protected override TEntity ConvertFromWorkingModelToEntity(TEntity constructedEntity)
    {
      return constructedEntity;
    }
  }
}