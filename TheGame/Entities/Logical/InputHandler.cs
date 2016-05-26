using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TheGame.Entities.Logical
{
    internal class InputHandler : GameComponent
    {
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;
        private static GamePadState[] _currentGamePadState;
        private static GamePadState[] _previousGamePadState;

        private static Dictionary<ActionControlls, ControllBinding> _controlls;

        /// <summary>
        /// Input to action binder
        /// </summary>
        private class ControllBinding
        {
            public Buttons? GamePadButton { get; set; }
            public Keys? KeyboardKey { get; set; }
            public MouseButtons? MouseButton { get; set; }
            public ControlEvents Events { get; } = new ControlEvents();

            private bool IsDown()
            {
                if (KeyboardKey.HasValue && _currentKeyboardState.IsKeyDown(KeyboardKey.Value)) return true;
                if (MouseButton.HasValue && _currentMouseState.IsButtonDown(MouseButton.Value)) return true;
                if (GamePadButton.HasValue && _currentGamePadState.Any(gp => gp.IsButtonDown(GamePadButton.Value))) return true;
                return false;
            }

            private bool WasDown()
            {
                if (KeyboardKey.HasValue && _previousKeyboardState.IsKeyDown(KeyboardKey.Value)) return true;
                if (MouseButton.HasValue && _previousMouseState.IsButtonDown(MouseButton.Value)) return true;
                if (GamePadButton.HasValue && _previousGamePadState.Any(gp => gp.IsButtonDown(GamePadButton.Value))) return true;
                return false;
            }

            private bool IsUp()
            {
                if (KeyboardKey.HasValue && _currentKeyboardState.IsKeyUp(KeyboardKey.Value)) return true;
                if (MouseButton.HasValue && _currentMouseState.IsButtonUp(MouseButton.Value)) return true;
                if (GamePadButton.HasValue && _currentGamePadState.Any(gp => gp.IsButtonUp(GamePadButton.Value))) return true;
                return false;
            }

            private bool WasUp()
            {
                if (KeyboardKey.HasValue && _previousKeyboardState.IsKeyUp(KeyboardKey.Value)) return true;
                if (MouseButton.HasValue && _previousMouseState.IsButtonUp(MouseButton.Value)) return true;
                if (GamePadButton.HasValue && _previousGamePadState.Any(gp => gp.IsButtonUp(GamePadButton.Value))) return true;
                return false;
            }

            public void Update(GameTime gameTime, ActionControlls action)
            {
                if (IsDown())
                {
                    Events.RaiseOnDown(gameTime, action);
                }

                if (IsUp())
                {
                    Events.RaiseOnUp(gameTime, action);
                }

                if (IsUp() && WasDown())
                {
                    Events.RaiseOnReleased(gameTime, action);
                }

                if (IsDown() && WasUp())
                {
                    Events.RaiseOnPressed(gameTime, action);
                }
            }
        }

        public ControlEvents this[ActionControlls actionControl] => _controlls[actionControl].Events;

        #region Singleton
        private InputHandler(Game game) : base(game)
        {
            _previousMouseState = Mouse.GetState();
            _currentMouseState = Mouse.GetState();

            _previousKeyboardState = Keyboard.GetState();
            _currentKeyboardState = Keyboard.GetState();

            _previousGamePadState = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length]; //Getting max gamepads supported
            _currentGamePadState = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];
            foreach (PlayerIndex playerIndex in Enum.GetValues(typeof (PlayerIndex)))
            {
                var i = (int)playerIndex;
                _previousGamePadState[i] = GamePad.GetState(playerIndex);
                _currentGamePadState[i] = GamePad.GetState(playerIndex);
            }

            //TODO: load settings
            _controlls = new Dictionary<ActionControlls, ControllBinding>
            {
                {ActionControlls.Exit, new ControllBinding {KeyboardKey = Keys.Escape} },
                {ActionControlls.Jump, new ControllBinding {KeyboardKey = Keys.Space} },
                {ActionControlls.Shoot, new ControllBinding
                                            {
                                                KeyboardKey = Keys.LeftControl,
                                                MouseButton = MouseButtons.Left
                                            }
                },
                {ActionControlls.Up, new ControllBinding {KeyboardKey = Keys.W} },
                {ActionControlls.Left, new ControllBinding {KeyboardKey = Keys.A} },
                {ActionControlls.Down, new ControllBinding {KeyboardKey = Keys.S} },
                {ActionControlls.Right, new ControllBinding {KeyboardKey = Keys.D} },
                {ActionControlls.DebugPhysics, new ControllBinding {KeyboardKey = Keys.F5} },
            };
        }

        public static InputHandler CreateInstance(Game game)
        {
            if(Instance != null)
                throw new InvalidOperationException("InputHandler is already created");

            Instance = new InputHandler(game);

            return Instance;
        }

        public static InputHandler Instance { get; private set; }

        #endregion

        public override void Update(GameTime gameTime)
        {
            //Update keyboard
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            //Update mouse
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            //Update GamePad
            foreach (PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
            {
                var i = (int)playerIndex;
                _previousGamePadState[i] = _currentGamePadState[i];
                _currentGamePadState[i] = GamePad.GetState(playerIndex);
            }

            //Raise input events
            foreach (var controlBinding in _controlls)
            {
                controlBinding.Value.Update(gameTime, controlBinding.Key);
            }

            base.Update(gameTime);
        }
    }

    internal class ControlEvents
    {
        /// <summary>
        /// Fires while button is down (pressed)
        /// </summary>
        public event Action<GameTime, ActionControlls> OnDown;

        /// <summary>
        /// Fires while button is up (not pressed)
        /// </summary>
        public event Action<GameTime, ActionControlls> OnUp;

        /// <summary>
        /// Fires when was up and now is down
        /// </summary>
        public event Action<GameTime, ActionControlls> OnPressed;

        /// <summary>
        /// Fires when was down and now is up
        /// </summary>
        public event Action<GameTime, ActionControlls> OnReleased;


        public void RaiseOnDown(GameTime gameTime, ActionControlls action)
        {
            OnDown?.Invoke(gameTime, action);
        }

        public void RaiseOnUp(GameTime gameTime, ActionControlls action)
        {
            OnUp?.Invoke(gameTime, action);
        }

        public void RaiseOnPressed(GameTime gameTime, ActionControlls action)
        {
            OnPressed?.Invoke(gameTime, action);
        }

        public void RaiseOnReleased(GameTime gameTime, ActionControlls action)
        {
            OnReleased?.Invoke(gameTime, action);
        }
    }

    /// <summary>
    /// Adds methods to some input classes like MouseState, KeyboardState and GamePadState
    /// </summary>
    internal static class InputExtensions
    {
        public static bool IsButtonUp(this MouseState mouseState, MouseButtons mouseButtons)
        {
            switch (mouseButtons)
            {
                case MouseButtons.Left:
                    return mouseState.LeftButton == ButtonState.Released;
                case MouseButtons.Right:
                    return mouseState.RightButton == ButtonState.Released;
                case MouseButtons.Middle:
                    return mouseState.MiddleButton == ButtonState.Released;
                case MouseButtons.X1:
                    return mouseState.XButton1 == ButtonState.Released;
                case MouseButtons.X2:
                    return mouseState.XButton2 == ButtonState.Released;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public static bool IsButtonDown(this MouseState mouseState, MouseButtons mouseButtons)
        {
            switch (mouseButtons)
            {
                case MouseButtons.Left:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.Right:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.Middle:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                case MouseButtons.X1:
                    return mouseState.XButton1 == ButtonState.Pressed;
                case MouseButtons.X2:
                    return mouseState.XButton2 == ButtonState.Pressed;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    /// <summary>
    /// All game actions that can be controlled through player input
    /// </summary>
    [Flags]
    internal enum ActionControlls
    {
        Exit,
        Up,
        Down,
        Left,
        Right,
        Jump,
        Shoot,
        DebugPhysics
    }

    /// <summary>
    /// Supported mouse buttons
    /// </summary>
    internal enum MouseButtons
    {
        Left,
        Right,
        Middle,
        X1,
        X2
    }
}
