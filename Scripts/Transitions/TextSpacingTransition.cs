// Copyright © 2026 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using TMPro;
using Toolkit.Tweens.Extensions;

namespace Toolkit.Tweens.Transitions
{
	// Moves the letters of a text apart or together.
	// While it plays, the text may go out of its box: a text that ends with an ellipsis would lose its letters.
	public class TextSpacingTransition : TweenTransition<TMP_Text>
	{
		public float TargetSpacing = 0;
		public float InitialSpacing = 50;
		public float InDuration = 1;
		public float OutDuration = 1;
		public CustomizableEase InEase = new CustomizableEase(Ease.OutCubic);
		public CustomizableEase OutEase = new CustomizableEase(Ease.InCubic);

		private TextOverflowModes _overflowMode;
		private bool _isOverflowAllowed;

		public override Tween PlayIn()
		{
			return Play(InitialSpacing, TargetSpacing, InDuration, InEase);
		}

		public override Tween PlayOut()
		{
			return Play(TargetSpacing, InitialSpacing, OutDuration, OutEase);
		}

		private Tween Play(float from, float to, float duration, CustomizableEase ease)
		{
			InitializeIfRequired();

			SetSpacing(from);

			return DOVirtual.Float(from, to, duration, SetSpacing)
				.SetEase(ease)
				.OnComplete(RestoreOverflow)
				.OnKill(RestoreOverflow)
				.SetLink(Target.gameObject);
		}

		private void SetSpacing(float spacing)
		{
			if (!_isOverflowAllowed)
			{
				_overflowMode = Target.overflowMode;
				Target.overflowMode = TextOverflowModes.Overflow;
				_isOverflowAllowed = true;
			}

			Target.characterSpacing = spacing;
		}

		private void RestoreOverflow()
		{
			// The tween of a destroyed text is killed too.
			if (!_isOverflowAllowed || Target == null)
				return;

			Target.overflowMode = _overflowMode;
			_isOverflowAllowed = false;
		}
	}
}
