﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NeuralNetworkNET.APIs.Interfaces.Data;
using NeuralNetworkNET.Extensions;
using NeuralNetworkNET.SupervisedLearning.Data;
using NeuralNetworkNET.SupervisedLearning.Optimization.Parameters;
using NeuralNetworkNET.SupervisedLearning.Optimization.Progress;

namespace NeuralNetworkNET.APIs
{
    /// <summary>
    /// A static class with helper methods to easily create datasets to use to train and test a network
    /// </summary>
    public static class DatasetLoader
    {
        #region Training

        /// <summary>
        /// Creates a new <see cref="ITrainingDataset"/> instance to train a network from the input collection, with the specified batch size
        /// </summary>
        /// <param name="data">The source collection to use to build the training dataset</param>
        /// <param name="size">The desired dataset batch size</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static ITrainingDataset Training([NotNull] IEnumerable<(float[] X, float[] Y)> data, int size) => BatchesCollection.From(data, size);

        /// <summary>
        /// Creates a new <see cref="ITrainingDataset"/> instance to train a network from the input collection, with the specified batch size
        /// </summary>
        /// <param name="data">The source collection to use to build the training dataset</param>
        /// <param name="size">The desired dataset batch size</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static ITrainingDataset Training([NotNull] IEnumerable<Func<(float[] X, float[] Y)>> data, int size) => BatchesCollection.From(data, size);

        /// <summary>
        /// Creates a new <see cref="ITrainingDataset"/> instance to train a network from the input matrices, with the specified batch size
        /// </summary>
        /// <param name="data">The source matrices to use to build the training dataset</param>
        /// <param name="size">The desired dataset batch size</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static ITrainingDataset Training((float[,] X, float[,] Y) data, int size) => BatchesCollection.From(data, size);

        #endregion

        #region Validation

        /// <summary>
        /// Creates a new <see cref="IValidationDataset"/> instance to validate a network accuracy from the input collection
        /// </summary>
        /// <param name="data">The source collection to use to build the validation dataset</param>
        /// <param name="tolerance">The desired tolerance to test the network for convergence</param>
        /// <param name="epochs">The epochs interval to consider when testing the network for convergence</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static IValidationDataset Validation([NotNull] IEnumerable<(float[] X, float[] Y)> data, float tolerance = 1e-2f, int epochs = 5)
            => new ValidationDataset(data.MergeLines(), tolerance, epochs);

        /// <summary>
        /// Creates a new <see cref="IValidationDataset"/> instance to validate a network accuracy from the input collection
        /// </summary>
        /// <param name="data">The source collection to use to build the validation dataset</param>
        /// <param name="tolerance">The desired tolerance to test the network for convergence</param>
        /// <param name="epochs">The epochs interval to consider when testing the network for convergence</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static IValidationDataset Validation([NotNull] IEnumerable<Func<(float[] X, float[] Y)>> data, float tolerance = 1e-2f, int epochs = 5)
            => Validation(data.AsParallel().Select(f => f()), tolerance, epochs);

        /// <summary>
        /// Creates a new <see cref="IValidationDataset"/> instance to validate a network accuracy from the input collection
        /// </summary>
        /// <param name="data">The source collection to use to build the validation dataset</param>
        /// <param name="tolerance">The desired tolerance to test the network for convergence</param>
        /// <param name="epochs">The epochs interval to consider when testing the network for convergence</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static IValidationDataset Validation((float[,] X, float[,] Y) data, float tolerance = 1e-2f, int epochs = 5) => new ValidationDataset(data, tolerance, epochs);

        #endregion

        #region Test

        /// <summary>
        /// Creates a new <see cref="ITestDataset"/> instance to test a network from the input collection
        /// </summary>
        /// <param name="data">The source collection to use to build the test dataset</param>
        /// <param name="progress">The optional progress callback to use</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static ITestDataset Test([NotNull] IEnumerable<(float[] X, float[] Y)> data, [CanBeNull] IProgress<TrainingProgressEventArgs> progress = null)
            => new TestDataset(data.MergeLines(), progress);

        /// <summary>
        /// Creates a new <see cref="ITestDataset"/> instance to test a network from the input collection
        /// </summary>
        /// <param name="data">The source collection to use to build the test dataset</param>
        /// <param name="progress">The optional progress callback to use</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static ITestDataset Test([NotNull] IEnumerable<Func<(float[] X, float[] Y)>> data, [CanBeNull] IProgress<TrainingProgressEventArgs> progress = null)
            => Test(data.AsParallel().Select(f => f()), progress);

        /// <summary>
        /// Creates a new <see cref="ITestDataset"/> instance to test a network from the input collection
        /// </summary>
        /// <param name="data">The source collection to use to build the test dataset</param>
        /// <param name="progress">The optional progress callback to use</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static ITestDataset Test((float[,] X, float[,] Y) data, [CanBeNull] IProgress<TrainingProgressEventArgs> progress = null) => new TestDataset(data, progress);

        #endregion
    }
}
