using System;
using System.Collections.Generic;
using System.Linq;

namespace YoloSnake.Models
{
    using Enums;
    using Interfaces;

    // Represents the snake in the game
    public class Snake : IPlayable
    {
        // Default starting direction
        private const Direction StartDirection = Direction.Right;

        // Stores all snake body positions
        private LinkedList<IPosition> positions;

        // Character used to draw the snake
        private readonly char symbol;

        public Snake(char symbol, int startX, int startY, int initialLength)
        {
            this.symbol = symbol;
            this.InitializeSnakeBody(startX, startY, initialLength);
            this.Direction = StartDirection;
        }

        // Returns the current head of the snake
        public IPosition Head
        {
            get { return this.positions.First.Value; }
        }

        // Current movement direction
        public Direction Direction { get; private set; }

        // Adds new body part when food is eaten
        public void Eat(IPosition position)
        {
            this.positions.AddLast(position);
        }

        // Changes direction (prevents opposite direction)
        public void ChangeDirection(Direction newDirection)
        {
            switch (newDirection)
            {
                case Direction.Up:
                    if (this.Direction != Direction.Down)
                        this.Direction = newDirection;
                    break;

                case Direction.Down:
                    if (this.Direction != Direction.Up)
                        this.Direction = newDirection;
                    break;

                case Direction.Left:
                    if (this.Direction != Direction.Right)
                        this.Direction = newDirection;
                    break;

                case Direction.Right:
                    if (this.Direction != Direction.Left)
                        this.Direction = newDirection;
                    break;

                default:
                    throw new ArgumentException("Unknown direction");
            }
        }

        // Moves the snake one step forward
        public void Move()
        {
            // Remove tail before adding new head
            this.positions.RemoveLast();

            var head = this.positions.First.Value;

            // Add new head based on direction
            switch (this.Direction)
            {
                case Direction.Up:
                    this.positions.AddFirst(new Position(head.X, head.Y - 1));
                    break;

                case Direction.Down:
                    this.positions.AddFirst(new Position(head.X, head.Y + 1));
                    break;

                case Direction.Left:
                    this.positions.AddFirst(new Position(head.X - 1, head.Y));
                    break;

                case Direction.Right:
                    this.positions.AddFirst(new Position(head.X + 1, head.Y));
                    break;

                default:
                    throw new ArgumentException("Unknown direction");
            }
        }

        // Draws the snake on the screen
        public void Draw(IDrawer drawer)
        {
            foreach (var position in this.positions)
            {
                drawer.DrawPoint(position.X, position.Y, this.symbol);
            }

            // Clears the last position
            var last = this.positions.Last;
            drawer.DrawPoint(last.Value.X, last.Value.Y, ' ');
        }

        // Initializes the snake body at the start
        private void InitializeSnakeBody(int startX, int startY, int initialLength)
        {
            this.positions = new LinkedList<IPosition>();

            for (int i = 0; i <= initialLength; i++)
            {
                this.positions.AddLast(new Position(startX + i, startY));
            }
        }
    }
}
