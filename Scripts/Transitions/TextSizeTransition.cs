// Copyright © 2025 Bogdan Nikolayev <bodix321@gmail.com>
// All Rights Reserved

using DG.Tweening;
using TMPro;

namespace Toolkit.Tweens.Transitions
{
	public class TextSizeTransition : TweenTransition<TMP_Text>
	{
		public float TargetSize = 36;
		public float InitialSize = 24;
		public float InDuration = 1;
		public float OutDuration = 1;
		public Ease EaseIn = Ease.Linear;
		public Ease EaseOut = Ease.Linear;

		public override Tween PlayIn()
		{
			InitializeIfRequired();

			return DOVirtual.Float(InitialSize, TargetSize, InDuration, value => Target.fontSize = value)
				.SetEase(EaseIn)
				.SetLink(Target.gameObject);
		}

		public override Tween PlayOut()
		{
			InitializeIfRequired();

			return DOVirtual.Float(TargetSize, InitialSize, OutDuration, value => Target.fontSize = value)
				.SetEase(EaseOut)
				.SetLink(Target.gameObject);
		}
	}
}