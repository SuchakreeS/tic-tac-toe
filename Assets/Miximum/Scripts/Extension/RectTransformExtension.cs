﻿using System;
using UnityEngine;

namespace Miximum
{
    public static class RectTransformExtension
    {

        public static IDisposable AnimationTranslate(this RectTransform _rectTransform, Vector2 _target, float _duration, Easing.Ease _ease = Easing.Ease.EaseInOutQuad, Action _completed = null)
        {

            var progress = 0f;
            var current = _rectTransform.anchoredPosition;
            int milliseconds = Mathf.RoundToInt(_duration * 1000f);

            Vector2 valueTarget;
            return LerpThread
                 .Execute
                 (
                     milliseconds,
                     _count =>
                     {
                         progress += Time.deltaTime / _duration;

                         valueTarget.x = EasingFormula.EasingFloat(_ease, current.x, _target.x, progress);
                         valueTarget.y = EasingFormula.EasingFloat(_ease, current.y, _target.y, progress);
                         _rectTransform.anchoredPosition = valueTarget;

                     },
                     () =>
                     {
                         valueTarget = _target;
                         _rectTransform.anchoredPosition = valueTarget;
                         _completed?.Invoke();
                     }
                 );

        }


        public static IDisposable AnimationWidth(this RectTransform _rectTransform, float _target, float _duration, Easing.Ease _ease = Easing.Ease.EaseInOutQuad, Action _completed = null)
        {
            var progress = 0f;
            var currentWidth = _rectTransform.rect.width;

            int milliseconds = Mathf.RoundToInt(_duration * 1000f);
            float valueTarget;

            return LerpThread
                .Execute
                (
                    milliseconds,
                    _count =>
                    {
                        progress += Time.deltaTime / _duration;
                        valueTarget = EasingFormula.EasingFloat(_ease, currentWidth, _target, progress);
                        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, valueTarget);

                    },
                    () =>
                    {
                        valueTarget = _target;
                        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, valueTarget);

                        _completed?.Invoke();
                    }
                );
        }



        public static IDisposable AnimationHeight(this RectTransform _rectTransform, float _target, float _duration, Easing.Ease _ease = Easing.Ease.EaseInOutQuad, Action _completed = null)
        {
            var progress = 0f;
            var currentHeight = _rectTransform.rect.height;

            int milliseconds = Mathf.RoundToInt(_duration * 1000f);
            float valueTarget;


            return LerpThread
                .Execute
                (
                    milliseconds,
                    _count =>
                    {
                        progress += Time.deltaTime / _duration;

                        valueTarget = EasingFormula.EasingFloat(_ease, currentHeight, _target, progress);
                        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, valueTarget);

                    },
                    () =>
                    {
                        valueTarget = _target;
                        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, valueTarget);

                        _completed?.Invoke();
                    }
                );
        }


        public static IDisposable AnimationSize(this RectTransform _rectTransform, Vector2 _target, float _duration, Easing.Ease _ease = Easing.Ease.EaseInOutQuad, Action _completed = null)
        {
            var progress = 0f;
            var rect = _rectTransform.rect;
            var currentSize = new Vector2(rect.width, rect.height);

            int milliseconds = Mathf.RoundToInt(_duration * 1000f);
            Vector2 valueTarget;

            return LerpThread
                 .Execute
                 (
                     milliseconds,
                     _count =>
                     {
                         progress += Time.deltaTime / _duration;

                         valueTarget.x = EasingFormula.EasingFloat(_ease, currentSize.x, _target.x, progress);
                         valueTarget.y = EasingFormula.EasingFloat(_ease, currentSize.y, _target.y, progress);

                         _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, valueTarget.y);
                         _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, valueTarget.x);


                     },
                     () =>
                     {
                         valueTarget = _target;
                         _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, valueTarget.y);
                         _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, valueTarget.x);

                         _completed?.Invoke();
                     }
                 );
        }

    }
}
