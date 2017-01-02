# Introduction to Unity Mecanim

## Mechanim basics
* [Unity 5 Tutorial - Animation Control](https://www.youtube.com/watch?v=wdOk5QXYC6Y)
* [Unity Third Person Control: Mecanim Nodes - Tutorial 5](https://www.youtube.com/watch?v=7-OUZecgXv0)
* [Unity: Mecanim Animation Basics](https://community.mixamo.com/hc/en-us/articles/203879268)
* [Animation Editor](http://docs.unity3d.com/Manual/animeditor-UsingAnimationEditor.html)
* [Solo and mute](http://docs.unity3d.com/Manual/AnimationSoloMute.html)

### [animation parameters](http://docs.unity3d.com/Manual/AnimationParameters.html)
* Animation Parameters are variables that are defined within an Animator Controller that can be accessed and assigned values from scripts. This is how a script can control or affect the flow of the state machine.
* Parameters can be assigned values from a script using functions in the Animator class: SetFloat, SetInt, SetBool, SetTrigger and ResetTrigger
* [Mecanim Animation Parameter Types: Boolean vs. Trigger](http://answers.unity3d.com/questions/600268/mecanim-animation-parameter-types-boolean-vs-trigg.html)
* Parameters can also be controlled animation using Curve and read in script
    * You cannot control an Animation parameter from both Curve and Script, so you have to structure your code correspondingly.


### Root motion, Blend Tree
* [RPG Character Controller 001 - Unity 5 Root Motion](https://www.youtube.com/watch?v=k12w-rEbuXI&index=1&list=PL_eGgISVYZkeD-q83hLtPESTB-lPKnfjH)

### Layer, Layer Mask
* Layer Usages ([Animator Controller Layers](https://unity3d.com/learn/tutorials/topics/animation/animator-controller-layers))
    * Additional layer for handling different body parts while the base layer handles the base movement, such as: wave hand while moving
    * Blending mode can be: Override or Additive. Normally `Override` will be used because using `Additive` can be highly unpredictable unless you're an advanced animator. ([advanced animation](https://community.mixamo.com/hc/en-us/articles/204581427-Unity-Mecanim-Advanced-Animation))
    * Blend additional movement based on character state such as: heavy breathing when tired
    * Sync layer when you have a set of animation for when the character state changes such as: wounded animations
* Avatar Mask([Avatar Masks](https://unity3d.com/learn/tutorials/topics/animation/avatar-masks))
    * Could be set in animation to see the effect of Masking only part of the body
    * Normally apply to layer which controls part of the body ([advanced animation](https://community.mixamo.com/hc/en-us/articles/204581427-Unity-Mecanim-Advanced-Animation))

### Equip weapon
* Equip immediately without animation
    * For models created in Unity
        * Create weapon holder node
        * Attach the weapons to it
        * Hide/Unhide the correct weapon when switching weapon
    * For models created outside Unity
        * Find the correct node in the model OR create a weapon holder node like before
        * Attach the weapons to it
        * Hide/Unhide the correct weapon when switching weapon
* Equip with equip/unequip animations. All weapons are treated the same when equipped ([Sword equipping](https://www.youtube.com/watch?v=7gsl43thTsk))
    * Create equip/unequip animation for each weapons
    * Using layer/layer mask to blend the equip/unequip animations, masking only the necessary parts of the body, such as the arm movement
    * Using Animation Event to set value to flags such as: sword_equipped, sword_unequipped
    * Attach the corresponding weapon using Animation Event calling functions and/or flags
* Weapons affect whole animation when equiped ([Applied Mecanim : Character Animation and Combat State Machines](https://www.youtube.com/watch?v=Is9C4i4XyXk))


### Using mechanim as state machine
* [Mecanim as generic state machine](http://forum.unity3d.com/threads/mecanim-as-generic-state-machine.311201/)
* [Reusing animator controllers with AnimatorOverrideController](http://pekalicious.com/blog/unity3d-reusing-animator-controllers-with-animatoroverridecontroller/)
* [Applied Mecanim : Character Animation and Combat State Machines](https://www.youtube.com/watch?v=Is9C4i4XyXk)
    * A character with
        * 8 weapon types
            * 9-12 ground attacks
            * 5 aerial attacks
            * 3 defensive maneuvers
        * 4 throwable items
        * 6 classes of magic each with 3-6 abilities
        * Basic locomotion, jump, hit react, death
    * One way is to use SubStateMachine, with several depth layers
    * An alternative organization is to use BaseLayer with Overrides Layer and Additive Layer
* [Leveraging Unity 5.2's Advanced Animation Features](https://www.youtube.com/watch?v=HOURak6BpSo)
    * You can manage all transitions between attacks manually
    * Or you can use a blend tree. This is slightly better. One problem is if you change the Blend parameter while playing animation, it will change the animation immediately.
    * Solution in Unity 5: StateMachineBehaviour on a State. This way you can have code that run at the Start or End of your blendtree.
    * You could also have StateMachineBehaviour on a StateMachine. Then you can override `OnStateMachineEnter` and `OnStateMachineExit` functions
    * When the SubStateMachine is a sequence of animations, you can have code on `OnStateMachineEnter` to equip weapon
    * Can also check for Input in `StateMachineBehaviour`



## Best Practices

* [Animation events not firing](http://answers.unity3d.com/questions/806949/animation-events-not-firing.html) when the event is near the endframe, so either use a third party event dispatcher, or use `StateMachineBehaviour`.
* When importing animations, make sure to `Bake into pose` the part where you don't want to move by Root motion. Also use `Offset` to fix Average velocity not zero problems, e.g. walking animation that has a X speed
* Change the animation speed of specific layer
    * There's no way to change the animation speed of specific layer. The current best way is to Use blend tree to control the speed of a particular layer based on parameter. See [Mecanim - Change animation speed of specific animation or layers](http://forum.unity3d.com/threads/mecanim-change-animation-speed-of-specific-animation-or-layers.160395/)
    * You can also change the speed of particular state in Editor and via script
* Use int parameter instead of boolean or trigger. This way you can use AnyState to transition using condition such as `skill=1`, then when entering the mecanim state we set it to another number immediately [Applied Mecanim : Character Animation and Combat State Machines](https://www.youtube.com/watch?v=Is9C4i4XyXk)
* Use substate to simplify your state machine. However, becareful when constructing AnyState transition to deeper substate, since AnyState is global. In this way, we can ignore the immediate substate and only care about the final subtate. We use the most specific condition, such as `skill=1 and subskill=2` to trigger the final substate from AnyState. This way we don't have to configure transition to immediate substate and only care about the final state
* Use SMB: [Applied Mecanim : Character Animation and Combat State Machines](https://www.youtube.com/watch?v=Is9C4i4XyXk)
* [Running an animation completely before transitioning back](http://answers.unity3d.com/questions/685968/running-an-animation-completely-before-transitioni.html)
    * Make sure Exit time is 1.00, with FixedDuration unchecked.
* How to Run the death animation then destroy GameObject
    * Use a timer. This is quite a robust solution but maybe not visually correct in some cases, i.e. the animation might be stopped too soon.
    * Use animation event. Add an Exit event to the animation at the last frame. This is not very robust, since Unity might skip Animation event
    * Use auto transition to a fake after death state. Then use StateMachineBehaviour to detect when we exit the Death state. This is also a robust solution, but requires you to modify the structure of the Animator.
* How to transition out of a substate machine ([Using substate machine](https://www.youtube.com/watch?v=lpekqN4_4xg))
    * Using Up node: Transition to state or StateMachine
    * Using Entry/Exit nodes: Transition to/from StateMachine. This facilitates better reusability
    * Try to only use one way
