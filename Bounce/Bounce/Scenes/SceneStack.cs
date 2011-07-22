using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bounce.Scenes
{
    class SceneStack
    {
        private Stack<Scene> stack;
        public int Count = 0;

        public Scene Top
        {
            get
            {
                return stack.First();
            }
        }

        public SceneStack()
        {
            stack = new Stack<Scene>();
        }

        public void Push(Scene scene)
        {
            try
            {
                Top.IsTopScene = false;
                Top.Enabled = false;
            }
            catch (InvalidOperationException)
            { }
            finally
            {
                scene.IsTopScene = true;
                stack.Push(scene);
                Count++;
            }
        }

        public void Pop()
        {
            var scene = stack.Pop();
            scene.Kill();

            try
            {
                Top.Enabled = true;
                Top.World.Enabled = true;
            }
            catch (InvalidOperationException)
            { }
            finally
            {
                Count--;
            }
        }

        public void PopToHead()
        {
            for (int i = Count; i > 1; i--)
                Pop();
        }
    }
}
