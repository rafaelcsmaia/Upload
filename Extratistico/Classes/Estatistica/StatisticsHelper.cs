using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Extratistico.Classes.Estatistica
{

    public class StatisticsHelper
    {
        // sortedData is used to calculate percentiles

        internal double[] sortedData;
        /// <summary>
        /// DescriptiveResult default constructor
        /// </summary>
        public StatisticsHelper()
        {
        }

        /// <summary>
        /// Count
        /// </summary>
        public uint Count;
        /// <summary>
        /// Sum
        /// </summary>
        public double Sum;
        /// <summary>
        /// Arithmatic mean
        /// </summary>
        public double Mean;
        /// <summary>
        /// Geometric mean
        /// </summary>
        public double GeometricMean;
        /// <summary>
        /// Harmonic mean
        /// </summary>
        public double HarmonicMean;
        /// <summary>
        /// Minimum value
        /// </summary>
        public double Min;
        /// <summary>
        /// Maximum value
        /// </summary>
        public double Max;
        /// <summary>
        /// The range of the values
        /// </summary>
        public double Range;
        /// <summary>
        /// Sample variance
        /// </summary>
        public double Variance;
        /// <summary>
        /// Sample standard deviation
        /// </summary>
        public double StdDev;
        /// <summary>
        /// Skewness of the data distribution
        /// </summary>
        public double Skewness;
        /// <summary>
        /// Kurtosis of the data distribution
        /// </summary>
        public double Kurtosis;
        /// <summary>
        /// Interquartile range
        /// </summary>
        public double IQR;
        /// <summary>
        /// Median, or second quartile, or at 50 percentile
        /// </summary>
        public double Median;
        /// <summary>
        /// First quartile, at 25 percentile
        /// </summary>
        public double FirstQuartile;
        /// <summary>
        /// Third quartile, at 75 percentile
        /// </summary>

        public double ThirdQuartile;

        /// <summary>
        /// Sum of Error
        /// </summary>

        internal double SumOfError;
        /// <summary>
        /// The sum of the squares of errors
        /// </summary>

        internal double SumOfErrorSquare;
        /// <summary>
        /// Percentile
        /// </summary>
        /// <param name="percent">Pecentile, between 0 to 100</param>
        /// <returns>Percentile</returns>
        public double Percentile(double percent)
        {
            return Descriptive.percentile(sortedData, percent);
        }
    }
    // end of class DescriptiveResult

    /// <summary>
    /// Descriptive class
    /// </summary>
    public class Descriptive
    {
        private double[] data;

        private double[] sortedData;
        /// <summary>
        /// Descriptive results
        /// </summary>

        public StatisticsHelper Result = new StatisticsHelper();
        #region "Constructors"
        /// <summary>
        /// Descriptive analysis default constructor
        /// </summary>
        public Descriptive()
        {
        }
        // default empty constructor
        /// <summary>
        /// Descriptive analysis constructor
        /// </summary>
        /// <param name="dataVariable">Data array</param>
        public Descriptive(double[] dataVariable)
        {
            data = dataVariable;
        }
        #endregion

        /// <summary>
        /// Run the analysis to obtain descriptive information of the data
        /// </summary>

        public void Analyze()
        {
            // initializations
            Result.Count = 0;
            Result.Min = InlineAssignHelper(ref Result.Max, InlineAssignHelper(ref Result.Range, InlineAssignHelper(ref Result.Mean, InlineAssignHelper(ref Result.Sum, InlineAssignHelper(ref Result.StdDev, InlineAssignHelper(ref Result.Variance, 0.0))))));

            double sumOfSquare = 0.0;
            double sumOfESquare = 0.0;
            // must initialize
            double[] squares = new double[data.Length];
            double cumProduct = 1.0;
            // to calculate geometric mean
            double cumReciprocal = 0.0;
            // to calculate harmonic mean
            // First iteration
            for (int i = 0; i <= data.Length - 1; i++)
            {
                if (i == 0)
                {
                    // first data point
                    Result.Min = data[i];
                    Result.Max = data[i];
                    Result.Mean = data[i];
                    Result.Range = 0.0;
                }
                else
                {
                    // not the first data point
                    if (data[i] < Result.Min)
                    {
                        Result.Min = data[i];
                    }
                    if (data[i] > Result.Max)
                    {
                        Result.Max = data[i];
                    }
                }
                Result.Sum += data[i];
                squares[i] = Math.Pow(data[i], 2);
                //TODO: may not be necessary
                sumOfSquare += squares[i];

                cumProduct *= data[i];
                cumReciprocal += 1.0 / data[i];
            }

            Result.Count = Convert.ToUInt32(data.Length);
            double n = Convert.ToDouble(Result.Count);
            // use a shorter variable in double type
            Result.Mean = Result.Sum / n;
            Result.GeometricMean = Math.Pow(cumProduct, 1.0 / n);
            Result.HarmonicMean = 1.0 / (cumReciprocal / n);
            // see http://mathworld.wolfram.com/HarmonicMean.html
            Result.Range = Result.Max - Result.Min;

            // second loop, calculate Stdev, sum of errors
            //double[] eSquares = new double[data.Length];
            double m1 = 0.0;
            double m2 = 0.0;
            double m3 = 0.0;
            // for skewness calculation
            double m4 = 0.0;
            // for kurtosis calculation
            // for skewness
            for (int i = 0; i <= data.Length - 1; i++)
            {
                double m = data[i] - Result.Mean;
                double mPow2 = m * m;
                double mPow3 = mPow2 * m;
                double mPow4 = mPow3 * m;

                m1 += Math.Abs(m);

                m2 += mPow2;

                // calculate skewness
                m3 += mPow3;

                // calculate skewness

                m4 += mPow4;
            }

            Result.SumOfError = m1;
            Result.SumOfErrorSquare = m2;
            // Added for Excel function DEVSQ
            sumOfESquare = m2;

            // var and standard deviation
            Result.Variance = sumOfESquare / (Convert.ToDouble(Result.Count) - 1);
            Result.StdDev = Math.Sqrt(Result.Variance);

            // using Excel approach
            double skewCum = 0.0;
            // the cum part of SKEW formula
            for (int i = 0; i <= data.Length - 1; i++)
            {
                skewCum += Math.Pow((data[i] - Result.Mean) / Result.StdDev, 3);
            }
            Result.Skewness = n / (n - 1) / (n - 2) * skewCum;

            // kurtosis: see http://en.wikipedia.org/wiki/Kurtosis (heading: Sample Kurtosis)
            double m2_2 = Math.Pow(sumOfESquare, 2);
            Result.Kurtosis = ((n + 1) * n * (n - 1)) / ((n - 2) * (n - 3)) * (m4 / m2_2) - 3 * Math.Pow(n - 1, 2) / ((n - 2) * (n - 3));
            // second last formula for G2
            // calculate quartiles
            sortedData = new double[data.Length];
            data.CopyTo(sortedData, 0);
            Array.Sort(sortedData);

            // copy the sorted data to result object so that
            // user can calculate percentile easily
            Result.sortedData = new double[data.Length];
            sortedData.CopyTo(Result.sortedData, 0);

            Result.FirstQuartile = percentile(sortedData, 25);
            Result.ThirdQuartile = percentile(sortedData, 75);
            Result.Median = percentile(sortedData, 50);
            Result.IQR = percentile(sortedData, 75) - percentile(sortedData, 25);

        }
        // end of method Analyze

        /// <summary>
        /// Calculate percentile of a sorted data set
        /// </summary>
        /// <param name="sortedData"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        static internal double percentile(double[] sortedData, double p)
        {
            // algo derived from Aczel pg 15 bottom
            if (p >= 100.0)
            {
                return sortedData[sortedData.Length - 1];
            }

            double position = Convert.ToDouble(sortedData.Length + 1) * p / 100.0;
            double leftNumber = 0.0;
            double rightNumber = 0.0;

            double n = p / 100.0 * (sortedData.Length - 1) + 1.0;

            if (position >= 1)
            {
                leftNumber = sortedData[Convert.ToInt32(Math.Truncate(System.Math.Floor(n))) - 1];
                rightNumber = sortedData[Convert.ToInt32(Math.Truncate(System.Math.Floor(n)))];
            }
            else
            {
                leftNumber = sortedData[0];
                // first data
                // first data
                rightNumber = sortedData[1];
            }

            if (leftNumber == rightNumber)
            {
                return leftNumber;
            }
            else
            {
                double part = n - System.Math.Floor(n);
                return leftNumber + part * (rightNumber - leftNumber);
            }
        }
        private static T InlineAssignHelper<T>(ref T target, T value)
        {
            target = value;
            return value;
        }
        // end of internal function percentile
    }
    // end of class Descriptive
}