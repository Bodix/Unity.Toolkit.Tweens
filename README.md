# Toolkit.Tweens

Utilities for DOTween: show and hide animations of elements, their presets and rules for tweens.

## Show and hide animations

A transition plays in and back out with shared settings, e.g. `CanvasGroupFadeTransition`, `ScaleTransition`, `MoveTransition`, `RotateTransition`, `FillTransition` and `TextSpacingTransition`. An animation plays once in one direction, e.g. `SweepAnimation`. An element of Perfect UI takes a transition in its **Animations** field as **One Tween Transition**.

- `CompositeTransition` plays its parts on one timeline. Its show plays **In Transitions** in and **In Animations** once, each at its position. **Out Transitions** and **Out Animations** make its own hide. Without them the hide is the show reversed.
- `MoveTransition` comes in from an offset to its own place and leaves to an offset. It does not save the place: if a layout moves the element, the new position is its place, so the transition works over any layout.
- `SweepAnimation` takes the element across its parent from edge to edge, in shares of the parent. Before and after the sweep the element waits outside the parent, so put it under a mask.
- **Ignore Time Scale** keeps the animation playing while the game is paused with `Time.timeScale` = 0. Set it on what the element plays: the parts of a composite follow the composite.
- **Target Source** tells where a tween takes what it animates: the **Target** field, its own object or its parent.
- A sound is not a part of an animation: start it from the events of the element, e.g. **Showing**.

### Presets

The **Presets** folder holds ready show and hide animations as prefabs. Drop a preset on an element: it becomes a child and animates its parent. Then drag the preset into the **Transition** field of **One Tween Transition** in the **Animations** field of the element.

- Simple presets hold one transition: Fade, Pop, Pop From Zero, Unfold Vertical, Unfold Horizontal, Rise, Slide From Top, Slide From Bottom, Slide From Left, Slide From Right, Drop, Swing, Spin In, Draw, Letters Converge and Shine.
- Composite presets combine them with the same values: Pop With Fade, Rise With Fade, Unfold With Fade, Letters Converge With Fade, Pop And Spin and Drop And Swing. Their composite transition goes first, so a dragged preset gives the field the whole animation.
- An instance stays linked to its preset: change the preset or make your own variant of it, and every element that uses it follows.
- The values are for a UI canvas 1080 high: a show slows down to its end, a hide is shorter and speeds up. Presets play during a pause.
- **Fade** needs a `CanvasGroup` on the element, **Draw** an `Image` of the Filled type. A preset has no `RectTransform`, so layouts skip it.

## Rules for working with DOTween

### 1. Always use `.SetLink(gameObject)` when creating a tween.

This way, when deleting an object, you will avoid a situation where the tween tries to access a non-existent object:

```text
The object of type 'Transform' has been destroyed but you are still trying to access it.
Your script should either check if it is null or you should not destroy the object.
```

[Documentation](https://dotween.demigiant.com/documentation.php?api=SetLink)

### 2. If you cache your tween in a variable (for example in `tween` variable) so you can stop it if needed, always add `.OnKill(() => tween = null)`.

This will ensure that the "Recycle Tweens" feature in DOTween works correctly. Do this even if you are not using this feature. If you don't do this and then decide to improve your project's performance by enabling the "Recycle Tweens" feature, you may have difficulty finding all the cached tweens in your project to fix this. Do this right away.

[Documentation](https://dotween.demigiant.com/documentation.php?api=Init)