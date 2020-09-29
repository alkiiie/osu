﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Judgements
{
    /// <summary>
    /// The scoring information provided by a <see cref="HitObject"/>.
    /// </summary>
    public class Judgement
    {
        /// <summary>
        /// The score awarded for a small bonus.
        /// </summary>
        public const double SMALL_BONUS_SCORE = 10;

        /// <summary>
        /// The score awarded for a large bonus.
        /// </summary>
        public const double LARGE_BONUS_SCORE = 50;

        /// <summary>
        /// The default health increase for a maximum judgement, as a proportion of total health.
        /// By default, each maximum judgement restores 5% of total health.
        /// </summary>
        protected const double DEFAULT_MAX_HEALTH_INCREASE = 0.05;

        /// <summary>
        /// Whether this <see cref="Judgement"/> should affect the current combo.
        /// </summary>
        [Obsolete("Has no effect. Use HitResult members instead (e.g. use small-tick or bonus to not affect combo).")] // Can be removed 20210328
        public virtual bool AffectsCombo => true;

        /// <summary>
        /// Whether this <see cref="Judgement"/> should be counted as base (combo) or bonus score.
        /// </summary>
        [Obsolete("Has no effect. Use HitResult members instead (e.g. use small-tick or bonus to not affect combo).")] // Can be removed 20210328
        public virtual bool IsBonus => !AffectsCombo;

        /// <summary>
        /// The maximum <see cref="HitResult"/> that can be achieved.
        /// </summary>
        public virtual HitResult MaxResult => HitResult.Perfect;

        /// <summary>
        /// The minimum <see cref="HitResult"/> that can be achieved - the inverse of <see cref="MaxResult"/>.
        /// </summary>
        public HitResult MinResult
        {
            get
            {
                switch (MaxResult)
                {
                    case HitResult.SmallBonus:
                    case HitResult.LargeBonus:
                    case HitResult.IgnoreHit:
                        return HitResult.IgnoreMiss;

                    case HitResult.SmallTickHit:
                        return HitResult.SmallTickMiss;

                    case HitResult.LargeTickHit:
                        return HitResult.LargeTickMiss;

                    default:
                        return HitResult.Miss;
                }
            }
        }

        /// <summary>
        /// The numeric score representation for the maximum achievable result.
        /// </summary>
        public double MaxNumericResult => ToNumericResult(MaxResult);

        /// <summary>
        /// The health increase for the maximum achievable result.
        /// </summary>
        public double MaxHealthIncrease => HealthIncreaseFor(MaxResult);

        /// <summary>
        /// Retrieves the numeric score representation of a <see cref="JudgementResult"/>.
        /// </summary>
        /// <param name="result">The <see cref="JudgementResult"/> to find the numeric score representation for.</param>
        /// <returns>The numeric score representation of <paramref name="result"/>.</returns>
        [Obsolete("Has no effect. Use ToNumericResult(HitResult) (standardised across all rulesets).")] // Can be removed 20210328
        protected virtual int NumericResultFor(HitResult result) => result == HitResult.Miss ? 0 : 1;

        /// <summary>
        /// Retrieves the numeric score representation of a <see cref="JudgementResult"/>.
        /// </summary>
        /// <param name="result">The <see cref="JudgementResult"/> to find the numeric score representation for.</param>
        /// <returns>The numeric score representation of <paramref name="result"/>.</returns>
        public double NumericResultFor(JudgementResult result) => ToNumericResult(result.Type);

        /// <summary>
        /// Retrieves the numeric health increase of a <see cref="HitResult"/>.
        /// </summary>
        /// <param name="result">The <see cref="HitResult"/> to find the numeric health increase for.</param>
        /// <returns>The numeric health increase of <paramref name="result"/>.</returns>
        protected virtual double HealthIncreaseFor(HitResult result)
        {
            switch (result)
            {
                default:
                    return 0;

                case HitResult.SmallTickHit:
                    return DEFAULT_MAX_HEALTH_INCREASE * 0.05;

                case HitResult.SmallTickMiss:
                    return -DEFAULT_MAX_HEALTH_INCREASE * 0.05;

                case HitResult.LargeTickHit:
                    return DEFAULT_MAX_HEALTH_INCREASE * 0.1;

                case HitResult.LargeTickMiss:
                    return -DEFAULT_MAX_HEALTH_INCREASE * 0.1;

                case HitResult.Miss:
                    return -DEFAULT_MAX_HEALTH_INCREASE;

                case HitResult.Meh:
                    return -DEFAULT_MAX_HEALTH_INCREASE * 0.05;

                case HitResult.Ok:
                    return -DEFAULT_MAX_HEALTH_INCREASE * 0.01;

                case HitResult.Good:
                    return DEFAULT_MAX_HEALTH_INCREASE * 0.5;

                case HitResult.Great:
                    return DEFAULT_MAX_HEALTH_INCREASE;

                case HitResult.Perfect:
                    return DEFAULT_MAX_HEALTH_INCREASE * 1.05;

                case HitResult.SmallBonus:
                    return DEFAULT_MAX_HEALTH_INCREASE * 0.1;

                case HitResult.LargeBonus:
                    return DEFAULT_MAX_HEALTH_INCREASE * 0.2;
            }
        }

        /// <summary>
        /// Retrieves the numeric health increase of a <see cref="JudgementResult"/>.
        /// </summary>
        /// <param name="result">The <see cref="JudgementResult"/> to find the numeric health increase for.</param>
        /// <returns>The numeric health increase of <paramref name="result"/>.</returns>
        public double HealthIncreaseFor(JudgementResult result) => HealthIncreaseFor(result.Type);

        public override string ToString() => $"MaxResult:{MaxResult} MaxScore:{MaxNumericResult}";

        public static double ToNumericResult(HitResult result)
        {
            switch (result)
            {
                default:
                    return 0;

                case HitResult.SmallTickHit:
                    return 1 / 30d;

                case HitResult.LargeTickHit:
                    return 1 / 10d;

                case HitResult.Meh:
                    return 1 / 6d;

                case HitResult.Ok:
                    return 1 / 3d;

                case HitResult.Good:
                    return 2 / 3d;

                case HitResult.Great:
                    return 1d;

                case HitResult.Perfect:
                    return 7 / 6d;

                case HitResult.SmallBonus:
                    return SMALL_BONUS_SCORE;

                case HitResult.LargeBonus:
                    return LARGE_BONUS_SCORE;
            }
        }
    }
}
